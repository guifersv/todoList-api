using System.ComponentModel.DataAnnotations;

namespace ToDoList.Domain.Entities;

public class TodoModel
{
    public int Id { get; set; }

    [MaxLength(20)]
    public required string Title { get; init; }

    [MaxLength(100)]
    public string? Description { get; init; }
    public DateTime DateCreated { get; init; }
    public bool IsCompleted { get; set; }
    public int TodoListModelId { get; set; }
    public required TodoListModel TodoListModelNavigation { get; set; }
}
