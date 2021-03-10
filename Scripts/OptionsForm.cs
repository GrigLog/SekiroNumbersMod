using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SekiroNumbersMod.Scripts {
    public partial class OptionsForm : Form {
        StreamWriter sw;
        public OptionsForm() {
            InitializeComponent();
        }

        protected override void OnFormClosed(FormClosedEventArgs e) {
            base.OnFormClosed(e);
        }

        private void OptionsForm_Load(object sender, EventArgs e) {
            Config.updateFromFile();
            playerList.Text = Config.selfVals.ToString();
            lockedList.Text = Config.lockedVals.ToString();
            damageList.Text = Config.damageVals.ToString();
        }

        private void playerList_SelectedIndexChanged(object sender, EventArgs e) {
            updateFile();
        }

        private void lockedList_SelectedIndexChanged(object sender, EventArgs e) {
            updateFile();
        }

        private void damageList_SelectedIndexChanged(object sender, EventArgs e) {
            updateFile();
        }

        void updateFile() {
            sw = new StreamWriter("NumbersMod\\config.txt");
            sw.WriteLine("SelfStats:" + playerList.SelectedItem);
            sw.WriteLine("LockedStats:" + lockedList.SelectedItem);
            sw.WriteLine("DamageNumbers:" + damageList.SelectedItem);
            sw.Close();
            Config.updateFromFile();
        }
    }
}
