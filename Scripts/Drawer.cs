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
        public static Font font, smallFont;
        static FontFamily fontFamily;
        public static Rectangle rect;


        int hp, post, maxHp, maxPost;
        int lastHp = -1;
        int lastPost = -1;

        Number health = new Number(new PointF(0.25f, 0.92f), Brushes.Crimson, "");
        Number posture = new Number(new PointF(0.5f, 0.92f), Brushes.Gold, "");
        static PointF hpPos = new PointF(0.3f, 0.85f);
        static PointF postPos = new PointF(0.5f, 0.85f);

        Brush postB = Brushes.Gold;
        Brush hpB = Brushes.Crimson;
        Brush hpHealB = Brushes.LightGreen;
        Brush hpDamB = Brushes.Red;
        Brush postHealB = Brushes.Aquamarine;
        Brush postDamB = Brushes.Orange;

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
            smallFont = new Font(fontFamily, 18);

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

            int dHp = hp - lastHp;
            int dPost = post - lastPost;
            if (dHp != 0 && lastHp != -1) {
                if (dHp > 0) {
                    numbers.Add(new FloatingNumber(hpPos, hpHealB, Config.formatSelfHp(dHp)));
                }
                else if (dHp < 0) {
                    numbers.Add(new FloatingNumber(hpPos, hpDamB, Config.formatSelfHp(-dHp)));
                    lastHitHp.Restart();
                    health.hidden = false;
                }
            }
            if (dPost != 0 && lastPost != -1) {
                if (dPost > 30) {
                    numbers.Add(new FloatingNumber(postPos, postHealB, Config.formatSelfPost(dPost)));
                }
                else if (dPost < 0) {
                    numbers.Add(new FloatingNumber(postPos, postDamB, Config.formatSelfPost(-dPost)));
                    lastHitPost.Restart();
                    posture.hidden = false;
                }
            }

            if (hp == maxHp && !health.hidden && lastHitHp.ElapsedMilliseconds >= 5000)
                health.hidden = true;
            if (post == maxPost && !posture.hidden && lastHitPost.ElapsedMilliseconds >= 5000)
                posture.hidden = true;

            health.value = Config.formatSelfHp(hp);
            posture.value = Config.formatSelfPost(post);

            lastHp = hp;
            lastPost = post;
        }

        void updateEnemyData() {
            entities = DataReader.entityList();
            if (entities.Count == lastEntities.Count) { //enemies are same, check hp changes
                for (int i = 0; i < entities.Count; i++) {
                    int dHp = entities[i].hp - lastEntities[i].hp;
                    int dPost = entities[i].post - lastEntities[i].post;
                    if (dHp < 0) {
                        numbers.Add(new FloatingNumber(new PointF(0.48f, 0.5f), hpDamB, Config.formatHpDam(-dHp)));
                    }
                    else if (dHp > 0) {
                        numbers.Add(new FloatingNumber(new PointF(0.48f, 0.5f), hpHealB, Config.formatHpDam(dHp)));
                    }
                    if (dPost < 0) {
                        numbers.Add(new FloatingNumber(new PointF(0.52f, 0.5f), postDamB, Config.formatPostDam(-dPost)));
                    }
                    else if (dPost > 30) {
                        numbers.Add(new FloatingNumber(new PointF(0.52f, 0.5f), postHealB, Config.formatPostDam(dPost)));
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
