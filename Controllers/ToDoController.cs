using System.Security.Claims;
using DayFlowAPI.Models.Entities;
using DayFlowAPI.Models.Requests;
using DayFlowAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DayFlowAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ToDoController(IToDoService toDoService) : ControllerBase
    {
        private readonly IToDoService _toDoService = toDoService;

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(
                userIdClaim ?? throw new InvalidOperationException("User id claim is missing.")
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllToDos()
        {
            var userId = GetCurrentUserId();
            var todos = await _toDoService.GetToDosAsync(userId);
            return Ok(todos);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetToDoById(Guid id)
        {
            var userId = GetCurrentUserId();
            var todo = await _toDoService.GetToDoByIdAsync(id, userId);
            if (todo is null)
                return NotFound();

            return Ok(todo);
        }

        private static ToDoItem ToEntity(ToDoItemRequest request, Guid id = default) =>
            new()
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                Status = request.Status,
                IsCompleted = request.IsCompleted,
                DueDate = request.DueDate,
                CompletedAt = request.CompletedAt,
            };

        [HttpPost]
        public async Task<IActionResult> CreateToDo([FromBody] ToDoItemRequest request)
        {
            var userId = GetCurrentUserId();
            var created = await _toDoService.CreateAsync(ToEntity(request), userId);
            return CreatedAtAction(nameof(GetToDoById), new { id = created.Id }, created);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateToDo(Guid id, [FromBody] ToDoItemRequest request)
        {
            var userId = GetCurrentUserId();
            try
            {
                await _toDoService.UpdateAsync(ToEntity(request, id), userId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteToDo(Guid id)
        {
            var userId = GetCurrentUserId();
            await _toDoService.DeleteAsync(id, userId);
            return NoContent();
        }
    }
}
