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
    
        /// Создание набора карточек на основе текста в формате термин-определение 
        /// POST: /import 200, 400
        [HttpPost]
        public ActionResult<List<CardResponse>> ImportDataSet([FromBody] CardsDataImportRequest request)
        {
            var (Cards, Error) = _cardSetCreationService.Create(
                request.Data, 
                request.PairSeparator, 
                request.LineSeparator);
            
            if (!string.IsNullOrEmpty(Error) || Cards is null)
            {
                return BadRequest(Error);
            }
            
            var response = Cards                                                                    // формирование ответа с предпросмотром
                .Select(c => new CardResponse(c.Id, c.FrontSide, c.BackSide, c.IsLearned))
                .ToList();

            return Ok(response);
        }
        
        /// Создание набора карточек на основе произвольного текста с помощью ИИ
        /// POST: /import/ai 200, 400
        [HttpPost("ai")]
        public async Task<ActionResult<List<CardResponse>>> ImportAiDataSet([FromBody] CardsDataAiImportRequest request)
        {
            var (Cards, Error) = await _cardSetAiCreationService.Create(
                request.Data, 
                request.CountCards);

            if (!string.IsNullOrEmpty(Error) || Cards is null)
            {
                return BadRequest(Error);
            }

            var cards = Cards;

            var response = cards
                .Select(c => new CardResponse(c.Id, c.FrontSide, c.BackSide, c.IsLearned))
                .ToList();

            return Ok(response);
        }
        
        /// Сохранение карточек из предпросмотра
        /// POST: /import/save/{moduleId}  200, 400
        [HttpPost("save/{moduleId:guid}")]
        public async Task<ActionResult<string>> SaveCardsFromDataSet([FromRoute] Guid moduleId, [FromBody] CardSetSaveRequest request)
        {
            if (request.Cards is null) return BadRequest();
        
            bool wasError = false;                  // флаг, была ли хоть одна ошибка
            foreach (var card in request.Cards)
            {
                var cardCreation = Card.Create(
                        Guid.NewGuid(),
                        card.FrontSide,
                        card.BackSide);
                        
                if (!string.IsNullOrEmpty(cardCreation.Error))  // есть ли ошибка
                {
                    wasError = true; 
                }
                else
                {
                    await _cardService.CreateCard(cardCreation.Card, moduleId);     // сохранение в бд
                }
            }
            string resultMsg = wasError ? "Созданы не все карточки из-за некорректности данных" : "Все карточки успешно созданы";
            return Ok(resultMsg);
        }
    }

}
