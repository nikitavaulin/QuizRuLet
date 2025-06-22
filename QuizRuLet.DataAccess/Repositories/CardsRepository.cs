using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using QuizRuLet.Core.Models;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.DataAccess.Entities;

namespace QuizRuLet.DataAccess.Repositories
{

    public class CardsRepository : ICardsRepository
    {
        private readonly QuizRuLetDbContext _dbContext;     // контекст БД 

        // Внедрение зависимостей
        public CardsRepository(QuizRuLetDbContext context)
        {
            _dbContext = context;
        }


        #region Маппинг на доменные модели
        /// Преобразование списка сущностей базы данных в список доменных моделей
        private static List<Card> GetDomain(List<CardEntity> cardEntities)
        {
            var cards = cardEntities
                .Select(c => Card.Create(c.Id, c.FrontSide, c.BackSide, c.IsLearned).Card)
                .ToList();

            return cards;
        }

        /// Преобразование сущности базы данных в доменную моделей
        private static Card? GetDomain(CardEntity? cardEntity)
        {
            if (cardEntity is null) return null;
            
            var card = Card.Create(
                cardEntity.Id,
                cardEntity.FrontSide,
                cardEntity.BackSide,
                cardEntity.IsLearned
            ).Card;

            return card;
        }
        #endregion

        /// Получения списка карточек (всех из БД)
        public async Task<List<Card>> Get()
        {
            var cardEntities = await _dbContext.Cards
                .AsNoTracking()
                .ToListAsync();

            return GetDomain(cardEntities);
        }

        /// Получение стопки "Знаю"/"Не знаю" карточек из конкретного модуля
        public async Task<List<Card>> GetByLearningFlag(Guid moduleId, bool isLearned)
        {
            var cardEntities = await _dbContext.Cards
                .AsNoTracking()
                .Where(c => c.ModuleId == moduleId)
                .Where(c => c.IsLearned == isLearned)
                .ToListAsync();

            return GetDomain(cardEntities);
        }

        /// Получение набора карточек из модуля
        public async Task<List<Card>> GetByModule(Guid moduleId)
        {
            var cardEntities = await _dbContext.Cards
                .AsNoTracking()
                .Where(c => c.ModuleId == moduleId)
                .ToListAsync();

            return GetDomain(cardEntities);
        }

        /// Получение карточки по Id
        public async Task<Card?> GetById(Guid id)
        {
            var cardEntity = await _dbContext.Cards
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return GetDomain(cardEntity);
        }

        /// Получение количества карточек в модуле
        public async Task<int> GetCountCardsInModule(Guid moduleId)
        {
            var countCards = await _dbContext.Cards
                .AsNoTracking()
                .CountAsync(c => c.ModuleId == moduleId);
            
            return countCards;           
        }
        
        /// Получение количества изученных / не изученных карточек в модуле 
        public async Task<int> GetCountCardsByLearningFlagInModule(Guid moduleId, bool isLearned)
        {
            var countCards = await _dbContext.Cards
                .AsNoTracking()
                .CountAsync(c => c.ModuleId == moduleId && c.IsLearned == isLearned);
            
            return countCards;           
        }

        /// Полное обновление конкретной карточки
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
        
        /// Обновление сторон карточки    
        public async Task<Guid> UpdateSides(Guid id, string frontSide, string backSide)
        {
            await _dbContext.Cards
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.FrontSide, frontSide)
                    .SetProperty(c => c.BackSide, backSide)
                );

            await _dbContext.SaveChangesAsync();

            return id;
        }
        
        /// Обновление флага изученности у конкретной карточки
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
        
        
        /// Обновление флага у всех карточек в модуле
        public async Task<Guid> UpdateLearningFlagInModule(Guid moduleId, bool isLearned)
        {
            await _dbContext.Cards
                .Where(c => c.ModuleId == moduleId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.IsLearned, isLearned)
                );
                
            await _dbContext.SaveChangesAsync();

            return moduleId;   
        }

        /// Создание карточки в БД
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

        /// Удаление карточки
        public async Task<Guid> Delete(Guid id)
        {
            await _dbContext.Cards
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();
            
            return id;
        }
        
        /// Удаление всех карточек из модуля
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
