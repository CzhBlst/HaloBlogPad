using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notepad.Bean;
using Notepad.Utils;
using RestSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Notepad.Services
{
    class AuthService
    {
        /**
         * Login，返回用户token，Halo默认的过期时间86400
         */
        public LoginInfo Login()
        {
            RestClient client = new RestClient(ConstantUtil.URL);
            LoginInfo token = new LoginInfo();
            string uri = @"/api/admin/login";
            User user = new User(ConstantUtil.USERNAME, ConstantUtil.PASSWORD);
            var contentData = JsonConvert.SerializeObject(user);

            // var client = new RestClient(ConstantUtil.URL);
            var request = new RestRequest(uri, Method.POST);
            request.AddJsonBody(contentData);

            IRestResponse restResponse = client.Execute(request);

            string statusCode = restResponse.StatusCode.ToString();
            if (restResponse.IsSuccessful)
            {
                var content = restResponse.Content;
                JObject jo = (JObject)JsonConvert.DeserializeObject(content);
                token.UsingToken = jo["data"]["access_token"].ToString();
                token.AccessTime = System.DateTime.Now;
                token.ExpireTime = System.DateTime.Now.AddSeconds(86400);
                token.RefreshToken = jo["data"]["refresh_token"].ToString();
                writeLoginToken(token);
                return token;
            }
            else
            {
                return null;
            }
        }

        /*
         * Login Async，异步登录
         */

        public async Task<LoginInfo> LoginAsync()
        {
            RestClient client = new RestClient(ConstantUtil.URL);
            LoginInfo token = new LoginInfo();
            string uri = @"/api/admin/login";
            User user = new User(ConstantUtil.USERNAME, ConstantUtil.PASSWORD);
            var contentData = JsonConvert.SerializeObject(user);
            var request = new RestRequest(uri, Method.POST);  
            request.AddJsonBody(contentData);
            // IRestResponse restResponse = client.Execute(request);
            IRestResponse restResponse = await client.ExecuteAsync(request);

            string statusCode = restResponse.StatusCode.ToString();
            if (restResponse.IsSuccessful)
            {
                var content = restResponse.Content;
                JObject jo = (JObject)JsonConvert.DeserializeObject(content);
                token.UsingToken = jo["data"]["access_token"].ToString();
                token.AccessTime = System.DateTime.Now;
                token.ExpireTime = System.DateTime.Now.AddSeconds(86400);
                token.RefreshToken = jo["data"]["refresh_token"].ToString();
                writeLoginToken(token);
                return token;
            }
            else
            {
                return null;
            }
        }

        public void writeLoginToken(LoginInfo token)
        {
            string tokenFile = "./token.json";
            string tokenContent = JsonConvert.SerializeObject(token);
            using (StreamWriter sr = new StreamWriter(tokenFile, append: false))
            {
                sr.Write(tokenContent);
                sr.Flush();
                sr.Close();
            }

        }

        /*
         * 通过检测token及其过期时间来判断是否需要重新登录
         */
        public Boolean checkToken(LoginInfo token)
        {
            string tokenFile = "./token.json";
            StreamReader sr = new StreamReader(tokenFile);
            string tokenContent = sr.ReadToEnd();
            sr.Close();
            JObject jo = (JObject)JsonConvert.DeserializeObject(tokenContent);

            if (DateTime.Now > token.ExpireTime)
            {
                return false;
            } 
            else
            {
                return true;
            }
        }

        /*
         * 获取上次登录token
         */
        public LoginInfo getLastLoginToken()
        {
            string tokenFile = "./token.json";
            try
            {
                using (StreamReader sr = new StreamReader(tokenFile))
                {
                    string tokenContent = sr.ReadToEnd();
                    JObject jo = (JObject)JsonConvert.DeserializeObject(tokenContent);
                    LoginInfo token = new LoginInfo();
                    if (jo.Count < 4)
                    {
                        return null;
                    }
                    token.UsingToken = jo["UsingToken"].ToString();
                    token.AccessTime = DateTime.Parse(jo["AccessTime"].ToString());
                    token.ExpireTime = DateTime.Parse(jo["ExpireTime"].ToString());
                    token.RefreshToken = jo["RefreshToken"].ToString();
                    Console.WriteLine("Normal Login");
                    return token;
                }
            } 
            catch (IOException e) 
            {
                Console.WriteLine(e.StackTrace);
            }
            return null;
        }

        public async Task<LoginInfo> getLastLoginInfoAsync()
        {
            string tokenFile = "./token.json";
            StreamReader sr = new StreamReader(tokenFile);
            /*
             * 异步读取内容
             */
            string tokenContent = await sr.ReadToEndAsync();
            sr.Close();
            JObject jo = (JObject)JsonConvert.DeserializeObject(tokenContent);
            LoginInfo token = new LoginInfo();
            if (jo.Count < 4)
            {
                return null;
            }
            token.UsingToken = jo["UsingToken"].ToString();
            token.AccessTime = DateTime.Parse(jo["AccessTime"].ToString());
            token.ExpireTime = DateTime.Parse(jo["ExpireTime"].ToString());
            token.RefreshToken = jo["RefreshToken"].ToString();
            return token;
        }
    }
}
