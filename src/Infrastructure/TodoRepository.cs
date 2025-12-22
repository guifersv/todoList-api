using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Interfaces;

namespace ToDoList.Infrastructure;

public class TodoRepository(TodoDbContext context) : ITodoRepository
{
    private readonly TodoDbContext _context = context;

    public async Task<TodoListModel> CreateTodoListAsync(TodoListModel todoListModel)
    {
        var createdModel = await _context.TodoLists.AddAsync(todoListModel);
        await _context.SaveChangesAsync();
        return createdModel.Entity;
    }

    public async Task<IEnumerable<TodoListModel>> GetAllTodoListsAsync()
    {
        return await _context.TodoLists.Include(t => t.Todos).ToListAsync();
    }

    public async Task<TodoListModel?> GetTodoListByIdAsync(int todoListId)
    {
        return await _context
            .TodoLists.Include(t => t.Todos)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == todoListId);
    }

    public async Task<TodoListModel?> FindTodoListByIdAsync(int todoListId)
    {
        return await _context.TodoLists.FindAsync(todoListId);
    }

    public async Task UpdateTodoListAsync(TodoListModel todoListModel)
    {
        _context.TodoLists.Update(todoListModel);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTodoListAsync(TodoListModel todoListModel)
    {
        _context.TodoLists.Remove(todoListModel);
        await _context.SaveChangesAsync();
    }

    public async Task<TodoModel?> GetTodoByIdAsync(int todoId)
    {
        return await _context.Todos.AsNoTracking().FirstOrDefaultAsync(w => w.Id == todoId);
    }

    public async Task<TodoModel?> FindTodoByIdAsync(int todoId)
    {
        return await _context.Todos.FindAsync(todoId);
    }

    public async Task DeleteTodoAsync(TodoModel todoModel)
    {
        _context.Todos.Remove(todoModel);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTodoAsync(TodoModel todoModel)
    {
        _context.Todos.Update(todoModel);
        await _context.SaveChangesAsync();
    }
}