using DayFlowAPI.Models.Entities;
using DayFlowAPI.Repositories.Interfaces;
using DayFlowAPI.Services.Interfaces;

namespace DayFlowAPI.Services
{
    public class ToDoService(IToDoRepository repository) : IToDoService
    {
        private readonly IToDoRepository _repository = repository;

        public async Task<IEnumerable<ToDoItem>> GetToDosAsync(Guid userId)
        {
            return await _repository.GetAllAsync(userId);
        }

        public async Task<ToDoItem?> GetToDoByIdAsync(Guid id, Guid userId)
        {
            return await _repository.GetByIdAsync(id, userId);
        }

        public async Task<ToDoItem> CreateAsync(ToDoItem item, Guid userId)
        {
            item.UserId = userId;
            return await _repository.CreateAsync(item);
        }

        public async Task UpdateAsync(ToDoItem item, Guid userId)
        {
            item.UserId = userId;
            await _repository.UpdateAsync(item, userId);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            await _repository.DeleteAsync(id, userId);
        }
    }
}
