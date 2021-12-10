﻿using Notepad.Bean;
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
        /*
         * 将博客内容缓存到本地进行处理
         */
        public static void WriteToCache(Post post)
        {
            string cachePath = ConstantUtil.CACHEPATH + post.title;
            FileStream fs = new FileStream(cachePath, FileMode.OpenOrCreate, FileAccess.Read);
            fs.Close();
            StreamWriter sw = new StreamWriter(cachePath);
            sw.Write(post.originalContent);
            sw.Flush();
            sw.Close();
        }
    }
}