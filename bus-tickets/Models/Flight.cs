using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bus_tickets.Models
{
    internal class Flight
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Number { get; set; }

        [Required]
        [StringLength(255)]
        public string? BusType { get; set; }

        [Required]
        [StringLength(255)]
        public string? Destination { get; set; }

        public DateOnly Date { get; set; }

        [Column(TypeName = "time(0)")]
        public TimeOnly DeparturesTime { get; set; }

        [Column(TypeName = "time(0)")]
        public TimeOnly ArrivalTime { get; set; }

        [Column(TypeName = "double(10,2) unsigned")]
        public double Cost { get; set; }

        public uint Left { get; set; }

        public uint Sold { get; set; }

        public override string ToString()
        {
            return string.Format("{0,5} | {1,5} | {2,12} | {3,16} | {4,10} | {5,11} | {6,8} | {7,9} | {8,8} | {9,7}",
                Id, Number, BusType, Destination, Date, DeparturesTime, ArrivalTime, Cost, Left, Sold);
        }
    }
}
