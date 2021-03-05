using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SekiroNumbersMod.Scripts {
    class Config {
        public static bool absoluteDamageVals = true;
        public static bool absoluteSelfVals = true;
        public static bool absoluteLockedVals = true;

        public static string formatSelfHp(int v) {
            if (absoluteSelfVals)
                return v.ToString();
            return (int)((float)v / DataReader.getMaxHealth() * 100) + "%";
        }

        public static string formatSelfPost(int v) {
            if (absoluteSelfVals)
                return v.ToString();
            return (int)((float)v / DataReader.getMaxPosture() * 100) + "%";
        }

        public static string formatHpDam(int v) {
            if (absoluteDamageVals)
                return v.ToString();
            return round((float)v / DataReader.baseHpDamage()) + "x";
        }

        public static string formatPostDam(int v) {
            if (absoluteDamageVals)
                return v.ToString();
            return round((float)v / DataReader.basePostDamage()) + "x";
        }

        public static string formatLockedHp(int v, int max) {
            if (absoluteLockedVals)
                return v + " / " + max;
            return round((float)v / DataReader.baseHpDamage()) + "x /" + round((float)max / DataReader.baseHpDamage()) + "x";
        }

        public static string formatLockedPost(int v, int max) {
            if (absoluteLockedVals)
                return v + "/" + max;
            return round((float)v / DataReader.basePostDamage()) + "x / " + round((float)max / DataReader.basePostDamage()) + "x";
        }

        static double round(float a) {
            double res = Math.Round(a, 2);
            if (res >= 0.98 && res <= 1.02) //sometimes damage calculation is not 100% accurate
                res = 1;
            return res;
        }
    }
}
