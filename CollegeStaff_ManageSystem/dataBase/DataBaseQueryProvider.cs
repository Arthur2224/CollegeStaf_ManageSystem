using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeStaff_ManageSystem.dataBase
{
    internal class DataBaseQueryProvider
    {
        private string connectionString = @"Data Source=..\..\\Files\\CollegeStaff.db;Version=3;";
        public void AddNewEntity(string query)
        {
            using(var connection=new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteRecord(string tableName,string PKName,string primaryKeyValue)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = $"DELETE FROM {tableName} WHERE {PKName} = @PrimaryKeyValue";
                using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                {  
                    command.Parameters.AddWithValue("@PrimaryKeyValue", primaryKeyValue);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
