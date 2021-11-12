using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CancellationTokenUsing
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await ExecuteCancellableTaskByTimeoutAsync();
            await ExecuteManuallyCancellableTaskAsync();
        }

        private static async Task<string> DoCancellableWork(CancellationToken token)
        {
            Task<string> taskResult;
            try
            {
                taskResult = new HttpClient().GetStringAsync("https://www.google.com", token);
                string result = await taskResult;

                await Task.Delay(5000, token);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        private static async Task ExecuteCancellableTaskByTimeoutAsync()
        {
            CancellationTokenSource source = new CancellationTokenSource(4000);

            Task<string> resultTask;
            try
            {
                resultTask = DoCancellableWork(source.Token);
                var result = await resultTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Task was cancelled", ex);
            }
        }
        
        
        
        
        
        

        //Manually Cancelling
        private static async Task<int> DoCancellableWork(int loop, CancellationToken cancellationToken)
        {
            int result = 0;
            
            for (int i = 0; i < loop; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();   //cancellationToken.IsCancellationRequested ile if içinde başka şeyler de yaptırılabilir. TaskCanceledException fırlatmak gibi

                await Task.Delay(10);
                result += i;
            }

            return result;
        }

        public static async Task ExecuteManuallyCancellableTaskAsync()
        {
            Task<int> longRunningTask;
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var keyBoardTask = Task.Run(() =>
                {
                    Console.WriteLine("Press enter to cancel");
                    Console.ReadKey();
                    cancellationTokenSource.Cancel();
                });

                try
                {
                    longRunningTask = DoCancellableWork(1000, cancellationTokenSource.Token);
                    var result = await longRunningTask;
                    Console.WriteLine("Result {0}", result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Task was cancelled", ex);
                }

                await keyBoardTask;
            }
        }
    }
}