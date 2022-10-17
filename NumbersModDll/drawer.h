#pragma once
#include "imgui/imgui.h"
#include "Windows.h"
#include "data.h"
#include <vector>
#include <forward_list>

enum ValueFormat {
    SIMPLE, SIMPLE_BOTH, COEFF, COEFF_BOTH, PERCENT, PERCENT_PRECISE, NONE
};

class FloatingNumber {
public:
    ImVec4 color;
    int timer;
    ImVec2 pos;
    int value, valueMax;
    float playerDmg;
    ValueFormat configOption;

    FloatingNumber(ImVec4 color, ImVec2 pos, int value, int valueMax, float playerDmg, ValueFormat configOption) {
        timer = 0;
        this->color = color;
        this->pos = pos;
        this->value = value;
        this->valueMax = valueMax;
        this->playerDmg = playerDmg;
        this->configOption = configOption;
    }
};

void
draw(HWND window, Player &player, Player &old_player, std::vector<Entity> &entities, std::vector<Entity> &old_entities);
