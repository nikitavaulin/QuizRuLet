using Microsoft.AspNetCore.Authorization;
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
    [Route("users")]
    [Authorize(Roles = "User")]
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

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<UserWithModulesResponse>> GetUserProfileWithModulesById([FromRoute] Guid userId)
        {
            var user = await _userService.GetUserById(userId);
            
            if (user is null)
            {
                return BadRequest("Пользователя не существует");
            }

            var modules = await _moduleService.GetUserModules(userId);

            var moduleList = new List<ModulesResponse>();
            foreach (var m in modules)
            {
                var statistic = await _progressService.GetModuleStatisticInfo(m.Id);
                moduleList.Add(new ModulesResponse(m.Id, m.Name, m.Description, statistic.Progress, statistic.CountCards));
            }

            var response = new UserWithModulesResponse(userId, user.Login, moduleList);

            return Ok(response);
        }
    }
    

}
