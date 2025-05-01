using QuizRuLet.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;  // инициализация конфигураций


// Заполнение DI контейнера
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QuizRuLetDbContext>(  // регистрация контекста бд
    options => 
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(QuizRuLetDbContext)));
    });


// !!!!!! TODO add scoped (InterfaceRepos, Repos)

// билд приложения
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();


// dotnet aspnet-codegenerator controller -name ModulesController -async -api -m Module -dc QuizRuLetDbContext -outDir Controllers