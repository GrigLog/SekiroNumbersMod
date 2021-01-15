using SekiroNumbersMod.Scripts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        int lastHp = -1;
        int lastPost = -1;

        Number health = new Number(new PointF(0.25f, 0.92f), Brushes.Crimson, 0);
        Number posture = new Number(new PointF(0.5f, 0.92f), Brushes.Gold, 0);
        static PointF hpPos = new PointF(0.3f, 0.85f);
        static PointF postPos = new PointF(0.5f, 0.85f);

        public static List<FloatingNumber> numbers = new List<FloatingNumber>();
        Stopwatch fullHpTimer = new Stopwatch();
        Stopwatch fullPostTimer = new Stopwatch();

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

            if (hp != lastHp && lastHp != -1) {
                if (hp > lastHp) {
                    numbers.Add(new FloatingNumber(hpPos, Brushes.LightGreen, hp - lastHp));
                }
                else if (hp < lastHp) {
                    //lastHitHp.Restart();
                    //health.Hidden = false;
                    numbers.Add(new FloatingNumber(hpPos, Brushes.Red, lastHp - hp));
                }
            }
            if (post != lastPost && lastPost != -1) {
                if (post > lastPost && post - lastPost > 30) {
                    numbers.Add(new FloatingNumber(postPos, Brushes.Aquamarine, post - lastPost));
                }
                else if (post < lastPost) {
                    //posture.Hidden = false;
                    numbers.Add(new FloatingNumber(postPos, Brushes.Orange, lastPost - post));
                }
            }

            health.value = hp;
            posture.value = post;

            lastHp = hp;
            lastPost = post;
        }

        void drawStatic(Graphics g) {
            health.draw(g);
            posture.draw(g);
        }

        void drawFloating(Graphics g) {
            foreach (var n in numbers.ToArray()) {
                n.draw(g);
            }
        }
    }
}
