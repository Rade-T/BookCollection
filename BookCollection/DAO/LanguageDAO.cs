using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BookCollection.Models;
using System.Data;

namespace BookCollection.DAO
{
    public class LanguageDAO
    {
        public const string CONNECTION_STR = @"data source=RADE-PC\SQLEXPRESS;
                                        initial catalog=bookCollection;
                                        integrated security=true";

        public List<Language> Read()
        {
            List<Language> languages = new List<Language>();
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Select * from Languages";
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "languages");

                foreach (DataRow row in ds.Tables["languages"].Rows)
                {
                    Language l = new Language();
                    l.Id = (int)row["Id"];
                    l.Name = (string)row["name"];
                    languages.Add(l);
                }
            }
            return languages;
        }

        public Language Find(int? id)
        {
            Language language = null;
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Select * from Languages where id = @id";
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "languages");

                foreach (DataRow row in ds.Tables["languages"].Rows)
                {
                    language = new Language();
                    language.Id = (int)row["Id"];
                    language.Name = (string)row["name"];
                }
            }
            return language;
        }

        public void Create(Language l)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "Insert Into Languages Values (@name)";
                cmd.Parameters.Add(new SqlParameter("@name", l.Name));

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

        public void Update(Language l)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Update Languages Set Name=@name Where Id=@id";
                cmd.Parameters.Add(new SqlParameter("@name", l.Name));
                cmd.Parameters.Add(new SqlParameter("@id", l.Id));

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

        public void Delete(Language l)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Delete From Languages Where Id=@id";
                cmd.Parameters.Add(new SqlParameter("@id", l.Id));

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