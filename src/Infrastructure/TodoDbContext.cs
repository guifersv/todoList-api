using Microsoft.EntityFrameworkCore;

using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<TodoListModel> TodoLists { get; set; }
    public DbSet<TodoModel> Todos { get; set; }
}