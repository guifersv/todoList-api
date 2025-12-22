using Microsoft.AspNetCore.Http.HttpResults;

namespace ToDoList.UnitTests;

public class EndpointsTests
{
    [Fact]
    public async Task GetAllTodoLists_ShouldReturnListOfDtos_ShouldCallService()
    {
        TodoListDto model = new() { Title = "string" };
        List<TodoListDto> models = [model];
        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.GetAllTodoListsAsync().Result)
            .Returns(models)
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.GetAllTodoLists(serviceMock.Object);

        Assert.IsType<IEnumerable<TodoListDto>>(result, exactMatch: false);
        Assert.Single(result, model);
        serviceMock.Verify();
    }

    [Fact]
    public async Task GetTodoList_ShouldReturnOk_WhenItExists()
    {
        TodoListDto model = new() { Id = 1, Title = "string" };
        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.GetTodoListByIdAsync(It.Is<int>(id => id == model.Id)).Result)
            .Returns(model)
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.GetTodoList(model.Id, serviceMock.Object);

        var returnedModel = Assert.IsType<Ok<TodoListDto>>(result.Result);
        Assert.Equal(model, returnedModel.Value);
        serviceMock.Verify();
    }

    [Fact]
    public async Task GetTodoList_ShouldReturnNotFound_WhenItDoesNotExist()
    {
        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.GetTodoListByIdAsync(It.IsAny<int>()).Result)
            .Returns((TodoListDto?)null)
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.GetTodoList(1, serviceMock.Object);

        Assert.IsType<NotFound>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task CreateTodoList_ShouldReturnCreatedAtRoute_WhenModelIsValid()
    {
        TodoListDto model = new() { Id = 1, Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s =>
                s.CreateTodoListAsync(
                    It.Is<TodoListDto>(d => d.Id == model.Id && d.Title == model.Title)
                ).Result
            )
            .Returns(model)
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.CreateTodoList(model, serviceMock.Object);
        var returnedModel = Assert.IsType<CreatedAtRoute>(result);

        var values = Assert.Single(returnedModel.RouteValues);
        Assert.Equal(model.Id, values.Value);
        Assert.Equal(nameof(TodoListEndpoints.GetTodoList), returnedModel.RouteName);
        serviceMock.Verify();
    }

    [Fact]
    public async Task UpdateTodoList_ShouldReturnNoContent_WhenModelExists()
    {
        TodoListDto updatedModel = new() { Id = 1, Title = "str" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s =>
                s.UpdateTodoListAsync(
                    It.Is<int>(id => id == updatedModel.Id),
                    It.Is<TodoListDto>(m =>
                        m.Id == updatedModel.Id && m.Title == updatedModel.Title
                    )
                ).Result
            )
            .Returns(updatedModel)
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.UpdateTodoList(
            updatedModel.Id,
            updatedModel,
            serviceMock.Object
        );

        Assert.IsType<NoContent>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task UpdateTodoList_ShouldReturnNotFound_WhenModelDoesNotExist()
    {
        TodoListDto updatedModel = new() { Id = 1, Title = "str" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s =>
                s.UpdateTodoListAsync(
                    It.Is<int>(id => id == updatedModel.Id),
                    It.Is<TodoListDto>(m =>
                        m.Id == updatedModel.Id && m.Title == updatedModel.Title
                    )
                ).Result
            )
            .Returns((TodoListDto?)null)
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.UpdateTodoList(
            updatedModel.Id,
            updatedModel,
            serviceMock.Object
        );

        Assert.IsType<NotFound>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task DeleteTodoList_ShouldReturnNoContent_WhenModelExists()
    {
        TodoListDto model = new() { Id = 1, Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.DeleteTodoListAsync(It.Is<int>(id => id == model.Id)).Result)
            .Returns(model)
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.DeleteTodoList(model.Id, serviceMock.Object);

        Assert.IsType<NoContent>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task DeleteTodoList_ShouldReturnNotFound_WhenModelDoesNotExist()
    {
        TodoListDto model = new() { Id = 1, Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.DeleteTodoListAsync(It.Is<int>(id => id == model.Id)).Result)
            .Returns((TodoListDto?)null)
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.DeleteTodoList(model.Id, serviceMock.Object);

        Assert.IsType<NotFound>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task CreateTodo_ShouldReturnCreated_WhenTodoListExists()
    {
        TodoDto model = new() { Title = "string" };
        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s =>
                s.CreateTodoAsync(
                    It.IsAny<int>(),
                    It.Is<TodoDto>(m => m.Title == model.Title)
                ).Result
            )
            .Returns(model)
            .Verifiable(Times.Once());

        var result = await TodoEndpoints.CreateTodo(1, model, serviceMock.Object);

        Assert.IsType<Created>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task CreateTodo_ShouldReturnNotFound_WhenTodoListDoesNotExist()
    {
        TodoDto model = new() { Title = "string" };
        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s =>
                s.CreateTodoAsync(
                    It.IsAny<int>(),
                    It.Is<TodoDto>(m => m.Title == model.Title)
                ).Result
            )
            .Returns((TodoDto?)null)
            .Verifiable(Times.Once());

        var result = await TodoEndpoints.CreateTodo(1, model, serviceMock.Object);

        Assert.IsType<NotFound>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task DeleteTodo_ShouldReturnNoContent_WhenTodoExists()
    {
        TodoDto model = new() { Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.DeleteTodoAsync(It.IsAny<int>()).Result)
            .Returns(model)
            .Verifiable(Times.Once());

        var result = await TodoEndpoints.DeleteTodo(1, serviceMock.Object);

        Assert.IsType<NoContent>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task DeleteTodo_ShouldReturnNotFound_WhenTodoDoesNotExist()
    {
        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.DeleteTodoAsync(It.IsAny<int>()).Result)
            .Returns((TodoDto?)null)
            .Verifiable(Times.Once());

        var result = await TodoEndpoints.DeleteTodo(1, serviceMock.Object);

        Assert.IsType<NotFound>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task ChangeTodoIsComplete_ShouldReturnNoContent_WhenTodoExists()
    {
        TodoDto model = new() { Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.ChangeTodoIsCompleteAsync(It.IsAny<int>()).Result)
            .Returns(model)
            .Verifiable(Times.Once());

        var result = await TodoEndpoints.ChangeTodoIsComplete(1, serviceMock.Object);

        Assert.IsType<NoContent>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task ChangeTodoIsComplete_ShouldReturnNotFound_WhenTodoDoesNotExist()
    {
        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.ChangeTodoIsCompleteAsync(It.IsAny<int>()).Result)
            .Returns((TodoDto?)null)
            .Verifiable(Times.Once());

        var result = await TodoEndpoints.ChangeTodoIsComplete(1, serviceMock.Object);

        Assert.IsType<NotFound>(result.Result);
        serviceMock.Verify();
    }
}
