using Microsoft.EntityFrameworkCore;
using QuizRuLet.Core.Models;


namespace QuizRuLet.DataAccess.Repositories
{
    public class UserRepository
    {

        private readonly QuizRuLetDbContext _context;

        public UserRepository(QuizRuLetDbContext context)
        {
            _context = context;
        }
    
        public async Task<List<Module>> GetModulesAsync()
        {
            var moduleEntities = await _context.Modules
                .AsNoTracking()
                .ToListAsync();
                
            var modules = moduleEntities;    // FIX
        }
    
    
    }
}
