using ToDoList.Application.Dtos;
using ToDoList.Domain.Entities;

namespace ToDoList.Utilities;

public static class Utils
{
    public static TodoListDto TodoList2Dto(TodoListModel todoListModel)
    {
        return new()
        {
            Id = todoListModel.Id,
            Title = todoListModel.Title,
            Description = todoListModel.Description,
        };
    }

    public static TodoDto Todo2Dto(TodoModel todoModel)
    {
        return new()
        {
            Id = todoModel.Id,
            Title = todoModel.Title,
            Description = todoModel.Description,
            DateCreated = todoModel.DateCreated,
            IsCompleted = todoModel.IsCompleted,
        };
    }
}
