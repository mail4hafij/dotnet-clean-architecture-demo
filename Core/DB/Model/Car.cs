using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DB.Model
{
    public class Car
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CarId { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Nameplate { get; set; }

        public bool Deleted { get; set; } = false;

        public User user { get; set; }

        public Car(long userId, string nameplate)
        {
            UserId = userId;
            Nameplate = nameplate;
        }

        // Used for seed data
        public Car(long carId, long userId, string nameplate)
        {
            CarId = carId;
            UserId = userId;
            Nameplate = nameplate;
        }
    }
}
