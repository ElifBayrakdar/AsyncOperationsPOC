using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskReturnAndWait
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //A task is a representation of an asynchronous operation
            //Calling Result or Wait will block and potentially deadlock your application
            //Using Result is that it will block the thread until de result is available, so code will run synchronously 
            Console.WriteLine(
                $"Main1 method thread name => {Thread.CurrentThread.ManagedThreadId}  {Thread.CurrentThread.ThreadState}");
            var responseTask = DoAnAsyncOperation();
            Console.WriteLine(
                $"Main2 method thread name => {Thread.CurrentThread.ManagedThreadId}  {Thread.CurrentThread.ThreadState}");
            var response = await responseTask;
            Console.WriteLine(
                $"Main3 method thread name => {Thread.CurrentThread.ManagedThreadId} {Thread.CurrentThread.ThreadState}");
            Console.ReadLine();
        }

        private static async Task<int> DoAnAsyncOperation()
        {
            await Task.Run(async() =>
            {
                Console.WriteLine(
                    $"***** Inner1 method thread name => {Thread.CurrentThread.ManagedThreadId}  {Thread.CurrentThread.ThreadState}");

                await Task.Delay(10000);
                // for (int j = 0; j < 10000000000; j++)
                // {
                //     var i = Math.Pow(2, j);
                // }

                Console.WriteLine(
                    $"***** Inner2 method thread name => {Thread.CurrentThread.ManagedThreadId}  {Thread.CurrentThread.ThreadState}");
            });
            return 0;
        }
    }
}