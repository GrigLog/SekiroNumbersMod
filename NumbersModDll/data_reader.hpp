#include <Windows.h>
#include <stdio.h>
#include <vector>
#include "data.h"

template <class T>
T deref(size_t ptr) {
    if (ptr == 0)
        return 0;
    T *res = reinterpret_cast<T *>(ptr);
    if (IsBadReadPtr(res, sizeof(res))) {
        printf("BadReadPtr at %zx\n", ptr);
        return 0;
    }
    return *res;
}

size_t deref(size_t ptr) {
    return deref<size_t>(ptr);
}

size_t deref_chain(const size_t* ptrs, int size) {
    size_t res = 0;
    for (int i = 0; i < size; i++) {
        res = deref(res + ptrs[i]);
        if (res == 0) {
            /*printf("chain nullptr: ");
            for (int j = 0; j <= i; j++) {
                printf(" %zx ", ptrs[j]);
            }
            printf("\n");*/
            return 0;
        }
    }
    return res;
}

//I believe macros suck but don't know any other way
#define deref_chain(ptrs) deref_chain(ptrs, sizeof(ptrs) / sizeof(*(ptrs)))

int get_hp_dmg(int ap) {
    if (ap <= 14)
        return (ap + 3) * 20;
    else if (ap <= 27)
        return 340 + (ap - 14) * 8;
    else if (ap <= 51)
        return 443 + (ap - 27) * 4;
    else
        return (int)(540 + (ap - 51) * 0.8);
}

Player get_player() {
    Player p = {};
    size_t player = deref(PLAYER.OFF);
    if (!player)
        return p;
    size_t playerStats = deref(player + PLAYER.DATA.OFF);
    if (playerStats == 0)
        return p;
    p.hp = deref<int>(playerStats + PLAYER.DATA.HEALTH);
    p.hpMax = deref<int>(playerStats + PLAYER.DATA.HEALTH_MAX);
    p.post = deref<int>(playerStats + PLAYER.DATA.POSTURE);
    p.postMax = deref<int>(playerStats + PLAYER.DATA.POSTURE_MAX);
    p.dmgHp = get_hp_dmg(deref<int>(playerStats + PLAYER.DATA.ATK_POWER));
    p.dmgPost = (int)(p.dmgHp * 0.375);

    size_t vec = deref_chain(CORDS.OFF);
    if (vec != 0)
        p.cors = {deref<float>(vec + CORDS.X), deref<float>(vec + CORDS.Y), deref<float>(vec + CORDS.Z)};
    if ((vec = deref_chain(LOCK_CORDS.OFF)) != 0)
        p.lock = {deref<float>(vec + LOCK_CORDS.X), deref<float>(vec + LOCK_CORDS.Y), deref<float>(vec + LOCK_CORDS.Z)};
    if ((vec = deref_chain(CAMERA_CORDS.OFF)) != 0)
        p.camera = {deref<float>(vec + CAMERA_CORDS.X), deref<float>(vec + CAMERA_CORDS.Y), deref<float>(vec + CAMERA_CORDS.Z)};
    return p;
}

std::vector<Entity> get_entities() {
    std::vector<Entity> res = {};
    size_t map = deref(MAP.OFF);
    if (map == 0)
        return res;
    size_t areas = map + MAP.AREAS_ARRAY;
    for (int i = 0; i < MAP.AREAS_TOTAL; i++) {
        size_t area = deref(areas + i * 8);
        if (area == 0)
            continue;
        int enemies = deref<int>(area);
        size_t enemiesArr = deref(area + 8);
        for (int j = 0; j < enemies; j++) {
            size_t entityPtr = enemiesArr + j * 0x38;
            try {
                size_t entity = deref(entityPtr);
                if (entity == 0)
                    continue;
                Entity e = {};
                e.id = (i << 16) | j;
                size_t cors = deref(entity + ENTITY.CORDS.OFF);
                if (cors) {
                    e.cors.x = deref<float>(cors + ENTITY.CORDS.X);
                    e.cors.y = deref<float>(cors + ENTITY.CORDS.Y);
                    e.cors.z = deref<float>(cors + ENTITY.CORDS.Z);
                } else {
                    continue;
                    //printf("enemy at %zx (area %d index %d) has no cors\n", entity, i, j);
                }

                size_t modules = deref(entity + ENTITY.MODULES.OFF);
                if (!modules) {
                    continue;
                    //printf("enemy at %zx (area %d index %d) has no modules\n", entity, i, j);
                }
                size_t data = deref(modules + ENTITY.MODULES.DATA.OFF);
                if (data) {
                    e.hp = deref<int>(data + ENTITY.MODULES.DATA.HP);
                    e.hpMax = deref<int>(data + ENTITY.MODULES.DATA.HP_MAX);
                    e.post = deref<int>(data + ENTITY.MODULES.DATA.POST);
                    e.postMax = deref<int>(data + ENTITY.MODULES.DATA.POST_MAX);
                } else {
                    continue;
                    //printf("enemy at %zx (area %d index %d) has no data\n", entity, i, j);
                }
                size_t status = deref(modules + ENTITY.MODULES.STATUS.OFF);
                if (status) {
                    e.poisonHp = deref<int>(status + ENTITY.MODULES.STATUS.POISON_HP);
                    e.poisonHpMax = deref<int>(status + ENTITY.MODULES.STATUS.POISON_HP_MAX);
                    e.fireHp = deref<int>(status + ENTITY.MODULES.STATUS.FIRE_HP);
                    e.fireHpMax = deref<int>(status + ENTITY.MODULES.STATUS.FIRE_HP_MAX);
                } else {
                    continue;
                    //printf("enemy at %zx (area %d index %d) has no status\n", entity, i, j);
                }
                res.push_back(e);
            } catch (const char *error) {
                printf("error parsing enemy at area %d index %d address %zx\n", i, j, entityPtr);
                throw error;
            }
        }
    }
    //printf("found %llu entities\n", res.size());
    return res;
}