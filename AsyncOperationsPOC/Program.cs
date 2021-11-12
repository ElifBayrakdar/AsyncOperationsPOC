using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsyncOperationsPOC
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            await GetStringAsync3();
            Console.ReadKey();
        }
        
        private static async Task<string> GetStringAsync()
        {
            var task = new HttpClient().GetStringAsync("https://www.google.com");
            
            var result = await task;

            return result;
        }

        private static Task<string> GetStringAsync2()  //Context switch
        {
            return new HttpClient().GetStringAsync("https://www.google.com");
        }
        
        private static async Task GetStringAsync3()
        {
            var response = new HttpClient().GetStringAsync("https://www.google.com").ContinueWith((data) =>
            {
                Console.WriteLine($"Data is: {data.Result}");
            });

            await response;
        }
        
        private static string GetStringAsync4()
        {
            return new HttpClient().GetStringAsync("https://www.google.com").Result;
        }
        
        private static Task<string> DoSomethingAsync()
        {
            using (var foo = new DisposableFoo())
            {
                return foo.GetFooStringAsync();
            }
        }

        private static async Task<string> DoSomethingAsync2()
        {
            using (var foo = new DisposableFoo())
            {
                return await foo.GetFooStringAsync();
            }
        }

        private static async Task<string[]> GetStringsWithWhenAllAsync()
        {
            var task1 = new HttpClient().GetStringAsync("https://www.google.com");
            var task2 = new HttpClient().GetStringAsync("https://www.yandex.com");

            var result = await Task.WhenAll(task1, task2); //Tüm taskların sonucunu geri döndürür.

            return result;
        }


        private static async Task<string> GetStringsWithWhenAnyAsync()
        {
            var task1 = new HttpClient().GetStringAsync("https://www.google.com");
            var task2 = new HttpClient().GetStringAsync("https://www.yandex.com");

            var firstRsult = await Task.WhenAny(task1, task2);
            
            return await firstRsult; 
                                     //Diğer tasklar çalışmaya devam eder ama sonuçları ignore edilir.
                                     //Diğer tasklar exception alırlarsa exception'lar ignore edilir.
        }


        private static string GetStringsWithWaitAllAsync()
        {
            var task1 = new HttpClient().GetStringAsync("https://www.google.com");
            var task2 = new HttpClient().GetStringAsync("https://www.yandex.com");

            Task.WaitAll(task1, task2); //Ana thread bloklanır, kod senkron çalışır.
            return task1.IsCompleted.ToString();
        }


        private static int GetStringsWithWaitAnyAsync()
        {
            var task1 = new HttpClient().GetStringAsync("https://www.google.com");
            var task2 = new HttpClient().GetStringAsync("https://www.yandex.com");
            
            var result = Task.WaitAny(task1, task2); //Ana thread bloklanır, kod senkron çalışır. İlk biten taskın index numarası döner.

            return result;
        }
    }

    internal class DisposableFoo : IDisposable
    {
        private bool isDisposed = false;
        
        private SampleObject _sampleObject;

        public DisposableFoo()
        {
            _sampleObject = new SampleObject();
            Dispose(false);
        }
        
        public async Task<string> GetFooStringAsync()
        {
            await Task.Delay(4000);
            int sampleProp = _sampleObject.SampleProp;
            return await Task.FromResult(sampleProp.ToString());
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _sampleObject = null;
            }
            isDisposed = true;
        }

        public void Dispose()
        {
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
    }

    internal class SampleObject
    {
        public int SampleProp { get; set; }
    }
}