using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            int processCount = 0;
            Process[] pa = Process.GetProcesses();//获取当前进程数组。
            foreach (Process PTest in pa)
            {
                if (PTest.ProcessName == Process.GetCurrentProcess().ProcessName)
                {
                    processCount += 1;
                }
            }
            if (processCount > 1)
            {
                //如果程序已经运行，则给出提示。并退出本进程。
                DialogResult dr;
                dr = MessageBox.Show(Process.GetCurrentProcess().ProcessName + "程序已经在运行！", "退出程序", MessageBoxButtons.OK, MessageBoxIcon.Error);//可能你不需要弹出窗口，在这里可以屏蔽掉
                return; //Exit;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
