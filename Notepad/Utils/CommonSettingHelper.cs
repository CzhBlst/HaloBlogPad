using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notepad.Bean;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Notepad.Utils
{
    public static class CommonSettingHelper
    {
        public static string BLOGCACHE;
        public static string ATTACHMENTCACHE;
        public static bool AUTOSAVE;
        public static bool AUTOLOGIN;
        public static int LOGINCHECKINTERVAL;
        public static int SAVEINTERVAL;

        /*
          * 读取配置
        */
        public static CommonSetting ReadSettings()
        {
            PostChoseHelper.POSTID = -1; // 重置博客，防止将settings提交到博客
            string settingsFile = "./config/CommonSettings.json";
            StreamReader sr = new StreamReader(settingsFile);
            string settingsText = sr.ReadToEnd();
            sr.Close();
            // this.path = settingsFile;
            JObject jo = (JObject)JsonConvert.DeserializeObject(settingsText);
            if (jo.Count < 6)
            {
                MessageBox.Show("Setting设置异常");
                return new CommonSetting();
            }
            CommonSetting settings = new CommonSetting();
            settings.BlogCache = jo["BlogCache"].ToString();
            settings.AttachmentCache = jo["AttachmentCache"].ToString();
            settings.AutoLogin = Boolean.Parse(jo["AutoLogin"].ToString());
            settings.AutoSave = Boolean.Parse(jo["AutoSave"].ToString());
            settings.LoginCheckInterval = int.Parse(jo["LoginCheckInterval"].ToString());
            settings.SaveInterval = int.Parse(jo["SaveInterval"].ToString());
            if (settings.BlogCache == "" ||
                settings.AttachmentCache == ""
                )
            {
                MessageBox.Show("Setting设置异常");
            }
            return settings;
        }

        public static void WriteSettings(CommonSetting setting)
        {
            string settingsFile = "./config/CommonSettings.json";
            // this.path = settingsFile;
            string settingsContent = JsonConvert.SerializeObject(setting);
            StreamWriter sr = new StreamWriter(settingsFile, append: false);
            sr.Write(settingsContent);
            sr.Close();
        }

        /*
         * 加载配置
         */
        public static void LoadSettings(CommonSetting setting)
        {
            ConstantUtil.BLOGCACHE = setting.BlogCache;
            ConstantUtil.ATTACHMENTCACHE = setting.AttachmentCache;
            ConstantUtil.AUTOSAVE = setting.AutoSave;
            ConstantUtil.AUTOLOGIN = setting.AutoLogin;
            ConstantUtil.LOGINCHECKINTERVAL = setting.LoginCheckInterval;
            ConstantUtil.SAVEINTERVAL = setting.SaveInterval;
        }

    }
}
