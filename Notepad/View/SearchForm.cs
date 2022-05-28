
using HZH_Controls.Forms;
using Notepad;
using System;
using System.Windows.Forms;

namespace NotePad
{
    public partial class SearchForm : Form
    {
        //实现对form1的关联
        MainForm mainfrom1;
        public SearchForm(MainForm form1)
        {
            InitializeComponent();
            mainfrom1 = form1;
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            radioButton2.Checked = true;//默认开启向下查找功能
        }

        int start = 0; int sun = 0; int count = 0;
        bool searched = false; // 指示是否曾经查找到元素

        private void button1_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            RichTextBox rbox = mainfrom1.textBox1;
            string str = this.textBox1.Text;
            if (this.checkBox1.Checked && !this.checkBox2.Checked)//是否开启区分大小写功能
            {
                // this.FindDownM(rbox, str);//向下查找
                this.FindUpWithCase(rbox, str);
            }
            else if (this.checkBox2.Checked && !this.checkBox1.Checked)
            {
                // 是否使用全字匹配
                FindUpWithWholeWord(rbox, str);
            }
            else if (this.checkBox1.Checked && this.checkBox2.Checked)
            {
                // 全字匹配 + 区分大小写
            }
            else
            {
                if (this.radioButton2.Checked)
                {
                    this.FindDown(rbox, str);
                }
                else
                {
                    this.FindUp(rbox, str);//向上查找
                }
            }
        }

        //替换textBox1中的文本为textBox2中的文本
        private void button2_Click(object sender, EventArgs e)
        {
            string str0 = this.textBox1.Text, str1 = this.textBox2.Text;
            this.replace(str0, str1);
        }

        //全部替换
        private void button3_Click(object sender, EventArgs e)
        {
            RichTextBox rbox = mainfrom1.textBox1;
            string str0 = this.textBox1.Text, str1 = this.textBox2.Text;
            this.ReplaceAll(rbox, str0, str1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //向上查找函数
        private void FindUp(RichTextBox rbox, string str)
        {
            int rbox1 = rbox.SelectionStart;
            start = rbox.Find(str, 0, rbox1, RichTextBoxFinds.Reverse);
            if (start > -1)
            {
                rbox.SelectionStart = start;
                rbox.SelectionLength = str.Length;
                rbox.Focus();
                searched = true;
            }
            else if (start < 0 && searched)
            {
                // 说明之前曾经搜索到过该元素，从0处重新搜索
                start = rbox.Find(str, 0, 0, RichTextBoxFinds.Reverse);
                // rbox.SelectionStart = rbox.Text.Length;
            } 
            else if (start < 0 && !searched)
            {
                FrmTips.ShowTipsError(this, "未能搜索到该元素");
            }
        }
        /*
         * 区分大小写
         */
        private void FindUpWithCase(RichTextBox rbox, string str)
        {
            int rbox1 = rbox.SelectionStart;
            start = rbox.Find(str, 0, rbox1, RichTextBoxFinds.MatchCase);
            if (start > -1)
            {
                rbox.SelectionStart = start;
                rbox.SelectionLength = str.Length;
                rbox.Focus();
                searched = true;
            }
            else if (start < 0 && searched)
            {
                // 说明之前曾经搜索到过该元素，从0处重新搜索
                start = rbox.Find(str, 0, 0, RichTextBoxFinds.MatchCase);
                // rbox.SelectionStart = rbox.Text.Length;
            }
            else if (start < 0 && !searched)
            {
                FrmTips.ShowTipsError(this, "未能搜索到该元素");
            }
        }
        /*
         * 全字匹配
         */
        private void FindUpWithWholeWord(RichTextBox rbox, string str)
        {
            int rbox1 = rbox.SelectionStart;
            start = rbox.Find(str, 0, rbox1, RichTextBoxFinds.WholeWord);
            if (start > -1)
            {
                rbox.SelectionStart = start;
                rbox.SelectionLength = str.Length;
                rbox.Focus();
                searched = true;
            }
            else if (start < 0 && searched)
            {
                // 说明之前曾经搜索到过该元素，从0处重新搜索
                start = rbox.Find(str, 0, 0, RichTextBoxFinds.MatchCase);
                // rbox.SelectionStart = rbox.Text.Length;
            }
            else if (start < 0 && !searched)
            {
                FrmTips.ShowTipsError(this, "未能搜索到该元素");
            }
        }
        /*
         * 全字匹配并区分大小写
         */
        private void FindUpWithCaseAndWholeWord(RichTextBox rbox, string str)
        {

        }

        private void FindDown(RichTextBox rbox, string str)
        {

        }
        private void FindDownM(RichTextBox rbox, string str)
        {
        }
        //查找完毕后的弹窗
        private void seeks(string str)
        {

        }
        //替换全部的函数
        private void ReplaceAll(RichTextBox rbox, string str0, string str1)
        {
            rbox.Text = rbox.Text.Replace(str0, str1);
        }
        private void replace(string str0, string str1)
        {
            RichTextBox rbox = mainfrom1.textBox1;
            Search();
            rbox.SelectionStart = start;
            rbox.SelectionLength = str0.Length;
            rbox.SelectedText = str1;
            Search();
            rbox.SelectionStart = start;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.button1.Enabled = true;
            searched = false;
        }
    }
}
