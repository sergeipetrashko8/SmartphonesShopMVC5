using System.Collections.Generic;
using SmartphoneStore.Domain.Abstract;
using SmartphoneStore.Domain.Entities;

namespace SmartphoneStore.Domain.Concrete
{
    public class EFSmartphoneRepository : ISmartphoneRepository
    {
        private readonly EFDbContext context = new EFDbContext();

        public IEnumerable<Smartphone> Smartphones => context.Smartphones;
    }
}