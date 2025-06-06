using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QuizRuLet.API.Contracts;
using QuizRuLet.Application.Services;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        private readonly IUserService _userService;
        private readonly IModuleProgressService _progressService;
        
        
        public UsersController(
            IUserService userService,
            IModuleService moduleService,
            IModuleProgressService moduleProgressService
            )
        {
            _userService = userService;
            _moduleService = moduleService;
            _progressService = moduleProgressService;
        }
    
        // public async Task<IResult> Register(UserRegisterRequest request)
        // {
        //     await _userService.Register(request.login, request.password);

        //     return Results.Ok();
        // }

        // public async Task<IResult> Login(UserLoginRequest request)
        // {
        //     var token = await _userService.Login(request.login, request.password);
        //     // проверить login и пароль

        //     // создать токен

        //     // сохранить в куки
        //     return Results.Ok(token);
        // }
    
        // [HttpGet]
        // public async Task<ActionResult<List<UserInfoResponse>>> GetAllUsers()
        // {
            
        // }
    }

}
