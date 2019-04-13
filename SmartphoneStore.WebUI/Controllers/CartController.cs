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

        public RedirectToRouteResult AddToCart(int smartphoneId, string returnUrl)
        {
            Smartphone smartphone = repository.Smartphones
                .FirstOrDefault(g => g.SmartphoneId == smartphoneId);

            if (smartphone != null)
            {
                GetCart().AddItem(smartphone, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int smartphoneId, string returnUrl)
        {
            Smartphone smartphone = repository.Smartphones
                .FirstOrDefault(g => g.SmartphoneId == smartphoneId);

            if (smartphone != null)
            {
                GetCart().RemoveLine(smartphone);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }
    }
}