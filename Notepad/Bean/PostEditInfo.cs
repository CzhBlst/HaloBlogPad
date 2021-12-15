using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Bean
{
    class PostEditInfo
    {
        private int id;
        private int lastEditPos;

        public PostEditInfo(int id, int lastEditPos)
        {
            this.id = id;
            this.lastEditPos = lastEditPos;
        }

        public int Id { get => id; set => id = value; }
        public int LastEditPos { get => lastEditPos; set => lastEditPos = value; }
    }
}
