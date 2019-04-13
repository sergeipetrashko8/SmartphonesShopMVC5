using System.Collections.Generic;
using SmartphoneStore.Domain.Abstract;
using SmartphoneStore.Domain.Entities;

namespace SmartphoneStore.Domain.Concrete
{
    public class EfSmartphoneRepository : ISmartphoneRepository
    {
        private readonly EfDbContext _context = new EfDbContext();

        public IEnumerable<Smartphone> Smartphones => _context.Smartphones;
    }
}