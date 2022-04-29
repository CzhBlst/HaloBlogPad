using Notepad.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Services
{
    class StaticStorageService
    {
        private String token;
        public StaticStorageService(String token)
        {
            this.token = token;
        }

        /*
         * 上传附件测试
         */
        public Boolean uploadFile(String filename)
        {
            RestClient client = new RestClient(ConstantUtil.URL);
            string uri = @"/api/admin/statics/upload";
            var request = new RestRequest(uri, Method.POST);
            request.AddParameter("admin_token", token);
            request.AddFile("test", filename);
            IRestResponse restResponse = client.Execute(request);
            string statusCode = restResponse.StatusCode.ToString();
            if (restResponse.IsSuccessful)
            {
                return true;
            }
            return false;
        }
    }
}
