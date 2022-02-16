using System.ComponentModel.DataAnnotations;

namespace bus_tickets.Models
{
    internal class Flight
    {
        [Key]
        public int Id { get; set; }

        public string? Number { get; set; }

        [StringLength(255)]
        public string? BusType { get; set; }

        [StringLength(255)]
        public string? Destination { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly DeparturesTime { get; set; }

        public TimeOnly ArrivalTime { get; set; }

        public double Cost { get; set; }

        public int Left { get; set; }

        public int Sold { get; set; }

        public override string ToString()
        {
            return string.Format("{0,5} | {1,5} | {2,12} | {3,16} | {4,10} | {5,11} | {6,8} | {7,9} | {8,8} | {9,7}",
                Id, Number, BusType, Destination, Date, DeparturesTime, ArrivalTime, Cost, Left, Sold);
        }
    }
}