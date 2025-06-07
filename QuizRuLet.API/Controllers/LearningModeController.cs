using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using QuizRuLet.API.Contracts;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

namespace QuizRuLet.API.Controllers
{
    [Route("learning-mode/{moduleId:guid}")]
    [ApiController]
    public class LearningModeController : ControllerBase
    {
        private readonly ILearningModuleService _learningModuleService;

        public LearningModeController(ILearningModuleService learningModuleService)
        {
            _learningModuleService = learningModuleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CardResponse>>> GetModuleCards([FromRoute] Guid moduleId)
        {
            var cards = (await _learningModuleService.GetAllCards(moduleId))
                .Select(c => new CardResponse(c.FrontSide, c.BackSide, c.IsLearned))
                .ToList();

            if (cards is null)
            {
                return NotFound("Модуль не найден");
            }
            
            return cards.Count > 0 ? Ok(cards) : NoContent();     // mb fix
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateCardLearningFlag([FromBody] CardLearningFlagUpdateRequest request)
        {
            await _learningModuleService.UpdateLearningFlag(request.CardId, request.IsLearned);
            return Ok();
        }
        
        
    }
}
