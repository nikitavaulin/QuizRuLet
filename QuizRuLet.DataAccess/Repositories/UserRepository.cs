using Microsoft.EntityFrameworkCore;
using QuizRuLet.Core.Models;
using QuizRuLet.DataAccess.Entities;


namespace QuizRuLet.DataAccess.Repositories
{
    public class UserRepository
    {

        private readonly QuizRuLetDbContext _dbContext;

        public UserRepository(QuizRuLetDbContext context)
        {
            _dbContext = context;
        }
    
        public async Task<List<User>> GetUsers()
        {
            var userEntities = await _dbContext.Users
                .AsNoTracking()         // отключение изменения сущности
                .OrderBy(u => u.Login)
                .ToListAsync();
            
            var users = userEntities
                .Select(u => User.Create(u.Id, u.Login, u.Password).User)
                .ToList();
            
            return users;
        }


        public async Task<List<UserEntity>> GetUserEntities()          // FIX
        {
            return await _dbContext.Users
                .AsNoTracking()         // отключение изменения сущности
                .OrderBy(u => u.Login)
                .ToListAsync();
        }
        
        /// <summary>
        /// Получение юзеров с модулями (sql join)
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserEntity>> GetWithModules()          // FIX
        {
            return await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Modules)
                .ToListAsync();
        }
        
        public async Task<UserEntity?> GetById(Guid id)          // FIX
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    
        public async Task Add(Guid id, string login, string password)
        {
            var userEntity = new UserEntity
            {
                Id = id,
                Login = login,
                Password = password
            };
            
            await _dbContext.Users.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync();
        }
    
        public async Task Update(Guid id, string login, string password, List<ModuleEntity> modules)
        {
            await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.Login, login)
                    .SetProperty(u => u.Password, password)
                    .SetProperty(u => u.Modules, modules)
                );
            
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task Delete(Guid id)
        {
            await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();
            
            await _dbContext.SaveChangesAsync();
        }
    
        // фильтры
        
        // пагинация
    
    
        // public async Task<List<Module>> GetModulesAsync()
        // {
        //     var moduleEntities = await _dbContext.Modules
        //         .AsNoTracking()
        //         .ToListAsync();
                
        //     var modules = moduleEntities;    // FIX
        // }

    }
}
