using InnoShop.Core.Models;
using InnoShop.Core.Repositories.Contexts;
using InnoShop.Core.Repositories.Repositories;
using InnoShop.Core.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

SQLitePCL.Batteries.Init();

var builder = WebApplication.CreateBuilder(args);

// JWT конфигурация
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

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRepository<User, int>, EntityRepository<User, int>>();

builder.Services.AddDbContext<ShopContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

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