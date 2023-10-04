using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConsoleAppTestDB
{
    internal class Employee
    {
        public object Name { get; set; }
        public object BirthDate { get; set; }
        public string Sex { get; set; }

        public Employee(string[] args)
        {
            this.Name = args[1];
            this.BirthDate = args[2];
            this.Sex = args[3];
        }

        public Employee(string name, object birthDay, string sex)
        {
            this.Name = name;
            this.BirthDate = birthDay;
            this.Sex = sex;
        }

        public async Task AddEmployeeToDirectory(string connectionString)
        {
            string sqlExpression = $" INSERT INTO Employees (Name, Birthday, Sex) VALUES (@name, @birthDate, @sex); ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("@name", Name);
                command.Parameters.AddWithValue("@birthDate", BirthDate);
                command.Parameters.AddWithValue("@sex", Sex);

                int number = await command.ExecuteNonQueryAsync();
            }
        }

        public static int GetAge(DateTime birthday)
        {
            var today = DateTime.Today;
            var age = today.Year - birthday.Year;

            //Проверка на високосные года
            if (birthday.Date > today.AddYears(-age)) age--;

            return age;
        }
    }
}