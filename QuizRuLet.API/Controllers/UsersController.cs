using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QuizRuLet.API.Contracts;
using QuizRuLet.Application.Services;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

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
        
        [HttpGet]
        public async Task<ActionResult<UserInfoResponse>> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            var response = users.Select(user => new UserInfoResponse(user.Id, user.Login));

            return Ok(response);
        }
    }

}
