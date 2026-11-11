using System.ComponentModel.DataAnnotations;
using DayFlowAPI.Enums;

namespace DayFlowAPI.Models.Requests;

// Only client-editable fields: Id, UserId, User and CreatedAt come from the route, the token or the server.
public class ToDoItemRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [EnumDataType(typeof(EPriorityToDoItem))]
    public EPriorityToDoItem Priority { get; set; }

    [EnumDataType(typeof(EStatusToDoItem))]
    public EStatusToDoItem Status { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedAt { get; set; }
}
