using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SekiroNumbersMod.Scripts {
    class Number {
        public PointF startPos;
        public Brush brush;
        public string text;
        public int value;
        public bool hidden = false;
        public bool small = false;

        public Number(PointF pos, Brush brush, string text, int value=0) {
            startPos = pos;
            this.text = text;
            this.brush = brush;
            this.value = value;
        }

        public virtual void draw(Graphics g) {
            if (!hidden) {
                if (!small)
                    g.DrawString(text, Drawer.font, brush, getPos());
                else
                    g.DrawString(text, Drawer.smallFont, brush, getPos());
            }
        }

        protected virtual PointF getPos() {
            return new PointF(Drawer.rect.Width * startPos.X - text.Length / 2 * Drawer.font.Size, Drawer.rect.Height * startPos.Y);
        }

        public static double distance(Number a, Number b) {
            PointF pos1 = a.getPos();
            PointF pos2 = b.getPos();
            return Math.Sqrt(Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2));
        }
    }
}
