using SekiroNumbersMod.Scripts;
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
        KeyboardHook keyboardHook = new KeyboardHook();
        public Overlay() {
            InitializeComponent();
            
            FormBorderStyle = FormBorderStyle.None;
            AllowTransparency = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            TransparencyKey = Color.Black;
            BackColor = TransparencyKey;

            TopMost = true;
            ShowInTaskbar = false;

            keyboardHook.KeyboardPressed += keyDown;
            d = new Drawer();
        }

        void keyDown(object sender, KeyboardHookEventArgs e) {
            if (e.KeyboardState == KeyboardHook.KeyboardState.KeyDown && e.KeyboardData.VirtualCode == 0x72) { //F3 virtual code
                (new OptionsForm()).ShowDialog();
            }
        }

        private void timer1_Tick(object sender, EventArgs e) {
            d.updateData();
            Refresh();
        }

        private void Overlay_Paint(object sender, PaintEventArgs e) {
            GetWindowRect(Program.window, out temp);
            temp = new Rectangle(temp.X, temp.Y, temp.Width - temp.X, temp.Height - temp.Y);
            Bounds = temp;
            Drawer.rect = temp;
            d.draw(e.Graphics);
            
        }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out Rectangle lpRect);

        private void Overlay_Load(object sender, EventArgs e) {

        }
    }
}
