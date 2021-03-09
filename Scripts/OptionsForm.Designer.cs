
namespace SekiroNumbersMod.Scripts {
    partial class OptionsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.playerList = new System.Windows.Forms.ComboBox();
            this.lockedList = new System.Windows.Forms.ComboBox();
            this.damageList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // playerList
            // 
            this.playerList.AutoCompleteCustomSource.AddRange(new string[] {
            "relative",
            "absolute",
            "off"});
            this.playerList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playerList.FormattingEnabled = true;
            this.playerList.Items.AddRange(new object[] {
            "relative",
            "absolute",
            "off"});
            this.playerList.Location = new System.Drawing.Point(206, 38);
            this.playerList.Name = "playerList";
            this.playerList.Size = new System.Drawing.Size(121, 21);
            this.playerList.TabIndex = 0;
            this.playerList.SelectedIndexChanged += new System.EventHandler(this.playerList_SelectedIndexChanged);
            // 
            // lockedList
            // 
            this.lockedList.AutoCompleteCustomSource.AddRange(new string[] {
            "relative",
            "absolute",
            "off"});
            this.lockedList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lockedList.FormattingEnabled = true;
            this.lockedList.Items.AddRange(new object[] {
            "relative",
            "absolute",
            "off"});
            this.lockedList.Location = new System.Drawing.Point(206, 86);
            this.lockedList.Name = "lockedList";
            this.lockedList.Size = new System.Drawing.Size(121, 21);
            this.lockedList.TabIndex = 1;
            this.lockedList.SelectedIndexChanged += new System.EventHandler(this.lockedList_SelectedIndexChanged);
            // 
            // damageList
            // 
            this.damageList.AutoCompleteCustomSource.AddRange(new string[] {
            "relative",
            "absolute",
            "off"});
            this.damageList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.damageList.FormattingEnabled = true;
            this.damageList.Items.AddRange(new object[] {
            "relative",
            "absolute",
            "off"});
            this.damageList.Location = new System.Drawing.Point(206, 135);
            this.damageList.Name = "damageList";
            this.damageList.Size = new System.Drawing.Size(121, 21);
            this.damageList.TabIndex = 2;
            this.damageList.SelectedIndexChanged += new System.EventHandler(this.damageList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(21, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Player stats";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(21, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Locked enemy stats";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(21, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Damage numbers";
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 460);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.damageList);
            this.Controls.Add(this.lockedList);
            this.Controls.Add(this.playerList);
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox playerList;
        private System.Windows.Forms.ComboBox lockedList;
        private System.Windows.Forms.ComboBox damageList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}