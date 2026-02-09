using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DB.Model
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderId { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } // Pending, Confirmed, Shipped, Delivered, Cancelled

        public decimal TotalAmount { get; set; }

        public bool Deleted { get; set; } = false;

        public User User { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public Order(long userId, string orderNumber, DateTime orderDate, string status)
        {
            UserId = userId;
            OrderNumber = orderNumber;
            OrderDate = orderDate;
            Status = status;
        }

        // Used for seed data
        public Order(long orderId, long userId, string orderNumber, DateTime orderDate, string status)
        {
            OrderId = orderId;
            UserId = userId;
            OrderNumber = orderNumber;
            OrderDate = orderDate;
            Status = status;
        }
    }
}
