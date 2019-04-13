using System.Data.Entity;
using SmartphoneStore.Domain.Entities;

namespace SmartphoneStore.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Smartphone> Smartphones { get; set; }
    }
}