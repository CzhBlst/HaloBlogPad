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
using System.Windows.Forms;

namespace Notepad
{
    public partial class Form1 : Form
    {
        String path = String.Empty;
        String token;
        int currentPost;
        Boolean isLogin = false;
        Boolean preViewMD = false;
        Boolean isSaved = true;
        String mdContent = "";
        String text = "";
        int editPos = 0;
        PostService postServices;
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
            HotKeyRegister();
            ChangePannelLayOut();
            LoadSettings(ReadSettings());
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
            this.Size = new System.Drawing.Size(650, 550);
            this.MinimumSize = new System.Drawing.Size(600, 500);
            // comboBox1.Left = this.Width - 180;
            ChangePannel();
            ChangePannelSize();
        }

        /*
         * 注册快捷键
         */
        private void HotKeyRegister()
        {
            HotKey.RegisterHotKey(Handle, 100, HotKey.KeyModifiers.Alt, Keys.Q);
            HotKey.RegisterHotKey(Handle, 101, HotKey.KeyModifiers.Alt, Keys.V);
        }

        /*
         * 启动线程，每隔十秒检测一次登录是否过期
         */
        private void LoginCheckThreadTask()
        {
            while (true)
            {
                checkLogin();
                Thread.Sleep(10000);
            }
        }
        /*
         * 自动保存
         */
        private void AutoSaveThreadTask()
        {
            while (true)
            {
                if (isLogin && !String.IsNullOrWhiteSpace(path) && AutoSaveBox.Checked)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        File.WriteAllText(path, text);
                        editPosts[PostChoseHelper.POSTID.ToString()] =
                            new KeyValuePair<string, string>(PostChoseHelper.TITLE, text);
                        if (PostChoseHelper.POSTID >= 0)
                        {
                            isSaved = postServices.UpdatePostById(PostChoseHelper.POSTID, PostChoseHelper.TITLE, text);
                            WritePostEditPosById();
                            if (!isSaved)
                                FrmTips.ShowTipsError(this, "自动保存失败");
                            // this.Text = "AutoBlog" + " " + PostChoseHelper.POSTID + "-" + PostChoseHelper.TITLE + " auto saved";
                        }
                    }));
                } 
                else if (!isLogin && AutoSaveBox.Checked)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        FrmTips.ShowTipsError(this, "未登录或Token已过期");
                    }));
                    
                }
                else if (!String.IsNullOrWhiteSpace(path) && AutoSaveBox.Checked)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        FrmTips.ShowTipsError(this, "未登录或Token已过期");
                    }));
                }
                Thread.Sleep(22333);
            }
        }

        private async void checkLogin()
        {
            // 检查登录状态
            AuthService authService = new AuthService();
            LoginInfo lastToken = authService.getLastLoginToken();
            if (lastToken == null || !authService.checkToken(lastToken))
            {
                isLogin = false;
                this.loginToolStripMenuItem.Text = "Login";
                if (AutoLogin.Checked)
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
                // 未登陆过或是token已过期
            }
            else
            {
                token = lastToken.UsingToken;
                postServices = new PostService(token);
                isLogin = true;
                this.loginToolStripMenuItem.Text = "重新登录";
            }
        }

        private async void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthService authService = new AuthService();
            LoginInfo info = await authService.LoginAsync();
            token = info.UsingToken;
            if (token.Equals("error"))
            {
                FrmTips.ShowTipsError(this, "登录失败");
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
            Form postsForm = new BlogChoseForm(token);
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
            this.textBox1.SelectionStart = ReadPostEditPosById(PostChoseHelper.POSTID);
            this.textBox1.Focus();
            this.textBox1.ScrollToCaret();
        }

        private void setChosePost(int postid, string title,string filepath)
        {
            PostChoseHelper.POSTID = postid;
            PostChoseHelper.TITLE = title;
            PostChoseHelper.FILEPATH = filepath;
        }

        private void addNewPostToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            string cachePath = ConstantUtil.CACHEPATH + post.title;
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
            WritePostEditPosById();
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(path))
            {
                File.WriteAllText(path, textBox1.Text);
                editPosts[PostChoseHelper.POSTID.ToString()] = 
                    new KeyValuePair<string, string>(PostChoseHelper.TITLE, this.textBox1.Text);
                if (PostChoseHelper.POSTID >= 0)
                {
                    isSaved = postServices.UpdatePostById(PostChoseHelper.POSTID, PostChoseHelper.TITLE, textBox1.Text);
                    WritePostEditPosById();
                    if (isSaved)
                        this.Text = "AutoBlog" + " " + PostChoseHelper.POSTID + "-" + PostChoseHelper.TITLE + " saved";
                }
            }
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
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
                        saveToolStripMenuItem_Click(sender, e);
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
                }
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
                // int pos = GetScrollPos(textBox1.Handle, 0);
                int pos = GetScrollPos(textBox1.Handle, 1);
                // int top = webBrowser1.Document.GetElementsByTagName("HTML")[0].ScrollTop;//滚动条垂直位置
                // webBrowser1.Document.Window.ScrollTo(new Point(1517, pos));
                // webBrowser1.AutoScrollOffset.Offset(1883, 722); 
                // webBrowser1.
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
            Setting setting = ReadSettings();
            FrmInputs frm = new FrmInputs("博客设置",
                new string[] { "博客地址", "缓存路径", "账号(邮箱)", "密码" },
                defaultValues: new Dictionary<string, string> { { "博客地址", setting.Url },
                    { "缓存路径", setting.CachePath },
                    { "账号(邮箱)", setting.Username },
                    { "密码", setting.Password } }
                );
            frm.ShowDialog(this);
            string[] test = frm.Values;
            setting.Url = test[0];
            setting.CachePath = test[1];
            setting.Username = test[2];
            setting.Password = test[3];
            if (setting.Url == null ||
                setting.CachePath == null ||
                setting.Username == null ||
                setting.Password == null)
            {
                return;
            }
            WriteSettings(setting);
            LoadSettings(setting);
            PostChoseHelper.POSTID = currentPost;
            this.path = PostChoseHelper.FILEPATH;
        }

        /*
         * 读取配置
         */
        private Setting ReadSettings()
        {
            PostChoseHelper.POSTID = -1; // 重置博客，防止将settings提交到博客
            string settingsFile = "./settings.json";
            StreamReader sr = new StreamReader(settingsFile);
            string settingsText = sr.ReadToEnd();
            sr.Close();
            // this.path = settingsFile;
            JObject jo = (JObject)JsonConvert.DeserializeObject(settingsText);
            if (jo.Count < 4)
            {
                MessageBox.Show("请修改配置文件");
                return new Setting();
            }
            Setting settings = new Setting();
            settings.Url = jo["Url"].ToString();
            settings.CachePath = jo["CachePath"].ToString();
            settings.Username = jo["Username"].ToString();
            settings.Password = jo["Password"].ToString();
            if (settings.Url == "" ||
                settings.CachePath == "" ||
                settings.Username == "" ||
                settings.Password == "")
            {
                MessageBox.Show("请修改配置文件");
            }
            return settings;
        }

        private void WriteSettings(Setting setting)
        {
            string settingsFile = "./settings.json";
            this.path = settingsFile;
            string settingsContent = JsonConvert.SerializeObject(setting);
            StreamWriter sr = new StreamWriter(settingsFile, append: false);
            sr.Write(settingsContent);
            sr.Close();
        }
        
        /*
         * 加载配置
         */
        private void LoadSettings(Setting setting)
        {
            ConstantUtil.CACHEPATH = setting.CachePath;
            ConstantUtil.URL = setting.Url;
            ConstantUtil.USERNAME = setting.Username;
            ConstantUtil.PASSWORD = setting.Password;
        }

        /*
         * 初始化所有博客的编辑位置
         */
        private void InitAllPost()
        {
            List<PostInfo> posts = postServices.GetAllPost();
            List<PostEditInfo> postEditInfo = new List<PostEditInfo>();
            foreach (PostInfo post in posts)
            {
                postEditInfo.Add(new PostEditInfo(post.Id, 0));
            }
            string jsonPosts = JsonConvert.SerializeObject(postEditInfo);
            string tmpPath = ConstantUtil.EDITPOSCACHE;
            FileStream fs = new FileStream(tmpPath, FileMode.OpenOrCreate, FileAccess.Read);
            fs.Close();
            StreamWriter sw = new StreamWriter(tmpPath);
            sw.Write(jsonPosts);
            sw.Flush();
            sw.Close();
        }

        /*
         * 通过ID更新
         */
        private void WritePostEditPosById()
        {
            Dictionary<int, int> oldInfo = ReadPostEditInfo();

            oldInfo[PostChoseHelper.POSTID] = editPos;

            List<PostEditInfo> postEditInfo = new List<PostEditInfo>();
            foreach (var post in oldInfo)
            {
                postEditInfo.Add(new PostEditInfo(post.Key, post.Value));
            }
            string jsonPosts = JsonConvert.SerializeObject(postEditInfo);
            string tmpPath = ConstantUtil.EDITPOSCACHE;

            StreamWriter sw = new StreamWriter(tmpPath, false);
            sw.Write(jsonPosts);
            sw.Flush();
            sw.Close();
        }

        /*
         * 读取所有博客编辑位置
         */
        private Dictionary<int, int> ReadPostEditInfo()
        {
            StreamReader sr = new StreamReader(ConstantUtil.EDITPOSCACHE);
            string jsoninfo = sr.ReadToEnd();
            sr.Close();
            JArray jo = (JArray)JsonConvert.DeserializeObject(jsoninfo);
            Dictionary<int, int> postEditInfos = new Dictionary<int, int>();
            foreach (var postEditInfo in jo)
            {
                postEditInfos.Add(int.Parse(postEditInfo["Id"].ToString()), 
                    int.Parse(postEditInfo["LastEditPos"].ToString()));
            }
            return postEditInfos;
        }

        /*
         * 根据ID读取博客编辑位置
         */
        private int ReadPostEditPosById(int id)
        {
            var posts = ReadPostEditInfo();
            if (!posts.ContainsKey(id))
            {
                posts.Add(PostChoseHelper.POSTID, 0);
            }
            return posts[id];
        }

        /*
         * 初始化编辑位置
         */
        private void initEditPostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitAllPost();
            ReadPostEditInfo();
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (editPosts.Count > 1 && PostChoseHelper.POSTID > 0)
            {
                saveToolStripMenuItem_Click(sender, e);
            }

            KeyValuePair<string, string> item = (KeyValuePair<string, string>)this.comboBox1.SelectedItem;
            String filePath = ConstantUtil.CACHEPATH + item.Value;
            setChosePost(int.Parse(item.Key), item.Value, filePath);
            setTextBox();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.Text = this.Text = "AutoBlog" + " " + PostChoseHelper.POSTID + "-" + PostChoseHelper.TITLE + " unsaved";
            isSaved = false;
            text = textBox1.Text;
            editPos = textBox1.SelectionStart;
        }
    }
}
