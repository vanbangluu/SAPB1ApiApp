using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DbContext")
        {
        }
        public virtual DbSet<SalesInvoice> SalesInvoices { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}