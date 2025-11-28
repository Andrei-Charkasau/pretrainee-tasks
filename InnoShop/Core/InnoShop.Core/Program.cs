using FluentValidation;
using FluentValidation.AspNetCore;
using InnoShop.Auth.Services.Services;
using InnoShop.Core;
using InnoShop.Core.Services.Services;
using InnoShop.Core.Services.Validators;
using InnoShop.Shared.Domain.Models;
using InnoShop.Shared.Infrastructure.Contexts;
using InnoShop.Shared.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Main API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();
builder.Services.AddDbContext<InnoDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IJwtService, JwtService>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRepository<User, int>, EntityRepository<User, int>>();

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IRepository<Product, int>, EntityRepository<Product, int>>();

builder.Services.AddOpenApi();

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<InnoDbContext>();
        while (!await context.Database.CanConnectAsync()) { }
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying migrations");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();

    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
