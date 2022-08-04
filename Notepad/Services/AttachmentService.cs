using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    /// <summary>
    /// 附件管理
    /// </summary>
    class AttachmentService
    {
        private string token;
        
        public AttachmentService(string token)
        {
            this.token = token;
        }
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>文件引用路径</returns>
        public string UploadAttachment(string filename)
        {
            RestClient client = RestClientFactory.GetRestClient(token);
            string uri = @"/api/admin/attachments/upload";
            var request = new RestRequest(uri, Method.POST);
            if (File.Exists(filename))
            {
                request.AddFile("file", filename);
                // request.AddParameter("file", filename);
            }
            IRestResponse restResponse = client.Execute(request);
            if (restResponse.IsSuccessful)
            {
                var json = restResponse.Content;
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                string path = jo["data"]["path"].ToString();
                return path;
            }
            return "error";
        }
    }
}
