using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BetaAirlinesMVC.ViewModel
{
    public class UserLoginViewModel
    {
        [MaxLength(30)]
        [Required]
        public String Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public String Password { get; set; }
    }
}