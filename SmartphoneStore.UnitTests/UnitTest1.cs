using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartphoneStore.Domain.Abstract;
using SmartphoneStore.Domain.Entities;
using SmartphoneStore.WebUI.Controllers;
using SmartphoneStore.WebUI.HtmlHelpers;
using SmartphoneStore.WebUI.Models;

namespace SmartphoneStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<ISmartphoneRepository> mock = new Mock<ISmartphoneRepository>();
            mock.Setup(m => m.Smartphones).Returns(new List<Smartphone>
            {
                new Smartphone { SmartphoneId = 1, Name = "Смартфон1"},
                new Smartphone { SmartphoneId = 2, Name = "Смартфон2"},
                new Smartphone { SmartphoneId = 3, Name = "Смартфон3"},
                new Smartphone { SmartphoneId = 4, Name = "Смартфон4"},
                new Smartphone { SmartphoneId = 5, Name = "Смартфон5"}
            });
            SmartphoneController controller = new SmartphoneController(mock.Object)
            {
                pageSize = 3
            };

            // Действие (act)
            SmartphonesListViewModel result = (SmartphonesListViewModel)controller.List(null, 2).Model;

            // Утверждение (assert)
            List<Smartphone> smartphones = result.Smartphones.ToList();
            Assert.IsTrue(smartphones.Count == 2);
            Assert.AreEqual(smartphones[0].Name, "Смартфон4");
            Assert.AreEqual(smartphones[1].Name, "Смартфон5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                            + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<ISmartphoneRepository> mock = new Mock<ISmartphoneRepository>();
            mock.Setup(m => m.Smartphones).Returns(new List<Smartphone>
            {
                new Smartphone { SmartphoneId = 1, Name = "Смартфон1"},
                new Smartphone { SmartphoneId = 2, Name = "Смартфон2"},
                new Smartphone { SmartphoneId = 3, Name = "Смартфон3"},
                new Smartphone { SmartphoneId = 4, Name = "Смартфон4"},
                new Smartphone { SmartphoneId = 5, Name = "Смартфон5"}
            });
            SmartphoneController controller = new SmartphoneController(mock.Object);
            controller.pageSize = 3;

            // Act
            SmartphonesListViewModel result
                = (SmartphonesListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Smartphones()
        {
            // Организация (arrange)
            Mock<ISmartphoneRepository> mock = new Mock<ISmartphoneRepository>();
            mock.Setup(m => m.Smartphones).Returns(new List<Smartphone>
            {
                new Smartphone { SmartphoneId = 1, Name = "Смартфон1", Manufacturer="Man1"},
                new Smartphone { SmartphoneId = 2, Name = "Смартфон2", Manufacturer="Man2"},
                new Smartphone { SmartphoneId = 3, Name = "Смартфон3", Manufacturer="Man1"},
                new Smartphone { SmartphoneId = 4, Name = "Смартфон4", Manufacturer="Man2"},
                new Smartphone { SmartphoneId = 5, Name = "Смартфон5", Manufacturer="Man3"}
            });
            SmartphoneController controller = new SmartphoneController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Smartphone> result = ((SmartphonesListViewModel)controller.List("Man2", 1).Model)
                .Smartphones.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Смартфон2" && result[0].Manufacturer == "Man2");
            Assert.IsTrue(result[1].Name == "Смартфон4" && result[1].Manufacturer == "Man2");
        }

        [TestMethod]
        public void Can_Create_Manegories()
        {
            // Организация - создание имитированного хранилища
            Mock<ISmartphoneRepository> mock = new Mock<ISmartphoneRepository>();
            mock.Setup(m => m.Smartphones).Returns(new List<Smartphone> {
                new Smartphone { SmartphoneId = 1, Name = "Смартфон1", Manufacturer="Xiaomi"},
                new Smartphone { SmartphoneId = 2, Name = "Смартфон2", Manufacturer="Xiaomi"},
                new Smartphone { SmartphoneId = 3, Name = "Смартфон3", Manufacturer="Apple"},
                new Smartphone { SmartphoneId = 4, Name = "Смартфон4", Manufacturer="Samsumg"},
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "Samsumg");
            Assert.AreEqual(results[1], "Xiaomi");
            Assert.AreEqual(results[2], "Apple");
        }

        [TestMethod]
        public void IndiManes_Selected_Manufacturer()
        {
            // Организация - создание имитированного хранилища
            Mock<ISmartphoneRepository> mock = new Mock<ISmartphoneRepository>();
            mock.Setup(m => m.Smartphones).Returns(new [] {
                new Smartphone { SmartphoneId = 1, Name = "Смартфон1", Manufacturer="Xiaomi"},
                new Smartphone { SmartphoneId = 2, Name = "Смартфон2", Manufacturer="Apple"}
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Организация - определение выбранной категории
            string manufacturerToSelect = "Apple";

            // Действие
            string result = target.Menu(manufacturerToSelect).ViewBag.SelectedManufacturer;

            // Утверждение
            Assert.AreEqual(manufacturerToSelect, result);
        }

        [TestMethod]
        public void Generate_Manufacturer_Specific_Smartphone_Count()
        {
            /// Организация (arrange)
            Mock<ISmartphoneRepository> mock = new Mock<ISmartphoneRepository>();
            mock.Setup(m => m.Smartphones).Returns(new List<Smartphone>
            {
                new Smartphone { SmartphoneId = 1, Name = "Смартфон1", Manufacturer="Man1"},
                new Smartphone { SmartphoneId = 2, Name = "Смартфон2", Manufacturer="Man2"},
                new Smartphone { SmartphoneId = 3, Name = "Смартфон3", Manufacturer="Man1"},
                new Smartphone { SmartphoneId = 4, Name = "Смартфон4", Manufacturer="Man2"},
                new Smartphone { SmartphoneId = 5, Name = "Смартфон5", Manufacturer="Man3"}
            });
            SmartphoneController controller = new SmartphoneController(mock.Object)
            {
                pageSize = 3
            };

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((SmartphonesListViewModel)controller.List("Man1").Model).PagingInfo.TotalItems;
            int res2 = ((SmartphonesListViewModel)controller.List("Man2").Model).PagingInfo.TotalItems;
            int res3 = ((SmartphonesListViewModel)controller.List("Man3").Model).PagingInfo.TotalItems;
            int resAll = ((SmartphonesListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }


    }
}