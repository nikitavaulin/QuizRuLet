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
        
        [HttpPost]
        public async Task<ActionResult> Login(UserLoginRequest request)          // VALID????????
        {
            
            var response = await _userService.Login(request.login, request.password);

            if (!string.IsNullOrEmpty(response.Error))
            {
                return BadRequest(response.Error);
            }

            // сохранение в куки
            Response.Cookies.Append("tasty-cookies", response.Token);
            
            return Ok();
        }
    }
}
