using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Task3_TaskManager
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            MenuOperations choice = 0;

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string sqliteConn = configuration["SqliteConnection"];
            string postgresConn = configuration["PostgresConnection"];

            IContext context = new DapperContext(sqliteConn);
            IRepository<Labor> laborRepository = new LaborRepository(context);
            LaborService laborService = new LaborService(laborRepository);
            LaborController laborController = new LaborController(laborService);
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
                        await DoOptionAsync(laborController, choice);
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
                "\n   2. Show Specific Labor Info" +
                "\n   3. Add Labor " +
                "\n   4. Delete Labor " +
                "\n   5. Change Labor Status " +
                "\n   6. Exit");
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
        public async static Task DoOptionAsync(LaborController laborController, MenuOperations choice)
        {
            switch (choice)
            {
                case MenuOperations.ShowAllLabor:
                    await laborController.ShowAllLaborsInfoAsync();
                    break;
                case MenuOperations.ShowLaborInfo:
                    await laborController.ShowLaborInfoAsync();
                    break;
                case MenuOperations.AddLabor:
                    await laborController.CreateLaborAsync();
                    break;
                case MenuOperations.DeleteLabor:
                    await laborController.DeleteLaborInfo();
                    break;
                case MenuOperations.ChangeLaborStatus:
                    await laborController.ChangeLaborInfo();
                    break;
                default: throw new Exception("Exiting...");  
            }
        }
    }
}
