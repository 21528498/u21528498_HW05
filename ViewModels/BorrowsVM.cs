using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using u21528498_HW05.Models;

namespace u21528498_HW05.ViewModels
{
    public class BorrowsVM
    {
        public List<Borrows> borrows { get; set; }
        public List<Books> books { get; set; }
    }
}