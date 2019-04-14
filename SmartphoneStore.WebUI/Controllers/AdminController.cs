using System.Web.Mvc;
using SmartphoneStore.Domain.Abstract;

namespace SmartphoneStore.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISmartphoneRepository repository;

        public AdminController(ISmartphoneRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Smartphones);
        }
    }
}