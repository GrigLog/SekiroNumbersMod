namespace SekiroNumbersMod {
    struct Entity {
        public Entity(int hp, int maxHp, int post, int maxPost, V3 cors) {
            this.hp = hp;
            this.maxHp = maxHp;
            this.post = post;
            this.maxPost = maxPost;
            this.cors = cors;
        }
        public int hp, post, maxHp, maxPost;
        public V3 cors;
    }
}
