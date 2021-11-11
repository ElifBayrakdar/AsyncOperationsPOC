using System;
using System.IO;
using System.Threading.Tasks;

namespace AvoidUsingVoid
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                 await ReadAllLines("myfile.txt");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("file not found");
            }
            catch (Exception e)
            {
                Console.WriteLine("exception");
            }
            Console.WriteLine("end");
        }
        private static async Task ReadAllLines(string filePath)
        {
            await File.ReadAllLinesAsync(filePath);
        }
    }
}