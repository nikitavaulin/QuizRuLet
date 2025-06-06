using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizRuLet.DataAccess.Entities;

namespace QuizRuLet.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Login)
            // MAX LENGTH FIXME
            // MIN LENGTH FIXME
            .IsRequired();
        
        builder.Property(u => u.PasswordHash)
            // MAX LENGTH FIXME
            // MIN LENGTH FIXME
            .IsRequired();
        
        // настройка связей
        builder
            .HasMany(u => u.Modules)
            .WithOne(m => m.User);
    }
}
