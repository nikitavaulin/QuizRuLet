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
        private readonly ICardService _cardService;

        public LearningModeController(ILearningModuleService learningModuleService, ICardService cardService)
        {
            _learningModuleService = learningModuleService;
            _cardService = cardService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CardResponse>>> GetNotLearnedCards([FromRoute] Guid moduleId)
        {
            var cards = (await _cardService.GetCardsByLearningFlag(moduleId, false))
                .Select(c => new CardResponse(c.Id, c.FrontSide, c.BackSide, c.IsLearned))
                .ToList();

            if (cards is null)
            {
                return NotFound("Модуль не найден");
            }
            
            return cards.Count > 0 ? Ok(cards) : NoContent();     // mb fix
        }
        
        
    }
}
