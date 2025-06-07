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
        private readonly ICardService _cardService;

        public ModuleEditController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateCard([FromRoute] Guid moduleId, [FromBody] CardCreateRequest request)
        {
            var cardResult = Card.Create(
                Guid.NewGuid(),
                request.FrontSide,
                request.BackSide);
                
            if (!string.IsNullOrEmpty(cardResult.Error))
            {
                return BadRequest(cardResult.Error);
            }

            var cardId = await _cardService.CreateCard(cardResult.Card, moduleId);

            return Ok(cardId);
        }
        
    }
}
