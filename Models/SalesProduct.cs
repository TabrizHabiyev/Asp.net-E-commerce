﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Models
{
    public class SalesProduct
    {
        public int Id { get; set; }
        public int SalesId { get; set; }
        public Sales Sales { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
