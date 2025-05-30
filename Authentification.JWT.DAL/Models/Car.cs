﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentification.JWT.DAL.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; } 
        public string Model { get; set; } 
        public int Year { get; set; }
        public string LicensePlate { get; set; }
        public int OwnerId { get; set; } 
        public User Owner { get; set; }
        public string ImageUrl { get; set; } 
    }
}
