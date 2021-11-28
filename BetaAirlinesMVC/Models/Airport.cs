using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetaAirlinesMVC.Models
{
    [Table("Airports")]
    public class Airport
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(80)]
        public String Name { get; set; }

        [MaxLength(6)]
        public String ICAO { get; set; }

        [MaxLength(50)]
        public String City { get; set; }
        
        [MaxLength(50)]
        public String State { get; set; }

        [MaxLength(50)]
        public String Country { get; set; }

        public int Active { get; set; }

        public virtual List<Flight> DepartingAirports { get; set; }
        public virtual List<Flight> ArrivingAirports { get; set; }
    }
}