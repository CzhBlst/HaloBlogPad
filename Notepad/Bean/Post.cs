using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Bean
{
    class Post
    {
        private string Id;
        private string Title;
        private string OriginalContent;
        private string MdContent;
        private string EditType = "MARKDOWN";
        private string Status;

        public Post(string title, string originalContent)
        {
            this.Title = title;
            this.OriginalContent = originalContent;
        }

        public Post(string id, string title, string originalContent)
        {
            this.id = id;
            this.Title = title;
            this.OriginalContent = originalContent;
        }

        public Post(string id, string title, string originalContent, string mdContent)
        {
            this.id = id;
            this.Title = title;
            this.OriginalContent = originalContent;
            this.MdContent = mdContent;
        }

        public string id { get => Id; set => Id = value; }
        public string title { get => Title; set => Title = value; }
        public string originalContent { get => OriginalContent; set => OriginalContent = value; }
        public string editType { get => EditType; set => EditType = value; }
        public string mdContent { get => MdContent; set => MdContent = value; }
        public string status { get => Status; set => Status = value; }

        public override string ToString()
        {
            return "Post [" +
                "title=" +
                Title +
                ",content=" +
                OriginalContent +
                "]";
        }
    }
}
