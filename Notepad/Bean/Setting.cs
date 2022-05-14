using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Bean
{
    public class Setting
    {
        private string url;
        private string username;
        private string password;

        public Setting()
        {
        }

        public Setting(string url, string username, string password)
        {
            this.url = url;
            this.username = username;
            this.password = password;
        }

        public string Url { get => url; set => url = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
    }
}
