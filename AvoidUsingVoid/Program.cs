using System;
using System.Threading.Tasks;

namespace AvoidUsingVoid
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("start");
            
            
            /* --1--
             * ThrowException();
             */
            
            /* --2--
            try
            {
                 VoidFuncAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            */
            
            /* --3--
            try
            {
                 await TaskFuncAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            */
            Console.WriteLine("end");
            Console.ReadKey();
        }
        
        public static async Task ThrowException()
        {
            throw new Exception();
        }

        public static async void VoidFuncAsync()
        {
            await ThrowException();
        }

        public static async Task TaskFuncAsync()
        {
            await ThrowException();
        }
    }
}