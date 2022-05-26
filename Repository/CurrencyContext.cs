using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainLayer;

namespace Repository
{
    public class CurrencyContext : IdentityDbContext
    {
        private readonly DbContextOptions _options;
        public CurrencyContext(DbContextOptions<CurrencyContext> options)
            : base(options)
        {
            _options = options;
        }

        public DbSet<Currency> Currency { get; set; }
        public DbSet<ExchangeHistory> ExchangeHistory { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().HasQueryFilter(pp => pp.IsActive);
            base.OnModelCreating(modelBuilder);

        }

    }
}
