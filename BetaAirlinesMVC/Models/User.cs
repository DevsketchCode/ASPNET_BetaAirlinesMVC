using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetaAirlinesMVC.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

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

        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime RegisteredDate { get; set; }

        [DefaultValue(1)]
        public int Active { get; set; }

        // supply a virtual object to the class to link the foreign key. 
        // Datatype of the foreign key object goes to, then whatever you name the virtual object, name the ForeignKey the same exactly.
        [Display(Name = "User Role")]
        [ForeignKey("UserRole")]
        [Required]
        public int UserRoleID { get; set; }
        public virtual UserRole UserRole { get; set; }


    }
}