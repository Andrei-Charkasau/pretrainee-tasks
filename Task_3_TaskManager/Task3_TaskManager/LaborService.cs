using System;
using System.Threading.Tasks;

namespace Task3_TaskManager
{
    internal class LaborService
    {
        private readonly IRepository<Labor> _repository;

        public LaborService(IRepository<Labor> repository)
        {
            _repository = repository;
        }

        public void ChangeLaborInfo()
        {
            bool isCompleted = false;
            Console.WriteLine("Enter labor's ID to change: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"Set labor {id} status to" +
                $"\n 1) Completed;" +
                $"\n 2) Incompleted; ");
            switch (Convert.ToInt32(Console.ReadLine()))
            {
                case 1:
                    isCompleted = true;
                    break;
                case 2:
                    isCompleted = false;
                    break;
                default: throw new Exception("Choose 1 or 2.");
            }
            _repository.UpdateStatusAsync(id, isCompleted);
            Console.WriteLine($"Labor {id} completed status is {isCompleted} now.");
        }

        public void DeleteLaborInfo()
        {
            Console.WriteLine("Enter labor's ID to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());
            _repository.DeleteAsync(id);
        }

        public async Task ShowAllLaborsInfoAsync()
        {
            var result = await _repository.GetAllAsync();
            foreach (Labor labor in result)
            {
                Console.WriteLine(labor.ToString());
            };
        }

        public async Task ShowLaborInfoAsync()
        {
            Console.WriteLine("Enter the ID of labor: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Labor labor = await _repository.GetAsync(id);
            Console.WriteLine(labor.ToString());
        }

        public async Task CreateLaborAsync()
        {
            Labor labor = new Labor();
            Console.WriteLine("Enter labor's TITLE: ");
            labor.Title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(labor.Title))
            {
                Console.WriteLine($"{nameof(labor.Title)} can't be empty.");
                return;
            }
            Console.WriteLine("Enter labor's DESCRIPTION: ");
            labor.Description = Console.ReadLine();
            labor.IsCompleted = false;
            labor.CreatedAt = DateTime.Now;
            try
            {
                await _repository.CreateAsync(labor);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}