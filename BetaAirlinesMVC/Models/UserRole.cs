using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BetaAirlinesMVC.Models
{
    [Table("UserRoles")]
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public String Role { get; set; }

        [MaxLength(150)]
        public String Description { get; set; }

        [DefaultValue(1)]
        public int Active { get; set; }
    }
}