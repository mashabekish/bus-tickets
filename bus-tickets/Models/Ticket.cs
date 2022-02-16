using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bus_tickets.Models
{
    internal class Ticket
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int FlightId { get; set; }

        public int Count { get; set; }

        public double Cost { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("FlightId")]
        public Flight? Flight { get; set; }

        public override string ToString()
        {
            return string.Format("{0,5} | {1,15} | {2,8} | {3,9} | {4,10}",
                Id, UserId, FlightId, Count, Cost);
        }
    }
}