using Notepad.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad.Utils
{
    class AttachmentUtil
    {

        public static string InsertImgFromClipBoard(AttachmentService attachmentService)
        {
            System.Drawing.Image tmp = Clipboard.GetImage();
            string filename = ConstantUtil.ATTACHMENTCACHE + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            tmp.Save(filename);
            string path = attachmentService.UploadAttachment(filename);
            if (path.Equals("error"))
            {
                return "Upload Error";
            }
            string completePath = "![image](" + ConstantUtil.URL + path + ")";
            return completePath;
        }

        /// <summary>
        /// 删除一周之前的附件缓存
        /// </summary>
        public static void DeleteOldCache()
        {
            string dir = ConstantUtil.ATTACHMENTCACHE;
            try
            {
                if (!Directory.Exists(dir))
                {
                    return;
                }
                var now = DateTime.Now;
                foreach (var f in Directory.GetFileSystemEntries(dir).Where(f => File.Exists(f)))
                {
                    var filetime = File.GetCreationTime(f);
                    var elapsedTicks = now.Ticks - filetime.Ticks;
                    var elapsedSpan = new TimeSpan(elapsedTicks);

                    if (elapsedSpan.TotalDays > 7) File.Delete(f);
                }
            }
            catch (Exception)
            {

            }
        }   
    }
}
