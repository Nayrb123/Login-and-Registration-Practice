using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace login_registration.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        [Required]
        [MinLength(3)]
        public string first_name { get; set; }
        [Required]
        [MinLength(3)]
        public string last_name { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        
        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters long")]
        public string password { get; set; }

        [NotMapped]
        [Compare("password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
    }
}