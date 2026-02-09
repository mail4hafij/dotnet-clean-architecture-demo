using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.DB.Model;

namespace Core.DB.Builder
{
    public class CarBuilder : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.ToTable("Cars");
            builder.Property(p => p.CarId).HasDefaultValueSql("NEXT VALUE FOR PrimaryKeyGenerator");
            builder.Property(p => p.UserId);
            builder.Property(p => p.Nameplate);

            builder.HasData(
                new Car(1, 1, "ABC123")
            );
        }
    }
}
