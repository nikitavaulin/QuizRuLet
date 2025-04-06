using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizRuLet.DataAccess.Entities;

namespace QuizRuLet.DataAccess.Configurations;

public class ModuleConfiguration : IEntityTypeConfiguration<ModuleEntity>
{
    public void Configure(EntityTypeBuilder<ModuleEntity> builder)
    {
        builder.HasKey(m => m.Id);
        builder
            .HasMany(m => m.Cards)
            .WithOne(c => c.Module)
            .HasForeignKey(c => c.ModuleId);
    }
}

