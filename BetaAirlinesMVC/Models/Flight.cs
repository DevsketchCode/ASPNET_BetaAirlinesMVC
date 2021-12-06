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

        [Required(ErrorMessage = "Please Select a Departure Date")]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "A departure airport was not selected")]
        [ForeignKey("DepartureAirport")]
        public int DepartureAirportId { get; set; }
        public virtual Airport DepartureAirport { get; set; }

        [Required(ErrorMessage = "An arrival airport was not selected")]
        [ForeignKey("ArrivalAirport")]
        public int ArrivalAirportId { get; set; }
        public virtual Airport ArrivalAirport { get; set; }

        [Range(0,5000)]
        public int FlightLengthInMinutes { get; set; }

        [DefaultValue(1)]
        public int Active { get; set; }
    }
}