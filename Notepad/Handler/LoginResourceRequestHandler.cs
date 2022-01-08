using CefSharp;
using Newtonsoft.Json;
using Notepad.Bean;
using Notepad.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Handler
{
    internal class LoginResourceRequestHandler : CefSharp.Handler.ResourceRequestHandler
    {
        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            Uri url;
            if (Uri.TryCreate(request.Url, UriKind.Absolute, out url) == false)
            {
                return CefReturnValue.Cancel;
            }
            User user = new User(ConstantUtil.USERNAME, ConstantUtil.PASSWORD);
            var contentData = JsonConvert.SerializeObject(user);
            // 登录之后如何获取Token?
            // 尝试使用OnResourceLoadComplete
            return CefReturnValue.Continue;
        }

        protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            var res = response;
        }
    }
}
