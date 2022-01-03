using System;
using Microsoft.Data.SqlClient;
using System.Data;


namespace ConexionV3
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source = ABEL-ASAA\\SQLEXPRESS; Integrated Security =true; Initial Catalog = Northwind";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(
                       "SELECT CustomerID, CompanyName, ContactName " +
                       "FROM Customers " +
                       "WHERE Country = 'USA' " +
                       "ORDER BY CustomerID",
                       conn
                    );

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet,"ClientesUsa");

                foreach (DataRow fila in dataSet.Tables["ClientesUsa"].Rows)
                {
                    String nombreCliente =fila.Field<String>("CompanyName");
                    Console.WriteLine("Cliente: " + nombreCliente);
                }

            }

            
            

        }
    }
}


