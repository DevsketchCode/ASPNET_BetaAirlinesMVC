using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BetaAirlinesMVC.Models;

namespace BetaAirlinesMVC.ViewModel
{
    public class BookAFlightViewModel
    {
        // This view should hold all the fields that you will want to retrieve from the Views Form
        // Then a controller can get this from the specific areas. 

        // The required and the names and types from the DataAnnotations can all go in the Models
        [Required(ErrorMessage = "Please select a flight.")]
        public int FlightId { get; set; }

        [Display(Name = "User")]
        [Required(ErrorMessage = "Please log in first.")]
        public int UserId { get; set; }

        [Display(Name = "Date Booked")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public String DateBooked { get; set; }

        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Please select an departure airport.")]
        public int DepartureAirportId { get; set; }
        public virtual Airport DepartureAirport { get; set; }

        [Required(ErrorMessage = "Please select an arrival airport.")]
        public int ArrivalAirportId { get; set; }
        public virtual Airport ArrivalAirport { get; set; }

        public int FlightLengthInMinutes { get; set; }

        public List<Flight> FlightList { get; set; }

        public List<Airport> DepartureAirportsList { get; set; }
        public List<Airport> ArrivalAirportsList { get; set; }

    }
}