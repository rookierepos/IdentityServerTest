using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerCenter.Models
{
    public class Login
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class SignIn
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ComfirmPassword { get; set; }
    }
}
