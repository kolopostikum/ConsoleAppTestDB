using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTestDB
{
    internal class BaseDirectory
    {
        public static readonly string ConnectionString = "Server=localhost;Database=master;Trusted_Connection=True;";

        public static async Task OutputOriginalRecords()
        {
            string sqlExpression = "SELECT DISTINCT Name, Birthday, Sex  FROM Employees " +
                "ORDER BY Name";

            //string sqlExpression = "SELECT DISTINCT * FROM Employees ORDER BY Name";

            await OutputRecords(sqlExpression);

            Console.Read();
        }

        public static async Task OutputRecords(string sqlExpression)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    string columnName1 = reader.GetName(0);
                    string columnName2 = reader.GetName(1);
                    string columnName3 = reader.GetName(2);
                    string columnName4 = "Age";

                    Console.WriteLine($"{columnName1}||\t{columnName2}||\t{columnName3}||\t{columnName4}");

                    while (await reader.ReadAsync()) 
                    {
                        object name = reader.GetValue(0);
                        object birthDay = reader.GetValue(1);
                        object sex = reader.GetValue(2);
                        object age = Employee.GetAge((DateTime)birthDay);

                        Console.WriteLine($"{name}||\t{birthDay}||\t{sex}||\t{age}");
                    }
                }

                reader.Close();
            }

        }

        public static async Task AddEmployeeDirectoryEntry(string[] args)
        {
            var employee = new Employee(args);
            await employee.AddEmployeeToDirectory(ConnectionString);
        }

        public static async Task MakeEmployeeDirectory()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand();
                command.CommandText = "CREATE TABLE Employees (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(100) NOT NULL, Birthday DATETIME NOT NULL, Sex NVARCHAR(100) NOT NULL)";
                command.Connection = connection;
                await command.ExecuteNonQueryAsync();

                new SqlCommand();
                command.CommandText =
                    "CREATE INDEX ix_fMale " +
                    "ON Employees(Name, Sex);";
                command.Connection = connection;
                await command.ExecuteNonQueryAsync();

                Console.WriteLine("Таблица Employees создана");
            }
            Console.Read();
        }
    }
}
