using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainLayer.DTOs
{
    public class AddingDetails
    {
        public string Name { get; set; }
        public string Sign { get; set; }
        public bool IsActive { get; set; }
        public double Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
