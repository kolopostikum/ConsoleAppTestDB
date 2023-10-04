using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Faker;
using System.Data;
using System.Windows.Input;


namespace ConsoleAppTestDB
{
    internal class TestDirectory
    {
        private readonly Employee[] pack = new Employee[100];

        public static async Task AutomaticFillingDirectory()
        {
            var rnd = new Random();
            var direction = new TestDirectory();
            for (int i = 1; i <= 10000; i++)
            {
                var name = Faker.Name.Last() + " " + Faker.Name.First() + " " + Faker.Name.Middle();
                var subBirthday = DateTime.Now;
                var sex = rnd.Next(0, 2) == 0 ? "Male" : "Female";
                var rndEmployee = new Employee(name, subBirthday, sex);
                direction.pack[i % 100] = rndEmployee;
                if (i%100 == 0)
                    await direction.AddPackEmployeeToDirectory(direction.pack);
            }

            await AddFTestEmployees(direction);
        }

        private static async Task AddFTestEmployees(TestDirectory direction)
        {
            var rnd = new Random();
            string sqlExpression = $"INSERT INTO Employees (Name, Birthday, Sex) VALUES (@name, @birthDate, @sex);";

            for (int i = 0; i < 100; i++)
            {
                var name = "F" + Faker.Name.Last().ToLower() + " " + Faker.Name.First() + " " + Faker.Name.Middle();
                var subBirthday = DateTime.Now;
                var sex ="Male";
                var rndEmployee = new Employee(name, subBirthday, sex);
                direction.pack[i % 100] = rndEmployee;
            }

            await direction.AddPackEmployeeToDirectory(direction.pack);
        }

        public async Task AddPackEmployeeToDirectory(
            Employee[] pack)
        {
            string sqlExpression = $" INSERT INTO Employees (Name, Birthday, Sex) VALUES (@name, @birthDate, @sex); ";

            using (SqlConnection connection = new SqlConnection(BaseDirectory.ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@birthDate", System.Data.SqlDbType.Date));
                command.Parameters.Add(new SqlParameter("@sex", System.Data.SqlDbType.NVarChar));

                foreach (var e in pack)
                {
                    command.Parameters["@name"].Value = e.Name;
                    command.Parameters["@birthDate"].Value = e.BirthDate;
                    command.Parameters["@sex"].Value = e.Sex;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task OutputRecordsMaleSurnameF()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string sqlExpression = "SELECT DISTINCT Name, Birthday, Sex  FROM Employees " +
                "WHERE LEFT(Name, 1) = \'F\' AND Sex = \'Male\'";

            //string sqlExpression = "SELECT DISTINCT * FROM Employees "
            //    + "WHERE LEFT(Name, 1) = \'F\' AND Sex = \'Male\'";

            await BaseDirectory.OutputRecords(sqlExpression);

            stopwatch.Stop();
            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
            Console.Read();
        }
    }
}
