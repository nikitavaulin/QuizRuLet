using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizRuLet.API.Contracts;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.API.Controllers
{
    [Route("register")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IUserService _userService;

        public RegistrationController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost]
        public async Task<ActionResult> Register(UserRegisterRequest request)   
        {
            var response = await _userService.Register(request.login, request.password);
            if (response.Success)
            {
                return Ok();
            }
            else
            {
                return BadRequest(response.Error);
            }
        }
    }
}
