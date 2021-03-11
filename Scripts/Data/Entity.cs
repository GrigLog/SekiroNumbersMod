using System;

namespace SekiroNumbersMod {
    struct Entity {

        public Entity(int hp, int maxHp, int post, int maxPost, V3 cors, IntPtr address) {
            this.hp = hp;
            this.maxHp = maxHp;
            this.post = post;
            this.maxPost = maxPost;
            this.cors = cors;
            this.address = address;
        }
        public int hp, post, maxHp, maxPost;
        public IntPtr address;
        public V3 cors;
        public static Entity None = new Entity(0, 0, 0, 0, new V3(0, 0, 0), new IntPtr(-1));
        public bool isNone() {
            return address.ToInt64() == -1;
        }

        public static bool operator==(Entity a, Entity b) {
            return a.address == b.address;
        }

        public static bool operator!=(Entity a, Entity b) {
            return !(a == b);
        }
    }
}
