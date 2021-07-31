using CurrencyConverterAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverterAPI.Data
{
    public class ConvertTransactionContext : DbContext
    {
        public DbSet<ConvertTransaction> ConvertTransactions { get; set; }

        public ConvertTransactionContext(DbContextOptions<ConvertTransactionContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CurrencyConverter.db");
        }
    }
}
