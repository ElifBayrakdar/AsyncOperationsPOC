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
                ReadAllLines("myfile.txt");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("file not found");
            }
            catch (Exception e)
            {
                Console.WriteLine("exception");
            }

            var input = Console.ReadLine();
            Console.WriteLine(input);
        }

        private static async void ReadAllLines(string filePath)
        {
            await File.ReadAllLinesAsync(filePath);
        }
    }
}