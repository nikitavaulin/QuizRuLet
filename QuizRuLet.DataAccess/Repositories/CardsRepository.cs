using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using QuizRuLet.Core.Models;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.DataAccess.Entities;

namespace QuizRuLet.DataAccess.Repositories
{

    public class CardsRepository : ICardsRepository
    {
        private readonly QuizRuLetDbContext _dbContext;

        public CardsRepository(QuizRuLetDbContext context)
        {
            _dbContext = context;
        }


        #region Маппинг на доменные модели
        private List<Card> GetDomain(List<CardEntity> cardEntities)
        {
            var cards = cardEntities
                .Select(c => Card.Create(c.Id, c.FrontSide, c.BackSide).Card)
                .ToList();

            return cards;
        }

        private Card? GetDomain(CardEntity cardEntity)
        {
            var card = Card.Create(
                cardEntity.Id,
                cardEntity.FrontSide,
                cardEntity.BackSide
            ).Card;

            return card;
        }
        #endregion

        /// <summary>
        /// Получения списка карточек (всех из БД)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Card>> Get()
        {
            var cardEntities = await _dbContext.Cards
                .AsNoTracking()
                .ToListAsync();

            return GetDomain(cardEntities);
        }

        /// <summary>
        /// Получение стопки "Знаю"/"Не знаю" карточек из конкретного модуля
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="isLearned"></param>
        /// <returns></returns>
        public async Task<List<Card>> GetByLearningFlag(Guid moduleId, bool isLearned)
        {
            var cardEntities = await _dbContext.Cards
                .AsNoTracking()
                .Where(c => c.ModuleId == moduleId)
                .Where(c => c.IsLearned == isLearned)
                .ToListAsync();

            return GetDomain(cardEntities);
        }

        /// <summary>
        /// Получение набора карточек из модуля
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public async Task<List<Card>> GetByModule(Guid moduleId)
        {
            var cardEntities = await _dbContext.Cards
                .AsNoTracking()
                .Where(c => c.ModuleId == moduleId)
                .ToListAsync();

            return GetDomain(cardEntities);
        }

        /// <summary>
        ///о  Получение карточки по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Card?> GetById(Guid id)
        {
            var cardEntity = await _dbContext.Cards
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return GetDomain(cardEntity);
        }

        /// <summary>
        /// Получение количества карточек в модуле
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public async Task<int> GetCountCardsInModule(Guid moduleId)
        {
            var countCards = await _dbContext.Cards
                .AsNoTracking()
                .CountAsync(c => c.ModuleId == moduleId);
            
            return countCards;           
        }
        
        /// <summary>
        /// Получение количества изученных / не изученных карточек в модуле 
        /// </summary>
        /// <param name="moduleId">id модуля</param>
        /// <param name="isLearned">флаг изучен / не изучен</param>
        /// <returns></returns>
        public async Task<int> GetCountCardsByLearningFlagInModule(Guid moduleId, bool isLearned)
        {
            var countCards = await _dbContext.Cards
                .AsNoTracking()
                .CountAsync(c => c.ModuleId == moduleId && c.IsLearned == isLearned);
            
            return countCards;           
        }

        /// <summary>
        /// Добавление карточки 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="frontSide"></param>
        /// <param name="backSide"></param>
        /// <param name="isLearned"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public async Task<Guid> Add(Guid id, string frontSide, string backSide, bool isLearned, Guid moduleId)
        {
            var cardEntity = new CardEntity
            {
                Id = id,
                FrontSide = frontSide,
                BackSide = backSide,
                IsLearned = isLearned,
                ModuleId = moduleId
            };

            await _dbContext.Cards.AddAsync(cardEntity);
            await _dbContext.SaveChangesAsync();
            
            return cardEntity.Id;
        }

        /// <summary>
        /// Обновление конкретной карточки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="frontSide"></param>
        /// <param name="backSide"></param>
        /// <param name="isLearned"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public async Task<Guid> Update(Guid id, string frontSide, string backSide, bool isLearned, Guid moduleId)
        {
            await _dbContext.Cards
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.FrontSide, frontSide)
                    .SetProperty(c => c.BackSide, backSide)
                    .SetProperty(c => c.IsLearned, isLearned)
                    .SetProperty(c => c.ModuleId, moduleId)
                );

            await _dbContext.SaveChangesAsync();

            return id;
        }
        
        public async Task<Guid> UpdateLearningFlag(Guid cardId, bool isLearned)
        {
            await _dbContext.Cards
                .Where(c => c.Id == cardId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.IsLearned, isLearned)
                );

            await _dbContext.SaveChangesAsync();

            return cardId;
        }

        public async Task<Guid> Create(Card card, Guid moduleId)
        {
            var cardEntity = new CardEntity
            {
                Id = card.Id,
                FrontSide = card.FrontSide,
                BackSide = card.BackSide,
                IsLearned = card.IsLearned,
                ModuleId = moduleId
            };

            await _dbContext.Cards.AddAsync(cardEntity);
            await _dbContext.SaveChangesAsync();
            
            return cardEntity.Id;
        }

        /// <summary>
        /// Удаление карточки
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Guid> Delete(Guid id)
        {
            await _dbContext.Cards
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();
            
            return id;
        }
        
        /// <summary>
        /// Удаление всех карточек из модуля
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public async Task<Guid> DeleteByModule(Guid moduleId)
        {
            await _dbContext.Cards
                .Where(c => c.ModuleId == moduleId)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();
            
            return moduleId;        
        }
    }
}
