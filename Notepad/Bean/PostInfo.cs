using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Bean
{
    class PostInfo
    {

        private int id;
        private string title;
        private string status;
        private long editTime;
        private long createTime;

        public PostInfo(int id, string title, string status, long editTime, long createTime)
        {
            this.Id = id;
            this.Title = title;
            this.Status = status;
            this.EditTime = editTime;
            this.CreateTime = createTime;
        }

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Status { get => status; set => status = value; }
        public long EditTime { get => editTime; set => editTime = value; }
        public long CreateTime { get => createTime; set => createTime = value; }
    }
}
