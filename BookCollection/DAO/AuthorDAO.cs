using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BookCollection.Models;
using System.Data;

namespace BookCollection.DAO
{
    public class AuthorDAO
    {
        public const string CONNECTION_STR = @"data source=RADE-PC\SQLEXPRESS;
                                        initial catalog=bookCollection;
                                        integrated security=true";

        private NationalityDAO nationalityDAO = new NationalityDAO();

        public List<Author> Read()
        {
            List<Author> authors = new List<Author>();
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Select * from Authors";
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "authors");

                foreach (DataRow row in ds.Tables["authors"].Rows)
                {
                    Author a = new Author();
                    a.Id = (int)row["Id"];
                    a.Name = (string)row["name"];
                    try
                    {
                        int Nationality = (int)row["nationality"];
                        a.Nationality = nationalityDAO.Find((int)row["nationality"]);
                    }
                    catch
                    {
                        var EmptyNationality = new Nationality();
                        EmptyNationality.Name = "";
                        EmptyNationality.Id = -1;
                        a.Nationality = EmptyNationality;
                    }
                    authors.Add(a);
                }
            }
            return authors;
        }

        public Author Find(int? id)
        {
            Author a = null;
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Select * from Authors where id = @id";
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "authors");

                foreach (DataRow row in ds.Tables["authors"].Rows)
                {
                    a = new Author();
                    a.Id = (int)row["Id"];
                    a.Name = (string)row["name"];
                    try
                    {
                        int Nationality = (int)row["nationality"];
                        a.Nationality = nationalityDAO.Find((int)row["nationality"]);
                    }
                    catch
                    {
                        var EmptyNationality = new Nationality();
                        EmptyNationality.Name = "";
                        EmptyNationality.Id = -1;
                        a.Nationality = EmptyNationality;
                        a.NationalityID = -1;
                    }
                }
            }
            return a;
        }

        public void Create(Author a)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "Insert Into Authors Values (@name, @nationality)";
                cmd.Parameters.Add(new SqlParameter("@name", a.Name));
                if (a.Nationality.Id == -1)
                {
                    cmd.Parameters.Add(new SqlParameter("@nationality", DBNull.Value));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@nationality", a.NationalityID));
                }

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

        public void Update(Author a)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Update Authors Set Name=@name, Nationality=@nationality Where Id=@id";
                cmd.Parameters.Add(new SqlParameter("@name", a.Name));
                if (a.NationalityID == -1)
                {
                    cmd.Parameters.Add(new SqlParameter("@nationality", DBNull.Value));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@nationality", a.NationalityID));
                }
                cmd.Parameters.Add(new SqlParameter("@id", a.Id));

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

        public void Delete(Author a)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Delete From Authors Where Id=@id";
                cmd.Parameters.Add(new SqlParameter("@id", a.Id));

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