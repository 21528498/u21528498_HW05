using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using u21528498_HW05.Models;

namespace u21528498_HW05.ViewModels
{
    public class AuthorVM
    {
        public List<Books> books { get; set; }
        public List<string> types { get; set; }
        public List<string> authors { get; set; }
    }
}