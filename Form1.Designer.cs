namespace Suikoden3Editor
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbCharacterName = new System.Windows.Forms.ComboBox();
            this.lbl1 = new System.Windows.Forms.Label();
            this.lblRaw = new System.Windows.Forms.Label();
            this.btnMaxAll = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            // 
            // cbCharacterName
            // 
            this.cbCharacterName.FormattingEnabled = true;
            this.cbCharacterName.Location = new System.Drawing.Point(73, 14);
            this.cbCharacterName.Name = "cbCharacterName";
            this.cbCharacterName.Size = new System.Drawing.Size(199, 23);
            this.cbCharacterName.TabIndex = 0;
            this.cbCharacterName.SelectedIndexChanged += new System.EventHandler(this.cbCharacterName_SelectedIndexChanged);
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(9, 17);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(58, 15);
            this.lbl1.TabIndex = 1;
            this.lbl1.Text = "Character";
            // 
            // lblRaw
            // 
            this.lblRaw.AutoSize = true;
            this.lblRaw.Location = new System.Drawing.Point(9, 607);
            this.lblRaw.Name = "lblRaw";
            this.lblRaw.Size = new System.Drawing.Size(0, 15);
            this.lblRaw.TabIndex = 2;
            // 
            // btnMaxAll
            // 
            this.btnMaxAll.Location = new System.Drawing.Point(299, 14);
            this.btnMaxAll.Name = "btnMaxAll";
            this.btnMaxAll.Size = new System.Drawing.Size(75, 23);
            this.btnMaxAll.TabIndex = 3;
            this.btnMaxAll.Text = "Max All";
            this.btnMaxAll.UseVisualStyleBackColor = true;
            this.btnMaxAll.Click += new System.EventHandler(this.btnMaxAll_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1482, 599);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1567, 635);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnMaxAll);
            this.Controls.Add(this.lblRaw);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.cbCharacterName);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "Suikoden 3 Editor";
            this.Load += new System.EventHandler(this.MainForm_Load);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbCharacterName;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label lblRaw;
        private System.Windows.Forms.Button btnMaxAll;
        private System.Windows.Forms.Button btnSave;
    }
}

