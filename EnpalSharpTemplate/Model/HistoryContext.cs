using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EnpalSharpTemplate.Model
{
    public class HistoryContext : DbContext
    {
        public HistoryContext(DbContextOptions<HistoryContext> options) : base(options) {}
        public HistoryContext() { }
        public virtual DbSet<PayloadModel> HistoryData { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PayloadModel>(entity => {
                entity.HasKey(e => e.Key);
            });
        }

    }
}
