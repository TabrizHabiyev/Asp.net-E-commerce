using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.ViewModels
{
    public class RegisterVM
    {
        [Required, StringLength(40)]
        public string UserName { get; set; }

        [Required, StringLength(25)]
        public string FirstName { get; set; }

        [Required, StringLength(25)]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";


        [Required, StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Gender { get; set; }
        public bool Newsletter { get; set; }
    }
}