using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_Homework_3.Model
{
    public struct BookSage
    {
        public BookSage() : this(0, 0) { }
        public BookSage(int BookId, int SageId)
        {
            this.BookId = BookId;
            this.SageId = SageId;
        }

        public int BookId { get; set; }
        public int SageId { get; set; }
    }
}
