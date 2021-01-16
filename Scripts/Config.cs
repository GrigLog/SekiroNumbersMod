using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SekiroNumbersMod.Scripts {
    class Config {
        public static bool absoluteDamageVals = false;
        public static bool absoluteSelfVals = false;

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
            return Math.Round((float)v / DataReader.baseHpDamage(), 2) + "x";
        }

        public static string formatPostDam(int v) {
            if (absoluteDamageVals)
                return v.ToString();
            return Math.Round((float)v / DataReader.basePostDamage(), 2) + "x";
        }
    }
}
