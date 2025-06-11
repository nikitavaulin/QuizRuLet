using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizRuLet.API.Contracts;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

namespace QuizRuLet.API.Controllers
{
    [Route("import")]
    [ApiController]
    public class ImportDataController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly ICardSetCreationService _cardSetCreationService;

        public ImportDataController(ICardService cardService, ICardSetCreationService cardSetCreationService)
        {
            _cardService = cardService;
            _cardSetCreationService = cardSetCreationService;
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
        
        [HttpPost("save/{moduleId:guid}")]
        public async Task<ActionResult> SaveCardsFromDataSet([FromRoute] Guid moduleId, [FromBody] CardSetSaveRequest request)
        {
            foreach (var card in request.Cards)
            {
                await _cardService.CreateCard(
                    Card.Create(
                        Guid.NewGuid(),
                        card.FrontSide,
                        card.BackSide).Card,
                    moduleId   
                );
            }

            return Ok();
        }
    }

}
