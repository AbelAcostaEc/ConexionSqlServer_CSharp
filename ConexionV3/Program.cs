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

                DataSet datos = new DataSet();
                adapter.Fill(datos, "ClientesUsa");

                Console.WriteLine("Listado Clientes USA: ");
                foreach (DataRow fila in datos.Tables["ClientesUsa"].Rows)
                {
                    String nombreCliente =fila.Field<String>("CompanyName");
                    Console.WriteLine("Cliente: " + nombreCliente);
                }

                String strUPDATE =
                    "UPDATE Customers " +
                    "SET " +
                    " CustomerID =  @CustomerID," +
                    " CompanyName = @CompanyName," +
                    " ContactName = @ContactName " +
                    "WHERE " +
                    "CustomerID = @oldCustomerID "+
                    "AND CompanyName = @oldCompanyName "+
                    "AND ContactName = @oldContactName ";

                adapter.UpdateCommand = new SqlCommand(strUPDATE, conn);
                //Configuracion  de parámetros
                adapter.UpdateCommand.Parameters.Add("@CustomerID", SqlDbType.NChar, 5, "CustomerID");
                adapter.UpdateCommand.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 40, "CompanyName");
                adapter.UpdateCommand.Parameters.Add("@ContactName", SqlDbType.NVarChar, 30, "ContactName");
                
                //Configuracion  de parámetros ORIGINALES
                SqlParameter sqlParam;
                sqlParam = adapter.UpdateCommand.Parameters.Add("@oldCustomerID", SqlDbType.NChar, 5, "CustomerID");
                sqlParam.SourceVersion = DataRowVersion.Original;
                sqlParam = adapter.UpdateCommand.Parameters.Add("@oldCompanyName", SqlDbType.NVarChar, 40, "CompanyName");
                sqlParam.SourceVersion = DataRowVersion.Original;
                sqlParam = adapter.UpdateCommand.Parameters.Add("@oldContactName", SqlDbType.NVarChar, 30, "ContactName");
                sqlParam.SourceVersion = DataRowVersion.Original;

                //Cambiar datos cargados en memoria
                Console.WriteLine( "");
                Console.WriteLine( "Modificando lista de clientes USA: ");
                foreach (DataRow fila in datos.Tables["ClientesUsa"].Rows)
                {
                    // Aumentar un caracter  - al inicio del nombre
                    String clienteNombre = "-" + fila.Field<String>("CompanyName");

                    /* //Quitar caracteres ingresados al inicio
                    String clienteNombre =  fila.Field<String>("CompanyName").Trim(new Char[] { '-' });
                    */

                    fila.SetField<string>("CompanyName", clienteNombre);
                    //Console.WriteLine("Cliente: " + clienteNombre);

                }

                //Enviar a grabar a BDD
                adapter.Update(datos, "ClientesUsa");

            }

            
            

        }
    }
}


