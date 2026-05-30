using DayFlowAPI.Models.Entities;

namespace DayFlowAPI.Repositories.Interfaces
{
    public interface IToDoRepository
    {
        Task<IEnumerable<ToDoItem>> GetAllAsync(Guid userId);
        Task<ToDoItem?> GetByIdAsync(Guid id, Guid userId);
        Task<ToDoItem> CreateAsync(ToDoItem item);
        Task UpdateAsync(ToDoItem item, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }
}
