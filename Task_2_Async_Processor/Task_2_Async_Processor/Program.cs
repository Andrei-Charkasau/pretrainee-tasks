using System;
using System.Diagnostics;
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

            StartAsyncProcessing(stopwatch, file1, file2, file3);

            stopwatch.Reset();

            StartSyncProcessing(stopwatch, file1, file2, file3);

            
            Console.ReadKey();
        }

        /// <summary>
        /// Вызов метода, запускающего выполнение АСИНХРОННЫХ операций с таймером.
        /// </summary>
        /// <param name="stopwatch">Таймер</param>
        /// <param name="file1">Файл для обработки №1</param>
        /// <param name="file2">Файл для обработки №2</param>
        /// <param name="file3">Файл для обработки №3</param>
        static void StartAsyncProcessing(Stopwatch stopwatch, string file1, string file2, string file3)
        {
            stopwatch.Start();
            var task1 = Task.Run(() => ProcessDataAsync(file1));
            var task2 = Task.Run(() => ProcessDataAsync(file2));
            var task3 = Task.Run(() => ProcessDataAsync(file3));
            Task.WaitAll(task1, task2, task3);
            stopwatch.Stop();
            Console.WriteLine($"Асинхронный код был выполнен за {stopwatch.Elapsed}.");
        }

        /// <summary>
        /// Вызов метода, запускающего выполнение СИНХРОННЫХ операций с таймером.
        /// </summary>
        /// <param name="stopwatch">Таймер</param>
        /// <param name="file1">Файл для обработки №1</param>
        /// <param name="file2">Файл для обработки №2</param>
        /// <param name="file3">Файл для обработки №3</param>
        static void StartSyncProcessing(Stopwatch stopwatch, string file1, string file2, string file3)
        {
            stopwatch.Start();
            ProcessData(file1);
            ProcessData(file2);
            ProcessData(file3);
            stopwatch.Stop();
            Console.WriteLine($"Синхронный код был выполнен за {stopwatch.Elapsed}.");
        }

        /// <summary>
        /// Выполнение операции (SYNC).
        /// </summary>
        /// <param name="dataName">Имя обрабатываемого файла.</param>
        static void ProcessData(string dataName)
        {
            Thread.Sleep(3000);
            Console.WriteLine($"Обработка {dataName} завершена за 3 секунды.");
        }

        /// <summary>
        /// Выполнение операции (ASYNC)
        /// </summary>
        /// <param name="dataName">Имя обрабатываемого файла.</param>
        /// <returns></returns>
        static async Task ProcessDataAsync(string dataName)
        {
            await Task.Delay(3000);
            Console.WriteLine($"Обработка {dataName} завершена за 3 секунды.");
        }

    }
}
