using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartphoneStore.Domain.Entities;

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
            Smartphone smartphone1 = new Smartphone { SmartphoneId = 1, Name = "Игра1", Price = 100 };
            Smartphone smartphone2 = new Smartphone { SmartphoneId = 2, Name = "Игра2", Price = 55 };

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
    }
}



//todo Создание кнопок добавления в корзину