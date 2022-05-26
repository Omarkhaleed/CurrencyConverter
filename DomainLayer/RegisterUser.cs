using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class RegisterUser
    {
        [Required]
        [Display(Name = "FullName")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Sorry, Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string phoneNumber { get; set; }
    }
}
