using Asp.net_E_commerce.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.ViewModels
{
    public class ForgotPassword
    {
        public AppUser User { get; set; }
        public string Token { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password) , Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}
