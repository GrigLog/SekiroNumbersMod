
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
            this.commitButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.resistList = new System.Windows.Forms.ComboBox();
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
            // commitButton
            // 
            this.commitButton.Location = new System.Drawing.Point(143, 279);
            this.commitButton.Name = "commitButton";
            this.commitButton.Size = new System.Drawing.Size(75, 23);
            this.commitButton.TabIndex = 6;
            this.commitButton.Text = "OK";
            this.commitButton.UseVisualStyleBackColor = true;
            this.commitButton.Click += new System.EventHandler(this.commitButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(21, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "Resistances";
            // 
            // resistList
            // 
            this.resistList.AutoCompleteCustomSource.AddRange(new string[] {
            "yes",
            "no"});
            this.resistList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resistList.FormattingEnabled = true;
            this.resistList.Items.AddRange(new object[] {
            "yes",
            "no"});
            this.resistList.Location = new System.Drawing.Point(206, 185);
            this.resistList.Name = "resistList";
            this.resistList.Size = new System.Drawing.Size(121, 21);
            this.resistList.TabIndex = 8;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 460);
            this.Controls.Add(this.resistList);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.commitButton);
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
        private System.Windows.Forms.Button commitButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox resistList;
    }
}