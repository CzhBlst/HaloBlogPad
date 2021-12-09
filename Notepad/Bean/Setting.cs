using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Bean
{
    class Setting
    {

        private string url;
        private string username;
        private string password;
        private string cachePath;

        public Setting()
        {
        }

        public Setting(string url, string username, string password, string cachePath)
        {
            this.url = url;
            this.username = username;
            this.password = password;
            this.cachePath = cachePath;
        }

        public string Url { get => url; set => url = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string CachePath { get => cachePath; set => cachePath = value; }

    }
}
