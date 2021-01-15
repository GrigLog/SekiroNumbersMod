using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SekiroNumbersMod.Scripts {
    class FloatingNumber : Number {
        int counter;
        int lifetime;
        public FloatingNumber(PointF pos, Brush brush, int value, int lifetime=30) : base(pos, brush, value) {
            this.lifetime = lifetime;
        }

        public override void draw(Graphics g) {
            base.draw(g);
            if (counter++ == lifetime) {
                Drawer.numbers.Remove(this);
            }
        }

        protected override PointF getPos() {
            return new PointF(Drawer.rect.Width * startPos.X - value.ToString().Length / 2 * Drawer.font.Size, Drawer.rect.Height * startPos.Y - counter);
        }
    }
}
