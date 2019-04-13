﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartphoneStore.Domain.Abstract;
using SmartphoneStore.Domain.Entities;
using SmartphoneStore.WebUI.Controllers;
using SmartphoneStore.WebUI.Models;

namespace SmartphoneStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Организация - создание нескольких тестовых игр
            Smartphone smartphone1 = new Smartphone { SmartphoneId = 1, Name = "Смартфон1" };
            Smartphone smartphone2 = new Smartphone { SmartphoneId = 2, Name = "Смартфон2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(smartphone1, 1);
            cart.AddItem(smartphone2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Smartphone, smartphone1);
            Assert.AreEqual(results[1].Smartphone, smartphone2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Организация - создание нескольких тестовых игр
            Smartphone smartphone1 = new Smartphone { SmartphoneId = 1, Name = "Смартфон1" };
            Smartphone smartphone2 = new Smartphone { SmartphoneId = 2, Name = "Смартфон2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(smartphone1, 1);
            cart.AddItem(smartphone2, 1);
            cart.AddItem(smartphone1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Smartphone.SmartphoneId).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Организация - создание нескольких тестовых игр
            Smartphone smartphone1 = new Smartphone { SmartphoneId = 1, Name = "Смартфон1" };
            Smartphone smartphone2 = new Smartphone { SmartphoneId = 2, Name = "Смартфон2" };
            Smartphone smartphone3 = new Smartphone { SmartphoneId = 3, Name = "Смартфон3" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - добавление нескольких игр в корзину
            cart.AddItem(smartphone1, 1);
            cart.AddItem(smartphone2, 4);
            cart.AddItem(smartphone3, 2);
            cart.AddItem(smartphone2, 1);

            // Действие
            cart.RemoveLine(smartphone2);

            // Утверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Smartphone == smartphone2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Организация - создание нескольких тестовых игр
            Smartphone smartphone1 = new Smartphone { SmartphoneId = 1, Name = "Смартфон1", Price = 100 };
            Smartphone smartphone2 = new Smartphone { SmartphoneId = 2, Name = "Смартфон2", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(smartphone1, 1);
            cart.AddItem(smartphone2, 1);
            cart.AddItem(smartphone1, 5);
            decimal result = cart.ComputeTotalValue();

            // Утверждение
            Assert.AreEqual(result, 655);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Организация - создание нескольких тестовых игр
            Smartphone smartphone1 = new Smartphone { SmartphoneId = 1, Name = "Смартфон1", Price = 100 };
            Smartphone smartphone2 = new Smartphone { SmartphoneId = 2, Name = "Смартфон2", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(smartphone1, 1);
            cart.AddItem(smartphone2, 1);
            cart.AddItem(smartphone1, 5);
            cart.Clear();

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        /// <summary>
        /// Проверяем добавление в корзину
        /// </summary>
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Организация - создание имитированного хранилища
            Mock<ISmartphoneRepository> mock = new Mock<ISmartphoneRepository>();
            mock.Setup(m => m.Smartphones).Returns(new List<Smartphone>
            {
                new Smartphone {SmartphoneId = 1, Name = "Смартфон1", Manufacturer = "Производитель1"},
            }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object);

            // Действие - добавить игру в корзину
            controller.AddToCart(cart, 1, null);

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Smartphone.SmartphoneId, 1);
        }

        /// <summary>
        /// После добавления игры в корзину, должно быть перенаправление на страницу корзины
        /// </summary>
        [TestMethod]
        public void Adding_Smartphone_To_Cart_Goes_To_Cart_Screen()
        {
            // Организация - создание имитированного хранилища
            Mock<ISmartphoneRepository> mock = new Mock<ISmartphoneRepository>();
            mock.Setup(m => m.Smartphones).Returns(new List<Smartphone>
            {
                new Smartphone {SmartphoneId = 1, Name = "Смартфон1", Manufacturer = "Производитель1"},
            }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object);

            // Действие - добавить игру в корзину
            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Утверждение
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        // Проверяем URL
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController target = new CartController(null);

            // Действие - вызов метода действия Index()
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Утверждение
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
    }
}



//todo Создание кнопок добавления в корзину