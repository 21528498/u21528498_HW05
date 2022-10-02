using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using u21528498_HW05.Models;
using u21528498_HW05.ViewModels;

namespace u21528498_HW05.Controllers
{
    public class HomeController : Controller
    {
        private SqlConnection myConnection = new SqlConnection(Globals.ConnectionString);
        // GET: Home
        int Id;
        public ActionResult Index()
        {
            ViewBag.SearchStatus = 1;
            ViewBag.Status = 1;
            AuthorVM books = new AuthorVM
            {
                books = Service.GetBooks(),
                types = Service.GetTypes(),
                authors = Service.GetAuthors()
            };
            return View(books);
        }

        public ActionResult StudentIndex(int i)
        {
            ViewBag.SearchStatus = 1;
            ViewBag.Status = 1;
            Session["BookID"] = i;
            Id = i;
            BooksVM book = new BooksVM
            {
                students = Service.getAllStudents(),
                types = Service.getAllClasses()

            };
           
            if (Service.getBookByIdandFindBookedStu(i).status == "Out")
            {
                Session["BookedStu"] = Service.bookedStu.StudentId;
            }
            else
            {
                Session["BookedStu"] = -1;
            }
            return View(book);
        }

        public ActionResult GetBook(int studentId)
        {
            Service.BorrowBook((int)Session["BookID"], studentId);
            
            Session["CallerId"] = (int)Session["BookID"];
            return RedirectToAction("CallerId", "Book");
        }
        public ActionResult ReturnBook(int studentId)
        {
            Service.ReturnBook((int)Session["BookID"], studentId);
            Session["CallerId"] = (int)Session["BookID"];
            return RedirectToAction("CallerId", "Book");
        }
        public ActionResult ComplexSearch(string searchText, string className)
        {
            ViewBag.SearchStatus = 1;
            ViewBag.Status = 1;
            BooksVM pass = new BooksVM
            {
                students = Service.GetSearchStudents(searchText, className),
                types = Service.getAllClasses(),

            };
            return View("StudentIndex", pass);
        }

        public ActionResult BookIndex(int bookId)
        {
            Books selectedBook = Service.getBookById(bookId);
            ViewBag.bookID = bookId;
            ViewBag.name = selectedBook.Name;
            ViewBag.status = selectedBook.status;
            ViewBag.borrowed = Service.getNumberBooksBorrowed(bookId);
            return View(Service.getBorrowsByID(bookId));
        }
        public ActionResult CallerId()
        {
            int id = (int)Session["CallerId"];
            Books selectedBook = Service.getBookById(id);
            ViewBag.bookID = id;
            ViewBag.name = selectedBook.Name;
            ViewBag.status = selectedBook.status;
            ViewBag.borrowed = Service.getNumberBooksBorrowed(id);
            return View("Index", Service.getBorrowsByID(id));
        }
    }
}