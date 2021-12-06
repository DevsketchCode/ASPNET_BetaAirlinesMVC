using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BetaAirlinesMVC.ViewModel
{
    public class BookAFlightViewModel
    {
        // This view should hold all the fields that you will want to retrieve from the Views Form
        // Then a controller can get this from the specific areas. 

        // The required and the names and types from the DataAnnotations can all go in the Models
        [Required(ErrorMessage = "Please select a flight.")]
        [Key]
        public int FlightId { get; set; }

        [Display(Name = "User")]
        [Required(ErrorMessage = "Please log in first.")]
        public int UserId { get; set; }

        [Display(Name = "Departure Airport")]
        [DataType(DataType.Date)]
        public String DateBooked { get; set; }

    }
}