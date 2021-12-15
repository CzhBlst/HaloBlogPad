using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Utils
{
    class LogHelper
    {
        private readonly ILogger<LogHelper> logger;

        public LogHelper(ILogger<LogHelper> logger)
        {
            this.logger = logger;
        }

        public void Test()
        {
            logger.LogDebug("测试日志文件写入");
        }
    }
}
