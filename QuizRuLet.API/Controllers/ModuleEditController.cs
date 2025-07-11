using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizRuLet.API.Contracts;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

namespace QuizRuLet.API.Controllers
{
    [Route("modules/edit/{moduleId:guid}")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class ModuleEditController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModuleEditController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }
        
        
        /// Изменение имени модуля
        /// PATCH: /modules/edit/{moduleId}/name  200
        [HttpPatch("name")]
        public async Task<ActionResult<Guid>> UpdateModuleName([FromRoute] Guid moduleId, [FromBody] ModuleNameEditRequest request)
        {
            var id = await _moduleService.UpdateModuleName(moduleId, request.Name);
            return Ok(id);
        }
        
        /// Изменение описания модуля
        /// PATCH: /modules/edit/{moduleId}/description  200
        [HttpPatch("description")]
        public async Task<ActionResult<Guid>> UpdateModuleDescription([FromRoute] Guid moduleId, [FromBody] ModuleDescriptionEditRequest request)
        {
            var id = await _moduleService.UpdateModuleDescription(moduleId, request.Description);
            return Ok(id);
        }
        
        /// Изменение имени и описания
        /// PATCH: /modules/edit/{moduleId}  200
        [HttpPatch]
        public async Task<ActionResult<Guid>> UpdateModuleNameAndDescription([FromRoute] Guid moduleId, [FromBody] ModuleEditRequest request)
        {
            var id = await _moduleService.UpdateModule(moduleId, request.Name, request.Description);
            return Ok(id);
        }
        

    }


}
