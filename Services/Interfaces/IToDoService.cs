using DayFlowAPI.Models.Entities;

namespace DayFlowAPI.Services.Interfaces
{
    public interface IToDoService
    {
        Task<IEnumerable<ToDoItem>> GetToDosAsync(Guid userId);
        Task<ToDoItem?> GetToDoByIdAsync(Guid id, Guid userId);
        Task<ToDoItem> CreateAsync(ToDoItem item, Guid userId);
        Task UpdateAsync(ToDoItem item, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }
}
