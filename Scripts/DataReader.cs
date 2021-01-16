using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SekiroNumbersMod {
    struct Entity {
        public Entity(int hp, int post, V3 cors) {
            this.hp = hp;
            this.post = post;
            this.cors = cors;
        }
        public int hp, post;
        public V3 cors;
    }
    public struct V3 {
        public float x, y, z;
        public V3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public bool isZero() {
            return x == 0 && y == 0 && z == 0;
        }
        public static double distance(V3 a, V3 b) {
            return Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2) + Math.Pow(a.z - b.z, 2));
        }
        public override string ToString() {
            return "{" + x + ", " + y + ", " + z + "}";
        }
    }
    class DataReader {
        static Process process;
        static IntPtr processPtr;
        static IntPtr modulePtr;

        static Int32 playerOffset = 0x3D5AAC0;
        static IntPtr mapAddress = new IntPtr(0x143D7A1E0);
        static int oneMoreOffset = 0x3D94970;
        static Dictionary<string, int[]> map = new Dictionary<string, int[]>() {
            {"money",  new int[] {0x8, 0x7c}},
            {"health", new int[] {0x8, 0x18}},
            {"max health", new int[] {0x8, 0x1c}},
            {"posture", new int[] {0x8, 0x34}},
            {"max posture", new int[]{0x8, 0x38}},
            {"attack power", new int[]{0x8, 0x48}},
            {"areas", new int[] {0x518}},
            {"entity hp", new int[] {0x1ff8, 0x18, 0x130}},
            {"entity post", new int[] {0x1ff8, 0x18, 0x148}},
            {"entity x", new int[] {0x98, 0xc}},
            {"entity y", new int[] {0x98, 0x1c}},
            {"entity z", new int[] {0x98, 0x2c}},
            {"player x", new int[]{0x10, 0x5e8, 0xd8, 0x40}},
            {"player y", new int[]{0x10, 0x5e8, 0xd8, 0x44}},
            {"player z", new int[]{0x10, 0x5e8, 0xd8, 0x48}},
        };

        public static int baseHpDamage() {
            int ap = getInt("attack power", modulePtr + playerOffset);
            if (ap <= 14)
                return (ap + 3) * 20;
            else if (ap <= 27)
                return 340 + (ap - 14) * 8;
            else if (ap <= 51)
                return 443 + (ap - 27) * 4;
            else
                return (int)(540 + (ap - 51) * 0.8);
        }

        public static int basePostDamage() {
            int ap = getInt("attack power", modulePtr + playerOffset);
            if (ap <= 14)
                return (int)(30 + (ap - 1) * 7.5);
            else if (ap <= 27)
                return 127 + (ap - 14) * 3;
            else if (ap <= 51)
                return (int)(166 + (ap - 27) * 1.5);
            else
                return (int)(202 + (ap - 51) * 0.25);
        }

        static DataReader(){
            process = Process.GetProcessesByName("Sekiro")[0];
            processPtr = OpenProcess(0x001F0FFF, false, process.Id);
            modulePtr = getModuleAddress(process, "sekiro.exe");
            Console.WriteLine("Module pointer: " + modulePtr);
        }

        public static V3 coords() {
            return new V3(getFloat("player x", modulePtr + oneMoreOffset),
                getFloat("player y", modulePtr + oneMoreOffset),
                getFloat("player z", modulePtr + oneMoreOffset));
        }

        public static int getMoney() {
            return getInt("money", modulePtr + playerOffset);
        }
        public static int getHealth() {
            return getInt("health", modulePtr + playerOffset);
        }
        public static int getMaxHealth() {
            return getInt("max health", modulePtr + playerOffset);
        }
        public static int getPosture() {
            return getInt("posture", modulePtr + playerOffset);
        }
        public static int getMaxPosture() {
            return getInt("max posture", modulePtr + playerOffset);
        }

        public static List<Entity> entityList(bool printCoords = false) {
            var res = new List<Entity>();
            IntPtr mapPtr = read(mapAddress);
            mapPtr += 0x518;
            for (int i = 0; i <= 18; i++) {
                IntPtr areaPtr = read(mapPtr + i * 8);
                if (areaPtr != IntPtr.Zero) {
                    IntPtr entityAddr = read(areaPtr + 0x8);  //first entity in a list
                    while (true) {
                        if (read(entityAddr) == IntPtr.Zero 
                            && read(entityAddr + 8) != IntPtr.Zero 
                            && read(entityAddr + 16) != IntPtr.Zero
                            && read(entityAddr + 24) != IntPtr.Zero 
                            && read(entityAddr + 32) != IntPtr.Zero) { //attempt to understand that entity list is over
                            break;
                        }
                        if (read(entityAddr) != IntPtr.Zero) {
                            Entity e = new Entity(getInt("entity hp", entityAddr),
                                               getInt("entity post", entityAddr),
                                               new V3(getFloat("entity x", entityAddr),
                                                      getFloat("entity y", entityAddr),
                                                      getFloat("entity z", entityAddr)));
                            res.Add(e);
                            if (printCoords)
                                Console.WriteLine(e.cors);
                        }
                        entityAddr += 0x38;
                    }
                }
            }
            return res;
        }

        public static int getInt(string key, IntPtr ptr) {
            return BitConverter.ToInt32(findData(ptr, map[key]), 0);
        }

        public static float getFloat(string key, IntPtr ptr) {
            return BitConverter.ToSingle(findData(ptr, map[key]), 0);
        }

        static byte[] findData(IntPtr pointer, int[] offsets) {
            foreach (int i in offsets) {
                pointer = read(pointer) + i;
            }
            byte[] buffer = new byte[8];
            ReadProcessMemory(processPtr, pointer, buffer, buffer.Length, 0);
            return buffer;
        }

        static IntPtr read(IntPtr ptr) {
            byte[] buffer = new byte[8];
            ReadProcessMemory(processPtr, ptr, buffer, buffer.Length, 0);
            return new IntPtr(BitConverter.ToInt64(buffer, 0));
        }

        static IntPtr getModuleAddress(Process proc, string modName) {
            foreach (ProcessModule m in proc.Modules) {
                if (m.ModuleName == modName)
                    return m.BaseAddress;
            }
            return new IntPtr();
        }


        [DllImport("kernel32.dll")]
        static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBase, byte[] lpBuffer, int nSize, int lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr process, IntPtr start, byte[] buffer, int size, int nBytesWritten);
        [DllImport("kernel32.dll")]
        static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr adress, int len, int newProtect, int oldProtectRef);
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(int access, bool inheritSomeShit, int id);
    }
}
