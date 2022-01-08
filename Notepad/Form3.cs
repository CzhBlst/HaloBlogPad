using HZH_Controls.Controls;
using Notepad.Bean;
using Notepad.Services;
using Notepad.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NotePad
{
    public partial class Form3 : Form
    {
        public Form3(string token)
        {
            InitializeComponent();
            PostService postServices = new PostService(token);
            var posts = postServices.GetAllPost();
            List<object> dataSource = new List<object>();
            List<DataGridViewColumnEntity> lstCulumns = new List<DataGridViewColumnEntity>();
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Id", HeadText = "编号", Width = 70, WidthType = SizeType.Absolute });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Title", HeadText = "博客", Width = 80, WidthType = SizeType.Percent });
            this.ucDataGridView1.Columns = lstCulumns;
            this.ucDataGridView1.IsShowCheckBox = true;
            foreach (var post in posts)
            {
                dataSource.Add(post);
            }
            UCPagerControl page = new UCPagerControl();
            page.DataSource = dataSource;
            this.ucDataGridView1.DataSource = dataSource;

            // this.ucDataGridView1.First();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PostInfo test = (PostInfo)this.ucDataGridView1.SelectRow.DataSource;
            AddBlogToEdits.POSTID = test.Id;
            AddBlogToEdits.TITLE = test.Title;
            AddBlogToEdits.FILEPATH = ConstantUtil.CACHEPATH + test.Title;
            // Console.WriteLine(test.ToString());
            this.Close();
        }
    }
}
