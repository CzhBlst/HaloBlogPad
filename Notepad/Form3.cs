using HZH_Controls.Controls;
using Notepad.Bean;
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
            var posts = postServices.GetAllPost();
            List<object> dataSource = new List<object>();
            List<DataGridViewColumnEntity> lstCulumns = new List<DataGridViewColumnEntity>();
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Id", HeadText = "编号", Width = 70, WidthType = SizeType.Absolute });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Title", HeadText = "博客", Width = 80, WidthType = SizeType.Percent });
            this.ucDataGridView1.Columns = lstCulumns;

            foreach (var post in posts)
            {
                dataSource.Add(post);
            }
            UCPagerControl page = new UCPagerControl();
            page.DataSource = dataSource;

            this.ucDataGridView1.DataSource = dataSource;
            // this.ucDataGridView1.First();

        }

        /*private void bak()
        {
            this.listView1.Columns.Clear();
            this.listView1.Columns.Add(new ColumnHeader());
            this.listView1.Columns[0].Text = "Id";
            this.listView1.Columns.Add(new ColumnHeader());
            this.listView1.Columns[1].Text = "Title";
            this.listView1.Columns[1].Width = 178;
            foreach (var post in posts)
            {
                dataSource.Add(post);
            }
            this.ucPagerControl21.PageSize = 5;
            this.ucPagerControl21.DataSource = dataSource;
            *//*            List<object> initPageData = this.ucPagerControl21.GetCurrentSource();
                        foreach (var data in initPageData) 
                        {
                            PostInfo post = (PostInfo)data;
                            ListViewItem item = listView1.Items.Add(post.Id.ToString());
                            item.SubItems.Add(post.Title);
                            listView1.View = View.Details;
                        }*//*

            UCPagerControl page = new UCPagerControl();
            //UCPagerControl2 page = new UCPagerControl2();
            page.DataSource = dataSource;
            this.ucPagerControl21.FirstPage();
        }*/
    }
}
