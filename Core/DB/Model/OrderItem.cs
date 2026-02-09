using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DB.Model
{
    public class OrderItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderItemId { get; set; }

        [Required]
        public long OrderId { get; set; }

        [Required]
        [MaxLength(500)]
        public string ProductName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public bool Deleted { get; set; } = false;

        public Order Order { get; set; }

        public OrderItem(long orderId, string productName, int quantity, decimal unitPrice)
        {
            OrderId = orderId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = quantity * unitPrice;
        }

        // Used for seed data
        public OrderItem(long orderItemId, long orderId, string productName, int quantity, decimal unitPrice)
        {
            OrderItemId = orderItemId;
            OrderId = orderId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = quantity * unitPrice;
        }
    }
}
