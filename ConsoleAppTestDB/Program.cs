using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTestDB
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
                ;
            else if (args[0] == "1")
                await BaseDirectory.MakeEmployeeDirectory();
            else if (args[0] == "2")
                await BaseDirectory.AddEmployeeDirectoryEntry(args);
            else if (args[0] == "3")
                await BaseDirectory.OutputOriginalRecords();
            else if (args[0] == "4")
                await TestDirectory.AutomaticFillingDirectory();
            else if (args[0] == "5")
                await TestDirectory.OutputRecordsMaleSurnameF();
            Console.Read();
        }
    }
}
