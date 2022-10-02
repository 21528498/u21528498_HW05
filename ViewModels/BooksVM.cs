using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using u21528498_HW05.Models;

namespace u21528498_HW05.ViewModels
{
    public class BooksVM
    {
        public List<Books> Books { get; set; }
        public List<Authors> authors { get; set; }
        public List<Students> students { get; set; }
        public List<string> types { get; set; }
        
    }
}