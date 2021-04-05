using SekiroNumbersMod.Scripts;
using SekiroNumbersMod.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using static System.Math;

namespace SekiroNumbersMod {
    class Drawer {
        public static Font font, smallFont, bigFont;
        static FontFamily fontFamily;
        public static Rectangle rect;


        int hp, post, maxHp, maxPost;
        int lastHp = -1;
        int lastPost = -1;
        Entity lockedEntity;
        static V3 playerCors;
        static V3 camCors;

        Color postC = Color.Gold;
        Color hpC = Color.Crimson;
        Color hpHealC = Color.LightGreen;
        Color hpDamC = Color.Red;
        Color postHealC = Color.Aquamarine;
        Color postDamC = Color.Orange;
        static Color poisonDamC = Color.Green;
        static Color fireDamC = Color.FromArgb(255, 0xf7, 0x5e, 0x25);


        Number health = new Number(new PointF(0.25f, 0.92f), Color.Crimson);
        Number posture = new Number(new PointF(0.5f, 0.92f), Color.Gold);
        static PointF hpPos = new PointF(0.3f, 0.85f);
        static PointF postPos = new PointF(0.5f, 0.85f);
        Number lockedHealth = new Number(new PointF(0.25f, 0.05f), Color.Crimson);
        Number lockedPosture = new Number(new PointF(0.5f, 0.05f), Color.Gold);
        Number lockedPoison = new Number(new PointF(0.75f, 0.05f), poisonDamC);
        Number lockedFire = new Number(new PointF(0.75f, 0.08f), fireDamC);


       
        List<Entity> entities = new List<Entity>();
        List<Entity> lastEntities = new List<Entity>();

        public static List<FloatingNumber> numbers = new List<FloatingNumber>();
        Stopwatch lastHitHp = new Stopwatch();
        Stopwatch lastHitPost = new Stopwatch();

        Stopwatch diagn = new Stopwatch();
        Stopwatch statUpdate = new Stopwatch();
        int time, count;

        public Drawer() {
            string workingDir = Environment.CurrentDirectory;
            PrivateFontCollection collection = new PrivateFontCollection();
            Console.WriteLine(Application.ExecutablePath);
            if (workingDir.EndsWith("Sekiro")) {
                collection.AddFontFile(workingDir + "\\NumbersMod\\Athelas-Regular.ttf");
            } else {
                string projDir = Directory.GetParent(workingDir).Parent.Parent.FullName;
                collection.AddFontFile(projDir + "\\Resources\\Athelas-Regular.ttf");
            }
            fontFamily = new FontFamily("Athelas", collection);
            int fontsize = (int)(rect.Height * 20f / 720f);
            font = new Font(fontFamily, fontsize, FontStyle.Bold);
            smallFont = new Font(fontFamily, fontsize - 2);
            bigFont = new Font(fontFamily, fontsize + 3);

            lockedHealth.customFont = smallFont;
            lockedPosture.customFont = smallFont;
            lockedPoison.customFont = smallFont;
            lockedFire.customFont = smallFont;

            diagn.Start();
            lastHitHp.Start();
            lastHitPost.Start();
            statUpdate.Start();
        }

        public void updateData() {
            //diagn.Restart();
            updatePlayerData();
            updateEnemyData();
            //Console.WriteLine(diagn.ElapsedMilliseconds);
        }

        public void draw(Graphics g) {
            drawStatic(g);
            drawFloating(g);
            
        }

        public static PointF toScreenCors(V3 cors) {
            V3 camRelative1 = camCors;
            camRelative1.y = camRelative1.y * 1.95f;
            V3 camRelative = Matrix.rotateY(camRelative1, -55.15 / 57.2956) * 2;

            V3 camGlobal = playerCors + camRelative;
            V3 corsForCamera = cors - camGlobal;
            V3 zaxis = -camRelative.normalized();
            V3 xaxis = V3.cross(new V3(0f, 1f, 0f), zaxis).normalize();
            V3 yaxis = V3.cross(zaxis, xaxis).normalize();

            V3 viewCors = new V3(corsForCamera * xaxis, corsForCamera * yaxis, corsForCamera * zaxis);
            if (viewCors.z < 1 || viewCors.z > 1000) {
                return new Point(-100, -100);
            }
            double near = 1;
            double far = 1000;
            double fovx = 1.347; //in radians?...
            double fovy = 1;
            V3 projected = new V3(viewCors.x / Tan(fovx / 2) / viewCors.z, viewCors.y / Tan(fovy / 2) / viewCors.z, ((far + near) + 2 * near * far / -viewCors.z) / (near - far));
            float x = (projected.x + 1) / 2f;
            float y = (-projected.y + 1) / 2f;
            return new PointF(x, y);

        }

