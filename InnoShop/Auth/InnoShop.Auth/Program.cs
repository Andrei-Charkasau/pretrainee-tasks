using FluentValidation;
using FluentValidation.AspNetCore;
using InnoShop.Auth;
using InnoShop.Auth.Services.Services;
using InnoShop.Core.Services.Services;
using InnoShop.Core.Services.Validators;
using InnoShop.Shared.Domain.Models;
using InnoShop.Shared.Infrastructure.Contexts;
using InnoShop.Shared.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<ProductDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IRepository<Product, int>, EntityRepository<Product, int>>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRepository<User, int>, EntityRepository<User, int>>();



builder.Services.AddDbContext<InnoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка CORS для доступа из основного проекта
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMainAPI", policy =>
    {
        policy.WithOrigins("https://localhost:7240", "http://localhost:5269")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<CustomExceptionHandlerMiddleware>();
app.UseStatusCodePages(async statusCodeContext =>
{
    statusCodeContext.HttpContext.Response.ContentType = "application/problem+json";
    await statusCodeContext.HttpContext.Response.WriteAsJsonAsync(new
    {
        Title = "Error",
        Status = statusCodeContext.HttpContext.Response.StatusCode,
        Detail = "An error occurred while processing your request"
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowMainAPI");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();