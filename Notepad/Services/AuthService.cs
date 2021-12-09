using Newtonsoft.Json;
using Notepad.Bean;
using Notepad.Utils;
using RestSharp;
using System;


namespace Notepad.Services
{
    class AuthService
    {
        /**
         * Login，返回用户token，Halo默认的过期时间86400
         */
        public string Login()
        {
            string token;
            string uri = @"/api/admin/login";
            User user = new User(ConstantUtil.USERNAME, ConstantUtil.PASSWORD);
            var contentData = JsonConvert.SerializeObject(user);

            var client = new RestClient(ConstantUtil.URL);
            var request = new RestRequest(uri, Method.POST);
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
