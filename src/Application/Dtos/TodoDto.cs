using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ToDoList.Application.Dtos;

public record TodoDto
{
    [BindNever]
    public int Id { get; set; }

    [StringLength(20)]
    public required string Title { get; set; }

    [StringLength(100)]
    public string? Description { get; set; }
    public DateTime DateCreated { get; set; }
    public bool IsCompleted { get; set; }
}