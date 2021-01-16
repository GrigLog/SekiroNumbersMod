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
        public string value;
        public bool hidden = false;
        public bool small = false;

        public Number(PointF pos, Brush brush, string value) {
            startPos = pos;
            this.value = value;
            this.brush = brush;
        }

        public virtual void draw(Graphics g) {
            if (!hidden) {
                if (!small)
                    g.DrawString(value, Drawer.font, brush, getPos());
                else
                    g.DrawString(value, Drawer.smallFont, brush, getPos());
            }
        }

        protected virtual PointF getPos() {
            return new PointF(Drawer.rect.Width * startPos.X - value.Length / 2 * Drawer.font.Size, Drawer.rect.Height * startPos.Y);
        }
    }
}
