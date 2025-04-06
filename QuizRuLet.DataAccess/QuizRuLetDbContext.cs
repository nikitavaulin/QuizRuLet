using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using QuizRuLet.DataAccess.Entities;

namespace QuizRuLet.DataAccess;

public class QuizRuLetDbContext : DbContext
{
    public QuizRuLetDbContext(DbContextOptions<QuizRuLetDbContext> options) : base(options) {}
    
    public DbSet<UserEntity> Users {get; set;}
}
