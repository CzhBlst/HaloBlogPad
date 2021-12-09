using Newtonsoft.Json;
using Notepad.Bean;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Services
{
    class AuthService
    {
        /**
         * Login，返回用户token，使用Halo默认的过期时间86400
         */
        public string Login()
        {
            string token;
            string url = @"http://121.4.203.203:8090/api/admin/login";
            // string contentData = "{'username': 'caozihao107@163.com' ,'password' : 'xiaoyuaidahai'}";
            User user = new User("caozihao107@163.com", "xiaoyuaidahai");
            var contentData = JsonConvert.SerializeObject(user);

            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(contentData);

            IRestResponse restResponse = client.Execute(request);

            string statusCode = restResponse.StatusCode.ToString();
            if (restResponse.IsSuccessful)
            {
                var content = restResponse.Content;
                var contents = content.Split(new[] { "," }, StringSplitOptions.None);
                var jsonObj = JsonConvert.DeserializeObject(content);
                int indexOfToken = contents[3].IndexOf("access_token") + 15;
                token = contents[3].Substring(indexOfToken, 32);
                return token;
            }
            else
            {
                return "获取AccessToken失败";
            }
        }
    }
}
