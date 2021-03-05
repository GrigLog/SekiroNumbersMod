using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SekiroNumbersMod.Scripts {
    class Number {
        public PointF startPos;
        public Color color;
        public Font customFont = null;
        public string text;
        public int value;
        public bool hidden = false;

        public Number(PointF pos, Color color, string text="", int value=0) {
            startPos = pos;
            this.text = text;
            this.color = color;
            this.value = value;
        }

        public virtual void draw(Graphics g) {
            if (!hidden) {
                g.DrawString(text, getFont(), new SolidBrush(color), getPos());
            }
        }

        protected Font getFont() {
            return customFont == null ? Drawer.font : customFont;
        }

        protected virtual PointF getPos() {
            return new PointF(Drawer.rect.Width * startPos.X - TextRenderer.MeasureText(text, getFont()).Width / 2, Drawer.rect.Height * startPos.Y);
        }

        public static double distance(Number a, Number b) {
            PointF pos1 = a.getPos();
            PointF pos2 = b.getPos();
            return Math.Sqrt(Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2));
        }
    }
}
