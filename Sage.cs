using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_Homework_3
{
    public class Sage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public Sage()
        {
            Birthday = DateTime.Now;
            Name = "";
            Books = new List<Book>();
        }
    }
}
