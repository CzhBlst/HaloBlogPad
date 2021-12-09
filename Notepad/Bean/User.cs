using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Bean
{
    class User
    {

        private string user;
        private string pass;

        public User(string username, string password)
        {
            this.user = username;
            this.pass = password;
        }

        public string username { get => user; set => user = value; }
        public string password { get => pass; set => pass = value; }
    }
}
