using System;

namespace SekiroNumbersMod {
    struct Entity {

        public Entity(Data data, Resist resist, V3 cors, IntPtr address) {
            this.d = data;
            this.res = resist;
            this.cors = cors;
            this.address = address;
        }

        public Data d;
        public Resist res;
        public IntPtr address;
        public V3 cors;
        public static Entity None = new Entity(new Data(0, 0, 0, 0), new Resist(0, 0, 0, 0), new V3(0, 0, 0), new IntPtr(-1));
        public bool isNone() {
            return address.ToInt64() == -1;
        }

        public static bool operator==(Entity a, Entity b) {
            return a.address == b.address;
        }

        public static bool operator!=(Entity a, Entity b) {
            return !(a == b);
        }

        public struct Data {
            public int hp, post, maxHp, maxPost;
            public Data(int hp, int maxHp, int post, int maxPost) {
                this.hp = hp;
                this.maxHp = maxHp;
                this.post = post;
                this.maxPost = maxPost;
            }
        }

        public struct Resist {
            public int poison, fire, maxPoison, maxFire;
            public Resist(int poison, int maxPoison, int fire, int maxFire) {
                this.poison = poison;
                this.fire = fire;
                this.maxPoison = maxPoison;
                this.maxFire = maxFire;
            }
        }
    }
}
