
namespace NotePad
{
    partial class SearchForm
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
            this.textBox1 = new HZH_Controls.Controls.TextBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new HZH_Controls.Controls.TextBoxEx();
            this.checkBox1 = new HZH_Controls.Controls.UCCheckBox();
            this.radioButton2 = new HZH_Controls.Controls.UCRadioButton();
            this.radioButton1 = new HZH_Controls.Controls.UCRadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.checkBox2 = new HZH_Controls.Controls.UCCheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.DecLength = 2;
            this.textBox1.InputType = HZH_Controls.TextInputType.NotControl;
            this.textBox1.Location = new System.Drawing.Point(59, 41);
            this.textBox1.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.textBox1.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.textBox1.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.OldText = null;
            this.textBox1.PromptColor = System.Drawing.Color.Gray;
            this.textBox1.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox1.PromptText = "";
            this.textBox1.RegexPattern = "";
            this.textBox1.Size = new System.Drawing.Size(145, 21);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "查找:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "替换为:";
            // 
            // textBox2
            // 
            this.textBox2.DecLength = 2;
            this.textBox2.InputType = HZH_Controls.TextInputType.NotControl;
            this.textBox2.Location = new System.Drawing.Point(59, 94);
            this.textBox2.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.textBox2.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.textBox2.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.textBox2.Name = "textBox2";
            this.textBox2.OldText = null;
            this.textBox2.PromptColor = System.Drawing.Color.Gray;
            this.textBox2.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox2.PromptText = "";
            this.textBox2.RegexPattern = "";
            this.textBox2.Size = new System.Drawing.Size(145, 21);
            this.textBox2.TabIndex = 2;
            // 
            // checkBox1
            // 
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.Checked = false;
            this.checkBox1.Location = new System.Drawing.Point(12, 169);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Padding = new System.Windows.Forms.Padding(1);
            this.checkBox1.Size = new System.Drawing.Size(122, 30);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.TextValue = "区分大小写";
            // 
            // radioButton2
            // 
            this.radioButton2.Checked = false;
            this.radioButton2.GroupName = null;
            this.radioButton2.Location = new System.Drawing.Point(178, 223);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(113, 30);
            this.radioButton2.TabIndex = 5;
            this.radioButton2.TextValue = "向下查找";
            // 
            // radioButton1
            // 
            this.radioButton1.Checked = false;
            this.radioButton1.GroupName = null;
            this.radioButton1.Location = new System.Drawing.Point(178, 169);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(113, 30);
            this.radioButton1.TabIndex = 6;
            this.radioButton1.TextValue = "向上查找";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(176, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "查找方向";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(377, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "查找";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(377, 97);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "替换";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(377, 154);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "全部替换";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(377, 207);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "退出";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.BackColor = System.Drawing.Color.Transparent;
            this.checkBox2.Checked = false;
            this.checkBox2.Location = new System.Drawing.Point(12, 223);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Padding = new System.Windows.Forms.Padding(1);
            this.checkBox2.Size = new System.Drawing.Size(122, 30);
            this.checkBox2.TabIndex = 12;
            this.checkBox2.TextValue = "全字匹配";
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 276);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "SearchForm";
            this.Text = "SearchForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HZH_Controls.Controls.TextBoxEx textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private HZH_Controls.Controls.TextBoxEx textBox2;
        private HZH_Controls.Controls.UCCheckBox checkBox1;
        private HZH_Controls.Controls.UCRadioButton radioButton2;
        private HZH_Controls.Controls.UCRadioButton radioButton1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private HZH_Controls.Controls.UCCheckBox checkBox2;
    }
}

