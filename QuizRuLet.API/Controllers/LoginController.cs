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
        
        /// Вход в аккаунт с сохранением jwt токена в куки
        /// POST: /login 200, 400
        [HttpPost]
        public async Task<ActionResult> Login(UserLoginRequest request)
        {
            var (Token, Error) = await _userService.Login(request.login, request.password);

            if (!string.IsNullOrEmpty(Error))
            {
                return BadRequest(Error);
            }

            // сохранение jwt токена в куки
            Response.Cookies.Append("tasty-cookies", Token);
            
            return Ok();
        }
    }
}
