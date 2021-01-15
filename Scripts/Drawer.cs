using SekiroNumbersMod.Scripts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SekiroNumbersMod {
    class Drawer {
        public static Font font;
        static FontFamily fontFamily;
        public static Rectangle rect;

        int hp, post;

        Number health = new Number(new PointF(0.25f, 0.92f), Brushes.Crimson, 0);
        Number posture = new Number(new PointF(0.5f, 0.92f), Brushes.Gold, 0);

        public Drawer() {
            string workingDir = Environment.CurrentDirectory;
            string projDir = Directory.GetParent(workingDir).Parent.Parent.FullName;
            PrivateFontCollection collection = new PrivateFontCollection();
            collection.AddFontFile(projDir + "\\Resources\\Athelas-Regular.ttf");
            fontFamily = new FontFamily("Athelas", collection);
            font = new Font(fontFamily, 20, FontStyle.Bold);

        }

        public void draw(Graphics g) {
            updateData();
            drawStatic(g);
            drawFloating(g);
        }

        void updateData() {
            hp = DataReader.getHealth();
            post = DataReader.getPosture();

            health.value = hp;
            posture.value = post;
        }

        void drawStatic(Graphics g) {
            health.draw(g);
            posture.draw(g);
        }

        void drawFloating(Graphics g) {

        }
    }
}
