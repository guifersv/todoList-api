using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;
using ToDoList.Application.Services;
using ToDoList.Application.Services.Interfaces;
using ToDoList.Domain.Interfaces;
using ToDoList.Endpoints;
using ToDoList.Infrastructure;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
{
    Log.Information("App started");
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddScoped<ITodoService, TodoService>();
    builder.Services.AddScoped<ITodoRepository, TodoRepository>();

    builder.Services.AddSerilog(
        (services, ls) =>
            ls
                .ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console()
    );

    builder.Services.AddDbContext<TodoDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

    builder.Services.AddOpenApi();
    builder.Services.AddProblemDetails();
    builder.Services.AddSwaggerGen(options =>
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoList Api", Version = "v1" })
    );

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(options => options.SwaggerEndpoint("v1/swagger.json", "ToDoList Api v1"));
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler();
        app.UseStatusCodePages();
    }

    app.UseHttpsRedirection();

    app.MapGroup("").RouteTodoListEndpoint();
    app.MapGroup("/todo").RouteTodoEndpoint();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
