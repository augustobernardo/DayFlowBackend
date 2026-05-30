using DayFlowAPI.Data;
using DayFlowAPI.Models.Entities;
using DayFlowAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DayFlowAPI.Repositories
{
    public class ToDoRepository(AppDbContext context) : IToDoRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<ToDoItem>> GetAllAsync(Guid userId)
        {
            return await _context.ToDoItems.Where(item => item.UserId == userId).ToListAsync();
        }

        public async Task<ToDoItem?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.ToDoItems.FirstOrDefaultAsync(item =>
                item.Id == id && item.UserId == userId
            );
        }

        public async Task<ToDoItem> CreateAsync(ToDoItem item)
        {
            _context.ToDoItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateAsync(ToDoItem item, Guid userId)
        {
            ToDoItem? existingItem =
                await _context.ToDoItems.FirstOrDefaultAsync(i =>
                    i.Id == item.Id && i.UserId == userId
                )
                ?? throw new InvalidOperationException(
                    "ToDo item not found or does not belong to the current user."
                );

            existingItem.Title = item.Title;
            existingItem.Description = item.Description;
            existingItem.Priority = item.Priority;
            existingItem.Status = item.Status;
            existingItem.IsCompleted = item.IsCompleted;
            existingItem.DueDate = item.DueDate;
            existingItem.CompletedAt = item.CompletedAt;
            existingItem.UpdatedAt = DateTime.UtcNow;

            _context.ToDoItems.Update(existingItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var item = await _context.ToDoItems.FirstOrDefaultAsync(i =>
                i.Id == id && i.UserId == userId
            );

            if (item != null)
            {
                _context.ToDoItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
