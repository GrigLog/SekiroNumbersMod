using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SekiroNumbersMod.Scripts {
    class Config {
        public enum NumbersMode {
            ABSOLUTE, RELATIVE, OFF
        }
        public static NumbersMode damageVals = NumbersMode.RELATIVE;
        public static NumbersMode selfVals = NumbersMode.RELATIVE;
        public static NumbersMode lockedVals = NumbersMode.RELATIVE;
        public static NumbersMode statusVals = NumbersMode.ABSOLUTE;
        static Dictionary<string, string> fileToCodeNames = new Dictionary<string, string> 
        { { "SelfStats", "selfVals" }, { "LockedStats", "lockedVals" }, { "DamageNumbers", "damageVals" } };

        public static NumbersMode parseMode(string s) {
            switch (s) {
                case "relative":
                    return NumbersMode.RELATIVE;
                case "absolute":
                    return NumbersMode.ABSOLUTE;
                case "off":
                    return NumbersMode.OFF;
                default:
                    return NumbersMode.RELATIVE;
            }
        }

        public static void updateFromFile() {
            StreamReader sr = new StreamReader("NumbersMod\\config.txt");
            string line;
            while ((line = sr.ReadLine()) != null) {
                string[] parts = line.Split(':');
                if (fileToCodeNames.ContainsKey(parts[0])) {
                    string fieldName = fileToCodeNames[parts[0]];
                    FieldInfo field = typeof(Config).GetField(fieldName, BindingFlags.Static | BindingFlags.Public);
                    field.SetValue(null, parseMode(parts[1]));
                }
            }
            sr.Close();
        }

        public static string formatSelfHp(int v) {
            switch (selfVals) {
                case NumbersMode.ABSOLUTE:
                    return v.ToString();
                case NumbersMode.RELATIVE:
                    return (int)((float)v / DataReader.getMaxHealth() * 100) + "%";
            }
            return "";
        }

        public static string formatSelfPost(int v) {
            switch (selfVals) {
                case NumbersMode.ABSOLUTE:
                    return v.ToString();
                case NumbersMode.RELATIVE:
                    return (int)((float)v / DataReader.getMaxPosture() * 100) + "%";
            }
            return "";
        }

        public static string formatHpDam(int v) {
            switch (damageVals) {
                case NumbersMode.ABSOLUTE:
                    return v.ToString();
                case NumbersMode.RELATIVE:
                    return round((float)v / DataReader.baseHpDamage()) + "x";
            }
            return "";
        }

        public static string formatPostDam(int v) {
            switch (damageVals) {
                case NumbersMode.ABSOLUTE:
                    return v.ToString();
                case NumbersMode.RELATIVE:
                    return round((float)v / DataReader.basePostDamage()) + "x";
            }
            return "";
        }

        public static string formatLockedHp(int v, int max) {
            switch (lockedVals) {
                case NumbersMode.ABSOLUTE:
                    return v + " / " + max;
                case NumbersMode.RELATIVE:
                    return round((float)v / DataReader.baseHpDamage()) + "x /" + round((float)max / DataReader.baseHpDamage()) + "x";
            }
            return "";
        }

        public static string formatLockedPost(int v, int max) {
            switch (lockedVals) {
                case NumbersMode.ABSOLUTE:
                    return v + " / " + max;
                case NumbersMode.RELATIVE:
                    return round((float)v / DataReader.basePostDamage()) + "x / " + round((float)max / DataReader.basePostDamage()) + "x";
            }
            return "";
        }



        public static string formatLockedPoison(int v, int max) {
            switch (lockedVals) {
                case NumbersMode.ABSOLUTE:
                    return v + " / " + max;
                case NumbersMode.RELATIVE:
                    return round(v / 31f) + "x /" + round(max / 31f) + "x";
            }
            return "";
        }

        public static string formatLockedFire(int v, int max) {
            switch (lockedVals) {
                case NumbersMode.ABSOLUTE:
                    return v + " / " + max;
                case NumbersMode.RELATIVE:
                    return round(v / 31f) + "x / " + round(max / 31f) + "x";
            }
            return "";
        }

        public static string formatPoisonDam(int v) {
            return v.ToString();
        }

        public static string formatFireDam(int v) {
            return v.ToString();
        }

        static double round(float a) {
            double res = Math.Round(a, 2);
            if (res >= 0.98 && res <= 1.02) //sometimes damage calculation is not 100% accurate
                res = 1;
            return res;
        }
    }
}
