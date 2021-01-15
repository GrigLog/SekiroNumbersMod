using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SekiroNumbersMod {
    public partial class Overlay : Form {
        Rectangle temp;
        Drawer d;
        public Overlay() {
            InitializeComponent();
            
            FormBorderStyle = FormBorderStyle.None;
            AllowTransparency = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            TransparencyKey = Color.Black;
            BackColor = TransparencyKey;

            TopMost = true;
            ShowInTaskbar = false;
            d = new Drawer();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            Refresh();
        }

        private void Overlay_Paint(object sender, PaintEventArgs e) {
            GetWindowRect(Program.window, out temp);
            //Console.WriteLine(temp);
            temp = new Rectangle(temp.X, temp.Y, temp.Width - temp.X, temp.Height - temp.Y);
            Bounds = temp;
            Drawer.rect = temp;
            d.draw(e.Graphics);
            
        }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out Rectangle lpRect);
    }
}
