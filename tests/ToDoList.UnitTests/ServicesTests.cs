namespace ToDoList.UnitTests;

public class ServicesTests
{
    [Fact]
    public async Task CreateTodoListAsync_ShouldReturnDto()
    {
        TodoListModel todoListModel = new() { Id = 1, Title = "string" };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r =>
                r.CreateTodoListAsync(
                    It.Is<TodoListModel>(m =>
                        m.Title == todoListModel.Title && m.Description == todoListModel.Description
                    )
                ).Result
            )
            .Returns(todoListModel)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.CreateTodoListAsync(Utils.TodoList2Dto(todoListModel));

        Assert.IsType<TodoListDto>(returnedModel);
        Assert.Equal(todoListModel.Id, returnedModel.Id);
        Assert.Equal(todoListModel.Title, returnedModel.Title);
        Assert.Equal(todoListModel.Description, returnedModel.Description);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task GetAllTodoListsAsync_ShouldReturnListOfDtos_WhenTheyExist()
    {
        TodoListModel todoListModel = new() { Title = "string" };
        List<TodoListModel> models = [todoListModel];

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.GetAllTodoListsAsync().Result)
            .Returns(models)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.GetAllTodoListsAsync();

        Assert.IsType<List<TodoListDto>>(returnedModel);
        Assert.Single(returnedModel);
        Assert.Equal(todoListModel.Title, returnedModel.ElementAt(0).Title);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task GetAllTodoListsAsync_ShouldReturnEmptyList_WhenTheyDoNotExist()
    {
        List<TodoListModel> models = [];

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.GetAllTodoListsAsync().Result)
            .Returns(models)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.GetAllTodoListsAsync();

