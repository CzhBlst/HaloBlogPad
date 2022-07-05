using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Utils
{
    class ConstantUtil
    {
        
        public static string URL = "";
        public static string USERNAME = "";
        public static string PASSWORD = "";
        public static string EDITPOSCACHE = @"./EditPosition.json";
        public static string ATTACHMENTCACHE = @"./cache/attachment/";
        public static string BLOGCACHE = @"./cache/blog/";
        public static bool AUTOSAVE = false;
        public static bool AUTOLOGIN = false;
        public static int LOGINCHECKINTERVAL = 10 * 1000;
        public static int SAVEINTERVAL = 60 * 1000;
        
    }
}
