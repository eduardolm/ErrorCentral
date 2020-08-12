using ErrorCentral.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErrorCentral.Domain.Mappings
{
    public class UserMapConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Email)
                .HasColumnType("varchar(100")
                .HasColumnName("email")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Password)
                .HasColumnName("password")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.FullName)
                .HasColumnName("full_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnType("datetime2")
                .HasColumnName("created_at")
                .IsRequired();
        }
    }
}