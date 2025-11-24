using ToDoList.Application.Dtos;
using ToDoList.Domain.Entities;

namespace ToDoList.Utilities;

public static class Utils
{
    public static TodoListDto TodoList2Dto(TodoListModel todoListModel)
    {
        TodoListDto todoListDto = new()
        {
            Id = todoListModel.Id,
            Title = todoListModel.Title,
            Description = todoListModel.Description,
            Todos = todoListModel
                .Todos.Select(t => new TodoDto()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    DateCreated = t.DateCreated,
                    IsCompleted = t.IsCompleted,
                })
                .ToList(),
        };
        return todoListDto;
    }

    public static TodoDto Todo2Dto(TodoModel todoModel)
    {
        TodoDto todoDto = new()
        {
            Id = todoModel.Id,
            Title = todoModel.Title,
            Description = todoModel.Description,
            DateCreated = todoModel.DateCreated,
            IsCompleted = todoModel.IsCompleted,
        };
        return todoDto;
    }
}