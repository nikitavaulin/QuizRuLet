using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizRuLet.API.Contracts;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        // private readonly IUserService _userService;
        private readonly IModuleProgressService _progressService;
        
        
        public UsersController(
            IModuleService moduleService,
            IModuleProgressService moduleProgressService)
        {
            _moduleService = moduleService;
            _progressService = moduleProgressService;
        }
    
        // [HttpGet]
        // public async Task<ActionResult<List<UserInfoResponse>>> GetAllUsers()
        // {
            
        // }
    }

}
