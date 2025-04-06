using QuizRuLet.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;  // инициализация конфигураций


// Заполнение DI контейнера
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QuizRuLetDbContext>(  // подключение бд
    options => 
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(QuizRuLetDbContext)));
    });


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
