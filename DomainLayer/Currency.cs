using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class Currency
    {
        public int CurrencyId { get; set; }

        [Required]
        [StringLength(maximumLength:20, MinimumLength =  3,
         ErrorMessage = "The property {0} should have {1} maximum characters and {2} minimum characters")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength:10,ErrorMessage = "The sign of currency must be less than 10 numbers")]
        public string Sign { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<ExchangeHistory> ExchangeHistories { set; get; } 

    }
}
