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
        }
        
        private static async Task<string> GetStringAsync()
        {
            var response = new HttpClient().GetStringAsync("https://www.google.com");

            var result = await response;

            return result;
        }


        private static Task<string> GetStringAsync2()   // bu konudaki thread switch konusuna bakılacak
        {
            return new HttpClient().GetStringAsync("https://www.google.com");
        }
        
        
        private static async Task GetStringAsync3()  
        {
            var response =  new HttpClient().GetStringAsync("https://www.google.com").ContinueWith((data) =>
            {
                Console.WriteLine($"Data is: {data.Result}");
            });

            await response;
        }
        
        
        private static async Task<string[]> GetStringsWithWhenAllAsync()  
        {
            var task1 = new HttpClient().GetStringAsync("https://www.google.com");
            var task2 = new HttpClient().GetStringAsync("https://www.yandex.com");

            List<Task<string>> tasks = new List<Task<string>>();
            tasks.Add(task1);
            tasks.Add(task2);

            var result = await Task.WhenAll(tasks.ToArray());   //Tüm taskların sonucunu geri döndürdü.
            return result;
        }
        
        
        private static async Task<string> GetStringsWithWhenAnyAsync()  
        {
            var task1 = new HttpClient().GetStringAsync("https://www.google.com");
            var task2 = new HttpClient().GetStringAsync("https://www.yandex.com");

            List<Task<string>> tasks = new List<Task<string>>();
            tasks.Add(task1);
            tasks.Add(task2);

            var result = await Task.WhenAny(tasks.ToArray());
            return result.Result;   //İlk biten taskın sonucuna Result ile güvenle ulaşabiliriz.
        }
        
        
        private static void GetStringsWithWaitAllAsync()  
        {
            var task1 = new HttpClient().GetStringAsync("https://www.google.com");
            var task2 = new HttpClient().GetStringAsync("https://www.yandex.com");

            List<Task> tasks = new List<Task>();
            tasks.Add(task1);
            tasks.Add(task2);

            Task.WaitAll(tasks.ToArray());    //Ana thread bloklanır, kod senkron çalışır.
        }
        
        
        private static int GetStringsWithWaitAnyAsync()  
        {
            var task1 = new HttpClient().GetStringAsync("https://www.google.com");
            var task2 = new HttpClient().GetStringAsync("https://www.yandex.com");

            List<Task> tasks = new List<Task>();
            tasks.Add(task1);
            tasks.Add(task2);

            var result = Task.WaitAny(tasks.ToArray());     //Ana thread bloklanır, kod senkron çalışır. İlk biten taskın index numarası döner.

            return result;
        }
    }
}  
