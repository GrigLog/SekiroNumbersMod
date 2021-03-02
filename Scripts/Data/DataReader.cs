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
    class DataReader {
        static Process process;
        static IntPtr processPtr;
        static IntPtr modulePtr;

        //declared here to optimize calculations
        static byte[] buffer = new byte[8];
        static IntPtr mapPtr, areaPtr, entityDecorPtr, hpPtr, corsPtr;

        static int playerOffset = 0x3D5AAC0;
        static IntPtr cameraSmthAddress = new IntPtr(0x143D91848);
        static IntPtr mapAddress = new IntPtr(0x143D7A1E0);
        static int oneMoreOffset = 0x3D94970;
        static Dictionary<string, int[]> map = new Dictionary<string, int[]>() {
            {"", new int[]{} },
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
            {"camera x", new int[]{0x18, 0x30 }},
            {"camera y", new int[]{0x18, 0x34 }},
            {"camera z", new int[]{0x18, 0x38 }}
        };

        static DataReader(){
            process = Process.GetProcessesByName("Sekiro")[0];
            processPtr = OpenProcess(0x001F0FFF, false, process.Id);
            modulePtr = getModuleAddress(process, "sekiro.exe");
            Console.WriteLine("Module pointer: " + modulePtr);
        }

        public static V3 coords() {
            return new V3(getFloat(modulePtr + oneMoreOffset, "player x"),
                getFloat(modulePtr + oneMoreOffset, "player y"),
                getFloat(modulePtr + oneMoreOffset, "player z"));
        }

        public static V3 cameraCoords() {
            return new V3(getFloat(cameraSmthAddress, "camera x"),
                getFloat(cameraSmthAddress, "camera y"),
                getFloat(cameraSmthAddress, "camera z"));
        }


        public static List<Entity> entityList(bool printCoords = false) {
            var res = new List<Entity>();
            mapPtr = read(mapAddress);
            mapPtr += 0x518;
            for (int i = 0; i <= 18; i++) {
                areaPtr = read(mapPtr + i * 8);
                if (areaPtr != IntPtr.Zero) {
                    entityDecorPtr = read(areaPtr + 0x8);  //first entity in a list
                    while (true) {
                        IntPtr entityStart = read(entityDecorPtr);
                        if (entityStart == IntPtr.Zero 
                            && read(entityDecorPtr + 8) != IntPtr.Zero 
                            && read(entityDecorPtr + 16) != IntPtr.Zero
                            && read(entityDecorPtr + 24) != IntPtr.Zero 
                            && read(entityDecorPtr + 32) != IntPtr.Zero) { //attempt to understand that entity list is over
                            break;
                        }
                        if (entityStart != IntPtr.Zero) {
                            hpPtr = findDataAddr(entityDecorPtr, map["entity hp"]);
                            corsPtr = findDataAddr(entityDecorPtr, map["entity x"]);
                            byte[] hpData = new byte[0x20];
                            ReadProcessMemory(processPtr, hpPtr, hpData, hpData.Length, 0);
                            int hp = BitConverter.ToInt32(hpData, 0);
                            int mHp = BitConverter.ToInt32(hpData, 4);
                            int post = BitConverter.ToInt32(hpData, 0x18);
                            int mPost = BitConverter.ToInt32(hpData, 0x1c);

                            byte[] corsData = new byte[0x30];
                            ReadProcessMemory(processPtr, corsPtr, corsData, corsData.Length, 0);
                            float x = BitConverter.ToSingle(corsData, 0);
                            float y = BitConverter.ToSingle(corsData, 0x10);
                            float z = BitConverter.ToSingle(corsData, 0x20);
                            Entity e = new Entity(hp, mHp, post, mPost, new V3(x, y, z));
                            res.Add(e);
                            if (printCoords)
                                Console.Write(e.cors);
                        }
                        entityDecorPtr += 0x38;
                    }
                    if (printCoords)
                        Console.WriteLine();
                }
            }
            return res;
        }

        public static int getInt(IntPtr ptr, string key = "") {
            return BitConverter.ToInt32(findData(ptr, map[key]), 0);
        }

        public static float getFloat(IntPtr ptr, string key = "") {
            return BitConverter.ToSingle(findData(ptr, map[key]), 0);
        }

        public static IntPtr findDataAddr(IntPtr pointer, int[] offsets) {
            foreach (int i in offsets) {
                pointer = read(pointer) + i;
            }
            return pointer;
        }

        static byte[] findData(IntPtr pointer, int[] offsets) {

            ReadProcessMemory(processPtr, findDataAddr(pointer, offsets), buffer, buffer.Length, 0);
            return buffer;
        }

        static IntPtr read(IntPtr ptr) {
            
            ReadProcessMemory(processPtr, ptr, buffer, buffer.Length, 0);
            return new IntPtr(BitConverter.ToInt64(buffer, 0));
        }

      

        public static int getMoney() {
            return getInt(modulePtr + playerOffset, "money");
        }
        public static int getHealth() {
            return getInt(modulePtr + playerOffset, "health");
        }
        public static int getMaxHealth() {
            return getInt(modulePtr + playerOffset, "max health");
        }
        public static int getPosture() {
            return getInt(modulePtr + playerOffset, "posture");
        }
        public static int getMaxPosture() {
            return getInt(modulePtr + playerOffset, "max posture");
        }

        static IntPtr getModuleAddress(Process proc, string modName) {
            foreach (ProcessModule m in proc.Modules) {
                if (m.ModuleName == modName)
                    return m.BaseAddress;
            }
            return new IntPtr();
        }

        public static int baseHpDamage() {
            int ap = getInt(modulePtr + playerOffset, "attack power");
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
            int ap = getInt(modulePtr + playerOffset, "attack power");
            if (ap <= 14)
                return (int)(30 + (ap - 1) * 7.5);
            else if (ap <= 27)
                return 127 + (ap - 14) * 3;
            else if (ap <= 51)
                return (int)(166 + (ap - 27) * 1.5);
            else
                return (int)(202 + (ap - 51) * 0.25);
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
