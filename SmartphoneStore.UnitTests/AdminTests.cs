using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartphoneStore.Domain.Abstract;
using SmartphoneStore.Domain.Entities;
using SmartphoneStore.WebUI.Controllers;

namespace SmartphoneStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Smartphones()
        {
            // Организация - создание имитированного хранилища данных
            Mock<ISmartphoneRepository> mock = new Mock<ISmartphoneRepository>();
            mock.Setup(m => m.Smartphones).Returns(new List<Smartphone>
            {
                new Smartphone { SmartphoneId = 1, Name = "Смартфон1"},
                new Smartphone { SmartphoneId = 2, Name = "Смартфон2"},
                new Smartphone { SmartphoneId = 3, Name = "Смартфон3"},
                new Smartphone { SmartphoneId = 4, Name = "Смартфон4"},
                new Smartphone { SmartphoneId = 5, Name = "Смартфон5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            List<Smartphone> result = ((IEnumerable<Smartphone>)controller.Index().
                ViewData.Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Смартфон1", result[0].Name);
            Assert.AreEqual("Смартфон2", result[1].Name);
            Assert.AreEqual("Смартфон3", result[2].Name);
        }
    }
}