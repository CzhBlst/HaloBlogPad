using Notepad.Bean;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Utils
{
    class PostUtil
    {
        // 文章保存时提交的默认状态
        public static String DefaultStatus = "DRAFT";
        /*
         * 将博客内容缓存到本地进行处理
         */
        public static void WriteToCache(Post post)
        {
            string cachePath = ConstantUtil.BLOGCACHE + post.title;
            if (!Directory.Exists(ConstantUtil.BLOGCACHE))
            {
                Directory.CreateDirectory(ConstantUtil.BLOGCACHE);
            }
            FileStream fs = new FileStream(cachePath, FileMode.OpenOrCreate, FileAccess.Read);
            fs.Close();
            StreamWriter sw = new StreamWriter(cachePath);
            sw.Write(post.originalContent);
            sw.Flush();
            sw.Close();
        }

        public static void setChosePost(int postid, string title, string filepath)
        {
            PostChoseHelper.POSTID = postid;
            PostChoseHelper.TITLE = title;
            PostChoseHelper.FILEPATH = filepath;
        }
    }
}
