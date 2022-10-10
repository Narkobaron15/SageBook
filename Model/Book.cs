using System.Collections.Generic;

namespace ADO.NET_Homework_3.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Sage> Sages { get; set; }

        public Book()
        {
            Title = "";
            Sages = new List<Sage>();
        }
    }
}