using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ActeAuto.Models
{
    public class DocumenteContext : DbContext
    {
        public DbSet<Document> documente { get; set; }
        public DbSet<DomainClasses.Masina> masini { get; set; }
        public DbSet<DomainClasses.Person> persoane { get; set; }
     }
}