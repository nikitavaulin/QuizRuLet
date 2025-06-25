using System.Data.Common;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using QuizRuLet.DataAccess.Entities;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.DataAccess.Repositories
{
    public class ModulesRepository : IModulesRepository
    {
        private readonly QuizRuLetDbContext _dbContext;     // контекст БД

        // Вндрение зависиимостей
        public ModulesRepository(QuizRuLetDbContext context)
        {
            _dbContext = context;
        }

        #region Маппинг на доменные модели

        /// Преобразование списка сущностей базы данных в список доменных моделей
        private List<Core.Models.Module> GetDomain(List<ModuleEntity> moduleEntities)
        {
            var modules = moduleEntities
                .Select(m => Core.Models.Module.Create(m.Id, m.Name, m.Description).Module)
                .ToList();

            return modules;
        }

        /// Преобразование одной сущности базы данных в доменную модель
        private static Core.Models.Module? GetDomain(ModuleEntity? moduleEntity)
        {
            if (moduleEntity is null) return null;
            
            var module = Core.Models.Module.Create(
                moduleEntity.Id,
                moduleEntity.Name,
                moduleEntity.Description
            ).Module; // преобразование сущность в доменную модель

            return module;
        }

        #endregion

        /// Получение всех модулей из базы данных
        public async Task<List<Core.Models.Module>> Get()
        {
            var moduleEntities = await _dbContext.Modules
                .AsNoTracking()
                .ToListAsync();

            return GetDomain(moduleEntities); // преобразование сущности в доменные модели
        }

        /// Получение всех модулей  пользователя
        public async Task<List<Core.Models.Module>> GetByUser(Guid userId)
        {
            var moduleEntities = await _dbContext.Modules
                .AsNoTracking() 
                .Where(m => m.UserId == userId)
                .ToListAsync(); 

            return GetDomain(moduleEntities); // преобразование сущности в доменные модели
        }

        /// Получение модуля по ID
        public async Task<Core.Models.Module?> GetById(Guid id)
        {
            var moduleEntity = await _dbContext.Modules
                .AsNoTracking() 
                .FirstOrDefaultAsync(m => m.Id == id);

            return GetDomain(moduleEntity); // преобразование сущность в доменную модель
        }

        /// Обновление полей модуля
        public async Task<Guid> Update(Guid id, string name, string description)
        {
            await _dbContext.Modules
                .Where(m => m.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(m => m.Name, name) 
                    .SetProperty(m => m.Description, description) 
                );

            await _dbContext.SaveChangesAsync(); // сохранение изменений в базе данных

            return id;
        }

        /// Обновление названия модуля
        public async Task<Guid> UpdateName(Guid id, string name)
        {
            await _dbContext.Modules
                .Where(m => m.Id == id) 
                .ExecuteUpdateAsync(s => s
                    .SetProperty(m => m.Name, name) 
                );

            await _dbContext.SaveChangesAsync(); // сохранение изменений в базе данных

            return id;
        }

        /// Обновление описания модуля
        public async Task<Guid> UpdateDescription(Guid id, string description)
        {
            await _dbContext.Modules
                .Where(m => m.Id == id) 
                .ExecuteUpdateAsync(s => s
                    .SetProperty(m => m.Description, description)
                );

            await _dbContext.SaveChangesAsync(); // сохранение изменений в базе данных

            return id;
        }

        /// Удаление модуля по его уникальному идентификатору
        public async Task<Guid> Delete(Guid id)
        {
            await _dbContext.Modules
                .Where(m => m.Id == id) 
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync(); // сохранение изменений в базе данных

            return id;
        }

        /// Создание модуля на основе доменной модели
        public async Task<Guid> Create(Core.Models.Module module, Guid userId)
        {
            var moduleEntity = new ModuleEntity
            {
                Id = module.Id,
                Name = module.Name,
                Description = module.Description,
                UserId = userId
            };

            await _dbContext.Modules.AddAsync(moduleEntity); // добавляем новую сущность в контекст
            await _dbContext.SaveChangesAsync(); // сохранение изменений в базе данных

            return moduleEntity.Id;
        }
    }
}