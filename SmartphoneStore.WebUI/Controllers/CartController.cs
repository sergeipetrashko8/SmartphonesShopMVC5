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
        private IOrderProcessor orderProcessor;

        public CartController(ISmartphoneRepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
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