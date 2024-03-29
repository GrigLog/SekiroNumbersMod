#include "config.h"
#include <stdio.h>

namespace Config {
    const char* PATH = "NumbersMod\\config.txt";
    const char* TEMPLATE = MACRO_STR(OPTIONS_LITERAL(:%d\n)); //hehe boi
    const char *FORMATS[] = {"simple", "simple\\simple", "coeff", "coeff\\coeff", "per cent", "per cent precise", "none"};
    const int FORMATS_LEN = sizeof(FORMATS) / sizeof(*FORMATS);
    bool isOpen = false;
    int cooldown = 0;

    ValueFormat OPTIONS(COMMA);

    void init(HWND window) {
        FILE* file = fopen(PATH, "r");
        if (!file || fscanf(file, TEMPLATE, &OPTIONS(COMMA &)) != OPTIONS_COUNT) {
            FILE* fwrite = fopen(PATH, "w");
            fprintf(fwrite, TEMPLATE, OPTIONS(COMMA));
            fclose(fwrite);
            file = fopen(PATH, "r");
            if (!file) {
                MessageBoxA(window, "Config file not found", PATH, MB_OK);
                return;
            }
        }
        fscanf(file, TEMPLATE, &OPTIONS(COMMA &));
        fclose(file);
    }

    void saveAndClose() {
        FILE* file = fopen(PATH, "w");
        fprintf(file, TEMPLATE, OPTIONS(COMMA));
        fclose(file);
        isOpen = false;
    }

    #define COMBO(x) ImGui::Combo(MACRO_STR(x), (int*)&x, FORMATS, FORMATS_LEN)
    void update() {
        if (ImGui::IsKeyDown(ImGui::GetKeyIndex(ImGuiKey_Escape))
                && ImGui::IsKeyDown(ImGui::GetKeyIndex(ImGuiKey_Enter)) && cooldown <= 0) {
            cooldown = 10;
            if (!isOpen)
                isOpen = true;
            else
                saveAndClose();
        }
        cooldown--;
        if (isOpen) {
            ImGui::Begin("Config");
            COMBO(playerHp);
            COMBO(playerPost);
            COMBO(playerHpDmg);
            COMBO(playerPostDmg);
            COMBO(playerHpHeal);
            COMBO(playerPostHeal);
            COMBO(enemyHp);
            COMBO(enemyPost);
            COMBO(enemyHpDmg);
            COMBO(enemyPostDmg);
            COMBO(enemyHpHeal);
            COMBO(enemyPostHeal);
            COMBO(enemyPoison);
            COMBO(enemyFire);
            COMBO(enemyPoisonDmg);
            COMBO(enemyFireDmg);

            if (ImGui::Button("Save"))
                saveAndClose();
            ImGui::End();
        }
    }

}