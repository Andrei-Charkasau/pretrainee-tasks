using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task3_TaskManager
{
    public class LaborService
    {
        private readonly IRepository<Labor> _laborRepository;

        public LaborService(IRepository<Labor> repository)
        {
            _laborRepository = repository;
        }

        public async Task ChangeLaborStatus(int id, bool isCompleted)
        {
            Labor labor = await _laborRepository.GetAsync(id);
            labor.IsCompleted = isCompleted;

            await _laborRepository.UpdateAsync(labor);
        }

        public async Task DeleteLaborInfo(int id)
        {
            await _laborRepository.DeleteAsync(id);
        }

        public async Task CreateLaborAsync(string title, string description)
        {
            var labor = new Labor
            {
                Title = title,
                Description = description,
                IsCompleted = false,
                CreatedAt = DateTime.Now
            };

            await _laborRepository.CreateAsync(labor);
        }

        public async Task<IEnumerable<Labor>> GetAllLaborsAsync()
        {
            return await _laborRepository.GetAllAsync();
        }

        public async Task<Labor> GetLaborByIdAsync(int id)
        {
            return await _laborRepository.GetAsync(id);
        }
    }
}