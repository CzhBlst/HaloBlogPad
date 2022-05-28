namespace Notepad
{
    partial class SettingForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.AttachmentBox = new HZH_Controls.Controls.TextBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.BlogBox = new HZH_Controls.Controls.TextBoxEx();
            this.LoginCheckBox = new HZH_Controls.Controls.UCCheckBox();
            this.SaveCheckBox = new HZH_Controls.Controls.UCCheckBox();
            this.LoginIntervalLabel = new System.Windows.Forms.Label();
            this.LoginIntervalBox = new HZH_Controls.Controls.TextBoxEx();
            this.SaveIntevalLabel = new System.Windows.Forms.Label();
            this.SaveIntervalBox = new HZH_Controls.Controls.TextBoxEx();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(84, 244);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "图片缓存路径：";
            // 
            // AttachmentBox
            // 
            this.AttachmentBox.DecLength = 2;
            this.AttachmentBox.InputType = HZH_Controls.TextInputType.NotControl;
            this.AttachmentBox.Location = new System.Drawing.Point(129, 63);
            this.AttachmentBox.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.AttachmentBox.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.AttachmentBox.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.AttachmentBox.Name = "AttachmentBox";
            this.AttachmentBox.OldText = null;
            this.AttachmentBox.PromptColor = System.Drawing.Color.Gray;
            this.AttachmentBox.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.AttachmentBox.PromptText = "";
            this.AttachmentBox.RegexPattern = "";
            this.AttachmentBox.Size = new System.Drawing.Size(145, 21);
            this.AttachmentBox.TabIndex = 6;
            this.AttachmentBox.Text = "博客缓存路径";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "博客缓存路径：";
            // 
            // BlogBox
            // 
            this.BlogBox.DecLength = 2;
            this.BlogBox.InputType = HZH_Controls.TextInputType.NotControl;
            this.BlogBox.Location = new System.Drawing.Point(129, 25);
            this.BlogBox.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.BlogBox.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.BlogBox.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.BlogBox.Name = "BlogBox";
            this.BlogBox.OldText = null;
            this.BlogBox.PromptColor = System.Drawing.Color.Gray;
            this.BlogBox.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.BlogBox.PromptText = "";
            this.BlogBox.RegexPattern = "";
            this.BlogBox.Size = new System.Drawing.Size(145, 21);
            this.BlogBox.TabIndex = 4;
            this.BlogBox.Text = "博客缓存路径";
            // 
            // LoginCheckBox
            // 
            this.LoginCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.LoginCheckBox.Checked = false;
            this.LoginCheckBox.Location = new System.Drawing.Point(100, 167);
            this.LoginCheckBox.Name = "LoginCheckBox";
            this.LoginCheckBox.Padding = new System.Windows.Forms.Padding(1);
            this.LoginCheckBox.Size = new System.Drawing.Size(101, 30);
            this.LoginCheckBox.TabIndex = 8;
            this.LoginCheckBox.TextValue = "自动登录";
            // 
            // SaveCheckBox
            // 
            this.SaveCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.SaveCheckBox.Checked = false;
            this.SaveCheckBox.Location = new System.Drawing.Point(100, 203);
            this.SaveCheckBox.Name = "SaveCheckBox";
            this.SaveCheckBox.Padding = new System.Windows.Forms.Padding(1);
            this.SaveCheckBox.Size = new System.Drawing.Size(101, 30);
            this.SaveCheckBox.TabIndex = 9;
            this.SaveCheckBox.TextValue = "自动保存";
            // 
            // LoginIntervalLabel
            // 
            this.LoginIntervalLabel.AutoSize = true;
            this.LoginIntervalLabel.Location = new System.Drawing.Point(34, 103);
            this.LoginIntervalLabel.Name = "LoginIntervalLabel";
            this.LoginIntervalLabel.Size = new System.Drawing.Size(89, 12);
            this.LoginIntervalLabel.TabIndex = 13;
            this.LoginIntervalLabel.Text = "登录间隔(秒)：";
            // 
            // LoginIntervalBox
            // 
            this.LoginIntervalBox.DecLength = 2;
            this.LoginIntervalBox.InputType = HZH_Controls.TextInputType.NotControl;
            this.LoginIntervalBox.Location = new System.Drawing.Point(129, 100);
            this.LoginIntervalBox.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.LoginIntervalBox.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.LoginIntervalBox.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.LoginIntervalBox.Name = "LoginIntervalBox";
            this.LoginIntervalBox.OldText = null;
            this.LoginIntervalBox.PromptColor = System.Drawing.Color.Gray;
            this.LoginIntervalBox.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.LoginIntervalBox.PromptText = "";
            this.LoginIntervalBox.RegexPattern = "";
            this.LoginIntervalBox.Size = new System.Drawing.Size(145, 21);
            this.LoginIntervalBox.TabIndex = 12;
            this.LoginIntervalBox.Text = "10";
            this.LoginIntervalBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntTextBox_KeyPress);
            // 
            // SaveIntevalLabel
            // 
            this.SaveIntevalLabel.AutoSize = true;
            this.SaveIntevalLabel.Location = new System.Drawing.Point(34, 140);
            this.SaveIntevalLabel.Name = "SaveIntevalLabel";
            this.SaveIntevalLabel.Size = new System.Drawing.Size(89, 12);
            this.SaveIntevalLabel.TabIndex = 11;
            this.SaveIntevalLabel.Text = "保存间隔(秒)：";
            // 
            // SaveIntervalBox
            // 
            this.SaveIntervalBox.DecLength = 2;
            this.SaveIntervalBox.InputType = HZH_Controls.TextInputType.NotControl;
            this.SaveIntervalBox.Location = new System.Drawing.Point(129, 137);
            this.SaveIntervalBox.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.SaveIntervalBox.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.SaveIntervalBox.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.SaveIntervalBox.Name = "SaveIntervalBox";
            this.SaveIntervalBox.OldText = null;
            this.SaveIntervalBox.PromptColor = System.Drawing.Color.Gray;
            this.SaveIntervalBox.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.SaveIntervalBox.PromptText = "";
            this.SaveIntervalBox.RegexPattern = "";
            this.SaveIntervalBox.Size = new System.Drawing.Size(145, 21);
            this.SaveIntervalBox.TabIndex = 10;
            this.SaveIntervalBox.Text = "60";
            this.SaveIntervalBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntTextBox_KeyPress);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 279);
            this.Controls.Add(this.LoginIntervalLabel);
            this.Controls.Add(this.LoginIntervalBox);
            this.Controls.Add(this.SaveIntevalLabel);
            this.Controls.Add(this.SaveIntervalBox);
            this.Controls.Add(this.SaveCheckBox);
            this.Controls.Add(this.LoginCheckBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AttachmentBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BlogBox);
            this.Controls.Add(this.button1);
            this.Name = "SettingForm";
            this.Text = "SettingForm";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private HZH_Controls.Controls.TextBoxEx AttachmentBox;
        private System.Windows.Forms.Label label1;
        private HZH_Controls.Controls.TextBoxEx BlogBox;
        private HZH_Controls.Controls.UCCheckBox LoginCheckBox;
        private HZH_Controls.Controls.UCCheckBox SaveCheckBox;
        private System.Windows.Forms.Label LoginIntervalLabel;
        private HZH_Controls.Controls.TextBoxEx LoginIntervalBox;
        private System.Windows.Forms.Label SaveIntevalLabel;
        private HZH_Controls.Controls.TextBoxEx SaveIntervalBox;
    }
}