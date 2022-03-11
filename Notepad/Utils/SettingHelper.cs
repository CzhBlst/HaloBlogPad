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
    class SettingHelper
    {
        public static string CACHEPATH;
        public static string URL;
        public static string USERNAME;
        public static string PASSWORD;
        private string path;

        /*
          * 读取配置
        */
        public static Setting ReadSettings()
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

        public static void WriteSettings(Setting setting)
        {
            string settingsFile = "./settings.json";
            // this.path = settingsFile;
            string settingsContent = JsonConvert.SerializeObject(setting);
            StreamWriter sr = new StreamWriter(settingsFile, append: false);
            sr.Write(settingsContent);
            sr.Close();
        }

        /*
         * 加载配置
         */
        public static void LoadSettings(Setting setting)
        {
            ConstantUtil.CACHEPATH = setting.CachePath;
            ConstantUtil.URL = setting.Url;
            ConstantUtil.USERNAME = setting.Username;
            ConstantUtil.PASSWORD = setting.Password;
        }

    }
}
