using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ToDoList.Application.Dtos;

public record TodoListDto
{
    [BindNever]
    public int Id { get; set; }

    [StringLength(20)]
    public required string Title { get; set; }

    [StringLength(100)]
    public string? Description { get; set; }

    [BindNever]
    public ICollection<TodoDto> Todos { get; set; } = new List<TodoDto>();
}