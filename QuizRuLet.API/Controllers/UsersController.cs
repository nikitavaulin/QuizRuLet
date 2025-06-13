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
        public async Task<ActionResult<UserWithModulesResponse>> GetUserProfileWithModulesById(Guid userId)
        {
            var user = await _userService.GetUserById(userId);
            
            if (user is null)
            {
                return BadRequest("Пользователя не существует");
            }

            var modules = (await _moduleService.GetUserModules(userId))
                .Select(async m => new ModulesResponse(
                    m.Id,
                    m.Name,
                    m.Description,
                    await _progressService.GetModuleProgressPercent(m.Id)));
            var moduleList = Task.WhenAll(modules).Result.ToList();

            var response = new UserWithModulesResponse(userId, user.Login, moduleList);

            return Ok(response);
        }
        
        // [HttpGet("{userName:string}")]
        // public async Task<ActionResult<UserWithModulesResponse>> GetUserProfileWithModules([FromRoute] string userName)
        // {
        //     var user = await _userService.GetUserByName(userName);
            
        //     if (user is null)
        //     {
        //         return BadRequest("Пользователя не существует");
        //     }

        //     var modules = (await _moduleService.GetUserModules(user.Id))
        //         .Select(async m => new ModulesResponse(
        //             m.Id,
        //             m.Name,
        //             m.Description,
        //             await _progressService.GetModuleProgressPercent(m.Id)));
        //     var moduleList = Task.WhenAll(modules).Result.ToList();

        //     var response = new UserWithModulesResponse(user.Id, user.Login, moduleList);

        //     return Ok(response);
        // }
    }
    

}
