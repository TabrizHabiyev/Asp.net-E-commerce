using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Models
{
    public class ContactDetails
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneMobile { get; set;}
        public string PhoneHotline { get; set; }
        public string MapUrl { get; set; }
        public string OpenClosed { get; set; }
    }
}
