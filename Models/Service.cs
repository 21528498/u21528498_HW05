using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace u21528498_HW05.Models
{
    public class Service
    {
        public static string ConnectionString = "Data Source=LAPTOP-DSINSBOU\\SQLEXPRESS;Initial Catalog=Library;Integrated Security=True";
        private static SqlConnection currConnection;
        public static void openConnection()
        {
            
            try
            {
                String conString = ConnectionString;
                currConnection = new SqlConnection(conString);
                currConnection.Open();
            }
            catch (Exception exc)
            {

                
            }
            
        }
        public static bool closeConnection()
        {
            if (currConnection != null)
            {
                currConnection.Close();
            }
            return true;
        }

        public static List<Borrows> GetBorrows()
        {
            List<Borrows> borrows = new List<Borrows>();
            try
            {
                openConnection();
             
                SqlCommand myCommand = new SqlCommand("select borrows.borrowId, borrows.studentId,borrows.bookId, borrows.takenDate, borrows.broughtDate, students.name, students.surname from borrows, students where borrows.studentId = students.studentId order by borrows.borrowId Desc", currConnection);
                SqlDataReader myReader = myCommand.ExecuteReader();
                

                while (myReader.Read())
                {
                    Borrows borrow = new Borrows();
                    borrow.borrowsId = (int)myReader["borrowId"];
                    borrow.studentId = (int)myReader["studentId"];
                    borrow.Name = myReader["name"].ToString();
                    borrow.Surname = myReader["surname"].ToString();
                    borrow.bookId = (int)myReader["bookId"];
                    borrow.taken = myReader["takenDate"].ToString();
                    borrow.brought = myReader["broughtDate"].ToString();
                    borrows.Add(borrow);

                }
            }
            catch (Exception err)
            {

            }
            finally
            {
                closeConnection();
            }
            return borrows;

        }
        public static List<string> GetTypes()
        {
            List<string> types = new List<string>();
            try
            {
                openConnection();
                SqlCommand myCommand = new SqlCommand("select types.name from types", currConnection);
                SqlDataReader myReader = myCommand.ExecuteReader();
               

                while (myReader.Read())
                {
                    types.Add(myReader["name"].ToString());
                }
            }
            catch (Exception err)
            {

            }
            finally
            {
                closeConnection();
            }
            return types;
        }
        public static List<string> GetAuthors()
        {
            List<string> authors = new List<string>();
            try
            {
                openConnection();
                SqlCommand myCommand = new SqlCommand("select Authors.surname from Authors", currConnection);
                SqlDataReader myReader = myCommand.ExecuteReader();
                

                while (myReader.Read())
                {
                    authors.Add(myReader["surname"].ToString());
                }
            }
            catch (Exception err)
            {

            }
            finally
            {
                closeConnection();
            }
            return authors;
        }
        public static List<Books> GetBooks()
        {
            List<Borrows> borrow = GetBorrows();
            List<Books> books = new List<Books>();
            try
            {
                openConnection();
               
                SqlCommand myCommand = new SqlCommand("select books.bookId, books.name, Authors.surname,types.Name as [Type],books.pagecount,books.point from books,authors,types where books.authorid = authors.authorId and books.typeId = types.typeId", currConnection);
                SqlDataReader myReader = myCommand.ExecuteReader();
             

                while (myReader.Read())
                {
                    Books book = new Books();
                    book.bookID = (int)myReader["bookId"];
                    book.Name = myReader["name"].ToString();
                    book.AuthorSurname = myReader["surname"].ToString();
                    book.type = myReader["Type"].ToString();
                    book.pageCount = (int)myReader["pagecount"];
                    book.point = (int)myReader["point"];
                    var row = borrow.Where(x => x.bookId == book.bookID && x.brought == "").SingleOrDefault();
                    if (row == null)
                    {
                        
                        book.status = "Available";
                    }
                    else
                    {
                    
                        book.status = "Out";
                    }
                    books.Add(book);
                    
                }
            }
            catch (Exception err)
            {

            }
            finally
            {
                closeConnection();
            }
            return books;
        }

        public static int getNumberBooksBorrowed(int bookId)
        {
            List<Borrows> borr = GetBorrows();
            return borr.Where(x => x.bookId == bookId).Count();

        }

        public static List<Borrows> getBorrowsByID(int bookID)
        {
            return GetBorrows().Where(x => x.bookId == bookID).ToList(); ;

        }

 
        public static List<Students> getAllStudents()
        {
            List<Borrows> borr = GetBorrows();
            List<Students> students = new List<Students>();
            try
            {
                openConnection();
            
                SqlCommand myCommand = new SqlCommand("select students.studentId,students.name, students.surname,students.class,students.point from students", currConnection);
                SqlDataReader myReader = myCommand.ExecuteReader();
            

                while (myReader.Read())
                {
                    Students student = new Students();
                    student.StudentId = (int)myReader["studentId"];
                    student.stuName = myReader["name"].ToString();
                    student.stuSurname = myReader["surname"].ToString();
                    student.stuClass = myReader["class"].ToString();
                    student.point = (int)myReader["point"];
                    students.Add(student);
                   
                }
            }
            catch (Exception err)
            {

            }
            finally
            {
                closeConnection();
            }
            return students;
        }
        public static List<string> getAllClasses()
        {
            List<string> types = new List<string>();
            try
            {
                openConnection();
                SqlCommand myCommand = new SqlCommand("select DISTINCT students.class from students ", currConnection);
                SqlDataReader myReader = myCommand.ExecuteReader();
              

                while (myReader.Read())
                {
                    types.Add(myReader["class"].ToString());
                }
            }
            catch (Exception err)
            {

            }
            finally
            {
                closeConnection();
            }
            return types;
        }
        public static List<Students> GetSearchStudents(string searchText, string className)
        {

            List<Borrows> borrow = GetBorrows();
            List<Students> students = new List<Students>();
       
            string comm = "select students.studentId,students.name, students.surname,students.class,students.point from students ";
            bool where = false;
            bool text = false;
            bool classNameB = false;

            if (searchText != "" && where == false && text == false)
            {
                comm += "where students.name LIKE '%" + searchText + "%'";
                where = true;
                text = true;
            }
            if (searchText != "" && where && text == false)
            {
                comm += "and students.name LIKE '%" + searchText + "%'";
                text = true;
            }
            if (className != "NADA" && where && classNameB == false)
            {
                comm += "and students.class = '" + className + "'";
                classNameB = true;
            }
            if (className != "NADA" && where == false && classNameB == false)
            {
                comm += "where students.class = '" + className + "'";
                where = true;
                classNameB = true;
            }

            try
            {
                openConnection();
                
                SqlCommand myCommand = new SqlCommand(comm, currConnection);
                SqlDataReader myReader = myCommand.ExecuteReader();
               

                while (myReader.Read())
                {
                    Students student = new Students();
                    student.StudentId = (int)myReader["studentId"];
                    student.stuName = myReader["name"].ToString();
                    student.stuSurname = myReader["surname"].ToString();
                    student.stuClass = myReader["class"].ToString();
                    student.point = (int)myReader["point"];

                    students.Add(student);
                   
                }
            }
            catch (Exception err)
            {

            }
            finally
            {
                closeConnection();
            }
            return students;
        }
       
        public static Students bookedStu = new Students();
        public static Students getStudentById(int stuId)
        {
            List<Borrows> borr = GetBorrows();
            List<Students> students = new List<Students>();
            try
            {
                openConnection();
              
                SqlCommand myCommand = new SqlCommand("select students.studentId,students.name, students.surname,students.class,students.point from students where students.studentId = " + stuId, currConnection);
                SqlDataReader myReader = myCommand.ExecuteReader();
               
                while (myReader.Read())
                {
                    Students student = new Students();
                    student.StudentId = (int)myReader["studentId"];
                    student.stuName = myReader["name"].ToString();
                    student.stuSurname = myReader["surname"].ToString();
                    student.stuClass = myReader["class"].ToString();
                    student.point = (int)myReader["point"];
                    students.Add(student);
                   
                }
            }
            catch (Exception err)
            {

            }
            finally
            {
                closeConnection();
            }
            return students[0];
        }
        public static Books getBookByIdandFindBookedStu(int bookId)
        {
            List<Borrows> borrow = GetBorrows();
            List<Books> books = new List<Books>();
            try
            {
                openConnection();
               
                SqlCommand myCommand = new SqlCommand("select books.bookId, books.name, Authors.surname,types.Name as [Type],books.pagecount,books.point from books,authors,types where books.authorid = authors.authorId and books.typeId = types.typeId and books.bookId =" + bookId, currConnection);
                SqlDataReader myReader = myCommand.ExecuteReader();
                

                while (myReader.Read())
                {
                    Books book = new Books();
                    book.bookID = (int)myReader["bookId"];
                    book.Name = myReader["name"].ToString();
                    book.AuthorSurname = myReader["surname"].ToString();
                    book.type = myReader["Type"].ToString();
                    
                    book.pageCount = (int)myReader["pagecount"];
                    book.point = (int)myReader["point"];
                    var row = borrow.Where(x => x.bookId == book.bookID && x.brought == "").SingleOrDefault();
                    if (row == null)
                    {
                        
                        book.status = "Available";
                    }
                    else
                    {
                         
                        book.status = "Out";
                        bookedStu = getStudentById(row.studentId);
                    }
                    books.Add(book);
                   
                }
            }
            catch (Exception err)
            {

            }
            finally
            {
                closeConnection();
            }
            return books[0];
        }
        public static Books getBookById(int bookId)
        {
            List<Borrows> borrow = GetBorrows();
            List<Books> books = new List<Books>();
            try
            {
                openConnection();
               
                SqlCommand myCommand = new SqlCommand("select books.bookId, books.name, Authors.surname,types.Name as [Type],books.pagecount,books.point from books,authors,types where books.authorid = authors.authorId and books.typeId = types.typeId and books.bookId =" + bookId, currConnection);
                SqlDataReader myReader = myCommand.ExecuteReader();
              
                while (myReader.Read())
                {
                    Books book = new Books();
                    book.bookID = (int)myReader["bookId"];
                    book.Name = myReader["name"].ToString();
                    book.AuthorSurname = myReader["surname"].ToString();
                    book.type = myReader["Type"].ToString();
                  
                    book.pageCount = (int)myReader["pagecount"];
                    book.point = (int)myReader["point"];
                    var row = borrow.Where(x => x.bookId == book.bookID && x.brought == "").SingleOrDefault();
                    if (row == null)
                    {
                       
                        book.status = "Available";
                    }
                    else
                    {
                        
                        book.status = "Out";

                    }
                    books.Add(book);
                   
                }
            }
            catch (Exception err)
            {

            }
            finally
            {
                closeConnection();
            }
            return books[0];
        }
        public static void BorrowBook(int bookId, int studentId)
        {

            try
            {
                openConnection();
                string command = "Insert Into Borrows values(" + studentId + "," + bookId + ",'" + DateTime.Now + "',null)";
                SqlCommand myCommand = new SqlCommand(command, currConnection);
                myCommand.ExecuteNonQuery();

            }
            catch (Exception err)
            {

            }
            finally
            {
                closeConnection();
            }

        }
        public static void ReturnBook(int bookId, int studentId)
        {

            try
            {
                openConnection();
                string command = "UPDATE Borrows SET Borrows.broughtDate = '" + DateTime.Now + "' WHERE Borrows.studentId =" + studentId + " and Borrows.bookId = " + bookId;
                SqlCommand myCommand = new SqlCommand(command, currConnection);
                myCommand.ExecuteNonQuery();

            }
            catch (Exception err)
            {

            }
            finally
            {
                closeConnection();
            }

        }

    }
}