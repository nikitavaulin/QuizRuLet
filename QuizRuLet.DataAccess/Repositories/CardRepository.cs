using Microsoft.EntityFrameworkCore;
using QuizRuLet.Core.Models;
using QuizRuLet.DataAccess.Entities;

namespace QuizRuLet.DataAccess.Repositories
{
    public class CardRepository
    {
        private readonly QuizRuLetDbContext _dbContext;

        public CardRepository(QuizRuLetDbContext context)
        {
            _dbContext = context;
        }
        
        /// <summary>
        /// Получения списка карточек (всех из БД)
        /// </summary>
        /// <returns></returns>
        public async Task<List<CardEntity>> Get()
        {
            return await _dbContext.Cards
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<List<Card>> GetCards()
        {
            var cardEntities = await _dbContext.Cards
                .AsNoTracking()
                .ToListAsync();
            
            var cards = cardEntities
                .Select(c => Card.Create(c.Id, c.FrontSide, c.BackSide).Card)
                .ToList();
            
            return cards;
        }
        
        /// <summary>
        /// Получение стопки "Знаю"/"Не знаю" карточек из конкретного модуля
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="isLearned"></param>
        /// <returns></returns>
        public async Task<List<CardEntity>> GetByLearningFlag(Guid moduleId, bool isLearned)          // FIX
        {
            return await _dbContext.Cards
                .AsNoTracking()
                .Where(c => c.ModuleId == moduleId)
                .Where(c => c.IsLearned == isLearned)
                .ToListAsync();
        }
        
        /// <summary>
        /// Получение набора карточек из модуля
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public async Task<List<CardEntity>> GetByModule(Guid moduleId)          // FIX
        {
            return await _dbContext.Cards
                .AsNoTracking()
                .Where(c => c.ModuleId == moduleId)
                .ToListAsync();
        }
        
        /// <summary>
        /// Получение карточки по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CardEntity?> GetById(Guid id)          // FIX
        {
            return await _dbContext.Cards
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
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
        public async Task Add(Guid id, string frontSide, string backSide, bool isLearned, Guid moduleId)
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
        public async Task Update(Guid id, string frontSide, string backSide, bool isLearned, Guid moduleId)
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

        }
        
        /// <summary>
        /// Удаление карточки
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(Guid id)
        {
            await _dbContext.Cards
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();
                
            await _dbContext.SaveChangesAsync();
        }
        
        
    }
}
