using QuizRuLet.DataAccess;
using Microsoft.EntityFrameworkCore;
using QuizRuLet.DataAccess.Repositories;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Application.Services;
using QuizRuLet.Infrastrucrture;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Authentication.Cookies;
using QuizRuLet.API.Extensions;
using QuizRuLet.Core.Models;
using Microsoft.Net.Http.Headers;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;          // ссылка на конфигурации
var services = builder.Services;                    // ссылка на сервисы

#region  Заполнение DI контейнера
services.AddControllers();

services.AddAuthentication();

// конфигурация аппсетингов для извлечения ключей и настроек
services.AddSingleton<IConfiguration>(provider =>       
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build());


services.Configure<AiApiOptions>(configuration.GetSection("AiApi"));


services.AddSwaggerGen();   // swagger

services.AddDbContext<QuizRuLetDbContext>(  // регистрация контекста бд
    options => 
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(QuizRuLetDbContext)));   // postgres
    });

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));       // jwt auth config
services.AddApiAuthentification(configuration);

// репозитории
services.AddScoped<IModulesRepository, ModulesRepository>();
services.AddScoped<ICardsRepository, CardsRepository>();
services.AddScoped<IUsersRepository, UsersRepository>();

// сервисы
services.AddScoped<IModuleService, ModuleService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<ICardService, CardService>();
services.AddScoped<ILearningModuleService, LearningModuleService>();
services.AddScoped<IModuleProgressService, ModuleProgressService>();
services.AddScoped<ICardSetCreationService, CardSetCreationService>();
services.AddScoped<ICardSetAiCreationService, CardSetAiCreationService>();

// ai
services.AddScoped<IAiApiProvider, AiApiProvider>();


// auth
services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();
#endregion

// билд приложения
var app = builder.Build();

// Конфигурация пайплайна HTTP запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region  Middlewares
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Статические файлы (фронтенд)
app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var headers = ctx.Context.Response.GetTypedHeaders();

        headers.CacheControl = new CacheControlHeaderValue
        {
            NoStore        = true,   // ничего не хранить
            NoCache        = true,   // всегда перепроверять
            MustRevalidate = true
        };
        ctx.Context.Response.Headers["Pragma"]  = "no-cache";   // отключене кэширования страницы
        ctx.Context.Response.Headers["Expires"] = "0";
    }
});

#endregion

// запуск приложения
app.Run();  