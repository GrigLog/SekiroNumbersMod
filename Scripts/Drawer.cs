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


        int hp, post, maxHp, maxPost;
        int lastHp = -1;
        int lastPost = -1;

        Number health = new Number(new PointF(0.25f, 0.92f), Brushes.Crimson, 0);
        Number posture = new Number(new PointF(0.5f, 0.92f), Brushes.Gold, 0);
        static PointF hpPos = new PointF(0.3f, 0.85f);
        static PointF postPos = new PointF(0.5f, 0.85f);

        List<Entity> entities = new List<Entity>();
        List<Entity> lastEntities = new List<Entity>();

        public static List<FloatingNumber> numbers = new List<FloatingNumber>();
        Stopwatch lastHitHp = new Stopwatch();
        Stopwatch lastHitPost = new Stopwatch();

        public Drawer() {
            string workingDir = Environment.CurrentDirectory;
            string projDir = Directory.GetParent(workingDir).Parent.Parent.FullName;
            PrivateFontCollection collection = new PrivateFontCollection();
            collection.AddFontFile(projDir + "\\Resources\\Athelas-Regular.ttf");
            fontFamily = new FontFamily("Athelas", collection);
            font = new Font(fontFamily, 20, FontStyle.Bold);

            lastHitHp.Start();
            lastHitPost.Start();
        }

        public void updateData() {
            updatePlayerData();
            updateEnemyData();
        }

        public void draw(Graphics g) {
            drawStatic(g);
            drawFloating(g);
        }

        void updatePlayerData() {
            hp = DataReader.getHealth();
            post = DataReader.getPosture();
            maxHp = DataReader.getMaxHealth();
            maxPost = DataReader.getMaxPosture();

            if (hp != lastHp && lastHp != -1) {
                if (hp > lastHp) {
                    numbers.Add(new FloatingNumber(hpPos, Brushes.LightGreen, hp - lastHp));
                }
                else if (hp < lastHp) {
                    numbers.Add(new FloatingNumber(hpPos, Brushes.Red, lastHp - hp));
                    lastHitHp.Restart();
                    health.hidden = false;
                }
            }
            if (post != lastPost && lastPost != -1) {
                if (post > lastPost && post - lastPost > 30) {
                    numbers.Add(new FloatingNumber(postPos, Brushes.Aquamarine, post - lastPost));
                }
                else if (post < lastPost) {
                    numbers.Add(new FloatingNumber(postPos, Brushes.Orange, lastPost - post));
                    lastHitPost.Restart();
                    posture.hidden = false;
                }
            }

            if (hp == maxHp && !health.hidden && lastHitHp.ElapsedMilliseconds >= 5000)
                health.hidden = true;
            if (post == maxPost && !posture.hidden && lastHitPost.ElapsedMilliseconds >= 5000)
                posture.hidden = true;

            health.value = hp;
            posture.value = post;

            lastHp = hp;
            lastPost = post;
        }

        void updateEnemyData() {
            entities = DataReader.entityList();

            if (entities.Count == lastEntities.Count) { //enemies are same, check hp changes
                for (int i = 0; i < entities.Count; i++) {
                    if (entities[i].hp < lastEntities[i].hp) {
                        numbers.Add(new FloatingNumber(new PointF(0.5f, 0.5f), Brushes.Red, lastEntities[i].hp - entities[i].hp));
                    }
                }
            }

            lastEntities.Clear();
            foreach (var e in entities)
                lastEntities.Add(e);
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
