using System.Collections.Generic;
using SmartphoneStore.Domain.Abstract;
using SmartphoneStore.Domain.Entities;

namespace SmartphoneStore.Domain.Concrete
{
    public class EfSmartphoneRepository : ISmartphoneRepository
    {
        private readonly EfDbContext _context = new EfDbContext();

        public IEnumerable<Smartphone> Smartphones => _context.Smartphones;

        public void SaveSmartphone(Smartphone smartphone)
        {
            if (smartphone.SmartphoneId == 0)
                _context.Smartphones.Add(smartphone);
            else
            {
                Smartphone dbEntry = _context.Smartphones.Find(smartphone.SmartphoneId);

                if (dbEntry != null)
                {
                    dbEntry.Name = smartphone.Name;
                    dbEntry.Description = smartphone.Description;
                    dbEntry.Price = smartphone.Price;
                    dbEntry.Manufacturer = smartphone.Manufacturer;
                    dbEntry.ImageData = smartphone.ImageData;
                    dbEntry.ImageMimeType = smartphone.ImageMimeType;
                }
            }
            _context.SaveChanges();
        }

        public Smartphone DeleteSmartphone(int smartphoneId)
        { 
            Smartphone dbEntry = _context.Smartphones.Find(smartphoneId);
            if (dbEntry != null)
            {
                _context.Smartphones.Remove(dbEntry);
                _context.SaveChanges();
            }
            return dbEntry;
        }
    }
}