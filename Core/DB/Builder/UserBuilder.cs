using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.DB.Model;

namespace Core.DB.Builder
{
    public class UserBuilder : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.Property(p => p.UserId).HasDefaultValueSql("NEXT VALUE FOR PrimaryKeyGenerator");
            builder.Property(p => p.Email);
            builder.Property(p => p.Password);

            builder.HasData(
                new User(1, "admin@efcore.com", "1234")
            );
        }
    }
}
