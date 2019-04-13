using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartphoneStore.Domain.Abstract;

namespace SmartphoneStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private ISmartphoneRepository _repository;

        public NavController(ISmartphoneRepository repo)
        {
            _repository = repo;
        }

        public PartialViewResult Menu(string manufacturer = null)
        {
            ViewBag.SelectedManufacturer = manufacturer;
            
            IEnumerable<string> manufacturers = _repository.Smartphones
                .Select(smartphone => smartphone.Manufacturer)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(manufacturers);
        }
    }
}