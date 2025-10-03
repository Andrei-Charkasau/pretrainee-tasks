using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task_2_Async_Processor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string file1 = "Файл_1";
            string file2 = "Файл_2";
            string file3 = "Файл_3";

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var task1 = Task.Run(() => ProcessDataAsync(file1));
            var task2 = Task.Run(() => ProcessDataAsync(file2));
            var task3 = Task.Run(() => ProcessDataAsync(file3));
            Task.WaitAll(task1, task2, task3);
            stopwatch.Stop();
            Console.WriteLine($"Асинхронный код был выполнен за {stopwatch.Elapsed}.");

            stopwatch.Reset();
            stopwatch.Start();
            ProcessData(file1);
            ProcessData(file2);
            ProcessData(file3);
            stopwatch.Stop();
            Console.WriteLine($"Синхронный код был выполнен за {stopwatch.Elapsed}.");

            
            Console.ReadKey();
        }
        static void ProcessData(string dataName)
        {
            Thread.Sleep(3000);
            Console.WriteLine($"Обработка {dataName} завершена за 3 секунды.");
        }

        static async Task ProcessDataAsync(string dataName)
        {
            await Task.Delay(3000);
            Console.WriteLine($"Обработка {dataName} завершена за 3 секунды.");
        }

    }
}
