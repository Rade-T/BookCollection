using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BookCollection.Models;
using System.Data;

namespace BookCollection.DAO
{
    public class BookDAO
    {
        public const string CONNECTION_STR = @"data source=RADE-PC\SQLEXPRESS;
                                        initial catalog=bookCollection;
                                        integrated security=true";

        private LanguageDAO languageDAO = new LanguageDAO();
        private GenreDAO genreDAO = new GenreDAO();
        private BookAuthorsDAO bookAuthorsDAO = new BookAuthorsDAO();
        private AuthorDAO authorDAO = new AuthorDAO();

        public List<Book> Search(string searchTerm)
        {
            List<Book> books = new List<Book>();
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Select * from Books Where Title like @term";
                cmd.Parameters.Add(new SqlParameter("@term", searchTerm + "%"));
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "books");

                foreach (DataRow row in ds.Tables["books"].Rows)
                {
                    Book b = new Book();
                    b.Id = (int)row["Id"];
                    b.Title = (string)row["Title"];
                    b.ISBN = (string)row["ISBN"];
                    b.GenreID = (int)row["Genre"];
                    b.LanguageID = (int)row["bookLanguage"];
                    b.Genre = genreDAO.Find(b.GenreID);
                    b.Language = languageDAO.Find(b.LanguageID);
                    b.Description = (string)row["bookDescription"];
                    b.Authors = bookAuthorsDAO.FindAuthorsForId(b.Id);
                    books.Add(b);
                }
            }
            return books;
        }

        public List<Book> Read()
        {
            List<Book> books = new List<Book>();
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Select * from Books";
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "books");

                foreach (DataRow row in ds.Tables["books"].Rows)
                {
                    Book b = new Book();
                    b.Id = (int)row["Id"];
                    b.Title = (string)row["Title"];
                    b.ISBN = (string)row["ISBN"];
                    b.LanguageID = (int)row["bookLanguage"];
                    try
                    {
                        b.GenreID = (int)row["Genre"];
                        b.Genre = genreDAO.Find(b.GenreID);
                    }
                    catch
                    {
                        Genre EmptyGenre = new Genre();
                        EmptyGenre.Id = -1;
                        EmptyGenre.Name = "";
                        b.Genre = EmptyGenre;
                    }
                    b.Language = languageDAO.Find(b.LanguageID);
                    b.Description = (string)row["bookDescription"];
                    b.Authors = bookAuthorsDAO.FindAuthorsForId(b.Id);
                    List<Author> filled = new List<Author>();
                    foreach (var Author in b.Authors)
                    {
                        Author a = authorDAO.Find(Author.Id);
                        //a.Name = (string)row["name"];
                        //Nationality n = new Nationality();
                        //try
                        //{
                        //    n.Id = (int)row["NationalityID"];
                        //    n.Name = (string)row["NationalityName"];
                        //    a.Nationality = n;
                        //    a.NationalityID = n.Id;
                        //}
                        //catch
                        //{
                        //    Nationality Empty = new Nationality();
                        //    Empty.Id = -1;
                        //    Empty.Name = "";
                        //    a.Nationality = Empty;
                        //    a.NationalityID = -1;
                        //}
                        filled.Add(a);
                    }
                    b.Authors = filled;
                    books.Add(b);
                }
            }
            return books;
        }

        public int CurrentID()
        {
            int id = 0;
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT IDENT_CURRENT('Books') As ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "result");

                foreach (DataRow row in ds.Tables["result"].Rows)
                {
                    id = System.Convert.ToInt32(row["ID"]);
                }
            }
            return id;
        }

        public Book Find(int? id)
        {
            Book book = null;
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Select * from Books where id = @id";
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "books");

                foreach (DataRow row in ds.Tables["books"].Rows)
                {
                    book = new Book();
                    book.Id = (int)row["Id"];
                    book.Title = (string)row["Title"];
                    book.ISBN = (string)row["ISBN"];
                    book.LanguageID = (int)row["bookLanguage"];
                    try
                    {
                        book.GenreID = (int)row["Genre"];
                        book.Genre = genreDAO.Find(book.GenreID);
                    }
                    catch
                    {
                        Genre EmptyGenre = new Genre();
                        EmptyGenre.Id = -1;
                        EmptyGenre.Name = "";
                        book.Genre = EmptyGenre;
                        book.GenreID = -1;
                    }
                    book.Language = languageDAO.Find(book.LanguageID);
                    book.Description = (string)row["bookDescription"];
                    book.Authors = bookAuthorsDAO.FindAuthorsForId(book.Id);
                    List<Author> filled = new List<Author>();
                    foreach (var Author in book.Authors)
                    {
                        Author a = authorDAO.Find(Author.Id);
                        //a.Name = (string)row["name"];
                        //Nationality n = new Nationality();
                        //try
                        //{
                        //    n.Id = (int)row["NationalityID"];
                        //    n.Name = (string)row["NationalityName"];
                        //    a.Nationality = n;
                        //    a.NationalityID = n.Id;
                        //}
                        //catch
                        //{
                        //    Nationality Empty = new Nationality();
                        //    Empty.Id = -1;
                        //    Empty.Name = "";
                        //    a.Nationality = Empty;
                        //    a.NationalityID = -1;
                        //}
                        filled.Add(a);
                    }
                    book.Authors = filled;
                }
            }
            return book;
        }

        public void Create(Book b)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "Insert Into Books Values (@title, @ISBN, @genre, @language, @description)";
                cmd.Parameters.Add(new SqlParameter("@title", b.Title));
                if (b.ISBN == null)
                {
                    cmd.Parameters.Add(new SqlParameter("@ISBN", ""));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@ISBN", b.ISBN));
                }
                if (b.Genre.Id == -1)
                {
                    cmd.Parameters.Add(new SqlParameter("@genre", DBNull.Value));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@genre", b.GenreID));
                }
                cmd.Parameters.Add(new SqlParameter("@language", b.LanguageID));
                if (b.Description == null)
                {
                    cmd.Parameters.Add(new SqlParameter("@description", ""));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@description", b.Description));
                }

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.Write(e.StackTrace);
                }
                foreach (var author in b.Authors)
                {
                    BookAuthors bookAuthor = new BookAuthors();
                    bookAuthor.AuthorID = author.Id;
                    bookAuthor.BookID = b.Id;
                    bookAuthor.Name = author.Name;
                    bookAuthorsDAO.Create(bookAuthor);
                }
            }
        }

        public void Update(Book b)
        {
            bookAuthorsDAO.Delete(b);
            foreach (var author in b.Authors)
            {
                BookAuthors bookAuthor = new BookAuthors();
                bookAuthor.AuthorID = author.Id;
                bookAuthor.BookID = b.Id;
                bookAuthor.Name = author.Name;
                bookAuthorsDAO.Create(bookAuthor);
            }

            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Update Books Set Title=@title, ISBN=@ISBN, Genre=@genre, bookLanguage=@language, bookDescription=@description Where Id=@id";
                cmd.Parameters.Add(new SqlParameter("@title", b.Title));
                cmd.Parameters.Add(new SqlParameter("@id", b.Id));
                if (b.ISBN == null)
                {
                    cmd.Parameters.Add(new SqlParameter("@ISBN", ""));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@ISBN", b.ISBN));
                }
                if (b.GenreID == -1)
                {
                    cmd.Parameters.Add(new SqlParameter("@genre", DBNull.Value));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@genre", b.GenreID));
                }
                cmd.Parameters.Add(new SqlParameter("@language", b.LanguageID));
                if (b.Description == null)
                {
                    cmd.Parameters.Add(new SqlParameter("@description", ""));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@description", b.Description));
                }

                try
                {
                    cmd.ExecuteScalar();
                }
                catch (SqlException e)
                {
                    Console.Write(e.StackTrace);
                }
            }
        }

        public void Delete(Book b)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Delete From Books Where Id=@id";
                cmd.Parameters.Add(new SqlParameter("@id", b.Id));

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.Write(e.StackTrace);
                }
            }
        }
    }
}