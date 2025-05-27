// Fix for CS0106: Remove the 'public' modifier from the local function 'Configure' as it is not valid for local functions.
// Fix for CS8321: Remove the unused local function 'Configure' since it is not being called anywhere in the code.
// Fix for IDE0062: If the local function 'Configure' were to remain, it could be made static, but since it is unused, it is removed.

using Application.Repositories;
using Domain.Common;
using Domain.Interfaces;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddInfrastructure(builder.Configuration);

// Register the correct DbContext
builder.Services.AddDbContext<UserManagementDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddDbContext<LLMResultManagementDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Infrastructure")
    )
);

builder.Services.AddDbContext<LegalDocumentManagementDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Infrastructure")
    )
);

builder.Services.AddDbContext<AnalysisResultManagementDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Infrastructure")
    )
);

builder.Services.Configure<DeepSeekSettings>(builder.Configuration.GetSection("DeepSeek"));
builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection("Google"));


// Register the repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILLMResultRepository, LLMResultRepository>();
builder.Services.AddScoped<ILegalDocumentRepository, LegalDocumentRepository>();
builder.Services.AddScoped<IAnalysisResultRepository, AnalysisResultRepository>();
// Register the service
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LLMResultService>();
builder.Services.AddScoped<LegalDocumentService>();
builder.Services.AddScoped<AnalysisResultService>();
//builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:11434"), "tinyllama"));
builder.Services.AddScoped<IOllamaModel, OllamaModel>();


builder.Services.AddScoped<IOllamaModel, OllamaModel>();
builder.Services.AddHttpClient("ExternalServices", client =>
{
    client.BaseAddress = new Uri("https://localhost:7263/");
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Simplex API V1");
        options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseCors(policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
