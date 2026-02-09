using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.DB.Model;

namespace Core.DB.Builder
{
    public class OrderItemBuilder : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItem");

            builder.HasKey(oi => oi.OrderItemId);

            builder.Property(oi => oi.OrderItemId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(oi => oi.OrderId)
                .IsRequired();

            builder.Property(oi => oi.ProductName)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            builder.Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(oi => oi.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(oi => oi.Deleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(oi => oi.OrderId);

            // Seed Data
            builder.HasData(
                // Items for Order 1
                new OrderItem(1, 1, "Laptop", 1, 100.00m) { TotalPrice = 100.00m },
                new OrderItem(2, 1, "Mouse", 2, 25.00m) { TotalPrice = 50.00m },
                // Items for Order 2
                new OrderItem(3, 2, "Keyboard", 1, 69.99m) { TotalPrice = 69.99m },
                new OrderItem(4, 2, "USB Cable", 2, 10.00m) { TotalPrice = 20.00m }
            );
        }
    }
}
