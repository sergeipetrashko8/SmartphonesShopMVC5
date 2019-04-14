using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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

        [TestMethod]
        public void Can_Edit_Smartphone()
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
            Smartphone smartphone1 = controller.Edit(1).ViewData.Model as Smartphone;
            Smartphone smartphone2 = controller.Edit(2).ViewData.Model as Smartphone;
            Smartphone smartphone3 = controller.Edit(3).ViewData.Model as Smartphone;

            // Assert
            Assert.AreEqual(1, smartphone1.SmartphoneId);
            Assert.AreEqual(2, smartphone2.SmartphoneId);
            Assert.AreEqual(3, smartphone3.SmartphoneId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Smartphone()
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
            Smartphone result = controller.Edit(6).ViewData.Model as Smartphone;

            // Assert
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<ISmartphoneRepository> mock = new Mock<ISmartphoneRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Smartphone
            Smartphone smartphone = new Smartphone { Name = "Test" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(smartphone);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveSmartphone(smartphone));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<ISmartphoneRepository> mock = new Mock<ISmartphoneRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Smartphone
            Smartphone smartphone = new Smartphone { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(smartphone);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SaveSmartphone(It.IsAny<Smartphone>()), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}