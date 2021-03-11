using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SekiroNumbersMod.Scripts {
    class FloatingNumber : Number {
        public int counter;
        public int lifetime = 30;
        public int value;
        public int big;
        public Entity entity;


        public FloatingNumber(PointF pos, Color color, string text) : base(pos, color, text) {
        }

        public FloatingNumber(Entity entity, Color color, string text) : base(new PointF(), color, text){
            startPos = Drawer.toScreenCors(entity.cors);
        }

        public override void draw(Graphics g) {
            if (!hidden) {
                if (big > 0) {
                    g.DrawString(text, Drawer.bigFont, new SolidBrush(Color.FromArgb(getOpacity(), color)), getPos());
                    big--;
                }
                else
                    g.DrawString(text, Drawer.smallFont, new SolidBrush(Color.FromArgb(getOpacity(), color)), getPos());
            }
            if (counter++ == lifetime) {
                Drawer.numbers.Remove(this);
            }
        }

        protected override PointF getPos() {
            return new PointF(Drawer.rect.Width * startPos.X - text.Length / 2 * Drawer.smallFont.Size, Drawer.rect.Height * startPos.Y - counter);
        }

        protected int getOpacity() {
            if (counter <= 20)
                return 255;
            else
                return 200 - (counter - 20) * 20;
        }
    }
}
