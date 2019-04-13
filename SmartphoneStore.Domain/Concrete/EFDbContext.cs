using System.Data.Entity;
using SmartphoneStore.Domain.Entities;

namespace SmartphoneStore.Domain.Concrete
{
    public class EfDbContext : DbContext
    {
        public DbSet<Smartphone> Smartphones { get; set; }
    }
}