using ToDoList.Application.Dtos;

namespace ToDoList.Application.Services.Interfaces;

public interface ITodoService
{
    public Task<TodoListDto> CreateTodoListAsync(TodoListDto todoListDto);
    public Task<IEnumerable<TodoListDto>> GetAllTodoListsAsync();
    public Task<TodoListDto?> GetTodoListByIdAsync(int todoListId);
    public Task<bool> UpdateTodoListAsync(int todoListId, TodoListDto todoListDto);
    public Task<bool> DeleteTodoListAsync(int todoListId);

    public Task<TodoDto?> CreateTodoAsync(int todoListId, TodoDto todoDto);
    public Task<IEnumerable<TodoDto>?> GetAllTodosAsync(int todoListId);
    public Task<TodoDto?> GetTodoByIdAsync(int todoId);
    public Task<bool> ChangeTodoIsCompleteAsync(int todoId);
    public Task<bool> DeleteTodoAsync(int todoId);
}
