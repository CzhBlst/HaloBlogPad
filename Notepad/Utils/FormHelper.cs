using Microsoft.Win32;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad.Utils
{
    class FormHelper
    {
        /*
         * 注册快捷键,需要窗口句柄
         */
        public static void HotKeyRegister(IntPtr handle)
        {
            HotKey.RegisterHotKey(handle, 100, HotKey.KeyModifiers.Alt, Keys.Q);
            HotKey.RegisterHotKey(handle, 101, HotKey.KeyModifiers.Alt, Keys.V);
        }
    }
}
