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


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;          // ссылка на конфигурации
var services = builder.Services;                    // ссылка на сервисы

#region  Заполнение DI контейнера
services.AddControllers();

services.AddAuthentication();

services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5047")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


services.AddSingleton<IConfiguration>(provider =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build());


services.Configure<AiApiOptions>(configuration.GetSection("AiApi"));

// services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)       // learn
//     .AddCookie(options => 
//     {
//         options.LoginPath = "/Account/Login";                       // перенаправление на указанный путь, в случае если юзер не аутентифицирован
//         options.AccessDeniedPath = "/Account/AccessDenied";         // перенаправление на указанный путь, в случае если юзер не имеет достаточно прав
//     });


services.AddSwaggerGen();

services.AddDbContext<QuizRuLetDbContext>(  // регистрация контекста бд
    options => 
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(QuizRuLetDbContext)));
    });

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));       // jwt auth config
services.AddApiAuthentification(configuration);

services.AddScoped<IModulesRepository, ModulesRepository>();
services.AddScoped<ICardsRepository, CardsRepository>();
services.AddScoped<IUsersRepository, UsersRepository>();

// !!!!!! TODO add scoped (InterfaceService, Service)
services.AddScoped<IModuleService, ModuleService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<ICardService, CardService>();
services.AddScoped<ILearningModuleService, LearningModuleService>();
services.AddScoped<IModuleProgressService, ModuleProgressService>();
services.AddScoped<ICardSetCreationService, CardSetCreationService>();
services.AddScoped<ICardSetAiCreationService, CardSetAiCreationService>();

// ai
services.AddScoped<IAiApiProvider, AiApiProvider>();

// 123456

// auth
services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();
#endregion

// билд приложения
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares
app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");         // CORS

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// app.UseCookiePolicy(new CookiePolicyOptions         // безопасность
// {
//     MinimumSameSitePolicy = SameSiteMode.None,
//     HttpOnly = HttpOnlyPolicy.Always,
//     Secure = CookieSecurePolicy.Always
// });

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
        ctx.Context.Response.Headers["Pragma"]  = "no-cache";
        ctx.Context.Response.Headers["Expires"] = "0";
    }
});

app.Run();