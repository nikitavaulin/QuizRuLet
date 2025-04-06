using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizRuLet.DataAccess.Entities;

namespace QuizRuLet.DataAccess.Configurations;

public class CardConfiguration : IEntityTypeConfiguration<CardEntity>
{
    public void Configure(EntityTypeBuilder<CardEntity> builder)
    {
        builder.HasKey(c => c.Id);
        builder
            .HasOne(c => c.Module)
            .WithMany(m => m.Cards);
    }
}

