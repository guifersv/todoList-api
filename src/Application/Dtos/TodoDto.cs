using System.ComponentModel.DataAnnotations;

namespace ToDoList.Application.Dtos;

public record TodoDto
{
    [StringLength(20)]
    public required string Title { get; set; }

    [StringLength(100)]
    public string? Description { get; set; }
    public DateTime DateCreated { get; set; }
    public bool IsCompleted { get; set; }
}
