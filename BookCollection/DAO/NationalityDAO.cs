using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BookCollection.Models;
using System.Data;

namespace BookCollection.DAO
{
    public class NationalityDAO
    {
        public const string CONNECTION_STR = @"data source=RADE-PC\SQLEXPRESS;
                                        initial catalog=bookCollection;
                                        integrated security=true";

        public List<Nationality> Read()
        {
            List<Nationality> nationalities = new List<Nationality>();
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Select * from Nationalities";
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "nationalities");

                foreach (DataRow row in ds.Tables["nationalities"].Rows)
                {
                    Nationality n = new Nationality();
                    n.Id = (int)row["Id"];
                    n.Name = (string)row["name"];
                    nationalities.Add(n);
                }
            }
            return nationalities;
        }

        public Nationality Find(int? id)
        {
            Nationality nationality = null;
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Select * from Nationalities where id = @id";
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds, "nationalities");

                foreach (DataRow row in ds.Tables["nationalities"].Rows)
                {
                    nationality = new Nationality();
                    nationality.Id = (int)row["Id"];
                    nationality.Name = (string)row["name"];
                }
            }
            return nationality;
        }

        public void Create(Nationality n)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "Insert Into Nationalities Values (@name)";
                cmd.Parameters.Add(new SqlParameter("@name", n.Name));

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

        public void Update(Nationality n)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Update Nationalities Set Name=@name Where Id=@id";
                cmd.Parameters.Add(new SqlParameter("@name", n.Name));
                cmd.Parameters.Add(new SqlParameter("@id", n.Id));

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

        public void Delete(Nationality n)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"Delete From Nationalities Where Id=@id";
                cmd.Parameters.Add(new SqlParameter("@id", n.Id));

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