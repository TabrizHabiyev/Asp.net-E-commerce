using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Models
{
    public class CustomerMailList
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public bool IsSubscriber { get; set; }
    }
}
