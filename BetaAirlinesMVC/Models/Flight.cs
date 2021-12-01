using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetaAirlinesMVC.Models
{
    [Table("Flights")]
    public class Flight
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        [ForeignKey("DepartureAirport")]
        public int DepartureAirportId { get; set; }
        public virtual Airport DepartureAirport { get; set; }


        [ForeignKey("ArrivalAirport")]
        public int ArrivalAirportId { get; set; }
        public virtual Airport ArrivalAirport { get; set; }

        public int FlightLengthInMinutes { get; set; }

        public int Active { get; set; }
    }
}