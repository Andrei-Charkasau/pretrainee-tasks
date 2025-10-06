using System;
using System.Threading.Tasks;

namespace Task3_TaskManager
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            MenuOperations choice = 0;


            //IContext context = new PostgresContext(@"Host=localhost;Port=5432;Username=postgres;Database=postgredatabase");
            IContext context = new DapperContext(@"Data Source = C:\Users\Andrey\AppData\Roaming\DBeaverData\workspace6\.metadata\sample-database-sqlite-1\Chinook.db");
            IRepository<Labor> laborRepository = new LaborRepository(context);
            LaborService laborService = new LaborService(laborRepository);
            do
            {
                try
                {
                    Console.Clear();
                    ShowMenu();
                    choice = GetChoice();
                    if (choice != MenuOperations.Exit)
                    {
                        Console.Clear(); 
                        await DoOptionAsync(laborService, choice);
                        Console.ReadKey();
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (choice != MenuOperations.Exit);

            Console.WriteLine("Work has been stoped..." +
                "\n (Press any key to Exit.)");
            Console.ReadKey();
        }

        public static void ShowMenu()
        {
            Console.WriteLine("|Task_Manager | Version 0.0.1|" +
                "\n   1. Show All Labors " +
                "\n   2. Add Labor " +
                "\n   3. Delete Labor " +
                "\n   4. Change Labor Status " +
                "\n   5. Exit");
        }

        private static MenuOperations GetChoice()
        {
            Console.WriteLine("\nEnter option №: ");
            string stringChoices = Console.ReadLine();
            bool isSuccess = int.TryParse(stringChoices, out var choice);
            if(!isSuccess)
            {
                throw new Exception("The input must be numeric.");
            }
            CheckForOptions(choice);
            return (MenuOperations)choice;
        }
        private static void CheckForOptions(int option)
        {
            if (!Enum.IsDefined(typeof(MenuOperations), option))
            {
                throw new Exception("Invalid operation choose option.");
            }
        }
        public async static Task DoOptionAsync(LaborService laborService, MenuOperations choice)
        {
            switch (choice)
            {
                case MenuOperations.ShowAllLabor:
                    await laborService.ShowAllLaborsInfoAsync();
                    break;
                case MenuOperations.AddLabor:
                    await laborService.CreateLaborAsync();
                    break;
                case MenuOperations.DeleteLabor:
                    laborService.DeleteLaborInfo();
                    break;
                case MenuOperations.ChangeLaborStatus:
                    laborService.ChangeLaborInfo();
                    break;
                default: throw new Exception("Exiting...");  
            }
        }
    }
}
