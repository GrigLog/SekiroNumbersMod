using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SekiroNumbersMod.Scripts {
    class FloatingNumber : Number {
        public int counter;
        public int lifetime;
        public FloatingNumber(PointF pos, Brush brush, string text, int value=0, int lifetime=30) : base(pos, brush, text, value) {
            this.lifetime = lifetime;
            small = true;
        }

        public override void draw(Graphics g) {
            base.draw(g);
            if (counter++ == lifetime) {
                Drawer.numbers.Remove(this);
            }
        }

        protected override PointF getPos() {
            return new PointF(Drawer.rect.Width * startPos.X - text.Length / 2 * Drawer.smallFont.Size, Drawer.rect.Height * startPos.Y - counter);
        }
    }
}
