using System;
using System.Threading.Tasks;
using Task3_TaskManager;

    public class LaborController
{
    private readonly LaborService _laborService;

    public LaborController(LaborService laborService)
    {
        _laborService = laborService;
    }

    public async Task ChangeLaborInfo()
    {
        bool isCompleted = false;
        bool success = false;
        int result;

        do
        {
            Console.WriteLine("Enter labor's ID to change: ");
            success = Int32.TryParse(Console.ReadLine(), out result);
        } while (!success);

        int id = result;

        do
        {
            Console.WriteLine($"Set labor {id} status to" +
                $"\n 1) Completed;" +
                $"\n 2) Incompleted; ");
            success = Int32.TryParse(Console.ReadLine(), out result);

            switch (result)
            {
                case 1:
                    isCompleted = true;
                    break;
                case 2:
                    isCompleted = false;
                    break;
                default:
                    Console.WriteLine("Wrong option select.");
                    success = false;
                    break;
            }
        } while (!success);

        await _laborService.ChangeLaborStatus(id, isCompleted);
        Console.WriteLine($"Labor {id} completed status is {isCompleted} now.");
    }

    public async Task DeleteLaborInfo()
    {
        Console.WriteLine("Enter labor's ID to delete: ");
        int id = Convert.ToInt32(Console.ReadLine());
        await _laborService.DeleteLaborInfo(id);
    }

    public async Task ShowAllLaborsInfoAsync()
    {
        foreach (Labor labor in await _laborService.GetAllLaborsAsync())
        {
            Console.WriteLine(labor.ToString());
        }
    }

    public async Task ShowLaborInfoAsync()
    {
        Console.WriteLine("Enter the ID of labor: ");
        int id = Convert.ToInt32(Console.ReadLine());
        Labor labor = await _laborService.GetLaborByIdAsync(id);
        if(labor != null)
        {
            Console.WriteLine(labor.ToString());
        }
        else
        {
            Console.WriteLine($"Labor with ID: {id} doesn't exist.");
        }
    }

    public async Task CreateLaborAsync()
    {
        Console.WriteLine("Enter labor's TITLE: ");
        string title = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine($"{nameof(title)} can't be empty.");
            return;
        }

        Console.WriteLine("Enter labor's DESCRIPTION: ");
        string description = Console.ReadLine();

        await _laborService.CreateLaborAsync(title, description);
    }
}