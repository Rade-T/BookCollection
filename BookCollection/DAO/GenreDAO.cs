using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BookCollection.Models;
using System.Data;

namespace BookCollection.DAO
{
    public class GenreDAO
    {
        public const string CONNECTION_STR = @"data source=RADE-PC\SQLEXPRESS;
                                        initial catalog=bookCollection;
                                        integrated security=true";

        public List<Genre> Read()
        {
            List<Genre> genres = new List<Genre>();
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Select * from Genres";
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "genres");

                foreach (DataRow row in ds.Tables["genres"].Rows)
                {
                    Genre g = new Genre();
                    g.Id = (int)row["Id"];
                    g.Name = (string)row["name"];
                    genres.Add(g);
                }
            }
            return genres;
        }

        public Genre Find(int? id)
        {
            Genre genre = null;
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Select * from Genres where id = @id";
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "genres");

                foreach (DataRow row in ds.Tables["genres"].Rows)
                {
                    genre = new Genre();
                    genre.Id = (int)row["Id"];
                    genre.Name = (string)row["name"];
                }
            }
            return genre;
        }

        public void Create(Genre g)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "Insert Into Genres Values (@name)";
                cmd.Parameters.Add(new SqlParameter("@name", g.Name));

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

        public void Update(Genre g)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Update Genres Set Name=@name Where Id=@id";
                cmd.Parameters.Add(new SqlParameter("@name", g.Name));
                cmd.Parameters.Add(new SqlParameter("@id", g.Id));

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

        public void Delete(Genre g)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Delete From Genres Where Id=@id";
                cmd.Parameters.Add(new SqlParameter("@id", g.Id));

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