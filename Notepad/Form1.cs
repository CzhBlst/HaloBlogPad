using CommonMark;
using HZH_Controls.Controls;
using HZH_Controls.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notepad.Bean;
using Notepad.Services;
using Notepad.Utils;
using NotePad;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad
{
    public partial class Form1 : Form
    {
        String path = String.Empty;
        String token;
        int currentPost;
        int loginInterval;
        int saveInterval;
        Boolean isLogin = false;
        Boolean preViewMD = false;
        Boolean isSaved = true;
        Boolean isWritingCache = false;
        Boolean autoSave = false;
        Boolean autoLogin = false;
        String mdContent = "";
        String text = "";
        PostService postServices;
        AttachmentService attachmentService;
        // id, title, content
        Dictionary<String, KeyValuePair<String, String>> editPosts;

        private Thread loginCheckTrd; // 用来检测Token是否过期
        private Thread autoSaveTrd;

        public Form1() => InitializeComponent();

        /*
         * 窗口加载时注册热键，并更改各个组件位置
         */
        private void Form1_Load_1(object sender, EventArgs e)
        {
            checkLogin();
            if (isLogin)
            {
                this.loginToolStripMenuItem.Text = "重新登录";
            }
            FormHelper.HotKeyRegister(Handle);
            ChangePannelLayOut();

            BlogSettingHelper.LoadSettings(BlogSettingHelper.ReadSettings());
            CommonSettingHelper.LoadSettings(CommonSettingHelper.ReadSettings());
            autoSave = ConstantUtil.AUTOSAVE;
            autoLogin = ConstantUtil.AUTOLOGIN;
            loginInterval = ConstantUtil.LOGINCHECKINTERVAL;
            saveInterval = ConstantUtil.SAVEINTERVAL;

            editPosts = new Dictionary<string, KeyValuePair<string, string>>();
            // textBox1.Font.Size = 12;
            // 修改默认字体大小
            textBox1.Font = new Font(textBox1.Font.FontFamily, 12, textBox1.Font.Style);
            // 创建线程，检测Token是否过期
            loginCheckTrd = new Thread(new ThreadStart(this.LoginCheckThreadTask));
            loginCheckTrd.IsBackground = true;
            loginCheckTrd.Start();
            autoSaveTrd = new Thread(new ThreadStart(this.AutoSaveThreadTask));
            autoSaveTrd.IsBackground = true;
            autoSaveTrd.Start();
            AttachmentUtil.DeleteOldCache();
        }

        /*
         * 主窗体退出时记录位置
         */
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            RegistryKey reg1 = Registry.CurrentUser;
            RegistryKey reg2 = reg1.CreateSubKey("SoftWare\\BlogPad");
            reg2.SetValue("MainFormX", this.Location.X);
            reg2.SetValue("MainFormY", this.Location.Y);
        }

        /*
         * 修改界面位置
         */
        private void ChangePannelLayOut()
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("SoftWare\\BlogPad");
            this.Location = new Point(Convert.ToInt32(reg.GetValue("MainFormX")), 
                Convert.ToInt32(reg.GetValue("MainFormY")));
            if (Location.X < 0 || Location.Y < 0)
            {
                this.Location = new Point(200, 200);
            }
            this.Size = new System.Drawing.Size(650, 550);
            this.MinimumSize = new System.Drawing.Size(600, 500);
            // comboBox1.Left = this.Width - 180;
            ChangePannel();
            ChangePannelSize();
        }
        /*
         * 启动线程，每隔十秒检测一次登录是否过期
         */
        private async void LoginCheckThreadTask()
        {
            while (autoLogin)
            {
                AuthService authService = new AuthService();
                LoginInfo lastToken = authService.getLastLoginToken();
                if (lastToken == null || !authService.checkToken(lastToken))
                {
                    this.Invoke(new EventHandler(async delegate
                    {
                        isLogin = false;
                        this.loginToolStripMenuItem.Text = "Login";
                        if (autoLogin)
                        {
                            LoginInfo info = await authService.LoginAsync();
                            token = info.UsingToken;
                            if (token.Equals("error"))
                            {
                                FrmTips.ShowTipsError(this, "自动登录失败");
                            }
                            else
                            {
                                lastToken = info;
                                FrmTips.ShowTipsInfo(this, "登录过期，已自动重新登录");
                                this.loginToolStripMenuItem.Text = "重新登录";
                                postServices = new PostService(token);
                                isLogin = true;
                            }
                        }
                    }));
                }
                else
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        token = lastToken.UsingToken;
                        postServices = new PostService(token);
                        isLogin = true;
                        this.loginToolStripMenuItem.Text = "重新登录";
                    }));
                }
                Thread.Sleep(loginInterval);
            }
        }
        /*
         * 自动保存
         */
        private void AutoSaveThreadTask()
        {
            while (autoSave)
            {
                if (isLogin && !String.IsNullOrWhiteSpace(path) && autoSave)
                {
                    this.Invoke(new EventHandler(async delegate
                    {
                        File.WriteAllText(path, text);
                        editPosts[PostChoseHelper.POSTID.ToString()] =
                            new KeyValuePair<string, string>(PostChoseHelper.TITLE, text);
                        if (PostChoseHelper.POSTID >= 0)
                        {
                            isSaved = await postServices.UpdatePostByIdAsync(PostChoseHelper.POSTID, PostChoseHelper.TITLE, text);
                            PostEditHelper.WritePostEditPosById(this.textBox1.SelectionStart);
                            if (!isSaved)
                                FrmTips.ShowTipsError(this, "自动保存失败");
                            else
                                this.Text = "AutoBlog" + " " + PostChoseHelper.POSTID + "-" + PostChoseHelper.TITLE + " saved";
                        }
                    }));
                } 
                else if (!isLogin && autoSave)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        FrmTips.ShowTipsError(this, "未登录或Token已过期");
                    }));
                    
                }
                else if (!String.IsNullOrWhiteSpace(path) && autoSave)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        FrmTips.ShowTipsError(this, "未选择博客");
                    }));
                }
                Thread.Sleep(saveInterval);
            }
        }
        /*
         * 登录状态检查
         */
        private void checkLogin()
        {
            // 检查登录状态
            AuthService authService = new AuthService();
            LoginInfo lastToken = authService.getLastLoginToken();
            if (lastToken == null || !authService.checkToken(lastToken))
            {
                isLogin = false;
            } 
            else
            {
                isLogin = true;
            }
        }

        private async void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthService authService = new AuthService();
            LoginInfo info;
            try
            {
                info = await authService.LoginAsync();
                token = info.UsingToken;
            }
            catch (Exception)
            {
                // FrmTips.ShowTipsError(this, "登录失败，请检查Settings是否配置");
            }
            // token = info.UsingToken;
            if (token == null || token.Equals("error"))
            {
                FrmTips.ShowTipsError(this, "登录失败，请检查Settings!");
            }
            else
            {
                FrmTips.ShowTipsInfo(this, "登录成功");
                this.loginToolStripMenuItem.Text = "重新登录";
                postServices = new PostService(token);
                isLogin = true;
            }
        }

        private void getBlogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isLogin || token == null || token == " " || postServices == null)
            {
                FrmTips.ShowTipsError(this, "未登录");
                return;
            }
            Form postsForm = new BlogChoseForm(token);
            postsForm.StartPosition = FormStartPosition.CenterScreen;
            postsForm.ShowDialog();
            if (!AddBlogToEdits.TITLE.Equals(""))
            {
                Post post = postServices.GetPostById(AddBlogToEdits.POSTID); // 获取所选博客详细信息
                PostUtil.WriteToCache(post); // 将博客内容读取到本地
                try
                {
                    editPosts.Add(post.id, new KeyValuePair<string, string>(post.title, post.originalContent));
                    BindEditPosts();
                }
                catch (ArgumentException e1)
                {
                    Console.WriteLine(e1.StackTrace);
                }
            }
            else
            {
                MessageBox.Show("未选择Blog");
            }
        }

        private void setTextBox()
        {
            currentPost = PostChoseHelper.POSTID;
            this.path = PostChoseHelper.FILEPATH;
            string originalContent = File.ReadAllText(path);
            originalContent = Regex.Replace(originalContent, "(?<!\r)\n", "\r\n");
            textBox1.Text = originalContent;
            this.Text = "AutoBlog" + " " + PostChoseHelper.POSTID + "-" + PostChoseHelper.TITLE;
            this.textBox1.SelectionStart = PostEditHelper.ReadPostEditPosById(PostChoseHelper.POSTID);
            this.textBox1.Focus();
            this.textBox1.ScrollToCaret();
        }

        private void addNewPostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isLogin || token == null || token == " " || postServices == null)
            {
                FrmTips.ShowTipsError(this, "未登录");
                return;
            }
            FrmInputs frm = new FrmInputs("新建博客",
                new string[] { "Title", "内容", "密码" },
                mastInputs: new List<string>() { "Title" });
            frm.ShowDialog(this);
            string[] test = frm.Values;
            if (test[0] == null)
            {
                return;
            }
            Post post = new Post(test[0], test[1]);
            string cachePath = ConstantUtil.BLOGCACHE + post.title;
            FileStream fs = new FileStream(cachePath, FileMode.OpenOrCreate, FileAccess.Read);
            fs.Close();
            int postId = postServices.AddNewPost(post.title);
            if (postId == -1)
            {
                FrmTips.ShowTipsError(this, "新建博客失败");
            }
            editPosts.Add(postId.ToString(), new KeyValuePair<string, string>(post.title, post.originalContent));
            FrmTips.ShowTipsInfo(this, "新建博客: " + postId + ":" + post.title + 
                "\n缓存路径为: " + cachePath);
            // 增加新的博客编辑位置
            PostEditHelper.WritePostEditPosById(0);
            BindEditPosts();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = File.ReadAllText(path = openFileDialog1.FileName);
                string[] SplitExtension = openFileDialog1.FileName.Split('.');
                // labelFormat.Text = ReturnMessageFromFormat(SplitExtension[1]);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(path = saveFileDialog1.FileName, textBox1.Text);
            }
        }

        private async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(path))
            {
                await SaveEditingPost();
            }
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        public async Task SaveEditingPost()
        {
            if (!String.IsNullOrWhiteSpace(path))
            {
                File.WriteAllText(path, textBox1.Text);
                editPosts[PostChoseHelper.POSTID.ToString()] =
                    new KeyValuePair<string, string>(PostChoseHelper.TITLE, this.textBox1.Text);
                if (PostChoseHelper.POSTID >= 0)
                {
                    // 异步方法会卡死在await，虽然成功调用了方法，但无法正确返回response
                    // 非异步方法中调用异步方法会导致无法正确返回（可能是因为程序本身已经跑过去了？）
                    isSaved = await postServices.UpdatePostByIdAsync(PostChoseHelper.POSTID, PostChoseHelper.TITLE, textBox1.Text);
                    PostEditHelper.WritePostEditPosById(this.textBox1.SelectionStart);
                    if (isSaved)
                        this.Text = "AutoBlog" + " " + PostChoseHelper.POSTID + "-" + PostChoseHelper.TITLE + " saved";
                }
            } 
            else
            {
                FrmTips.ShowTipsError(this, "未选择博客");
            }
        }

        private void exitPrompt()
        {
            if (!isSaved)
            {
                DialogResult = MessageBox.Show("Do you want to save current file?",
                "Notepad",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            }
        }
        //相应的查找子菜单
        private void SearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchForm f2 = new SearchForm(this);
            f2.StartPosition = FormStartPosition.CenterParent;
            f2.Show();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(textBox1.Text))
            {
                exitPrompt();

                if (DialogResult == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                    textBox1.Text = String.Empty;
                    path = String.Empty;;
                }
                else if (DialogResult == DialogResult.No)
                {
                    textBox1.Text = String.Empty;;
                    path = String.Empty;;
                }

            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) => textBox1.SelectAll();

        private void cutToolStripMenuItem_Click(object sender, EventArgs e) => textBox1.Cut();

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) => textBox1.Copy();

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e) => textBox1.Paste();

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) => textBox1.SelectedText = String.Empty;

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wordWrapToolStripMenuItem.Checked == true)
            {
                textBox1.WordWrap = false;
                textBox1.ScrollBars = RichTextBoxScrollBars.Both;
                wordWrapToolStripMenuItem.Checked = false;
            }
            else
            {
                textBox1.WordWrap = true;
                textBox1.ScrollBars = RichTextBoxScrollBars.Vertical;
                wordWrapToolStripMenuItem.Checked = true;
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Font = textBox1.Font = new Font(fontDialog1.Font, fontDialog1.Font.Style);

                textBox1.ForeColor = fontDialog1.Color;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form aboutForm = new Form2();
            aboutForm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();
        // 选择博客的下拉框事件
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (editPosts.Count > 1 && PostChoseHelper.POSTID > 0)
            {
                saveToolStripMenuItem_Click(sender, e);
            }

            KeyValuePair<string, string> item = (KeyValuePair<string, string>)this.comboBox1.SelectedItem;
            String filePath = ConstantUtil.BLOGCACHE + item.Value;
            PostUtil.setChosePost(int.Parse(item.Key), item.Value, filePath);
            setTextBox();
        }
        // 博客文字发生变化时的事件
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.Text = this.Text = "AutoBlog" + " " + PostChoseHelper.POSTID + "-" + PostChoseHelper.TITLE + " unsaved";
            isSaved = false;

            text = textBox1.Text;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                exitPrompt();

                if (DialogResult == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
                else if (DialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Control)
            {

                switch (e.KeyCode)
                {
                    case Keys.A:
                        e.SuppressKeyPress = true;
                        textBox1.SelectAll();
                        break;
                    case Keys.N:
                        e.SuppressKeyPress = true;
                        newToolStripMenuItem_Click(sender, e);
                        break;
                    case Keys.S:
                        e.SuppressKeyPress = true;
                        await SaveEditingPost();
                        break;
                    case Keys.Q:
                        if (this.TopMost == true)
                        {
                            this.TopMost = false;
                        } 
                        else
                        {
                            this.TopMost = true;
                        }
                        break;
                    case Keys.F:
                        e.SuppressKeyPress= true;
                        SearchToolStripMenuItem_Click(sender, e);
                        break;
                    case Keys.I:
                        e.SuppressKeyPress = true;
                        string path = InsertImageFromClipboard();
                        this.textBox1.SelectedText = path;
                        break;
                }
            }
            
        }
        /// <summary>
        /// 从剪切板获取图片插入到文本中
        /// </summary>
        /// <returns>返回MD格式引用</returns>
        private string InsertImageFromClipboard()
        {
            if (Clipboard.ContainsImage())
            {
                attachmentService = new AttachmentService(token);
                string path = AttachmentUtil.InsertImgFromClipBoard(attachmentService);
                return path;
            }
            else
            {
                FrmTips.ShowTipsError(this, "剪切板没有图片");
                return "";
            }
            
        }

        private void blackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.White;
            textBox1.BackColor = Color.Black;
            this.BackColor = Color.Gray;
        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.Black;
            textBox1.BackColor = Color.Gray;
            this.BackColor = Color.Gray;
        }

        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.Black;
            textBox1.BackColor = Color.White;
            this.BackColor = Color.White;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            ChangePannelSize();
        }

        // 热键对应功能代码
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            //按快捷键    
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:    //隐藏窗口
                            HideForm();
                            break;
                        case 101:   //切换MD和文本
                            if (preViewMD)
                                preViewMD = false;
                            else
                                preViewMD = true;
                            ChangePannel();
                            break;
                        case 102:    //按下的是Alt+D   
                            //此处填写快捷键响应代码   
                            this.Text = "按下的是Ctrl+Alt+D";
                            break;
                        case 103:
                            this.Text = "F5";
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        public void HideForm() //alt+q隐藏窗体，再按显示窗体。
        {
            if (this.Visible == true)
                this.Visible = false;
            else
                this.Visible = true;
        }

        private void ChangePannelSize()
        {
            panel1.Width = this.Width - 20;
            panel1.Height = this.Height - 90;
            comboBox1.Left = this.Width - 180;
            if (preViewMD)
            {
                // webBrowser1.DocumentText = mdContent;
                webBrowser1.Left = 5;
                webBrowser1.Width = panel1.Width - 10;
                webBrowser1.Top = 0;
                webBrowser1.Height = panel1.Height - 10;
            }
            else
            {
                textBox1.Left = 5;
                textBox1.Width = panel1.Width - 10;
                textBox1.Top = 0;
                textBox1.Height = panel1.Height - 10;
            }
        }
        // alt+V 切换文本和MarkDown预览模式
        private void ChangePannel()
        {
            if (preViewMD)
            {
                textBox1.Hide();
                webBrowser1.Show();
                string originalContent = textBox1.Text;
                mdContent = CommonMarkConverter.Convert(originalContent);
                string url = ConstantUtil.URL + "/archives/" + PostChoseHelper.TITLE;
                webBrowser1.DocumentText = mdContent;
                int pos = GetScrollPos(textBox1.Handle, 1);
            }
            else
            {
                webBrowser1.Hide();
                textBox1.Show();
                textBox1.Focus();
            }
            ChangePannelSize(); 
        }

        [DllImport("user32.dll", EntryPoint = "GetScrollPos")]
        public static extern int GetScrollPos(
            IntPtr hwnd,
            int nBar
        );

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting setting = BlogSettingHelper.ReadSettings();
            FrmInputs frm = new FrmInputs("博客设置",
                new string[] { "博客地址", "账号(邮箱)", "密码" },
                defaultValues: new Dictionary<string, string> { { "博客地址", setting.Url },
                    { "账号(邮箱)", setting.Username },
                    { "密码", setting.Password } }
                );
            frm.ShowDialog(this);
            string[] test = frm.Values;
            setting.Url = test[0];
            setting.Username = test[1];
            setting.Password = test[2];
            if (setting.Url == null ||
                setting.Username == null ||
                setting.Password == null)
            {
                return;
            }
            BlogSettingHelper.WriteSettings(setting);
            BlogSettingHelper.LoadSettings(setting);
        }

        private void CommonSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingForm frm = new SettingForm();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);
            CommonSettingHelper.LoadSettings(CommonSettingHelper.ReadSettings());
            autoSave = ConstantUtil.AUTOSAVE;
            autoLogin = ConstantUtil.AUTOLOGIN;
            loginInterval = ConstantUtil.LOGINCHECKINTERVAL;
            saveInterval = ConstantUtil.SAVEINTERVAL;
            if (!loginCheckTrd.IsAlive)
            {
                loginCheckTrd = new Thread(new ThreadStart(this.LoginCheckThreadTask));
                loginCheckTrd.IsBackground = true;
                loginCheckTrd.Start();
            }
            if (!autoSaveTrd.IsAlive)
            {
                autoSaveTrd = new Thread(new ThreadStart(this.AutoSaveThreadTask));
                autoSaveTrd.IsBackground = true;
                autoSaveTrd.Start();
            }
        }

        /*
         * 初始化编辑位置
         */
        public void initEditPostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (postServices == null)
            {
                FrmTips.ShowTipsError(this, "未登录");
                return;
            }
            bool success = PostEditHelper.InitAllPost(postServices);
            if (!success)
            {
                FrmTips.ShowTipsError(this, "初始化编辑位置失败，请检查初始化步骤");
                return;
            }
            PostEditHelper.ReadPostEditInfo();
        }
        /*
         * 添加正在编辑的博客到List
         */
        private void BindEditPosts()
        {
            List<KeyValuePair<string, string>> lstCom = new List<KeyValuePair<string, string>>();
            foreach (var post in editPosts)
            {
                lstCom.Add(new KeyValuePair<string, string>(post.Key, post.Value.Key));
            }
            this.comboBox1.DataSource = lstCom;
        }

        private async void CacheAllBlogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isWritingCache)
            {
                FrmTips.ShowTipsWarning(this, "正在写入中，请耐心等待");
                return;
            }
            isWritingCache = true;
            if (!isLogin || postServices == null)
            {
                FrmTips.ShowTipsError(this, "未登录");
                return;
            }
            List<PostInfo> posts = postServices.GetAllPost();
            foreach (PostInfo post in posts)
            {
                await postServices.SavePostToLocalByIdAsync(post.Id);
            }
            isWritingCache = false;
            FrmTips.ShowTipsInfo(this, "全部写入完成");
        }
    }
}
