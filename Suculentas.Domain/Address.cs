using System;
using System.Collections.Generic;
using Suculentas.Domain.Identity;

namespace Suculentas.Domain
{
    public class Address
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Road { get; set; }
        public int Number { get; set; }
        public string Complement { get; set; }
        public string CEP { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}