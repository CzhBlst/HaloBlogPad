using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Notepad.Utils
{
    class JsonUtil
    {
        /*
         * 获取Json字符串中指定Key值
         */
        public static List<String> GetJsonValue(String jsonString, String key)
        {
            String pattern = $"\"{key}\":\"(.*?)\\\"";
            MatchCollection matches = Regex.Matches(jsonString, pattern, RegexOptions.IgnoreCase);
            List<string> lst = new List<string>();
            foreach (Match m in matches)
            {
                lst.Add(m.Groups[1].Value);
            }

            return lst;
        }
    }
}
