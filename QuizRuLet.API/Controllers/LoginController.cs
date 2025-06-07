using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizRuLet.API.Contracts;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.API.Controllers
{
    [Route("login")]
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
        public async Task<ActionResult> Login(UserLoginRequest request)          // VALID????????
        {
            // проверить login и пароль
            
            // создание токена
            var token = await _userService.Login(request.login, request.password);

            // сохранение в куки
            Response.Cookies.Append("tasty-cookies", token);            // fix 
            
            return Ok();
        }
    }
}
