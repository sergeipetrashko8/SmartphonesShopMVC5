using System.Linq;
using System.Web.Mvc;
using SmartphoneStore.Domain.Abstract;
using SmartphoneStore.Domain.Entities;
using SmartphoneStore.WebUI.Models;

namespace SmartphoneStore.WebUI.Controllers
{
    public class SmartphoneController : Controller
    {
        private readonly ISmartphoneRepository _repository;
        public int PageSize = 4;

        public SmartphoneController(ISmartphoneRepository repo)
        {
            _repository = repo;
        }

        public ViewResult List(string manufacturer, int page = 1)
        {
            SmartphonesListViewModel model = new SmartphonesListViewModel
            {
                Smartphones = _repository.Smartphones
                    .Where(p => manufacturer == null || p.Manufacturer == manufacturer)
                    .OrderBy(smartphone => smartphone.SmartphoneId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = manufacturer == null ?
                        _repository.Smartphones.Count() :
                        _repository.Smartphones.Count(game => game.Manufacturer == manufacturer)
                },
                CurrentManufacturer = manufacturer
            };
            return View(model);
        }

        public FileContentResult GetImage(int smartphoneId)
        {
            Smartphone smartphone = _repository.Smartphones
                .FirstOrDefault(g => g.SmartphoneId == smartphoneId);

            if (smartphone != null)
            {
                return File(smartphone.ImageData, smartphone.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}