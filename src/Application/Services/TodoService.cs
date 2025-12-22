using ToDoList.Application.Dtos;
using ToDoList.Application.Services.Interfaces;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Interfaces;
using ToDoList.Utilities;

namespace ToDoList.Application.Services;

public class TodoService(ITodoRepository repository, ILogger<TodoService> logger) : ITodoService
{
    private readonly ILogger<TodoService> _logger = logger;
    private readonly ITodoRepository _repository = repository;

    public async Task<ValueTuple<int, TodoListDto>> CreateTodoListAsync(TodoListDto todoListDto)
    {
        _logger.LogInformation("TodoService: Creating the todo list");

        TodoListModel todoList = new()
        {
            Title = todoListDto.Title,
            Description = todoListDto.Description,
        };
        var createdModel = await _repository.CreateTodoListAsync(todoList);

        return new(createdModel.Id, Utils.TodoList2Dto(createdModel));
    }

    public async Task<IEnumerable<TodoListDto>> GetAllTodoListsAsync()
    {
        _logger.LogInformation("TodoService: Retrieving all todo lists");

        var todoListModels = await _repository.GetAllTodoListsAsync();

        return todoListModels.Select(Utils.TodoList2Dto).ToList();
    }

    public async Task<TodoListDto?> GetTodoListByIdAsync(int todoListId)
    {
        _logger.LogInformation("TodoService: Retrieving todo list by id");

        var result = await _repository.GetTodoListByIdAsync(todoListId);

        if (result is not null)
            return Utils.TodoList2Dto(result);

        _logger.LogWarning("TodoService: TodoListModel does not exist in database");
        return null;
    }

    public async Task<bool> UpdateTodoListAsync(int todoListId, TodoListDto todoListDto)
    {
        _logger.LogInformation("TodoService: Updating todo list");

        var model = await _repository.FindTodoListByIdAsync(todoListId);

        if (model is not null)
        {
            model.Title = todoListDto.Title;
            model.Description = todoListDto.Description;
            await _repository.UpdateTodoListAsync(model);
            return true;
        }

        _logger.LogWarning("TodoService: TodoListModel does not exist in database");
        return false;
    }

    public async Task<bool> DeleteTodoListAsync(int todoListId)
    {
        _logger.LogInformation("TodoService: Updating todo list");

        var model = await _repository.FindTodoListByIdAsync(todoListId);

        if (model is not null)
        {
            await _repository.DeleteTodoListAsync(model);
            return true;
        }

        _logger.LogWarning("TodoService: TodoListModel does not exist in database");
        return false;
    }

    public async Task<bool> CreateTodoAsync(int todoListId, TodoDto todoDto)
    {
        _logger.LogInformation("TodoService: Creating todo");

        var todoList = await _repository.FindTodoListByIdAsync(todoListId);

        if (todoList is not null)
        {
            TodoModel todoModel = new()
            {
                Title = todoDto.Title,
                Description = todoDto.Description,
                DateCreated = todoDto.DateCreated,
                IsCompleted = todoDto.IsCompleted,
                TodoListModelNavigation = todoList,
            };
            todoList.Todos.Add(todoModel);
            await _repository.UpdateTodoListAsync(todoList);
            return true;
        }

        _logger.LogWarning("TodoService: TodoListModel does not exist in database");
        return false;
    }

    public async Task<bool> DeleteTodoAsync(int todoId)
    {
        _logger.LogInformation("TodoService: Deleting todo");

        var model = await _repository.FindTodoByIdAsync(todoId);

        if (model is not null)
        {
            await _repository.DeleteTodoAsync(model);
            return true;
        }

        _logger.LogWarning("TodoService: TodoModel does not exist in database");
        return false;
    }

    public async Task<bool> ChangeTodoIsCompleteAsync(int todoId)
    {
        _logger.LogInformation("TodoService: Changing todo IsComplete property");

        var model = await _repository.FindTodoByIdAsync(todoId);

        if (model is not null)
        {
            model.IsCompleted = !model.IsCompleted;
            await _repository.UpdateTodoAsync(model);
            return true;
        }

        _logger.LogWarning("TodoService: TodoModel does not exist in database");
        return false;
    }
}
