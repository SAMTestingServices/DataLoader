
using System.Collections.Generic;
using System.Linq;

using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.Data.OleDb;
using System.Configuration;

using System.Diagnostics;
using System;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    class ExcelMethods
    {

       
        public static string getExcelConnection(string fileName)
        {
            string con = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = {0}; Extended Properties=Excel 12.0;", fileName);
            return con;
        }


     
        public static List<T> GetData<T>(string fileName, string tab) where T :class
        {
            Debug.WriteLine("log: Entered GetTestDataMethod");
            using (var connection = new OleDbConnection(getExcelConnection(fileName)))
            {
                connection.Open();
                Debug.WriteLine("log: Ran connection.open");
                var query = string.Format("select * from [{0}$] where ID > 0",tab);
                Debug.WriteLine("Formed Query");
                var value = connection.Query<T>(query).ToList();
                Debug.WriteLine("USed Query");
                // FirstOrDefault();

                connection.Close();
                Debug.WriteLine("Closed connection");
                return value;

            }
        }


        public static void WriteToCell(int row, string column, string value, string tab, string fileName)
        {
            
            var connection = new OleDbConnection(getExcelConnection(fileName));
            
                connection.Open();

                string query = "Update [" + tab + "$] set " + column + " = '" + value + "' where id=" + row;

                connection.Query(query);

                connection.Close();
            
        }
    }
}
