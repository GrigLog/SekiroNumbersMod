using System;

namespace SekiroNumbersMod {
    public struct V3 {
        public float x, y, z;
        public V3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public bool isZero() {
            return x == 0 && y == 0 && z == 0;
        }
        public static double distance(V3 a, V3 b) {
            return Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2) + Math.Pow(a.z - b.z, 2));
        }

        public override bool Equals(object obj) {
            if (!(obj is V3))
                return false;
            V3 v = (V3)obj;
            return x == v.x && y == v.y && z == v.z;
        }
        public static bool operator==(V3 a, V3 b) {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(V3 a, V3 b) {
            return !(a == b);
        }
        public override string ToString() {
            return "{" + x + ", " + y + ", " + z + "}";
        }
    }
}
