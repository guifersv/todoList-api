using ToDoList.Domain.Entities;

namespace ToDoList.Domain.Interfaces;

public interface ITodoRepository
{
    public Task<TodoListModel> CreateTodoListAsync(TodoListModel todoListModel);
    public Task<IEnumerable<TodoListModel>> GetAllTodoListsAsync();
    public Task<TodoListModel?> GetTodoListByIdAsync(int todoListId);
    public Task<TodoListModel?> FindTodoListByIdAsync(int todoListId);
    public Task UpdateTodoListAsync(TodoListModel todoListModel);
    public Task DeleteTodoListAsync(TodoListModel todoListModel);

    public Task<TodoModel?> GetTodoByIdAsync(int todoId);
    public Task<TodoModel?> FindTodoByIdAsync(int todoId);
    public Task UpdateTodoAsync(TodoModel todoModel);
    public Task DeleteTodoAsync(TodoModel todoModel);
}
