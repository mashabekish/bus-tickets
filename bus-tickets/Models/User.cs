using System.ComponentModel.DataAnnotations;

namespace bus_tickets.Models
{
    internal class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string? Login { get; set; }

        [StringLength(255)]
        public string? Password { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }

        public override string ToString()
        {
            return string.Format("{0,5} | {1,9} | {2,8} | {3,5} | {4,11}",
                Id, Login, "********", IsAdmin, IsActive);
        }
    }
}