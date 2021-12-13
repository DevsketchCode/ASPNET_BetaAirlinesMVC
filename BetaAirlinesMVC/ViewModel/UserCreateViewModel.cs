using BetaAirlinesMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BetaAirlinesMVC.ViewModel
{
    public class UserCreateViewModel
    {
        [Display(Name = "First Name")]
        [MaxLength(30)]
        [Required]
        public String FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(30)]
        [Required]
        public String LastName { get; set; }

        [MaxLength(30)]
        [Required]
        public String Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public String Password { get; set; }

        [NotMapped]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public String ConfirmPassword { get; set; }

        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] // Must be here for default value to be entered
        [Required]
        public DateTime RegisteredDate { get; set; }

        public int Active { get; set; }

        // supply a virtual object to the class to link the foreign key. 
        // Datatype of the foreign key object goes to, then whatever you name the virtual object, name the ForeignKey the same exactly.
        [Display(Name = "User Role")]
        [Required]
        public int UserRoleID { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}