        Assert.IsType<List<TodoListDto>>(returnedModel);
        Assert.Empty(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task GetTodoListByIdAsync_ShouldReturnTodoListDto_WhenItExists()
    {
        TodoListModel todoListModel = new() { Id = 1, Title = "string" };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.GetTodoListByIdAsync(It.Is<int>(id => id == todoListModel.Id)).Result)
            .Returns(todoListModel)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.GetTodoListByIdAsync(todoListModel.Id);

        Assert.NotNull(returnedModel);
        Assert.IsType<TodoListDto>(returnedModel);
        Assert.Equal(todoListModel.Title, returnedModel.Title);
        Assert.Equal(todoListModel.Description, returnedModel.Description);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task GetTodoListByIdAsync_ShouldReturnNull_WhenItDoesNotExists()
    {
        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.GetTodoListByIdAsync(It.IsAny<int>()).Result)
            .Returns((TodoListModel?)null)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.GetTodoListByIdAsync(1);

        Assert.Null(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task UpdateTodoListAsync_ShouldReturnTrue_WhenItExists()
    {
        TodoListModel model = new() { Id = 1, Title = "string" };
        TodoListModel updatedModel = new() { Id = 1, Title = "new" };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.FindTodoListByIdAsync(It.Is<int>(id => id == updatedModel.Id)).Result)
            .Returns(model)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r =>
                r.UpdateTodoListAsync(
                    It.Is<TodoListModel>(s =>
                        s.Id == updatedModel.Id
                        && s.Description == updatedModel.Description
                        && s.Title == updatedModel.Title
                    )
                )
            )
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.UpdateTodoListAsync(
            updatedModel.Id,
            Utils.TodoList2Dto(updatedModel)
        );

        Assert.True(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task UpdateTodoListAsync_ShouldReturnFalse_WhenTodoListDoesNotExist()
    {
        TodoListModel updatedModel = new() { Id = 1, Title = "new" };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.FindTodoListByIdAsync(It.Is<int>(id => id == updatedModel.Id)).Result)
            .Returns((TodoListModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r =>
                r.UpdateTodoListAsync(
                    It.Is<TodoListModel>(s =>
                        s.Id == updatedModel.Id
                        && s.Description == updatedModel.Description
                        && s.Title == updatedModel.Title
                    )
                )
            )
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Never());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.UpdateTodoListAsync(
            updatedModel.Id,
            Utils.TodoList2Dto(updatedModel)
        );

        Assert.False(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task DeleteTodoListAsync_ShouldReturnTrue_WhenItExists()
    {
        TodoListModel model = new() { Id = 1, Title = "string" };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.FindTodoListByIdAsync(It.Is<int>(id => id == model.Id)).Result)
            .Returns(model)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.DeleteTodoListAsync(It.Is<TodoListModel>(s => s == model)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.DeleteTodoListAsync(model.Id);

        Assert.True(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task DeleteTodoListAsync_ShouldReturnFalse_WhenTodoListDoesNotExist()
    {
        TodoListModel model = new() { Id = 1, Title = "string" };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.FindTodoListByIdAsync(It.Is<int>(id => id == model.Id)).Result)
            .Returns((TodoListModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.DeleteTodoListAsync(It.Is<TodoListModel>(s => s == model)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Never());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.DeleteTodoListAsync(model.Id);

        Assert.False(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task GetTodoByIdAsync_ShouldReturnDto_WhenTodoExists()
    {
        TodoListModel todoList = new() { Id = 1, Title = "string" };
        TodoModel todoModel = new()
        {
            Id = 1,
            Title = "str",
            TodoListModelId = todoList.Id,
            TodoListModelNavigation = todoList,
        };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.GetTodoByIdAsync(It.Is<int>(id => id == todoModel.Id)).Result)
            .Returns(todoModel)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.GetTodoByIdAsync(todoModel.Id);

        Assert.NotNull(returnedModel);
        Assert.IsType<TodoDto>(returnedModel);
        Assert.Equal(todoModel.Id, returnedModel.Id);
        Assert.Equal(todoModel.Title, returnedModel.Title);
        Assert.Equal(todoModel.Description, returnedModel.Description);
        Assert.Equal(todoModel.DateCreated, returnedModel.DateCreated);
        Assert.Equal(todoModel.IsCompleted, returnedModel.IsCompleted);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task GetTodoByIdAsync_ShouldReturnNull_WhenTodoDoesNotExist()
    {
        TodoListModel todoList = new() { Id = 1, Title = "string" };
        TodoModel todoModel = new()
        {
            Id = 1,
            Title = "str",
            TodoListModelId = todoList.Id,
            TodoListModelNavigation = todoList,
        };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.GetTodoByIdAsync(It.Is<int>(id => id == todoModel.Id)).Result)
            .Returns((TodoModel?)null)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.GetTodoByIdAsync(todoModel.Id);

        Assert.Null(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task CreateTodoAsync_ShouldReturnDto_WhenTodoListExists()
    {
        TodoListModel todoList = new() { Id = 1, Title = "string" };
        TodoModel todoModel = new()
        {
            Id = 1,
            Title = "str",
            TodoListModelId = todoList.Id,
            TodoListModelNavigation = todoList,
        };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.FindTodoListByIdAsync(It.Is<int>(id => id == todoList.Id)).Result)
            .Returns(todoList)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r =>
                r.CreateTodoAsync(
                    It.Is<int>(i => i == todoList.Id),
                    It.Is<TodoModel>(m =>
                        m.Title == todoModel.Title
                        && m.Description == todoModel.Description
                        && m.DateCreated == todoModel.DateCreated
                        && m.IsCompleted == todoModel.IsCompleted
                        && m.TodoListModelNavigation == todoModel.TodoListModelNavigation
                    )
                ).Result
            )
            .Returns(todoModel)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.CreateTodoAsync(todoList.Id, Utils.Todo2Dto(todoModel));

        Assert.NotNull(returnedModel);
        Assert.IsType<TodoDto>(returnedModel);
        Assert.Equal(todoModel.Title, returnedModel.Title);
        Assert.Equal(todoModel.Description, returnedModel.Description);
        Assert.Equal(todoModel.DateCreated, returnedModel.DateCreated);
        Assert.Equal(todoModel.IsCompleted, returnedModel.IsCompleted);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task CreateTodoAsync_ShouldReturnNull_WhenTodoListDoesNotExist()
    {
        TodoListModel todoList = new() { Id = 1, Title = "string" };
        TodoModel todoModel = new()
        {
            Id = 1,
            Title = "str",
            TodoListModelId = todoList.Id,
            TodoListModelNavigation = todoList,
        };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.FindTodoListByIdAsync(It.Is<int>(id => id == todoList.Id)).Result)
            .Returns((TodoListModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r =>
                r.CreateTodoAsync(
                    It.Is<int>(i => i == todoList.Id),
                    It.Is<TodoModel>(m =>
                        m.Title == todoModel.Title
                        && m.Description == todoModel.Description
                        && m.DateCreated == todoModel.DateCreated
                        && m.IsCompleted == todoModel.IsCompleted
                        && m.TodoListModelNavigation == todoModel.TodoListModelNavigation
                    )
                ).Result
            )
            .Returns(todoModel)
            .Verifiable(Times.Never());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.CreateTodoAsync(todoList.Id, Utils.Todo2Dto(todoModel));

        Assert.Null(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task DeleteTodoAsync_ShouldReturnTrue_WhenItExists()
    {
        TodoListModel todoList = new() { Id = 1, Title = "string" };
        TodoModel todoModel = new()
        {
            Id = 1,
            Title = "string",
            TodoListModelId = todoList.Id,
            TodoListModelNavigation = todoList,
        };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.FindTodoByIdAsync(It.Is<int>(id => id == todoModel.Id)).Result)
            .Returns(todoModel)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.DeleteTodoAsync(It.Is<TodoModel>(s => s == todoModel)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.DeleteTodoAsync(todoModel.Id);

        Assert.True(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task DeleteTodoAsync_ShouldReturnFalse_WhenTodoDoesNotExist()
    {
        TodoListModel todoList = new() { Id = 1, Title = "string" };
        TodoModel todoModel = new()
        {
            Id = 1,
            Title = "string",
            TodoListModelId = todoList.Id,
            TodoListModelNavigation = todoList,
        };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.FindTodoByIdAsync(It.Is<int>(id => id == todoModel.Id)).Result)
            .Returns((TodoModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.DeleteTodoAsync(It.Is<TodoModel>(s => s == todoModel)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Never());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.DeleteTodoAsync(todoModel.Id);

        Assert.False(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task ChangeTodoIsCompleteAsync_ShouldReturnTrue_WhenItExists()
    {
        TodoListModel todoList = new() { Id = 1, Title = "string" };
        TodoModel todoModel = new()
        {
            Id = 1,
            Title = "string",
            IsCompleted = true,
            TodoListModelId = todoList.Id,
            TodoListModelNavigation = todoList,
        };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.FindTodoByIdAsync(It.Is<int>(id => id == todoModel.Id)).Result)
            .Returns(todoModel)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.UpdateTodoAsync(It.Is<TodoModel>(s => s == todoModel)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.ChangeTodoIsCompleteAsync(todoModel.Id);

        Assert.True(returnedModel);
        Assert.False(todoModel.IsCompleted);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task ChangeTodoIsCompleteAsync_ShouldReturnFalse_WhenTodoDoesNotExist()
    {
        TodoListModel todoList = new() { Id = 1, Title = "string" };
        TodoModel todoModel = new()
        {
            Id = 1,
            Title = "string",
            IsCompleted = true,
            TodoListModelId = todoList.Id,
            TodoListModelNavigation = todoList,
        };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.FindTodoByIdAsync(It.Is<int>(id => id == todoModel.Id)).Result)
            .Returns((TodoModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.UpdateTodoAsync(It.Is<TodoModel>(s => s == todoModel)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Never());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.ChangeTodoIsCompleteAsync(todoModel.Id);

        Assert.False(returnedModel);
        Assert.True(todoModel.IsCompleted);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task GetAllTodosAsync_ShouldReturnListOfDtos_WhenTodoListExists()
    {
        TodoListModel todoList = new() { Id = 1, Title = "string" };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.GetTodoListByIdAsync(It.Is<int>(id => id == todoList.Id)).Result)
            .Returns(todoList)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.GetAllTodosAsync(todoList.Id);

        Assert.NotNull(returnedModel);
        Assert.IsType<List<TodoDto>>(returnedModel);
        Assert.Empty(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task GetAllTodosAsync_ShouldReturnNull_WhenTodoListDoesNotExist()
    {
        TodoListModel todoList = new() { Id = 1, Title = "string" };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.GetTodoListByIdAsync(It.Is<int>(id => id == todoList.Id)).Result)
            .Returns((TodoListModel?)null)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.GetAllTodosAsync(todoList.Id);

        Assert.Null(returnedModel);
        repositoryMock.Verify();
    }
}
