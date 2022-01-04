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
        private string EditType = "MARKDOWN";

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

        public string id { get => Id; set => Id = value; }
        public string title { get => Title; set => Title = value; }
        public string originalContent { get => OriginalContent; set => OriginalContent = value; }
        public string editType { get => EditType; set => EditType = value; }

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
