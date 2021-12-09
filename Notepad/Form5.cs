using Notepad.Bean;
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
    public partial class Form5 : Form
    {
        string token;
        public Form5(string token)
        {
            this.token = token;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PostChoseHelper.POSTID = int.Parse(this.textBox1.Text);
            PostService postService = new PostService(token);
            Post post = postService.GetPostById(PostChoseHelper.POSTID);
            PostChoseHelper.TITLE = post.title;
            PostChoseHelper.FILEPATH = ConstantUtil.CACHEPATH + PostChoseHelper.TITLE;
            PostUtil.WriteToCache(post);

            this.Close();
        }

    }
}
