using Microsoft.AspNetCore.Http.HttpResults;

using ToDoList.Application.Dtos;
using ToDoList.Application.Services.Interfaces;

namespace ToDoList.Endpoints;

public static class TodoListEndpoints
{
    public static RouteGroupBuilder RouteTodoListEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllTodoLists);
        group.MapGet("/{todoListId}", GetTodoList).WithName(nameof(GetTodoList));
        group.MapPost("/", CreateTodoList);
        group.MapPut("/{todoListId}", UpdateTodoList);
        group.MapDelete("/{todoListId}", DeleteTodoList);

        return group;
    }

    [EndpointSummary("Get all TodoList models")]
    public static async Task<IEnumerable<TodoListDto>> GetAllTodoLists(ITodoService service)
    {
        return await service.GetAllTodoListsAsync();
    }

    [EndpointSummary("Get TodoList model")]
    public static async Task<Results<Ok<TodoListDto>, NotFound>> GetTodoList(
        int todoListId,
        ITodoService service
    )
    {
        var returnedModel = await service.GetTodoListByIdAsync(todoListId);

        return returnedModel is not null ? TypedResults.Ok(returnedModel) : TypedResults.NotFound();
    }

    [EndpointSummary("Create TodoList")]
    public static async Task<CreatedAtRoute> CreateTodoList(
        TodoListDto todoListDto,
        ITodoService service
    )
    {
        var createdModel = await service.CreateTodoListAsync(todoListDto);
        return TypedResults.CreatedAtRoute(
            nameof(GetTodoList),
            new { todoListId = createdModel.Id }
        );
    }

    [EndpointSummary("Update TodoList")]
    public static async Task<Results<NoContent, NotFound>> UpdateTodoList(
        int todoListId,
        TodoListDto todoListDto,
        ITodoService service
    )
    {
        var updatedModel = await service.UpdateTodoListAsync(todoListId, todoListDto);

        return updatedModel is not null ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    [EndpointSummary("Delete TodoList")]
    public static async Task<Results<NoContent, NotFound>> DeleteTodoList(
        int todoListId,
        ITodoService service
    )
    {
        var deletedModel = await service.DeleteTodoListAsync(todoListId);

        return deletedModel is not null ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}