using Microsoft.EntityFrameworkCore;
using Task_4_1_Library_ControlSystem.Contexts;
using Task_4_1_Library_ControlSystem.Extensions;
using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.Repositories;
using Task_4_1_Library_ControlSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Swagger Init.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LibraryContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddTransient<IRepository<Book, int>, EntityRepository<Book, int>>();

builder.Services.AddTransient<IAuthorService, AuthorService>();
builder.Services.AddTransient<IRepository<Author, int>, EntityRepository<Author, int>>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseGlobalExceptionHandler();

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
