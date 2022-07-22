﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Utils
{
    /// <summary>
    /// 提供单例Client
    /// </summary>
    static class RestClientFactory
    {
        static RestClient client;
        static string lastToken;

        static public RestClient GetRestClient(string token)
        {
            if (client == null || token != lastToken)
            {
                client = new RestClient(ConstantUtil.URL);
                client.AddDefaultHeader("ADMIN-Authorization", token);
            }
            return client;
        }
    }
}
