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

        static Int32 playerOffset = 0x3D5AAC0;
        static Dictionary<string, int[]> map = new Dictionary<string, int[]>() {
            {"player", new int[] {0x3D5AAC0}},
            {"money",  new int[] {0x8, 0x7c}},
            {"health", new int[] {0x8, 0x18}},
            {"maxHealth", new int[] {0x8, 0x1c}},
            {"posture", new int[] {0x8, 0x34}},
            {"maxPosture", new int[]{0x8, 0x38}}
        };

        static DataReader(){
            process = Process.GetProcessesByName("Sekiro")[0];
            processPtr = OpenProcess(0x001F0FFF, false, process.Id);
            modulePtr = getModuleAddress(process, "sekiro.exe");
        }

        public static int getMoney() {
            return getInt("money");
        }
        public static int getHealth() {
            return getInt("health");
        }
        public static int getMaxHealth() {
            return getInt("maxHealth");
        }
        public static int getPosture() {
            return getInt("posture");
        }
        public static int getMaxPosture() {
            return getInt("maxPosture");
        }

        /*public static void healFull() {
            int h = getMaxHealth();
            byte[] mx = BitConverter.GetBytes(h);
           
            Console.WriteLine(writeData(modulePtr + playerOffset, map["health"], mx));
            Console.WriteLine(Kernel32Methods.GetLastError());
        }*/



        public static int getInt(string key) {
            return BitConverter.ToInt32(findData(modulePtr + playerOffset, map[key]), 0);
        }


        static byte[] findData(IntPtr pointer, int[] offsets) {
            byte[] buffer = new byte[IntPtr.Size];
            foreach (int i in offsets) {
                ReadProcessMemory(processPtr, pointer, buffer, buffer.Length, 0);
                if (IntPtr.Size == 4)
                    pointer = new IntPtr(BitConverter.ToInt32(buffer, 0)) + i;
                else
                    pointer = new IntPtr(BitConverter.ToInt64(buffer, 0)) + i;
            }
            ReadProcessMemory(processPtr, pointer, buffer, buffer.Length, 0);
            return buffer;
        }

        /*static bool writeData(IntPtr pointer, int[] offsets, byte[] data) {
            byte[] buffer = new byte[IntPtr.Size];
            foreach (int i in offsets) {
                ReadProcessMemory(processPtr, pointer, buffer, buffer.Length, 0);
                if (IntPtr.Size == 4)
                    pointer = new IntPtr(BitConverter.ToInt32(buffer, 0)) + i;
                else
                    pointer = new IntPtr(BitConverter.ToInt64(buffer, 0)) + i;
            }

            //Console.WriteLine(VirtualProtectEx(processPtr, pointer, data.Length, 0x04, 0));
            //Console.WriteLine(Kernel32Methods.GetLastError());
            return WriteProcessMemory(processPtr, pointer, data, data.Length, 0);
        }*/

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
