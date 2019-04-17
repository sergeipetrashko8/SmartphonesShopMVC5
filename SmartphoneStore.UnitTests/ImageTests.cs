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
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Организация - создание объекта Smartphone с данными изображения
            Smartphone smartphone = new Smartphone
            {
                SmartphoneId = 2,
                Name = "Смартфон2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Организация - создание имитированного хранилища
            Mock<ISmartphoneRepository> mock = new Mock<ISmartphoneRepository>();
            mock.Setup(m => m.Smartphones).Returns(new List<Smartphone> {
                new Smartphone {SmartphoneId = 1, Name = "Смартфон1"},
                smartphone,
                new Smartphone {SmartphoneId = 3, Name = "Смартфон3"}
            }.AsQueryable());

            // Организация - создание контроллера
            SmartphoneController controller = new SmartphoneController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(2);

            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(smartphone.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Организация - создание имитированного хранилища
            Mock<ISmartphoneRepository> mock = new Mock<ISmartphoneRepository>();
            mock.Setup(m => m.Smartphones).Returns(new List<Smartphone> {
                new Smartphone {SmartphoneId = 1, Name = "Смартфон1"},
                new Smartphone {SmartphoneId = 2, Name = "Смартфон2"}
            }.AsQueryable());

            // Организация - создание контроллера
            SmartphoneController controller = new SmartphoneController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(10);

            // Утверждение
            Assert.IsNull(result);
        }
    }
}