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
using QuizRuLet.Core.Models;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.DataAccess;
using QuizRuLet.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace QuizRuLet.API.Controllers
{
    [ApiController]
    [Route("modules")]
    [Authorize(Roles = "User")]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        private readonly IModuleProgressService _progressService;
        private readonly ILearningModuleService _learningModuleService;

        public ModulesController(
            IModuleService moduleService,
            IModuleProgressService moduleProgressService,
            ILearningModuleService learningModuleService)
        {
            _moduleService = moduleService;
            _progressService = moduleProgressService;
            _learningModuleService = learningModuleService;
        }
        
        
        [HttpGet]
        public async Task<ActionResult<List<ModulesResponse>>> GetModules()
        {
            var modules = await _moduleService.GetAllModules();
            
            var response = modules.Select(async m => new ModulesResponse(
                m.Id, 
                m.Name, 
                m.Description,
                (await _progressService.GetModuleStatisticInfo(m.Id)).Progress)
            );
            
            return Ok(response);
        }

        [HttpGet("{moduleId:guid}")]
        public async Task<ActionResult<List<ModuleWithCardsResponse>>> GetModuleWithCard(Guid moduleId)
        {
            var module = await _moduleService.GetModuleById(moduleId);
            
            if (module is null)
            {
                return NotFound("Учебный модуль не найден");
            }

            var cards = (await _learningModuleService.GetAllCards(moduleId))
                .Select(c => new CardResponse(c.Id, c.FrontSide, c.BackSide, c.IsLearned))
                .ToList();

            var statistic = await _progressService.GetModuleStatisticInfo(moduleId);

            var response = new ModuleWithCardsResponse(
                moduleId, 
                module.Name, 
                module.Description, 
                statistic.Progress,
                statistic.CountCards, 
                statistic.CountLearned,
                cards);
            
            return Ok(response);
        }
        
        [HttpGet("statistic/{moduleId:guid}")]
        public async Task<ActionResult<ModuleStatisticResponse>> GetModuleStatistic([FromRoute] Guid moduleId)
        {
            var statistic = await _progressService.GetModuleStatisticInfo(moduleId);
            var response = new ModuleStatisticResponse(
                statistic.Progress,
                statistic.CountCards,
                statistic.CountLearned,
                statistic.CountCards - statistic.CountLearned
            );
            
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
