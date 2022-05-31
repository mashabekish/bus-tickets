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

        public uint Count { get; set; }

        [Column(TypeName = "double(10,2) unsigned")]
        public double Cost { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("FlightId")]
        public Flight? Flight { get; set; }

        public override string ToString()
        {
            return string.Format("{0,5} | {1,15} | {2,8} | {3,10} | {4,9}",
                Id, UserId, FlightId, Count, Cost);
        }

        public string ToString(Flight flight)
        {
            return string.Format("{0,17} | {1,10} | {2,11} | {3,8} | {4,10} | {5,9} | {6,8}",
                flight.Destination, flight.Date, flight.DeparturesTime, flight.ArrivalTime, Count, flight.Cost, Cost);
        }
    }
}
