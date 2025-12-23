using Microsoft.AspNetCore.Http.HttpResults;
using ToDoList.Application.Dtos;
using ToDoList.Application.Services.Interfaces;

namespace ToDoList.Endpoints;

public static class TodoEndpoints
{
    public static RouteGroupBuilder RouteTodoEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{todoId}", GetTodo).WithName(nameof(GetTodo));
        group.MapPost("/{todoListId}", CreateTodo);
        group.MapDelete("/{todoId}", DeleteTodo);
        group.MapPatch("/{todoId}", ChangeTodoIsComplete);

        return group;
    }

    [EndpointSummary("Get Todo model")]
    public static async Task<Results<Ok<TodoDto>, NotFound>> GetTodo(
        int todoId,
        ITodoService service
    )
    {
        var returnedModel = await service.GetTodoByIdAsync(todoId);

        return returnedModel is not null ? TypedResults.Ok(returnedModel) : TypedResults.NotFound();
    }

    [EndpointSummary("Create Todo")]
    public static async Task<Results<CreatedAtRoute<TodoDto>, NotFound>> CreateTodo(
        int todoListId,
        TodoDto todoDto,
        ITodoService service
    )
    {
        var createdModel = await service.CreateTodoAsync(todoListId, todoDto);

        return createdModel is not null
            ? TypedResults.CreatedAtRoute(
                createdModel,
                nameof(GetTodo),
                new { todoId = createdModel.Id }
            )
            : TypedResults.NotFound();
    }

    [EndpointSummary("Delete Todo")]
    public static async Task<Results<NoContent, NotFound>> DeleteTodo(
        int todoId,
        ITodoService service
    )
    {
        var deletedModel = await service.DeleteTodoAsync(todoId);

        return deletedModel ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    [EndpointSummary("Change Todo IsComplete property")]
    public static async Task<Results<NoContent, NotFound>> ChangeTodoIsComplete(
        int todoId,
        ITodoService service
    )
    {
        var changedModel = await service.ChangeTodoIsCompleteAsync(todoId);

        return changedModel ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
