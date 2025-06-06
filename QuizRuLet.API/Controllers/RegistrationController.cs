using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizRuLet.API.Contracts;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IUserService _userService;

        public RegistrationController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost]
        public async Task<IResult> Register(UserRegisterRequest request)        // VALID????
        {
            await _userService.Register(request.login, request.password);

            return Results.Ok();
        }
    
    
        // [HttpPost]
        // public async Task Register()
    }
}
