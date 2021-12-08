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

        [Display(Name ="Departure Date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DepartureDate { get; set; }

        [Display(Name = "Departure Airport")]
        [Required]
        [ForeignKey("DepartureAirport")]
        public int DepartureAirportId { get; set; }
        
        public virtual Airport DepartureAirport { get; set; }

        [Required]
        [ForeignKey("ArrivalAirport")]
        [Display(Name = "Arrival Airport")]
        public int ArrivalAirportId { get; set; }

        public virtual Airport ArrivalAirport { get; set; }

        [Display(Name ="Duration")]
        [Range(0,5000)]
        public int FlightLengthInMinutes { get; set; }

        [DefaultValue(1)]
        public int Active { get; set; }
    }
}