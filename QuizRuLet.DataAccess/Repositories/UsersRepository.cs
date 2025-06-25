using Microsoft.EntityFrameworkCore;
using QuizRuLet.Core.Models;
using QuizRuLet.DataAccess.Entities;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly QuizRuLetDbContext _dbContext;     // контекст БД

        /// Внедрение зависимостей
        public UsersRepository(QuizRuLetDbContext context)
        {
            _dbContext = context;
        }

        #region Маппинг на доменные модели

        /// Преобразование списка сущностей БД в список доменных моделей пользователей
        private List<User> GetDomain(List<UserEntity> userEntities)
        {
            var user = userEntities
                .Select(u => User.Create(u.Id, u.Login, u.PasswordHash).User)
                .ToList();

            return user;
        }

        /// Преобразование одной сущности БД в доменную модель пользователя
        private static User? GetDomain(UserEntity? userEntity)
        {
            if (userEntity is null) return null;
            
            var user = User.Create(
                userEntity.Id,
                userEntity.Login,
                userEntity.PasswordHash
            ).User;

            return user;
        }

        #endregion

        /// Получение всех пользователей из БД
        public async Task<List<User>> Get()
        {
            var userEntities = await _dbContext.Users
                .AsNoTracking()         // отключение отслеживания изменений
                .OrderBy(u => u.Login)
                .ToListAsync();

            return GetDomain(userEntities);
        }

        /// Получение пользователей вместе с их модулями (используется SQL JOIN)
        public async Task<List<User>> GetWithModules()
        {
            var userEntities = await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Modules)
                .ToListAsync();

            return GetDomain(userEntities);
        }

        /// Получение пользователя по ID
        public async Task<User?> GetById(Guid id)
        {
            var userEntity = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            return GetDomain(userEntity);
        }

        /// Получение пользователя по логину
        public async Task<User?> GetByLogin(string login)
        {
            var userEntity = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login);

            return GetDomain(userEntity);
        }

        /// Обновление данных пользователя (логин и хэш пароля)
        public async Task<Guid> Update(Guid id, string login, string passwordHash)
        {
            await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.Login, login)
                    .SetProperty(u => u.PasswordHash, passwordHash)
                );

            await _dbContext.SaveChangesAsync();
            
            return id;
        }

        /// Удаление пользователя по его уникальному идентификатору
        public async Task<Guid> Delete(Guid id)
        {
            await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();
            
            return id;
        }

        /// Создание нового пользователя в базе данных
        public async Task<Guid> Create(User user)
        {
            var userEntity = new UserEntity
            {
                Id = user.Id,
                Login = user.Login,
                PasswordHash = user.PasswordHash
            };

            await _dbContext.Users.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync();
            
            return userEntity.Id;
        }

        /// Проверка существования пользователя с указанным логином
        public async Task<bool> IsUserLoginExist(string login)
        {
            var isExist = await _dbContext.Users
                .AnyAsync(u => u.Login == login);

            return isExist;
        }
    }
}