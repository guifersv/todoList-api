using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ToDoList.Application.Dtos;

public record TodoListDto
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
    public int Id { get; set; }

    [StringLength(20)]
    public required string Title { get; set; }

    [StringLength(100)]
    public string? Description { get; set; }
}
