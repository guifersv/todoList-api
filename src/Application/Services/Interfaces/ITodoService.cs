using ToDoList.Application.Dtos;

namespace ToDoList.Application.Services.Interfaces;

public interface ITodoService
{
    public Task<TodoListDto> CreateTodoListAsync(TodoListDto todoListDto);
    public Task<IEnumerable<TodoListDto>> GetAllTodoListsAsync();
    public Task<TodoListDto?> GetTodoListByIdAsync(int todoListId);
    public Task<TodoListDto?> UpdateTodoListAsync(int todoListId, TodoListDto todoListDto);
    public Task<TodoListDto?> DeleteTodoListAsync(int todoListId);

    public Task<TodoDto?> CreateTodoAsync(int todoListId, TodoDto todoDto);
    public Task<TodoDto?> ChangeTodoIsCompleteAsync(int todoId);
    public Task<TodoDto?> DeleteTodoAsync(int todoId);
}
