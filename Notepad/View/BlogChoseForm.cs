using HZH_Controls.Controls;
using HZH_Controls.Forms;
using Notepad.Bean;
using Notepad.Services;
using Notepad.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NotePad
{
    public partial class BlogChoseForm : Form
    {
        public BlogChoseForm(string token)
        {
            this.Text = "博客选择";
            InitializeComponent();
            PostService postServices = new PostService(token);
            List<PostInfo> posts = postServices.GetAllPost();
            List<DataGridViewColumnEntity> lstColumns = new List<DataGridViewColumnEntity>();
            lstColumns.Add(new DataGridViewColumnEntity() { DataField = "Id", HeadText = "编号", Width = 70, WidthType = SizeType.Absolute });
            lstColumns.Add(new DataGridViewColumnEntity() { DataField = "Title", HeadText = "博客", Width = 80, WidthType = SizeType.Percent });
            this.ucDataGridView1.Columns = lstColumns;
            List<object> dataSource = new List<object>();
            foreach (var post in posts)
            {
                 dataSource.Add(post);
            }

            UCPagerControl2 page = new UCPagerControl2();
            page.DataSource = dataSource;
            this.ucDataGridView1.Page = page;
            this.ucDataGridView1.IsShowCheckBox = false;
            this.ucDataGridView1.First();
        }

        private void ucBtnExt1_BtnClick(object sender, EventArgs e)
        {
            if (this.ucDataGridView1.SelectRow == null)
            {
                FrmTips.ShowTipsError(this, "未选择博客");
                return;
            }
            PostInfo test;
            test = (PostInfo)this.ucDataGridView1.SelectRow.DataSource;
            AddBlogToEdits.POSTID = test.Id;
            AddBlogToEdits.TITLE = test.Title;
            AddBlogToEdits.FILEPATH = ConstantUtil.BLOGCACHE + test.Title;
            // Console.WriteLine(test.ToString());
            this.Close();
        }
    }
}
