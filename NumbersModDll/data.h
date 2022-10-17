#pragma once
#include <ostream>
#include "V3.hpp"

const size_t MODULE_BASE = 0x140000000;

struct {
    size_t OFF = MODULE_BASE + 0x3D5AAC0;
    struct {
        size_t OFF = 0x8;
        size_t HEALTH = 0x18;
        size_t HEALTH_MAX = 0x1C;
        size_t POSTURE = 0x34;
        size_t POSTURE_MAX = 0x38;
        size_t ATK_POWER = 0x48;
    } DATA;
} PLAYER;

struct {
    size_t OFF[4] = {MODULE_BASE + 0x3D94970, 0x10, 0x5E8, 0xD8};
    size_t X = 0x40;
    size_t Y = 0x44;
    size_t Z = 0x48;
} CORDS;

struct {
    size_t OFF[2] = {MODULE_BASE + 0x3D91848, 0x18};
    size_t X = 0x30;
    size_t Y = 0x34;
    size_t Z = 0x38;
} CAMERA_CORDS;

struct {
    size_t OFF = MODULE_BASE + 0x3D7A1E0;
    size_t AREAS_ARRAY = 0x518;
    struct {
        size_t ENTITY_LIST = 0x8;
    } AREA;
    int AREAS_TOTAL = 19;
} MAP;

struct {
    //player instance: 1FF8 - modules; 108 - AutoHoming module, 60 - ChrCamera
    size_t OFF[9] = {MODULE_BASE + 0x3D92828, 0x8, 0x0, 0x28, 0x0, 0x28, 0x1FF8, 0x108, 0x60};
    size_t X = 0x13C;
    size_t Y = 0x14C;
    size_t Z = 0x15C;
} LOCK_CORDS;

struct {
    struct {
        size_t OFF = 0x1FF8;
        struct {
            size_t OFF = 0x18;
            size_t HP = 0x130;
            size_t HP_MAX = 0x134;
            size_t POST = 0x148;
            size_t POST_MAX = 0x14C;
        } DATA;
        struct {
            size_t OFF = 0x20;
            size_t POISON_HP = 0x10;
            size_t FIRE_HP = 0x18;
            size_t POISON_HP_MAX = 0x24;
            size_t FIRE_HP_MAX = 0x2C;
        } STATUS;
    } MODULES;
    struct {
        size_t OFF = 0x98;
        size_t X = 0xC;
        size_t Y = 0x1C;
        size_t Z = 0x2C;
    } CORDS;
} ENTITY;

struct Entity {
    int id; //this is my own stat used for convenience. No clue if Sekiro enemies have any kind of id in-game
    int hp, hpMax;
    int post, postMax;
    int poisonHp, poisonHpMax;
    int fireHp, fireHpMax;
    V3 cors;

    friend std::ostream &operator<<(std::ostream &os, const Entity &entity) {
        os << "hp: " << entity.hp << " hpMax: " << entity.hpMax << " post: " << entity.post << " postMax: "
           << entity.postMax << " poisonHp: " << entity.poisonHp << " poisonHpMax: " << entity.poisonHpMax
           << " fireHp: " << entity.fireHp << " fireHpMax: " << entity.fireHpMax << " x: " << entity.cors.x << " y: "
           << entity.cors.y << " z: " << entity.cors.z;
        return os;
    }
};

struct Player {
    int hp, hpMax;
    int post, postMax;
    float dmgHp, dmgPost;
    V3 cors, camera, lock;

    friend std::ostream &operator<<(std::ostream &os, const Player &player) {
        os << "hp: " << player.hp << " hpMax: " << player.hpMax << " post: " << player.post << " postMax: "
           << player.postMax << " dmgHp: " << player.dmgHp << " dmgPost: " << player.dmgPost << " cors: " << player.cors
           << " camera: " << player.camera << " lock: " << player.lock;
        return os;
    }
};
