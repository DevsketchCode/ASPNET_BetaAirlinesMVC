using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BetaAirlinesMVC.Models;

namespace BetaAirlinesMVC.ViewModel
{
    public class MyFlightsViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name ="Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Date Booked")]
        public DateTime DateBooked { get; set; }

        [Display(Name= "Active")]
        public int ActiveBookedFlight { get; set; }

        // Flights booked by the user
        //public List<Flight> FlightList { get; set; }

        [Display(Name = "Departure Airport")]
        public string DepartureAirport { get; set; }

        [Display(Name = "Arrival Airport")]
        public string ArrivalAirport { get; set; }
    }
}