using Microsoft.EntityFrameworkCore;
using QuizRuLet.Core.Models;
using QuizRuLet.DataAccess.Entities;


namespace QuizRuLet.DataAccess.Repositories
{

    public class UsersRepository : IUsersRepository
    {

        private readonly QuizRuLetDbContext _dbContext;

        public UsersRepository(QuizRuLetDbContext context)
        {
            _dbContext = context;
        }

        #region Маппинг на доменные модели
        private List<User> GetDomain(List<UserEntity> userEntities)
        {
            var user = userEntities
                .Select(u => User.Create(u.Id, u.Login, u.Password).User)
                .ToList();

            return user;
        }

        private User GetDomain(UserEntity userEntities)
        {
            var user = User.Create(
                userEntities.Id,
                userEntities.Login,
                userEntities.Password
            ).User;

            return user;
        }
        #endregion

        public async Task<List<User>> Get()
        {
            var userEntities = await _dbContext.Users
                .AsNoTracking()         // отключение изменения сущности
                .OrderBy(u => u.Login)
                .ToListAsync();

            return GetDomain(userEntities);
        }

        /// <summary>
        /// Получение юзеров с модулями (sql join)
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetWithModules()
        {
            var userEntities = await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Modules)
                .ToListAsync();

            return GetDomain(userEntities);
        }

        public async Task<User?> GetById(Guid id)
        {
            var userEntity = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            return GetDomain(userEntity);
        }

        public async Task<Guid> Add(Guid id, string login, string password)
        {
            var userEntity = new UserEntity
            {
                Id = id,
                Login = login,
                Password = password
            };

            await _dbContext.Users.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync();
            
            return userEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string login, string password)
        {
            await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.Login, login)
                    .SetProperty(u => u.Password, password)
                );

            await _dbContext.SaveChangesAsync();
            
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();
            
            return id;
        }

        public async Task<Guid> Create(User user)
        {
            var userEntity = new UserEntity
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password
            };

            await _dbContext.Users.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync();
            
            return userEntity.Id;
        }

        // фильтры

        // пагинация
    }
}
