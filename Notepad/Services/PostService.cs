using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notepad.Bean;
using Notepad.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Services
{
    class PostService
    {
        private string token;
        private int pages;

        public PostService(string token)
        {
            this.token = token;
        }

        /*
         * 获取页数
         */
        public int GetPages()
        {
            string url = @"http://121.4.203.203:8090/api/admin/posts";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddParameter("admin_token", token);
            request.AddParameter("more", true);
            request.AddParameter("page", 0);
            IRestResponse restResponse = client.Execute(request);

            string statusCode = restResponse.StatusCode.ToString();
            if (restResponse.IsSuccessful)
            {
                var json = restResponse.Content;
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                pages = int.Parse(jo["data"]["pages"].ToString());
            }
            return pages;
        }

        /*
         * 获取特定页的博客
         */
        public List<PostInfo> GetPosts(int page)
        {
            List<PostInfo> postsList = new List<PostInfo>();
            string url = @"http://121.4.203.203:8090/api/admin/posts";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddParameter("admin_token", token);
            request.AddParameter("more", true);
            request.AddParameter("page", page - 1);

            IRestResponse restResponse = client.Execute(request);

            string statusCode = restResponse.StatusCode.ToString();
            if (restResponse.IsSuccessful)
            {
                var json = restResponse.Content;
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                var contents = jo["data"]["content"];
                foreach (var content in contents)
                {
                    if (content["status"].ToString().Equals("DRAFT") || content["status"].ToString().Equals("PUBLISHED"))
                    {
                        postsList.Add(new PostInfo(int.Parse(content["id"].ToString()),
                            content["title"].ToString(),
                            content["status"].ToString(),
                            long.Parse(content["editTime"].ToString()),
                            long.Parse(content["updateTime"].ToString())
                        ));
                    }
                }
            }
            else
            {
                return null;
            }
            return postsList;
        }

        /*
         * 获取特定博客详细内容
         */
        public Post GetPostById(int postId)
        {
            Post post = null;

            string url = "http://121.4.203.203:8090/api/admin/posts/" + postId;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddParameter("admin_token", token);
            IRestResponse restResponse = client.Execute(request);

            string statusCode = restResponse.StatusCode.ToString();
            if (restResponse.IsSuccessful)
            {
                var json = restResponse.Content;
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                string title = jo["data"]["title"].ToString();
                string originalContent = jo["data"]["originalContent"].ToString();
                post = new Post(title, originalContent);
            }
            return post;
        }

        /*
         * 添加新的博客
         */
        public string AddNewPost(string title)
        {
            string cachePath = ConstantUtil.CACHEPATH + title;
            StreamReader sr = new StreamReader(cachePath);
            string originalContent = sr.ReadToEnd();
            sr.Close();
            Post newPost = new Post(title, originalContent);
            var jsonPost = JsonConvert.SerializeObject(newPost);

            string url = @"http://121.4.203.203:8090";
            // string url = @"http://121.4.203.203:8090/api/admin/categories";
            var client = new RestClient(url);
            var request = new RestRequest(@"/api/admin/posts", Method.POST);
            client.AddDefaultHeader("ADMIN-Authorization", token);
            request.AddJsonBody(jsonPost);

            IRestResponse restResponse = client.Execute(request);

            string statusCode = restResponse.StatusCode.ToString();
            if (restResponse.IsSuccessful)
            {
                return "success";
            }
            return "error";
        }

        /**
         * 通过Id更新现有博客
         */
        public string UpdatePostById(int id, string path)
        {   
            var client = new RestClient(ConstantUtil.URL);
            var request = new RestRequest(@"/api/admin/posts/" + id, Method.PUT);
            client.AddDefaultHeader("ADMIN-Authorization", token);
            // 创建更新参数
            string title = GetPostById(id).title;
            StreamReader readStream = new StreamReader(path);
            string originalContent = readStream.ReadToEnd();
            readStream.Close();
            var jsonPost = JsonConvert.SerializeObject(new Post(title, originalContent));

            request.AddJsonBody(jsonPost);

            IRestResponse restResponse = client.Execute(request);

            string statusCode = restResponse.StatusCode.ToString();
            if (restResponse.IsSuccessful)
            {
                return "success";
            }
            return "error";
        }

    }
}
