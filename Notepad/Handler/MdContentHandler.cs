using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.Handler;
using Notepad.Utils;

namespace Notepad.Handler
{
    internal class MdContentHandler : ResourceRequestHandler
    {

        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            Uri url;
            if (Uri.TryCreate(request.Url, UriKind.Absolute, out url) == false)
            {
                return CefReturnValue.Cancel;
            }
            var headers = request.Headers;
            headers["Authorization"] = TokenUtil.token; //传递进去认证Token
            request.Headers = headers;
            return CefReturnValue.Continue;
        }
    }
}
