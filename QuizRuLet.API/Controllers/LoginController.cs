using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizRuLet.API.Contracts;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        
        // public async Task<IResult> Login(UserLoginRequest request, HttpContext context)          // VALID????????
        [HttpPost]
        public async Task<IResult> Login(UserLoginRequest request)          // VALID????????
        {
            var token = await _userService.Login(request.login, request.password);

            Response.Cookies.Append("tasty-cookies", token);            // fix 
            
            // проверить login и пароль

            // создать токен

            // сохранить в куки
            return Results.Ok(token);
        }
    }
}
