namespace ToDoList.UnitTests;

public class ServicesTests
{
    [Fact]
    public async Task CreateTodoListAsync_ShouldReturnTodoListDto_ShouldCallRepositoryOnce()
    {
        TodoListModel todoListModel = new() { Title = "string" };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r =>
                r.CreateTodoListAsync(
                    It.Is<TodoListModel>(m => m.Title == todoListModel.Title)
                ).Result
            )
            .Returns(todoListModel)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.CreateTodoListAsync(Utils.TodoList2Dto(todoListModel));

        Assert.IsType<TodoListDto>(returnedModel);
        Assert.Equal(todoListModel.Title, returnedModel.Title);
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

        Assert.IsType<TodoListDto>(returnedModel);
        Assert.NotNull(returnedModel);
        Assert.Equal(todoListModel.Title, returnedModel.Title);
        Assert.Equal(todoListModel.Id, returnedModel.Id);
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
    public async Task UpdateTodoListAsync_ShouldReturnTodoListDto_WhenItExists()
    {
        TodoListModel model = new() { Id = 1, Title = "string" };
        TodoListModel updatedModel = new() { Id = 1, Title = "new" };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.GetTodoListByIdAsync(It.Is<int>(id => id == updatedModel.Id)).Result)
            .Returns(model)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r =>
                r.UpdateTodoListAsync(
                    It.Is<TodoListModel>(s =>
                        s.Id == updatedModel.Id && s.Title == updatedModel.Title
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

        Assert.NotNull(returnedModel);
        Assert.IsType<TodoListDto>(returnedModel);
        Assert.Equal(model.Title, returnedModel.Title);
        Assert.Equal(model.Id, returnedModel.Id);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task UpdateTodoListAsync_ShouldReturnNull_WhenTodoListDoesNotExist()
    {
        TodoListModel updatedModel = new() { Id = 1, Title = "new" };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.GetTodoListByIdAsync(It.Is<int>(id => id == updatedModel.Id)).Result)
            .Returns((TodoListModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r =>
                r.UpdateTodoListAsync(
                    It.Is<TodoListModel>(s =>
                        s.Id == updatedModel.Id && s.Title == updatedModel.Title
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

        Assert.Null(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task DeleteTodoListAsync_ShouldReturnTodoListDto_WhenItExists()
    {
        TodoListModel model = new() { Id = 1, Title = "string" };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.GetTodoListByIdAsync(It.Is<int>(id => id == model.Id)).Result)
            .Returns(model)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r =>
                r.DeleteTodoListAsync(
                    It.Is<TodoListModel>(s => s.Id == model.Id && s.Title == model.Title)
                )
            )
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.DeleteTodoListAsync(model.Id);

        Assert.NotNull(returnedModel);
        Assert.IsType<TodoListDto>(returnedModel);
        Assert.Equal(model.Title, returnedModel.Title);
        Assert.Equal(model.Id, returnedModel.Id);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task DeleteTodoListAsync_ShouldReturnNull_WhenTodoListDoesNotExist()
    {
        TodoListModel model = new() { Id = 1, Title = "string" };

        var logger = Mock.Of<ILogger<TodoService>>();

        var repositoryMock = new Mock<ITodoRepository>();
        repositoryMock
            .Setup(r => r.GetTodoListByIdAsync(It.Is<int>(id => id == model.Id)).Result)
            .Returns((TodoListModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r =>
                r.DeleteTodoListAsync(
                    It.Is<TodoListModel>(s => s.Id == model.Id && s.Title == model.Title)
                )
            )
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Never());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.DeleteTodoListAsync(model.Id);

        Assert.Null(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task CreateTodoAsync_ShouldReturnTodoDto_WhenTodoListExists()
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
            .Setup(r =>
                r.GetTodoListByIdAsync(It.Is<int>(id => id == todoModel.TodoListModelId)).Result
            )
            .Returns(todoList)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r =>
                r.UpdateTodoListAsync(
                    It.Is<TodoListModel>(s => s.Id == todoList.Id && s.Title == todoList.Title)
                )
            )
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.CreateTodoAsync(todoList.Id, Utils.Todo2Dto(todoModel));

        Assert.NotNull(returnedModel);
        Assert.IsType<TodoDto>(returnedModel);
        Assert.Equal(todoModel.Title, returnedModel.Title);
        Assert.Equal(todoModel.Id, returnedModel.Id);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task CreateTodoAsync_ShouldReturnNull_WhenTodoListDoesNotExist()
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
            .Setup(r =>
                r.GetTodoListByIdAsync(It.Is<int>(id => id == todoModel.TodoListModelId)).Result
            )
            .Returns((TodoListModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r =>
                r.UpdateTodoListAsync(
                    It.Is<TodoListModel>(s => s.Id == todoList.Id && s.Title == todoList.Title)
                )
            )
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Never());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.CreateTodoAsync(todoList.Id, Utils.Todo2Dto(todoModel));

        Assert.Null(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task DeleteTodoAsync_ShouldReturnTodoDto_WhenItExists()
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
            .Setup(r => r.GetTodoByIdAsync(It.Is<int>(id => id == todoModel.Id)).Result)
            .Returns(todoModel)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r =>
                r.DeleteTodoAsync(
                    It.Is<TodoModel>(s =>
                        s.Id == todoModel.Id
                        && s.Title == todoModel.Title
                        && s.TodoListModelId == todoModel.TodoListModelId
                        && s.TodoListModelNavigation == todoModel.TodoListModelNavigation
                    )
                )
            )
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.DeleteTodoAsync(todoModel.Id);

        Assert.NotNull(returnedModel);
        Assert.IsType<TodoDto>(returnedModel);
        Assert.Equal(todoModel.Title, returnedModel.Title);
        Assert.Equal(todoModel.Id, returnedModel.Id);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task DeleteTodoAsync_ShouldReturnNull_WhenTodoDoesNotExist()
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
            .Setup(r => r.GetTodoByIdAsync(It.Is<int>(id => id == todoModel.Id)).Result)
            .Returns((TodoModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.DeleteTodoAsync(It.Is<TodoModel>(s => s.Id == todoModel.Id)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Never());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.DeleteTodoAsync(todoModel.Id);

        Assert.Null(returnedModel);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task ChangeTodoIsCompleteAsync_ShouldReturnTodoDto_WhenItExists()
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
            .Setup(r => r.GetTodoByIdAsync(It.Is<int>(id => id == todoModel.Id)).Result)
            .Returns(todoModel)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.UpdateTodoAsync(It.Is<TodoModel>(w => w == todoModel)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.ChangeTodoIsCompleteAsync(todoModel.Id);

        Assert.NotNull(returnedModel);
        Assert.IsType<TodoDto>(returnedModel);
        Assert.False(returnedModel.IsCompleted);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task ChangeTodoIsCompleteAsync_ShouldReturnNull_WhenTodoDoesNotExist()
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
            .Setup(r => r.GetTodoByIdAsync(It.IsAny<int>()).Result)
            .Returns((TodoModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.UpdateTodoAsync(It.Is<TodoModel>(w => w == todoModel)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Never());

        var service = new TodoService(repositoryMock.Object, logger);
        var returnedModel = await service.ChangeTodoIsCompleteAsync(todoModel.Id);

        Assert.Null(returnedModel);
        repositoryMock.Verify();
    }
}
