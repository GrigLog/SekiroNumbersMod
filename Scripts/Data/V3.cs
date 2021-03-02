using System;

namespace SekiroNumbersMod {
    public struct V3 {
        public float x, y, z;
        public V3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public V3(double x, double y, double z) : this((float)x, (float)y, (float)z) {

        }
        public bool isZero() {
            return x == 0 && y == 0 && z == 0;
        }
        public V3 normalize() {
            float len = length();
            x /= len;
            y /= len;
            z /= len;
            return this;
        }
        public float length() {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }
        public static double distance(V3 a, V3 b) {
            return Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2) + Math.Pow(a.z - b.z, 2));
        }
        public static V3 cross(V3 a, V3 b) {
            V3 res = new V3(a.y*b.z - a.z*b.y, a.z*b.x - a.x*b.z, a.x*b.y - a.y*b.x);
            //Console.WriteLine(res);
            return res;
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
        public static V3 operator-(V3 a, V3 b) {
            return new V3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static V3 operator +(V3 a, V3 b) {
            return new V3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static V3 operator -(V3 a) {
            return new V3(-a.x, -a.y, -a.z);
        }
        public static double operator*(V3 a, V3 b) {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }
        public override string ToString() {
            return "{" + x + ", " + y + ", " + z + "}";
        }
    }
}
