using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21528498_HW05.Models
{
    public class Books
    {
        public int bookID { get; set; }
        public string Name { get; set; }
        public int pageCount { get; set; }
        public int point { get; set; }
        public string AuthorSurname { get; set; }
        public string type { get; set; }
        public string status { get; set; }

    }
}