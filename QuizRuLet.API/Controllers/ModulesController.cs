using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuizRuLet.API.Contracts;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.DataAccess;
using QuizRuLet.DataAccess.Entities;

namespace QuizRuLet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        
        public ModulesController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<ModulesResponse>>> GetModules()
        {
            var modules = await _moduleService.GetAllModules();
            
            var response = modules.Select(m => new ModulesResponse(m.Id, m.Name, m.Description));
            
            return Ok(response);
        }
        
        [HttpPost]  // TODO validation
        public async Task<ActionResult<Guid>> CreateModule([FromBody] ModuleCreationRequest request)
        {
            var (module, error) = Core.Models.Module.Create(
                Guid.NewGuid(),
                request.Name,
                request.Description
            );
            
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }
            
            var moduleId = await _moduleService.CreateModule(module, request.UserId);
            
            return Ok(moduleId);           
        }      
        
        // PUT: Modules/id
        [HttpPut("{id:guid}")]  // TODO validation
        public async Task<ActionResult<Guid>> UpdateModule(Guid id, [FromBody] ModulesRequest request)
        {
            var moduleId = await _moduleService.UpdateModule(id, request.Name, request.Description);
            
            return Ok(moduleId);
        }
        
        // DELETE: Modules/id
        [HttpDelete("{id:guid}")]   // TODO validation
        public async Task<ActionResult<Guid>> DeleteModule(Guid id)
        {
            var moduleId = await _moduleService.DeleteModule(id);
            
            return Ok(moduleId);
        }
        
    }


}
