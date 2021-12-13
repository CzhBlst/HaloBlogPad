using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using HeyRed.MarkdownSharp;
using HZH_Controls.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notepad.Bean;
using Notepad.Services;
using Notepad.Utils;
using NotePad;

namespace Notepad
{
    public partial class Form1 : Form
    {
        String path = String.Empty;
        String token;
        int currentPost;
        Boolean isLogin = false;
        Boolean preViewMD = false;
        PostService postServices;
        

        public Form1() => InitializeComponent();

        private string ReturnMessageFromFormat(string type)
        {
            switch (type)
            {
                case "ino":
                    return "Arduino";
                    break;
                case "cs":
                    return "C#";
                    break;
                case "cpp":
                    return "C++";
                    break;
                case "c":
                    return "C";
                    break;
                case "btwo":
                    return "Braintwo";
                    break;
                case "json":
                    return "Json";
                    break;
                case "xml":
                    return "Xml";
                    break;
                case "html":
                    return "HTML";
                    break;
                case "css":
                    return "CSS";
                    break;
                case "js":
                    return "JavaScript";
                    break;
                case "md":
                    return "MarkDown";
                    break;
                default:
                    return "Text";
                    break;

            }
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthService authService = new AuthService();
            token = authService.Login();
            if (token.Equals("error"))
            {
                MessageBox.Show("登录失败");
            }
            else
            {
                MessageBox.Show("登录成功，连接到博客服务...");
                postServices = new PostService(token);
                isLogin = true;
            }
        }

        private void getBlogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form postsForm = new Form3(token);
            postsForm.ShowDialog();
            if (!PostChoseHelper.TITLE.Equals(""))
            {
                Post post = postServices.GetPostById(PostChoseHelper.POSTID); // 获取所选博客详细信息
                PostUtil.WriteToCache(post); // 将博客内容读取到本地
                setTextBox();
               
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
                MessageBox.Show("新建博客失败");
            }
            setChosePost(postId, post.title, cachePath); // 设置当前Post信息
            MessageBox.Show("新建博客: " + PostChoseHelper.POSTID + ":" + PostChoseHelper.TITLE + 
                "\n缓存路径为: " + PostChoseHelper.FILEPATH);
            setTextBox();

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = File.ReadAllText(path = openFileDialog1.FileName);
                string[] SplitExtension = openFileDialog1.FileName.Split('.');
                labelFormat.Text = ReturnMessageFromFormat(SplitExtension[1]);
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
                if (PostChoseHelper.POSTID >= 0)
                {
                    MessageBox.Show(postServices.UpdatePostById(PostChoseHelper.POSTID, path));
                }
            }
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        private void exitPrompt()
        {
            DialogResult = MessageBox.Show("Do you want to save current file?",
                "Notepad",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
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
                textBox1.ScrollBars = ScrollBars.Both;
                wordWrapToolStripMenuItem.Checked = false;
            }
            else
            {
                textBox1.WordWrap = true;
                textBox1.ScrollBars = ScrollBars.Vertical;
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

        /*
         * 窗口加载时注册热键，并更改各个组件位置
         */
        private void Form1_Load_1(object sender, EventArgs e)
        {   
            HotKey.RegisterHotKey(Handle, 100, HotKey.KeyModifiers.Alt, Keys.Q);  
            HotKey.RegisterHotKey(Handle, 101, HotKey.KeyModifiers.Alt, Keys.V);
            ChangePannel();
            LoadSettings(ReadSettings());
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            ChangePannel();
        }

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
        // alt+V 切换文本和MarkDown预览模式
        private void ChangePannel()
        {
            if (preViewMD)
            {
                textBox1.Hide();
                webBrowser1.Show();
                string originalContent = textBox1.Text;
                Markdown md = new Markdown();
                string mdContent = md.Transform(originalContent);
                webBrowser1.DocumentText = mdContent;
                webBrowser1.Left = 5;
                webBrowser1.Width = this.Width - 30;
                webBrowser1.Top = 5;
                webBrowser1.Height = this.Height - 100;
            }
            else
            {
                webBrowser1.Hide();
                textBox1.Show();
                textBox1.Focus();
                textBox1.Left = 5;
                textBox1.Width = this.Width - 30;

                textBox1.Top = 5;
                textBox1.Height = this.Height - 100;
            }
        }

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

        private Setting ReadSettings()
        {
            PostChoseHelper.POSTID = -1; // 重置博客，防止将settings提交到博客
            string settingsFile = "./settings.json";
            StreamReader sr = new StreamReader(settingsFile);
            string settingsText = sr.ReadToEnd();
            sr.Close();
            // this.path = settingsFile;
            JObject jo = (JObject)JsonConvert.DeserializeObject(settingsText);
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

        private void LoadSettings(Setting setting)
        {
            ConstantUtil.CACHEPATH = setting.CachePath;
            ConstantUtil.URL = setting.Url;
            ConstantUtil.USERNAME = setting.Username;
            ConstantUtil.PASSWORD = setting.Password;
        }
    }
}
