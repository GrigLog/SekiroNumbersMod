#include "drawer.h"
#include "imgui/imgui.h"
#include <Windows.h>

namespace Config {
    #define OPTIONS_LITERAL(S) playerHp##S##playerHpDmg##S##playerHpHeal##\
    S##playerPost##S##playerPostDmg##S##playerPostHeal##\
    S##enemyHp##S##enemyHpDmg##S##enemyHpHeal##\
    S##enemyPost##S##enemyPostDmg##S##enemyPostHeal##\
    S##enemyPoison##S##enemyPoisonDmg##S##enemyFire##S##enemyFireDmg##S
    #define OPTIONS(S) playerHp S playerHpDmg S playerHpHeal \
    S playerPost S playerPostDmg S playerPostHeal \
    S enemyHp S enemyHpDmg S enemyHpHeal \
    S enemyPost S enemyPostDmg S enemyPostHeal \
    S enemyPoison S enemyPoisonDmg S enemyFire S enemyFireDmg
    #define OPTIONS_COUNT 16
    #define COMMA ,
    #define MACRO_VALUE(x) #x
    #define MACRO_STR(x) MACRO_VALUE(x)

    extern ValueFormat OPTIONS(COMMA);
    void init(HWND window);
    void update();
}
