using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using QuizRuLet.DataAccess.Configurations;
using QuizRuLet.DataAccess.Entities;

namespace QuizRuLet.DataAccess;

public class QuizRuLetDbContext : DbContext
{
    public QuizRuLetDbContext(DbContextOptions<QuizRuLetDbContext> options) : base(options) {}
    
    public DbSet<UserEntity> Users {get; set;}
    
    public DbSet<ModuleEntity> Modules {get; set;}
    
    public DbSet<CardEntity> Cards {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ModuleConfiguration());
        modelBuilder.ApplyConfiguration(new CardConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}
