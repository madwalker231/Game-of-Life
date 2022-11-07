namespace GOLStartUpTemplate1
{
    partial class OptionsMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.cellWidth = new System.Windows.Forms.NumericUpDown();
            this.cellHeight = new System.Windows.Forms.NumericUpDown();
            this.timerSetting = new System.Windows.Forms.NumericUpDown();
            this.cellWLable = new System.Windows.Forms.Label();
            this.cellHLable = new System.Windows.Forms.Label();
            this.timerLable = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cellWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerSetting)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(20, 94);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(101, 94);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // cellWidth
            // 
            this.cellWidth.Location = new System.Drawing.Point(101, 16);
            this.cellWidth.Name = "cellWidth";
            this.cellWidth.Size = new System.Drawing.Size(54, 20);
            this.cellWidth.TabIndex = 2;
            // 
            // cellHeight
            // 
            this.cellHeight.Location = new System.Drawing.Point(101, 42);
            this.cellHeight.Name = "cellHeight";
            this.cellHeight.Size = new System.Drawing.Size(54, 20);
            this.cellHeight.TabIndex = 3;
            // 
            // timerSetting
            // 
            this.timerSetting.Location = new System.Drawing.Point(101, 68);
            this.timerSetting.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.timerSetting.Name = "timerSetting";
            this.timerSetting.Size = new System.Drawing.Size(54, 20);
            this.timerSetting.TabIndex = 4;
            // 
            // cellWLable
            // 
            this.cellWLable.AutoSize = true;
            this.cellWLable.Location = new System.Drawing.Point(22, 18);
            this.cellWLable.Name = "cellWLable";
            this.cellWLable.Size = new System.Drawing.Size(55, 13);
            this.cellWLable.TabIndex = 5;
            this.cellWLable.Text = "Cell Width";
            // 
            // cellHLable
            // 
            this.cellHLable.AutoSize = true;
            this.cellHLable.Location = new System.Drawing.Point(22, 44);
            this.cellHLable.Name = "cellHLable";
            this.cellHLable.Size = new System.Drawing.Size(58, 13);
            this.cellHLable.TabIndex = 6;
            this.cellHLable.Text = "Cell Height";
            // 
            // timerLable
            // 
            this.timerLable.AutoSize = true;
            this.timerLable.Location = new System.Drawing.Point(22, 70);
            this.timerLable.Name = "timerLable";
            this.timerLable.Size = new System.Drawing.Size(76, 13);
            this.timerLable.TabIndex = 7;
            this.timerLable.Text = "Timer Intervals";
            // 
            // OptionsMenu
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(200, 129);
            this.Controls.Add(this.timerLable);
            this.Controls.Add(this.cellHLable);
            this.Controls.Add(this.cellWLable);
            this.Controls.Add(this.timerSetting);
            this.Controls.Add(this.cellHeight);
            this.Controls.Add(this.cellWidth);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.cellWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerSetting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.NumericUpDown cellWidth;
        private System.Windows.Forms.NumericUpDown cellHeight;
        private System.Windows.Forms.NumericUpDown timerSetting;
        private System.Windows.Forms.Label cellWLable;
        private System.Windows.Forms.Label cellHLable;
        private System.Windows.Forms.Label timerLable;
    }
}