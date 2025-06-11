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
    public class ModuleEditController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModuleEditController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }
        
        [HttpPatch("name")]  // TODO validation
        public async Task<ActionResult<Guid>> UpdateModuleName([FromRoute] Guid moduleId, [FromBody] ModuleNameEditRequest request)
        {
            var id = await _moduleService.UpdateModuleName(moduleId, request.Name);
            
            return Ok(id);
        }
        
        [HttpPatch("description")]  // TODO validation
        public async Task<ActionResult<Guid>> UpdateModuleDescription([FromRoute] Guid moduleId, [FromBody] ModuleDescriptionEditRequest request)
        {
            var id = await _moduleService.UpdateModuleDescription(moduleId, request.Description);
            
            return Ok(id);
        }
        
        

    }


}
