using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DB.Model
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Email { get; set; }

        [Required]
        [MaxLength(500)]
        public string Password { get; set; }

        public bool Deleted { get; set; } = false;

        public List<Car> Cars { get; set; }

        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }

        // Used for seed data
        public User(long userId, string email, string password)
        {
            UserId = userId;
            Email = email;
            Password = password;
        }
    }
}
