#include <Windows.h>
#include "imgui/imgui.h"
#include <string>
#include <format>
#include <vector>
#include <forward_list>
#include <iostream>
#include "data.h"
#include "drawer.h"
#include "config.h"

const ImVec4 HEALTH = ImVec4(1, 0, 0, 1);
const ImVec4 POSTURE = ImVec4(1, 0.8, 0, 1);
static const int MAX_FORMAT_CHARS = 10;



void centered_text(float x, float y, const std::string& text, const ImVec4& color, ImVec2 screenSize) {
    ImVec2 textSize = ImGui::CalcTextSize(text.c_str());
    ImGui::SetCursorPos(ImVec2(screenSize.x * x - textSize.x / 2, screenSize.y * y - textSize.y / 2));
    ImGui::TextColored(color, "%s", text.c_str());
}

std::string short_string_format(const char* format, ...) {
    va_list argptr;
    va_start(argptr, format);
    char buf[MAX_FORMAT_CHARS + 1];
    int chars = vsnprintf(buf, MAX_FORMAT_CHARS, format, argptr);
    va_end(argptr);
    return std::string(buf, chars);
}

std::string format(int stat, int max, int swordDmg, ValueFormat vf) {
    switch (vf) {
        case SIMPLE:
            return std::to_string(stat);
        case SIMPLE_BOTH:
            return std::to_string(stat).append("/").append(std::to_string(max));
        case COEFF:
            swordDmg = max(20, swordDmg);
            return short_string_format("%.2fx", (float) stat / swordDmg);
        case COEFF_BOTH:
            swordDmg = max(20, swordDmg);
            return short_string_format("%.2fx/", (float) stat / swordDmg)
                .append(short_string_format("%.2fx", (float) max / swordDmg));
        case PERCENT:
            max = max(1, max);
            return std::to_string(stat * 100 / max).append("%");
        case PERCENT_PRECISE:
            max = max(1, max);
            return short_string_format("%.1f%%", (float) stat * 100 / max);
        case NONE:
        default:
            return {};
    }
}

//Maybe more efficient calculations?
ImVec2 toScreenCors(V3 cors, Player& p) {
    V3 camRelative1 = p.camera;
    //std::cout <<"camRelative1" << camRelative1 << "\n";
    camRelative1.y = camRelative1.y * 1.95f;
    V3 camRelative = camRelative1.rotateY(-55.15 / 57.2956) * 2;
    //std::cout <<"camRelative" << camRelative << "\n";

    V3 camGlobal = p.cors + camRelative;
    //::cout <<"camGlobal" << camGlobal << "\n";
    V3 corsForCamera = cors - camGlobal;
    //std::cout << "corsForCamera" << corsForCamera << "\n";
    V3 zaxis = -camRelative.normalized();
    V3 xaxis = V3(0, 1, 0).cross(zaxis).normalize();
    V3 yaxis = zaxis.cross(xaxis).normalize();
    //std::cout << "axes" << xaxis << yaxis << zaxis << "\n";

    V3 viewCors = V3(corsForCamera * xaxis, corsForCamera * yaxis, corsForCamera * zaxis);
    //std::cout << "viewCors" << viewCors << "\n";
    if (viewCors.z < 1 || viewCors.z > 1000) {
        return {-1, -1};
    }
    float dist_min = 1;
    float dist_max = 1000;
    float fovx = 1.347;
    float fovy = 1;
    V3 projected = V3(viewCors.x / tanf(fovx / 2) / viewCors.z,
                          viewCors.y / tanf(fovy / 2) / viewCors.z,
                          ((dist_max + dist_min) + 2 * dist_min * dist_max / -viewCors.z) / (dist_min - dist_max));
    //std::cout << "projected" << projected << "\n";
    float x = (projected.x + 1) / 2.0f;
    float y = (-projected.y + 1) / 2.0f;
    return {x, y};
}

const ImVec4 colFromHex(uint32_t color) {
    return ImVec4((float)((color & 0xFF0000) >> 16) / 255,
                  (float)((color & 0xFF00) >> 8) / 255,
                  (float)(color & 0xFF) / 255, 1);

}
namespace Colors {

    const ImVec4 hp = colFromHex(0xFF0000);
    const ImVec4 hpHeal = colFromHex(0x00FF00);
    const ImVec4 playerHpDmg = colFromHex(0xB32428);
    const ImVec4 post = colFromHex(0xFFCC00);
    const ImVec4 postDmg = colFromHex(0xFFA500);
    const ImVec4 postHeal = colFromHex(0x3CB371);
    const ImVec4 poison = colFromHex(0x304B26);
    const ImVec4 fire = colFromHex(0xFF7F49);
}

std::forward_list<FloatingNumber> numbers = {};

