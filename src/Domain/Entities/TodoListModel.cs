using System.ComponentModel.DataAnnotations;

namespace ToDoList.Domain.Entities;

public class TodoListModel
{
    public int Id { get; set; }

    [MaxLength(20)]
    public required string Title { get; set; }

    [MaxLength(100)]
    public string? Description { get; set; }
    public ICollection<TodoModel> Todos { get; set; } = new List<TodoModel>();
}