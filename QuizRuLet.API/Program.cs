using QuizRuLet.DataAccess;
using Microsoft.EntityFrameworkCore;
using QuizRuLet.DataAccess.Repositories;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Application.Services;
using QuizRuLet.Infrastrucrture;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;          // ссылка на конфигурации
var services = builder.Services;                    // ссылка на сервисы

#region  Заполнение DI контейнера
services.AddControllers();

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)       // learn
    .AddCookie(options => 
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

services.AddSwaggerGen();

services.AddDbContext<QuizRuLetDbContext>(  // регистрация контекста бд
    options => 
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(QuizRuLetDbContext)));
    });

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));       // jwt auth config


services.AddScoped<IModulesRepository, ModulesRepository>();
services.AddScoped<ICardsRepository, CardsRepository>();
services.AddScoped<IUsersRepository, UsersRepository>();

// !!!!!! TODO add scoped (InterfaceService, Service)
services.AddScoped<IModuleService, ModuleService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<ICardService, CardService>();
services.AddScoped<ILearningModuleService, LearningModuleService>();
services.AddScoped<IModuleProgressService, ModuleProgressService>();

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCookiePolicy(new CookiePolicyOptions         // безопасность
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.Run();