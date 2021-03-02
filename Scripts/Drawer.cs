using SekiroNumbersMod.Scripts;
using SekiroNumbersMod.Scripts.Data;
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
        public static Font font, smallFont, bigFont;
        static FontFamily fontFamily;
        public static Rectangle rect;


        int hp, post, maxHp, maxPost;
        int lastHp = -1;
        int lastPost = -1;
        V3 cors;

        Number health = new Number(new PointF(0.25f, 0.92f), Color.Crimson, "");
        Number posture = new Number(new PointF(0.5f, 0.92f), Color.Gold, "");
        static PointF hpPos = new PointF(0.3f, 0.85f);
        static PointF postPos = new PointF(0.5f, 0.85f);

        Color postB = Color.Gold;
        Color hpB = Color.Crimson;
        Color hpHealB = Color.LightGreen;
        Color hpDamB = Color.Red;
        Color postHealB = Color.Aquamarine;
        Color postDamB = Color.Orange;

        List<Entity> entities = new List<Entity>();
        List<Entity> lastEntities = new List<Entity>();

        public static List<FloatingNumber> numbers = new List<FloatingNumber>();
        Stopwatch lastHitHp = new Stopwatch();
        Stopwatch lastHitPost = new Stopwatch();

        Stopwatch diagn = new Stopwatch();
        int time, count;

        public Drawer() {
            string workingDir = Environment.CurrentDirectory;
            string projDir = Directory.GetParent(workingDir).Parent.Parent.FullName;
            PrivateFontCollection collection = new PrivateFontCollection();
            collection.AddFontFile(projDir + "\\Resources\\Athelas-Regular.ttf");
            fontFamily = new FontFamily("Athelas", collection);
            font = new Font(fontFamily, 20, FontStyle.Bold);
            smallFont = new Font(fontFamily, 18);
            bigFont = new Font(fontFamily, 23);

            diagn.Start();
            lastHitHp.Start();
            lastHitPost.Start();
        }

        public void updateData() {
            updatePlayerData();
            updateEnemyData();
        }

        public void draw(Graphics g) {
            V3 camCors = DataReader.cameraCoords();
            if (true || diagn.ElapsedMilliseconds > 1000) {
                diagn.Restart();
                if (entities.Count > 2) {
                    V3 mobCors = entities[2].cors;
                    Point pos = toScreenCors2(mobCors);
                    g.DrawString("0", font, Brushes.White, pos);
                }
            }
            drawStatic(g);
            drawFloating(g);
            
            
        }

        public Point toScreenCors2(V3 cors) {
            V3 camRelative1 = DataReader.cameraCoords();
            V3 camRelative = Matrix.rotateY(camRelative1, -55.8 / 57.2956);
            V3 playerCors = DataReader.coords();

            V3 cam = playerCors + camRelative;
            V3 corsTransCam = cors - cam;
            V3 zaxis = -camRelative.normalize();
            V3 xaxis = V3.cross(new V3(0f, 1f, 0f), zaxis).normalize();
            V3 yaxis = V3.cross(xaxis, zaxis).normalize();

            V3 viewCors = new V3(corsTransCam * xaxis, corsTransCam * yaxis, corsTransCam * zaxis);
            if (viewCors.z < 1 || viewCors.z > 1000) {
                return new Point(-100, -100);
            }
            V3 projected = Matrix.getProjected(viewCors);
            int x = (int)((projected.x + 1) / 2f * rect.Width);
            int y = (int)((projected.y + 1) / 2f * rect.Height);
            Point pos = new Point(x, y);

            Console.WriteLine("Axes: " + xaxis + " " + yaxis + " " + zaxis);
            Console.WriteLine("Mob relative: " + (cors - playerCors));
            Console.WriteLine("View: " + viewCors);
            Console.WriteLine("Projected: " + projected);
            Console.WriteLine("Screen: " + pos);
            return pos;

        }

        void updatePlayerData() {
            cors = DataReader.coords();
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
                if (dPost > 30 && dPost != maxPost) {
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

            health.text = Config.formatSelfHp(hp);
            posture.text = Config.formatSelfPost(post);

            lastHp = hp;
            lastPost = post;
        }

        void updateEnemyData() {
            entities = DataReader.entityList(false);
            if (entities.Count == lastEntities.Count) { //enemies are same, check hp changes
                for (int i = 0; i < entities.Count; i++) {
                    if (entities[i].cors.isZero() || (entities[i].maxHp == maxHp && entities[i].maxPost == maxPost) || V3.distance(entities[i].cors, cors) > 35)
                        continue;

                    int dHp = entities[i].hp - lastEntities[i].hp;
                    int dPost = entities[i].post - lastEntities[i].post;
                    if (Math.Abs(dPost) > 1000000 || Math.Abs(dHp) > 1000000)
                        continue;
                    if (dHp < 0) {
                        //Point pos = toScreenCors(entities[i].cors);
                        //PointF rel = new PointF(pos.X / (float)rect.Width, pos.Y / (float)rect.Height);
                        addNumber(new PointF(0.48f, 0.40f), hpDamB, -dHp);  //dont spawn right in the center of the screen, it causes micro stagger!
                    }
                    else if (dHp > 0 && dHp != entities[i].maxHp) {
                        addNumber(new PointF(0.48f, 0.40f), hpHealB, dHp);
                    }
                    if (dPost < 0) {
                        addNumber(new PointF(0.52f, 0.40f), postDamB, -dPost);
                    }
                    else if (dPost > 30 && dPost > entities[i].maxPost / 8 && dPost != entities[i].maxPost) {
                        Console.WriteLine(dPost + " "  + entities[i].maxPost);
                        addNumber(new PointF(0.52f, 0.40f), postHealB, dPost);
                    }
                }
            }

            lastEntities.Clear();
            foreach (var e in entities)
                lastEntities.Add(e);
        }

        void addNumber(PointF pos, Color color, int value, bool combo = false) {
            FloatingNumber n;
            if (color == hpHealB || color == hpDamB) {
                n = new FloatingNumber(pos, color, Config.formatHpDam(value), value);
            } else {
                n = new FloatingNumber(pos, color, Config.formatPostDam(value), value);
            }
            if (combo)
                n.big = 5;
            foreach (var e in numbers.ToArray()) {
                if (e.color == n.color && e.counter <= 20 && Number.distance(e, n) <= 50) {  //merge two close numbers
                    numbers.Remove(e);
                    addNumber(pos, color, value + e.value, true);
                    return;
                }
                    
            }
            numbers.Add(n);
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
