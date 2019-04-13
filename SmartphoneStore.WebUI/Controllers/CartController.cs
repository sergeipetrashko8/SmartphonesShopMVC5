using System.Linq;
using System.Web.Mvc;
using SmartphoneStore.Domain.Abstract;
using SmartphoneStore.Domain.Entities;
using SmartphoneStore.WebUI.Models;

namespace SmartphoneStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private ISmartphoneRepository repository;

        public CartController(ISmartphoneRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
    
        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int smartphoneId, string returnUrl)
        {
            Smartphone smartphone = repository.Smartphones
                .FirstOrDefault(g => g.SmartphoneId == smartphoneId);

            if (smartphone != null)
            {
                cart.AddItem(smartphone, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int smartphoneId, string returnUrl)
        {
            Smartphone smartphone = repository.Smartphones
                .FirstOrDefault(g => g.SmartphoneId == smartphoneId);

            if (smartphone != null)
            {
                cart.RemoveLine(smartphone);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
    }
}