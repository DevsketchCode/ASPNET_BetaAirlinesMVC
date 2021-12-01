using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetaAirlinesMVC.Models
{
    [Table("BookedFlights")]
    public class BookedFlight
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateBooked { get; set; }

        public int Active { get; set; }


        // supply a virtual object to the class to link the foreign key. 
        // Datatype of the foreign key object goes to, then whatever you name the virtual object, name the ForeignKey the same exactly.
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Flight")]
        public int FlightId { get; set; }
        public virtual Flight Flight { get; set; }

    }
}