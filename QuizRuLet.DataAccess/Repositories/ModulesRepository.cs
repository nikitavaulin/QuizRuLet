using System.Data.Common;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using QuizRuLet.DataAccess.Entities;
using QuizRuLet.Core.Abstractions;


namespace QuizRuLet.DataAccess.Repositories
{

    public class ModulesRepository : IModulesRepository
    {
        private readonly QuizRuLetDbContext _dbContext;

        public ModulesRepository(QuizRuLetDbContext context)
        {
            _dbContext = context;
        }

        #region Маппинг на доменные модели
        private List<Core.Models.Module> GetDomain(List<ModuleEntity> moduleEntities)
        {
            var modules = moduleEntities
                .Select(m => Core.Models.Module.Create(m.Id, m.Name, m.Description).Module)
                .ToList();

            return modules;
        }

        private Core.Models.Module? GetDomain(ModuleEntity moduleEntity)
        {
            var module = Core.Models.Module.Create(
                moduleEntity.Id,
                moduleEntity.Name,
                moduleEntity.Description
            ).Module;

            return module;
        }
        #endregion

        public async Task<List<Core.Models.Module>> Get()
        {
            var moduleEntities = await _dbContext.Modules
                .AsNoTracking()
                .OrderBy(m => m.Name)
                .ToListAsync();

            return GetDomain(moduleEntities);
        }

        /// <summary>
        /// Получение всех модулей пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Core.Models.Module>> GetByUser(Guid userId)    // мб удалить? есть замена в user repos
        {
            var moduleEntities = await _dbContext.Modules
                .AsNoTracking()
                .Where(m => m.UserId == userId)
                .ToListAsync();

            return GetDomain(moduleEntities);
        }

        /// <summary>
        /// получение модуля по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Core.Models.Module?> GetById(Guid id)
        {
            var moduleEntity = await _dbContext.Modules
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            return GetDomain(moduleEntity);
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
        public async Task<Guid> Add(Guid id, string name, string description, Guid userId)
        {
            var moduleEntity = new ModuleEntity
            {
                Id = id,
                Name = name,
                Description = description,
                // Cards = cards,
                UserId = userId
            };

            await _dbContext.Modules.AddAsync(moduleEntity);
            await _dbContext.SaveChangesAsync();
            
            return moduleEntity.Id;
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
        public async Task<Guid> Update(Guid id, string name, string description)
        {
            await _dbContext.Modules
                .Where(m => m.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(m => m.Name, name)
                    .SetProperty(m => m.Description, description)
                );

            await _dbContext.SaveChangesAsync();
            
            return id;
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
        public async Task<Guid> Delete(Guid id)
        {
            await _dbContext.Modules
                .Where(m => m.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();
            
            return id;
        }

        public async Task<Guid> Create(Core.Models.Module module, Guid userId)
        {
            var moduleEntity = new ModuleEntity
            {
                Id = module.Id,
                Name = module.Name,
                Description = module.Description,
                UserId = userId
            };

            await _dbContext.Modules.AddAsync(moduleEntity);
            await _dbContext.SaveChangesAsync();
            
            return moduleEntity.Id;
        }
    }
}
