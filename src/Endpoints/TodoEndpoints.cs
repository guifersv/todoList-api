using Microsoft.AspNetCore.Http.HttpResults;
using ToDoList.Application.Dtos;
using ToDoList.Application.Services.Interfaces;

namespace ToDoList.Endpoints;

public static class TodoEndpoints
{
    public static RouteGroupBuilder RouteTodoEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/{todoListId}", CreateTodo);
        group.MapDelete("/{todoId}", DeleteTodo);
        group.MapPatch("/{todoId}", ChangeTodoIsComplete);

        return group;
    }

    [EndpointSummary("Create Todo")]
    public static async Task<Results<Created, NotFound>> CreateTodo(
        int todoListId,
        TodoDto todoDto,
        ITodoService service
    )
    {
        var createdModel = await service.CreateTodoAsync(todoListId, todoDto);

        return createdModel is not null ? TypedResults.Created() : TypedResults.NotFound();
    }

    [EndpointSummary("Delete Todo")]
    public static async Task<Results<NoContent, NotFound>> DeleteTodo(
        int todoId,
        ITodoService service
    )
    {
        var deletedModel = await service.DeleteTodoAsync(todoId);

        return deletedModel is not null ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    [EndpointSummary("Change Todo IsComplete property")]
    public static async Task<Results<NoContent, NotFound>> ChangeTodoIsComplete(
        int todoId,
        ITodoService service
    )
    {
        var changedModel = await service.ChangeTodoIsCompleteAsync(todoId);

        return changedModel is not null ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
