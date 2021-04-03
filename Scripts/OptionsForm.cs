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

        private void OptionsForm_Load(object sender, EventArgs e) {
            Config.updateFromFile();
            playerList.Text = Config.selfVals.ToString();
            lockedList.Text = Config.lockedVals.ToString();
            damageList.Text = Config.damageVals.ToString();
            resistList.Text = Config.status ? "yes" : "no";
        }

        private void commitButton_Click(object sender, EventArgs e) {
            sw = new StreamWriter("NumbersMod\\config.txt");
            sw.WriteLine("Self Stats:" + playerList.Text);
            sw.WriteLine("Locked Stats:" + lockedList.Text);
            sw.WriteLine("Damage Numbers:" + damageList.Text);
            sw.WriteLine("Resistances:" + resistList.Text);
            sw.Close();
            Config.updateFromFile();
        }
    }
}
