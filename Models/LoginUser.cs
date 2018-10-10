using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace login_registration.Models
{
    public class LoginUser
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}