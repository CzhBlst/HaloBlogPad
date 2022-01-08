using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Bean
{
    internal class LoginInfo
    {
        private String using_token;
        private DateTime access_time;
        private DateTime expire_time;
        private String refresh_token;
        private String user;

        public LoginInfo()
        {
            
        }

        public string UsingToken { get => using_token; set => using_token = value; }
        public DateTime AccessTime { get => access_time; set => access_time = value; }
        public DateTime ExpireTime { get => expire_time; set => expire_time = value; }
        public string RefreshToken { get => refresh_token; set => refresh_token = value; }
        public string User { get => user; set => user = value; }
    }
}