void
draw(HWND window, Player &player, Player &old_player, std::vector<Entity> &entities, std::vector<Entity> &old_entities) {
    static RECT rect;
    GetWindowRect(window, &rect);
    float width = rect.right - rect.left;
    float height = rect.bottom - rect.top;
    ImVec2 screenSize = {width, height};
    static int ctr = 0;

    ImGui::SetNextWindowPos(ImVec2(0, 0));
    ImGui::SetNextWindowSize(ImVec2(width, height));
    ImGui::Begin("Main", NULL, ImGuiWindowFlags_NoBackground | ImGuiWindowFlags_NoMove | ImGuiWindowFlags_NoDecoration);

    auto a_ptr = old_entities.begin();
    auto b_ptr = entities.begin();
    int d;
    while (a_ptr != old_entities.end() && b_ptr != entities.end()) {
        int diff = a_ptr->id - b_ptr->id;
        if (diff == 0) {
            Entity a = *a_ptr, b = *b_ptr;
            ImVec2 screenCors = toScreenCors(a.cors, player);
            if ((d = b.hp - a.hp) != 0)
                numbers.push_front(FloatingNumber(
                    d < 0 ? Colors::hp : Colors::hpHeal,
                    {screenCors.x - 0.02f, screenCors.y},
                    abs(d), b.hpMax, player.dmgHp,
                    d < 0 ? Config::enemyHpDmg : Config::enemyHpHeal));
            if ((d = b.post - a.post) != 0 && d < 0 || d > b.postMax * 0.05f)
                numbers.push_front(FloatingNumber(
                    d < 0 ? Colors::postDmg : Colors::postHeal,
                    {screenCors.x + 0.02f, screenCors.y},
                    abs(d), b.postMax, player.dmgPost,
                    d < 0 ? Config::enemyPostDmg : Config::enemyPostHeal));
            if ((d = b.fireHp - a.fireHp) < 0)
                numbers.push_front(FloatingNumber(
                    Colors::fire,
                    {screenCors.x - 0.02f, screenCors.y + 0.01f},
                    -d, b.fireHpMax, 31,
                    Config::enemyFireDmg));
            if ((d = b.poisonHp - a.poisonHp) < 0)
                numbers.push_front(FloatingNumber(
                    Colors::poison,
                    {screenCors.x + 0.02f, screenCors.y + 0.01f},
                    -d, b.poisonHpMax, 31,
                    Config::enemyPoisonDmg));
        }
        if (diff >= 0)
            b_ptr++;
        if (diff <= 0)
            a_ptr++;
    }

    if (!entities.empty() && !player.lock.isZero()) {
        Entity& closest = entities.front();
        float closest_dist = V3::distance(closest.cors, player.lock);
        for (int i = 1; i < entities.size(); i++) {
            float dist = V3::distance(entities[i].cors, player.lock);
            if (dist < closest_dist) {
                closest = entities.at(i);
                closest_dist = dist;
            }
        }
        centered_text(0.25f, 0.05f,
              format(closest.hp, closest.hpMax, player.dmgHp, Config::enemyHp),
              Colors::hp, screenSize);
        centered_text(0.5f, 0.05f,
                      format(closest.post, closest.postMax, player.dmgPost, Config::enemyPost),
                      Colors::post, screenSize);
        centered_text(0.75f, 0.05f,
                      format(closest.fireHp, closest.fireHpMax, 31, Config::enemyFire),
                      Colors::fire, screenSize);
        centered_text(0.75f, 0.08f,
                      format(closest.poisonHp, closest.poisonHpMax, 31, Config::enemyPoison),
                      Colors::poison, screenSize);
    }

    static int noHpDmgCtr = 0, noPostDmgCtr = 0;
    if ((d = player.hp - old_player.hp) != 0) {
        numbers.push_front(FloatingNumber(
                d < 0 ? Colors::playerHpDmg : Colors::hpHeal,
                {0.3f, 0.85f},
                abs(d), player.hpMax, player.dmgHp,
                d < 0 ? Config::playerHpDmg : Config::playerHpHeal));
        noHpDmgCtr = 300;
    }
    if ((d = player.post - old_player.post) != 0 && fabs(d) > player.postMax * 0.03f) {
        numbers.push_front(FloatingNumber(
                d < 0 ? Colors::postDmg : Colors::postHeal,
                {0.5f, 0.85f},
                abs(d), player.postMax, player.dmgPost,
                d < 0 ? Config::playerPostDmg : Config::playerPostHeal));
        noPostDmgCtr = 300;
    }
    if (noHpDmgCtr-- > 0 || player.hp != player.hpMax)
        centered_text(0.25f, 0.92f, format(player.hp, player.hpMax, player.dmgHp, Config::playerHp), Colors::hp, screenSize);
    if (noPostDmgCtr-- > 0 || player.post != player.postMax)
        centered_text(0.5f, 0.92f, format(player.post, player.postMax, player.dmgPost, Config::playerPost), Colors::post, screenSize);


    numbers.remove_if([screenSize](FloatingNumber& num) {
        ImGui::SetCursorPos(ImVec2(num.pos.x, num.pos.y - num.timer));
        centered_text(num.pos.x, num.pos.y - num.timer / 600.0f, format(num.value, num.valueMax, num.playerDmg, num.configOption), num.color, screenSize);
        return num.timer++ == 60;
    });
    ImGui::End();
}
