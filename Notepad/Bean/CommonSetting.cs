using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Bean
{
    public class CommonSetting
    {
        private string _BlogCache;
        private string _AttachmentCache;
        private bool _AutoSave;
        private bool _AutoLogin;
        private int _LoginCheckInterval;
        private int _SaveInterval;

        public string BlogCache { get => _BlogCache; set => _BlogCache = value; }
        public string AttachmentCache { get => _AttachmentCache; set => _AttachmentCache = value; }
        public bool AutoSave { get => _AutoSave; set => _AutoSave = value; }
        public bool AutoLogin { get => _AutoLogin; set => _AutoLogin = value; }
        public int LoginCheckInterval { get => _LoginCheckInterval; set => _LoginCheckInterval = value; }
        public int SaveInterval { get => _SaveInterval; set => _SaveInterval = value; }

        public CommonSetting() { }

        public CommonSetting(string blogCache, string attachmentCache, bool autoSave, bool autoLogin, int loginCheckInterval, int saveInterval)
        {
            _BlogCache = blogCache;
            _AttachmentCache = attachmentCache;
            _AutoSave = autoSave;
            _AutoLogin = autoLogin;
            _LoginCheckInterval = loginCheckInterval;
            _SaveInterval = saveInterval;
        }
    }
}
