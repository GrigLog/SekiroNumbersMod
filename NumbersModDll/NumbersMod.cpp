#include <Windows.h>
#include <iostream>
#include <array>
#include <vector>
#include "data.h"
#include "data_reader.hpp"
#include "drawer.h"
#include <forward_list>
#include "imgui/imgui.h"
#include "config.h"


std::vector<Entity> old_entities = {};
Player old_player = {};

void NumbersModLoop(HWND window) {
    std::vector<Entity> entities = {};
    Player player = {};
    try {
        entities = get_entities();
    } catch (const char* message) {
        std::cout << "Error reading enemy data: " << message << "\n";
    }
    try {
        player = get_player();
        //std:: cout << player << "\n";
    } catch (const char* message) {
        std::cout << "Error reading player data: " << message << "\n";
    }

    draw(window, player, old_player, entities, old_entities);

    old_entities = entities;
    old_player = player;
}