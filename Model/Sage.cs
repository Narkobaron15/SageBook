using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ADO.NET_Homework_3.Model
{
    public class Sage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        [Column(TypeName = "image")]
        public byte[]? Image { get; set; }

        public BitmapImage? GetBitmapImage() => Image is null ? null : GetBitmapImage(Image);

        public static BitmapImage GetBitmapImage(byte[] source)
        {
            using MemoryStream memoryStream = new(source);
            BitmapImage imageSource = new();

            imageSource.BeginInit();
            imageSource.StreamSource = memoryStream;
            imageSource.EndInit();

            return imageSource;
        }

        public virtual ICollection<Book> Books { get; set; }

        public Sage()
        {
            Birthday = DateTime.Now;
            Name = "";
            Books = new List<Book>();
        }
    }
}
