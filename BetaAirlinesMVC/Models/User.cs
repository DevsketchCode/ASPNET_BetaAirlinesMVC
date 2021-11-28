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

        [MaxLength(30)]
        public String FirstName { get; set; }

        [MaxLength(30)]
        public String LastName { get; set; }

        [MaxLength(30)]
        public String Username { get; set; }

        public String Password { get; set; }

        public DateTime RegisteredDate { get; set; }

        public int Active { get; set; }

        // supply a virtual object to the class to link the foreign key. 
        // Datatype of the foreign key object goes to, then whatever you name the virtual object, name the ForeignKey the same exactly.
        [ForeignKey("UserRole")]
        public int UserRoleID { get; set; }
        public virtual UserRole UserRole { get; set; }

    }
}