using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartphoneStore.Domain.Abstract;
using SmartphoneStore.Domain.Entities;

namespace SmartphoneStore.WebUI.Controllers
{
    [Authorize]
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

        public ViewResult Edit(int smartphoneId)
        {
            Smartphone smartphone = repository.Smartphones
                .FirstOrDefault(g => g.SmartphoneId == smartphoneId);
            return View(smartphone);
        }

        // Перегруженная версия Edit() для сохранения изменений
        [HttpPost]
        public ActionResult Edit(Smartphone smartphone, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    smartphone.ImageMimeType = image.ContentType;
                    smartphone.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(smartphone.ImageData, 0, image.ContentLength);
                }
                repository.SaveSmartphone(smartphone);
                TempData["message"] = string.Format("Изменения в смартфоне \"{0}\" были сохранены", smartphone.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(smartphone);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Smartphone());
        }

        [HttpPost]
        public ActionResult Delete(int smartphoneId) 
        {
            Smartphone deletedSmartphone = repository.DeleteSmartphone(smartphoneId);
            if (deletedSmartphone != null)
            {
                TempData["message"] = string.Format("Смартфон \"{0}\" был удален",
                    deletedSmartphone.Name);
            }
            return RedirectToAction("Index");
        }
    }
}