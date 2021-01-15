using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SekiroNumbersMod.Scripts {
    class Number {
        protected PointF startPos;
        protected Brush brush;
        public int value;
        public Number(PointF pos, Brush brush, int value) {
            startPos = pos;
            this.value = value;
            this.brush = brush;
        }

        public virtual void draw(Graphics g) {
            g.DrawString(value.ToString(), Drawer.font, brush, getPos());
        }

        protected virtual PointF getPos() {
            return new PointF(Drawer.rect.Width * startPos.X - value.ToString().Length / 2 * Drawer.font.Size, Drawer.rect.Height * startPos.Y);
        }
    }
}
