using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizRuLet.API.Contracts;
using QuizRuLet.Application.Services;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

namespace QuizRuLet.API.Controllers
{
    [Route("import")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class ImportDataController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly ICardSetCreationService _cardSetCreationService;
        private readonly ICardSetAiCreationService _cardSetAiCreationService;

        public ImportDataController(
            ICardService cardService, 
            ICardSetCreationService cardSetCreationService,
            ICardSetAiCreationService cardSetAiCreationService)
        {
            _cardService = cardService;
            _cardSetCreationService = cardSetCreationService;
            _cardSetAiCreationService = cardSetAiCreationService;
        }
    
        [HttpPost]
        public async Task<ActionResult<List<CardResponse>>> ImportDataSet([FromBody] CardsDataImportRequest request)
        {
            var cards = _cardSetCreationService.Create(
                request.Data, 
                request.PairSeparator, 
                request.LineSeparator);

            var response = cards
                .Select(c => new CardResponse(c.Id, c.FrontSide, c.BackSide, c.IsLearned))
                .ToList();

            return Ok(response);
        }
        
        [HttpPost("ai")]
        public async Task<ActionResult<List<CardResponse>>> ImportAiDataSet([FromBody] CardsDataAiImportRequest request)
        {
            var result = await _cardSetAiCreationService.Create(
                request.Data, 
                request.CountCards);

            if (!string.IsNullOrEmpty(result.Error) || result.Cards is null)
            {
                return BadRequest(result.Error);
            }

            var cards = result.Cards;

            var response = cards
                .Select(c => new CardResponse(c.Id, c.FrontSide, c.BackSide, c.IsLearned))
                .ToList();

            return Ok(response);
        }
        
        [HttpPost("save/{moduleId:guid}")]
        public async Task<ActionResult<string>> SaveCardsFromDataSet([FromRoute] Guid moduleId, [FromBody] CardSetSaveRequest request)
        {
            bool wasError = false;
            foreach (var card in request.Cards)
            {
                var cardCreation = Card.Create(
                        Guid.NewGuid(),
                        card.FrontSide,
                        card.BackSide);
                        
                if (!string.IsNullOrEmpty(cardCreation.Error))
                {
                    wasError = true;
                }
                else
                {
                    await _cardService.CreateCard(cardCreation.Card, moduleId);
                }
            }
            string resultMsg = wasError ? "Созданы не все карточки из-за некорректности данных" : "Все карточки успешно созданы";
            return Ok(resultMsg);
        }
    }

}