        void updatePlayerData() {
            playerCors = DataReader.coords();
            camCors = DataReader.cameraCoords();
            hp = DataReader.getHealth();
            post = DataReader.getPosture();
            maxHp = DataReader.getMaxHealth();
            maxPost = DataReader.getMaxPosture();

            int dHp = hp - lastHp;
            int dPost = post - lastPost;
            if (dHp != 0 && lastHp != -1) {
                if (dHp > 0) {
                    numbers.Add(new FloatingNumber(hpPos, hpHealC, Config.formatSelfHp(dHp)));
                }
                else if (dHp < 0) {
                    numbers.Add(new FloatingNumber(hpPos, hpDamC, Config.formatSelfHp(-dHp)));
                    lastHitHp.Restart();
                    health.hidden = false;
                }
            }
            if (dPost != 0 && lastPost != -1) {
                if (dPost > 30 && dPost != maxPost) {
                    numbers.Add(new FloatingNumber(postPos, postHealC, Config.formatSelfPost(dPost)));
                }
                else if (dPost < 0) {
                    numbers.Add(new FloatingNumber(postPos, postDamC, Config.formatSelfPost(-dPost)));
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
            lockedEntity = Entity.None;
            V3 lockCors = DataReader.lockCoords();
            bool locked = !lockCors.isZero();

            entities = DataReader.entityList(false);
            if (entities.Count == lastEntities.Count) { //enemies are same, check hp changes
                for (int i = 0; i < entities.Count; i++) {
                    Entity entity = entities[i];
                    if (locked) {
                        if (V3.distance(entity.cors, lockCors) < V3.distance(lockedEntity.cors, lockCors)) {
                            lockedEntity = entity;
                        }
                    }
                    if (entity.cors.isZero() || (entity.d.maxHp == maxHp && entity.d.maxPost == maxPost) || V3.distance(entity.cors, playerCors) > 35)
                        continue;

                    int dHp = entity.d.hp - lastEntities[i].d.hp;
                    int dPost = entity.d.post - lastEntities[i].d.post;
                    int dPsn = entity.res.poison - lastEntities[i].res.poison;
                    int dFire = entity.res.fire - lastEntities[i].res.fire;
                    if (Math.Abs(dPost) > 1000000 || Math.Abs(dHp) > 1000000)
                        continue;


                    //TODO: dont spawn right in the center of the screen, it causes micro stagger!
                    PointF screenCors = toScreenCors(entity.cors);
                    if (dHp < 0) {
                        addNumber(new PointF(screenCors.X - 0.02f, screenCors.Y - 0.1f), hpDamC, -dHp, entity);
                        //Console.WriteLine(entity.address);
                    } else if (dHp > 0 && dHp != entity.d.maxHp) {
                        addNumber(new PointF(screenCors.X - 0.02f, screenCors.Y - 0.1f), hpHealC, dHp, entity);
                    }
                    if (dPost < 0) {
                        addNumber(new PointF(screenCors.X + 0.02f, screenCors.Y - 0.1f), postDamC, -dPost, entity);
                    } else if (dPost > 30 && dPost > entity.d.maxPost / 8 && dPost != entity.d.maxPost) {
                        addNumber(new PointF(screenCors.X + 0.02f, screenCors.Y - 0.1f), postHealC, dPost, entity);
                    }
                    if (dPsn < 0) {
                        addNumber(new PointF(screenCors.X, screenCors.Y + 0.01f), poisonDamC, -dPsn, entity);
                    }
                    if (dFire < 0) {
                        addNumber(new PointF(screenCors.X, screenCors.Y + 0.01f), fireDamC, -dFire, entity);
                    }
                }
            }

            if (locked) {
                lockedHealth.hidden = false;
                lockedPosture.hidden = false;
                lockedPoison.hidden = false;
                lockedFire.hidden = false;
                lockedHealth.text = Config.formatLockedHp(lockedEntity.d.hp, lockedEntity.d.maxHp);
                lockedPosture.text = Config.formatLockedPost(lockedEntity.d.post, lockedEntity.d.maxPost);
                lockedPoison.text = Config.formatLockedPoison(lockedEntity.res.poison, lockedEntity.res.maxPoison);
                lockedFire.text = Config.formatLockedPoison(lockedEntity.res.fire, lockedEntity.res.maxFire);
            } else {
                lockedHealth.hidden = true;
                lockedPosture.hidden = true;
                lockedPoison.hidden = true;
                lockedFire.hidden = true;
            }

            lastEntities.Clear();
            foreach (var e in entities)
                lastEntities.Add(e);
        }

        void addNumber(PointF pos, Color color, int value, Entity entity, bool combo = false) {
            FloatingNumber n;
            if (color == hpHealC || color == hpDamC) {
                n = new FloatingNumber(pos, color, Config.formatHpDam(value)) { value = value, entity = entity };
            } else if (color == postHealC || color == postDamC) {
                n = new FloatingNumber(pos, color, Config.formatPostDam(value)) { value = value, entity = entity };
            } else if (color == poisonDamC) {
                n = new FloatingNumber(pos, color, Config.formatPoisonDam(value)) { value = value, entity = entity };
            } else {
                n = new FloatingNumber(pos, color, Config.formatFireDam(value)) { value = value, entity = entity };
            }
            if (combo)  //TODO wtf it should be separate for each number
                n.big = 5;
            foreach (var e in numbers.ToArray()) {
                if (e.color == n.color && e.counter <= Config.comboTime && e.entity == n.entity) {  //merge two close numbers
                    Console.WriteLine(numbers.Count);
                    numbers.Remove(e);
                    Console.WriteLine(numbers.Count);
                    addNumber(pos, color, value + e.value, entity, false);
                    return;
                }
                    
            }
            numbers.Add(n);
        }

        void drawStatic(Graphics g) {
            health.draw(g);
            posture.draw(g);
            lockedHealth.draw(g);
            lockedPosture.draw(g);
            lockedPoison.draw(g);
            lockedFire.draw(g);
        }

        void drawFloating(Graphics g) {
            foreach (var n in numbers.ToArray()) {
                n.draw(g);
            }
        }
    }
}
