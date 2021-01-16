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
            Console.WriteLine("Waiting for Sekiro to start...");
            while (Process.GetProcessesByName("sekiro").Length == 0) {
                Thread.Sleep(100);
            }
            Console.WriteLine("Process found.");
            process = Process.GetProcessesByName("sekiro")[0];
            window = process.MainWindowHandle;

            Console.WriteLine("Starting an overlay.");

            DataReader.entityList(true);

            Application.Run(new Overlay());
        }
    }
}
