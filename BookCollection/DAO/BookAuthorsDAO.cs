using BookCollection.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BookCollection.DAO
{
    public class BookAuthorsDAO
    {
        public const string CONNECTION_STR = @"data source=RADE-PC\SQLEXPRESS;
                                        initial catalog=bookCollection;
                                        integrated security=true";
        private AuthorDAO authorDAO = new AuthorDAO();

        public List<BookAuthors> Read()
        {
            List<BookAuthors> bookAuthorsList = new List<BookAuthors>();
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Select * from BookAuthors";
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "bookAuthorsList");

                foreach (DataRow row in ds.Tables["bookAuthorsList"].Rows)
                {
                    BookAuthors b = new BookAuthors();
                    b.AuthorID = (int)row["author"];
                    b.BookID = (int)row["book"];
                    b.Name = (string)row["name"];
                    bookAuthorsList.Add(b);
                }
            }
            return bookAuthorsList;
        }

        public List<BookAuthors> Find(int? id)
        {
            List<BookAuthors> bookAuthors = new List<BookAuthors>();
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Select * from BookAuthors where book = @book";
                cmd.Parameters.Add(new SqlParameter("@book", id));
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "bookAuthorsList");

                foreach (DataRow row in ds.Tables["bookAuthorsList"].Rows)
                {
                    BookAuthors b = new BookAuthors();
                    b.AuthorID = (int)row["author"];
                    b.BookID = (int)row["book"];
                    b.Name = (string)row["name"];
                    bookAuthors.Add(b);
                }
            }
            return bookAuthors;
        }

        public List<Author> FindAuthorsForId(int? id)
        {
            List<Author> Authors = new List<Author>();
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"select A.id, A.name 
                From Authors A, BookAuthors B
                Where B.book = @book And A.id = B.author";
                cmd.Parameters.Add(new SqlParameter("@book", id));
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "bookAuthorsList");

                foreach (DataRow row in ds.Tables["bookAuthorsList"].Rows)
                {
                    Author a = new Author();
                    a.Id= (int)row["id"];
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
                    Authors.Add(a);
                }
            }
            return Authors;
        }

        public void Create(BookAuthors b)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "Insert Into BookAuthors Values (@author, @book, @name)";
                cmd.Parameters.Add(new SqlParameter("@author", b.AuthorID));
                cmd.Parameters.Add(new SqlParameter("@book", b.BookID));
                cmd.Parameters.Add(new SqlParameter("@name", b.Name));

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

        public void Delete(Book b)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Delete From BookAuthors Where book=@book";
                cmd.Parameters.Add(new SqlParameter("@book", b.Id));

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