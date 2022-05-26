using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class ExchangeHistory
    {
        public int Id { get; set; }

        [ForeignKey("Currency")]
        public int CurrencyId { get; set; } 
        public Currency currency { get; set; }

        [Required]
        public DateTime? ExchangeDate { get; set; }

        [Required]
        public Double   CurruencyRate { get; set; }

        internal object ToList()
        {
            throw new NotImplementedException();
        }
    }
}
