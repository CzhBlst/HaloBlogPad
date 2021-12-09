using Notepad.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotePad
{
    public partial class Form3 : Form
    {
        public Form3(string token)
        {
            InitializeComponent();
            PostService postServices = new PostService(token);
            var posts = postServices.GetPosts(0);
            this.listView1.Columns.Clear();
            this.listView1.Columns.Add(new ColumnHeader());
            this.listView1.Columns[0].Text = "Id";
            this.listView1.Columns.Add(new ColumnHeader());
            this.listView1.Columns[1].Text = "Title";
            foreach (var post in posts)
            {
                ListViewItem item = listView1.Items.Add(post.Id.ToString());
                item.SubItems.Add(post.Title);
                listView1.View = View.Details;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
