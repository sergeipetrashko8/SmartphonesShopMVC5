using System.Linq;
using System.Web.Mvc;
using SmartphoneStore.Domain.Abstract;
using SmartphoneStore.WebUI.Models;

namespace SmartphoneStore.WebUI.Controllers
{
    public class SmartphoneController : Controller
    {
        private ISmartphoneRepository repository;
        public int pageSize = 4;

        public SmartphoneController(ISmartphoneRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string manufacturer, int page = 1)
        {
            SmartphonesListViewModel model = new SmartphonesListViewModel
            {
                Smartphones = repository.Smartphones
                    .Where(p => manufacturer == null || p.Manufacturer == manufacturer)
                    .OrderBy(smartphone => smartphone.SmartphoneId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = manufacturer == null ?
                        repository.Smartphones.Count() :
                        repository.Smartphones.Count(game => game.Manufacturer == manufacturer)
                },
                CurrentManufacturer = manufacturer
            };
            return View(model);
        }
    }
}