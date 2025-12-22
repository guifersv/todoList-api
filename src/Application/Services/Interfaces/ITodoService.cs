using ToDoList.Application.Dtos;

namespace ToDoList.Application.Services.Interfaces;

public interface ITodoService
{
    public Task<ValueTuple<int, TodoListDto>> CreateTodoListAsync(TodoListDto todoListDto);
    public Task<IEnumerable<TodoListDto>> GetAllTodoListsAsync();
    public Task<TodoListDto?> GetTodoListByIdAsync(int todoListId);
    public Task<bool> UpdateTodoListAsync(int todoListId, TodoListDto todoListDto);
    public Task<bool> DeleteTodoListAsync(int todoListId);

    public Task<bool> CreateTodoAsync(int todoListId, TodoDto todoDto);
    public Task<bool> ChangeTodoIsCompleteAsync(int todoId);
    public Task<bool> DeleteTodoAsync(int todoId);
}
