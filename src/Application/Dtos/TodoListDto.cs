using System.ComponentModel.DataAnnotations;

namespace ToDoList.Application.Dtos;

public record TodoListDto
{
    [StringLength(20)]
    public required string Title { get; set; }

    [StringLength(100)]
    public string? Description { get; set; }
}
