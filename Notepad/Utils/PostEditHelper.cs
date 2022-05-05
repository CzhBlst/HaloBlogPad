using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notepad.Bean;
using Notepad.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Utils
{
    class PostEditHelper
    {
        /*
        * 初始化所有博客的编辑位置
        */
        public static bool InitAllPost(PostService postServices)
        {
            bool res = true;
            try
            {
                List<PostInfo> posts = postServices.GetAllPost();
                List<PostEditInfo> postEditInfo = new List<PostEditInfo>();
                foreach (PostInfo post in posts)
                {
                    postEditInfo.Add(new PostEditInfo(post.Id, 0));
                }
                string jsonPosts = JsonConvert.SerializeObject(postEditInfo);
                string tmpPath = ConstantUtil.EDITPOSCACHE;
                FileStream fs = new FileStream(tmpPath, FileMode.OpenOrCreate, FileAccess.Read);
                fs.Close();
                StreamWriter sw = new StreamWriter(tmpPath);
                sw.Write(jsonPosts);
                sw.Flush();
                sw.Close();
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }

        /*
         * 通过ID更新
         */
        public static void WritePostEditPosById(int selectionStart)
        {
            Dictionary<int, int> oldInfo = ReadPostEditInfo();

            // oldInfo[PostChoseHelper.POSTID] = this.textBox1.SelectionStart;
            oldInfo[PostChoseHelper.POSTID] = selectionStart;
            List<PostEditInfo> postEditInfo = new List<PostEditInfo>();
            foreach (var post in oldInfo)
            {
                postEditInfo.Add(new PostEditInfo(post.Key, post.Value));
            }
            string jsonPosts = JsonConvert.SerializeObject(postEditInfo);
            string tmpPath = ConstantUtil.EDITPOSCACHE;

            StreamWriter sw = new StreamWriter(tmpPath, false);
            sw.Write(jsonPosts);
            sw.Flush();
            sw.Close();
        }

        /*
         * 读取所有博客编辑位置
         */
        public static Dictionary<int, int> ReadPostEditInfo()
        {
            StreamReader sr = new StreamReader(ConstantUtil.EDITPOSCACHE);
            string jsoninfo = sr.ReadToEnd();
            sr.Close();
            JArray jo = (JArray)JsonConvert.DeserializeObject(jsoninfo);
            Dictionary<int, int> postEditInfos = new Dictionary<int, int>();
            foreach (var postEditInfo in jo)
            {
                postEditInfos.Add(int.Parse(postEditInfo["Id"].ToString()),
                    int.Parse(postEditInfo["LastEditPos"].ToString()));
            }
            return postEditInfos;
        }

        /*
         * 根据ID读取博客编辑位置
         */
        public static int ReadPostEditPosById(int id)
        {
            var posts = ReadPostEditInfo();
            if (!posts.ContainsKey(id))
            {
                posts.Add(PostChoseHelper.POSTID, 0);
            }
            return posts[id];
        }

    }
}
