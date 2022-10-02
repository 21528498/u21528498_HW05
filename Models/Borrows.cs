using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21528498_HW05.Models
{
    public class Borrows
    {
        public int borrowsId { get; set; }
        public string taken { get; set; }
        public string brought { get; set; }
        public int bookId { get; set; }
        public int studentId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}