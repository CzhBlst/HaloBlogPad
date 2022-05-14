using HZH_Controls.Forms;
using Notepad.Bean;
using Notepad.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CommonSetting setting = new CommonSetting();
            bool autoSave = this.SaveCheckBox.Checked;
            bool autoLogin = this.LoginCheckBox.Checked;
            int saveInterval = int.Parse(this.SaveIntervalBox.Text) * 1000;
            int loginInterval = int.Parse(this.LoginIntervalBox.Text) * 1000;
            string blogCache = this.BlogBox.Text;
            string attchmentCache = this.AttachmentBox.Text;
            if (saveInterval < 1 || loginInterval < 1)
            {
                FrmTips.ShowTipsError(this, "检查间隔至少为1s");
                return;
            }
            if (blogCache.Trim() == "" || attchmentCache.Trim() == "")
            {
                FrmTips.ShowTipsError(this, "缓存位置不能为空");
                return;
            }
            setting.BlogCache = blogCache;
            setting.SaveInterval = saveInterval;
            setting.AutoLogin = autoLogin;
            setting.AutoSave = autoSave;
            setting.LoginCheckInterval = loginInterval;
            setting.AttachmentCache = attchmentCache;
            CommonSettingHelper.WriteSettings(setting);
            CommonSettingHelper.LoadSettings(setting);
            this.Close();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            CommonSetting setting = CommonSettingHelper.ReadSettings();
            CommonSettingHelper.LoadSettings(setting);
            this.SaveCheckBox.Checked = setting.AutoSave;
            this.LoginCheckBox.Checked = setting.AutoLogin;
            this.SaveIntervalBox.Text = (setting.SaveInterval / 1000).ToString();
            this.LoginIntervalBox.Text = (setting.LoginCheckInterval / 1000).ToString();
            this.BlogBox.Text = setting.BlogCache;
            this.AttachmentBox.Text = setting.AttachmentCache;
        }

        private void IntTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == '\b' || (e.KeyChar >= '0' && e.KeyChar <= '9')))
            {
                e.Handled = true;
            }
        }
    }
}
