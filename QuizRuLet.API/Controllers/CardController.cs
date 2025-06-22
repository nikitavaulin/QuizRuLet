using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizRuLet.API.Contracts;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

namespace QuizRuLet.API.Controllers
{
    [Route("cards")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class CardController : ControllerBase
    {
        private readonly ILearningModuleService _learningModuleService;
        private readonly ICardService _cardService;

        public CardController(ILearningModuleService learningModuleService, ICardService cardService)
        {
            _learningModuleService = learningModuleService;
            _cardService = cardService;
        }


        /// Создание карточки в модуле
        /// POST: /cards/{moduleId} 200, 400
        [HttpPost("{moduleId:guid}")]
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
        
        /// Изменение статуса "знаю"\"не знаю"
        /// PATCH: /cards/update-flag/{cardId} 200, 400
        [HttpPatch("update-flag/{cardId:guid}")]
        public async Task<ActionResult<Guid>> UpdateCardLearningFlag([FromRoute] Guid cardId, [FromBody] CardLearningFlagUpdateRequest request)
        {
            await _learningModuleService.UpdateLearningFlag(cardId, request.IsLearned);
            return Ok(cardId);
        }
        
        /// Изменение сторон карточек
        /// PATCH: /cards/update/{cardId} 200, 400
        [HttpPatch("update/{cardId:guid}")]
        public async Task<ActionResult<Guid>> UpdateCardSides([FromRoute] Guid cardId, [FromBody] CardRequest request)
        {
            var error = Card.Create(Guid.NewGuid(), request.FrontSide, request.BackSide).Error; // валидация новых сторон
            
            if (string.IsNullOrEmpty(error))
            {
                await _cardService.UpdatePartlyCard(cardId, request.FrontSide, request.BackSide);
                return Ok(cardId);
            }
            return BadRequest(error);
             
        }
        
        /// Перевод всех карточек в модуле в состояние "не знаю"
        /// PATCH: /cards/reset/{moduleId} 200
        [HttpPatch("reset/{moduleId:guid}")]
        public async Task<ActionResult> ResetCardsInModule([FromRoute] Guid moduleId)
        {
            await _learningModuleService.UpdateLearningFlagInModule(moduleId, false);
            return Ok();
        }
        
        
        /// Удаление карточки
        /// DELETe: /cards/{cardId} 200
        [HttpDelete("{cardId:guid}")]
        public async Task<ActionResult> DeleteCard([FromRoute] Guid cardId)
        {
            await _cardService.DeleteCard(cardId);
            return Ok(cardId);
        }
    }
}
