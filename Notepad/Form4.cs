using Notepad.Services;
using Notepad.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotePad
{
    public partial class Form4 : Form
    {
        string token;
        public Form4(string token)
        {
            this.token = token;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PostService postServices = new PostService(token);
            // PostChoseHelper.POSTID = int.Parse(this.textBox1.Text);
            PostChoseHelper.TITLE = this.textBox2.Text;
            string cachePath = ConstantUtil.CACHEPATH + this.textBox2.Text;
            FileStream fs = new FileStream(cachePath, FileMode.OpenOrCreate, FileAccess.Read);
            fs.Close();
            PostChoseHelper.FILEPATH = cachePath;
            postServices.AddNewPost(PostChoseHelper.TITLE);
            MessageBox.Show("新建博客: " + PostChoseHelper.TITLE + "\n缓存路径为: " + cachePath);
            this.Close();
        }

    }
}
