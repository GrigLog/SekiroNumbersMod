using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SekiroNumbersMod {
    class Program {
        public static Process process;
        public static IntPtr window;
        static void Main(string[] args) {
            //Thread.Sleep(20000);
            Console.WriteLine("Searching for sekiro process...");
            while (Process.GetProcessesByName("sekiro").Length == 0) {
                Thread.Sleep(100);
            }
            Console.WriteLine("Process found.");
            process = Process.GetProcessesByName("sekiro")[0];
            window = process.MainWindowHandle;

            Console.WriteLine("Starting an overlay.");

            Application.Run(new Overlay());
        }
    }
}
