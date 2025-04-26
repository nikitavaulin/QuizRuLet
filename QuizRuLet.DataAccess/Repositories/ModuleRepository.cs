using System.Data.Common;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using QuizRuLet.DataAccess.Entities;


namespace QuizRuLet.DataAccess.Repositories
{
    public class ModuleRepository
    {
        private readonly QuizRuLetDbContext _dbContext;

        public ModuleRepository(QuizRuLetDbContext context)
        {
            _dbContext = context;
        }
        
        /// <summary>
        /// Добавление нового модуля
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="cards"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Add(Guid id, string name, string description, List<CardEntity> cards, Guid userId)
        {
            var module = new ModuleEntity
            {
                Id = id,
                Name = name,
                Description = description,
                Cards = cards,
                UserId = userId
            };
            
            await _dbContext.AddAsync(module);
            await _dbContext.SaveChangesAsync();
        }
        
        /// <summary>
        /// Получение всех модулей
        /// </summary>
        /// <returns></returns>
        public async Task<List<ModuleEntity>> Get()
        {
            return await _dbContext.Modules
                .AsNoTracking()
                .OrderBy(m => m.Name)
                .ToListAsync();
        }
        
        /// <summary>
        /// Получение всех модулей пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ModuleEntity>> GetByUser(Guid userId)    // DELETE? есть замена в user repos
        {
            return await _dbContext.Modules
                .AsNoTracking()
                .Where(m => m.UserId == userId)
                .ToListAsync();
        }
        
        /// <summary>
        /// Получение модулей с карточками (sql join)
        /// </summary>
        /// <returns></returns>
        public async Task<List<ModuleEntity>> GetWithCards()
        {
            return await _dbContext.Modules
                .AsNoTracking()
                .Include(m => m.Cards)
                .ToListAsync();
        }
        
        /// <summary>
        /// получение модуля по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ModuleEntity?> GetById(Guid id)
        {
            return await _dbContext.Modules
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        
        /// <summary>
        /// обновление модуля
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="cards"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Update(Guid id, string name, string description, List<CardEntity> cards, Guid userId)
        {
            await _dbContext.Modules
                .Where(m => m.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(m => m.Name, name)
                    .SetProperty(m => m.Description, description)
                    .SetProperty(m => m.Cards, cards)
                    .SetProperty(m => m.UserId, userId));
                    
            await _dbContext.SaveChangesAsync();
        }
        
        /// <summary>
        /// удаление модуля
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="cards"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Delete(Guid id, string name, string description, List<CardEntity> cards, Guid userId)
        {
            await _dbContext.Modules
                .Where(m => m.Id == id)
                .ExecuteDeleteAsync();
                    
            await _dbContext.SaveChangesAsync();
        }
    }
}
