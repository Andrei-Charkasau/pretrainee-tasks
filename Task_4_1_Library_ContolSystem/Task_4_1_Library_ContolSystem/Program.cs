using Task_4_1_Library_ControlSystem.Repositories;
using Task_4_1_Library_ControlSystem.Services;
using Task_4_1_Library_ControlSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Swagger Init.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Book Init.
builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddTransient<IRepository<Book>, BookRepository>();

builder.Services.AddTransient<IAuthorService, AuthorService>();
builder.Services.AddTransient<IRepository<Author>, AuthorRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();

    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
