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
        const int modelId = 1;
        TodoListDto model = new() { Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.GetTodoListByIdAsync(It.Is<int>(id => id == modelId)).Result)
            .Returns(model)
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.GetTodoList(modelId, serviceMock.Object);

        var returnedModel = Assert.IsType<Ok<TodoListDto>>(result.Result);
        Assert.Equal(model, returnedModel.Value);
        serviceMock.Verify();
    }

    [Fact]
    public async Task GetTodoList_ShouldReturnNotFound_WhenItDoesNotExist()
    {
        const int modelId = 1;
        TodoListDto model = new() { Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.GetTodoListByIdAsync(It.Is<int>(id => id == modelId)).Result)
            .Returns((TodoListDto?)null)
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.GetTodoList(modelId, serviceMock.Object);

        Assert.IsType<NotFound>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task CreateTodoList_ShouldReturnCreatedAtRoute_WhenModelIsValid()
    {
        const int modelId = 1;
        TodoListDto model = new() { Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.CreateTodoListAsync(It.Is<TodoListDto>(t => t == model)).Result)
            .Returns(new ValueTuple<int, TodoListDto>(modelId, model))
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.CreateTodoList(model, serviceMock.Object);

        var returnedModel = Assert.IsType<CreatedAtRoute<TodoListDto>>(result);
        var values = Assert.Single(returnedModel.RouteValues);
        Assert.Equal(model, returnedModel.Value);
        Assert.Equal(nameof(TodoListEndpoints.GetTodoList), returnedModel.RouteName);
        Assert.Equal(modelId, values.Value);
        serviceMock.Verify();
    }

    [Fact]
    public async Task UpdateTodoList_ShouldReturnNoContent_WhenModelExists()
    {
        const int modelId = 1;
        TodoListDto updatedModel = new() { Title = "str" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s =>
                s.UpdateTodoListAsync(
                    It.Is<int>(id => id == modelId),
                    It.Is<TodoListDto>(m => m == updatedModel)
                ).Result
            )
            .Returns(true)
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.UpdateTodoList(
            modelId,
            updatedModel,
            serviceMock.Object
        );

        Assert.IsType<NoContent>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task UpdateTodoList_ShouldReturnNotFound_WhenModelDoesNotExist()
    {
        const int modelId = 1;
        TodoListDto updatedModel = new() { Title = "str" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s =>
                s.UpdateTodoListAsync(
                    It.Is<int>(id => id == modelId),
                    It.Is<TodoListDto>(m => m == updatedModel)
                ).Result
            )
            .Returns(false)
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.UpdateTodoList(
            modelId,
            updatedModel,
            serviceMock.Object
        );

        Assert.IsType<NotFound>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task DeleteTodoList_ShouldReturnNoContent_WhenModelExists()
    {
        const int modelId = 1;
        TodoListDto model = new() { Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.DeleteTodoListAsync(It.Is<int>(id => id == modelId)).Result)
            .Returns(true)
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.DeleteTodoList(modelId, serviceMock.Object);

        Assert.IsType<NoContent>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task DeleteTodoList_ShouldReturnNotFound_WhenModelDoesNotExist()
    {
        const int modelId = 1;
        TodoListDto model = new() { Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.DeleteTodoListAsync(It.Is<int>(id => id == modelId)).Result)
            .Returns(false)
            .Verifiable(Times.Once());

        var result = await TodoListEndpoints.DeleteTodoList(modelId, serviceMock.Object);

        Assert.IsType<NotFound>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task CreateTodo_ShouldReturnCreated_WhenTodoListExists()
    {
        const int modelId = 1;
        TodoDto model = new() { Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s =>
                s.CreateTodoAsync(
                    It.Is<int>(id => id == modelId),
                    It.Is<TodoDto>(m => m == model)
                ).Result
            )
            .Returns(true)
            .Verifiable(Times.Once());

        var result = await TodoEndpoints.CreateTodo(modelId, model, serviceMock.Object);

        Assert.IsType<Created>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task CreateTodo_ShouldReturnNotFound_WhenTodoListDoesNotExist()
    {
        const int modelId = 1;
        TodoDto model = new() { Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s =>
                s.CreateTodoAsync(
                    It.Is<int>(id => id == modelId),
                    It.Is<TodoDto>(m => m == model)
                ).Result
            )
            .Returns(false)
            .Verifiable(Times.Once());

        var result = await TodoEndpoints.CreateTodo(modelId, model, serviceMock.Object);

        Assert.IsType<NotFound>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task DeleteTodo_ShouldReturnNoContent_WhenTodoExists()
    {
        const int modelId = 1;
        TodoDto model = new() { Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.DeleteTodoAsync(It.Is<int>(id => id == modelId)).Result)
            .Returns(true)
            .Verifiable(Times.Once());

        var result = await TodoEndpoints.DeleteTodo(modelId, serviceMock.Object);

        Assert.IsType<NoContent>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task DeleteTodo_ShouldReturnNotFound_WhenTodoDoesNotExist()
    {
        const int modelId = 1;
        TodoDto model = new() { Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.DeleteTodoAsync(It.Is<int>(id => id == modelId)).Result)
            .Returns(false)
            .Verifiable(Times.Once());

        var result = await TodoEndpoints.DeleteTodo(modelId, serviceMock.Object);

        Assert.IsType<NotFound>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task ChangeTodoIsComplete_ShouldReturnNoContent_WhenTodoExists()
    {
        const int modelId = 1;
        TodoDto model = new() { Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.ChangeTodoIsCompleteAsync(It.Is<int>(id => id == modelId)).Result)
            .Returns(true)
            .Verifiable(Times.Once());

        var result = await TodoEndpoints.ChangeTodoIsComplete(modelId, serviceMock.Object);

        Assert.IsType<NoContent>(result.Result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task ChangeTodoIsComplete_ShouldReturnNotFound_WhenTodoDoesNotExist()
    {
        const int modelId = 1;
        TodoDto model = new() { Title = "string" };

        var serviceMock = new Mock<ITodoService>();
        serviceMock
            .Setup(s => s.ChangeTodoIsCompleteAsync(It.Is<int>(id => id == modelId)).Result)
            .Returns(false)
            .Verifiable(Times.Once());

        var result = await TodoEndpoints.ChangeTodoIsComplete(modelId, serviceMock.Object);

        Assert.IsType<NotFound>(result.Result);
        serviceMock.Verify();
    }
}
