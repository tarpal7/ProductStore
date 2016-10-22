using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstarct;
using System.Collections.Generic;
using Domain.Entities;
using WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.HtmlHelpers;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {

                new Product {ProductId = 1, Name ="Product1" },
                new Product {ProductId = 2, Name ="Product2" },
                new Product {ProductId = 3, Name ="Product3" },
                new Product {ProductId = 4, Name ="Product4" },
                new Product {ProductId = 5, Name ="Product5" }
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

           ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;

            List<Product> products = result.Products.ToList();
            Assert.IsTrue(products.Count == 2);
            Assert.AreEqual(products[0].Name, "Product4");
            Assert.AreEqual(products[1].Name, "Product5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // 1
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // 2
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // 3
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // 1
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product{ProductId = 1, Name = "Product1"},
                new Product{ProductId = 2, Name = "Product2"},
                new Product{ProductId = 3, Name = "Product3"},
                new Product{ProductId = 4, Name = "Product4"},
                new Product{ProductId = 5, Name = "Product5"}
            });

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // 2
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product{ProductId = 1, Name = "Product1", Kind="Kind1"},
                new Product{ProductId = 2, Name = "Product2", Kind="Kind2"},
                new Product{ProductId = 3, Name = "Product3", Kind="Kind1"},
                new Product{ProductId = 4, Name = "Product4", Kind="Kind3"},
                new Product{ProductId = 5, Name = "Product5", Kind="Kind2"}
            });

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            List<Product> result = ((ProductsListViewModel)controller.List("Kind2", 1).Model).Products.ToList();

            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Product2" && result[0].Kind == "Kind2");
            Assert.IsTrue(result[1].Name == "Product5" && result[1].Kind == "Kind2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product{ProductId = 1, Name = "Product1", Kind="Kind1"},
                new Product{ProductId = 2, Name = "Product2", Kind="Kind2"},
                new Product{ProductId = 3, Name = "Product3", Kind="Kind1"},
                new Product{ProductId = 4, Name = "Product4", Kind="Kind3"},
                new Product{ProductId = 5, Name = "Product5", Kind="Kind2"}
            });

            NavController target = new NavController(mock.Object);

            // Действие (act)
            List<string> result = ((IEnumerable<string>)target.Menu().Model).ToList();

            Assert.AreEqual(result.Count(), 3);
            Assert.AreEqual(result[0], "Kind1");
            Assert.AreEqual(result[1], "Kind2");
            Assert.AreEqual(result[2], "Kind3");
        }

        [TestMethod]
        public void Indicates_Selected_Kind()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product{ProductId = 1, Name = "Product1", Kind="Kind1"},
                new Product{ProductId = 2, Name = "Product2", Kind="Kind2"},
                new Product{ProductId = 3, Name = "Product3", Kind="Kind1"},
                new Product{ProductId = 4, Name = "Product4", Kind="Kind3"},
                new Product{ProductId = 5, Name = "Product5", Kind="Kind2"}
            });

            NavController target = new NavController(mock.Object);

           string KindToSelect = "Kind2";


            string result = target.Menu(KindToSelect).ViewBag.SelectedKind;

            Assert.AreEqual(KindToSelect, result);
        }

        [TestMethod]
        public void Generete_Kind_Specific_Product_Count()
        {
           
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product{ProductId = 1, Name = "Product1", Kind="Kind1"},
                new Product{ProductId = 2, Name = "Product2", Kind="Kind2"},
                new Product{ProductId = 3, Name = "Product3", Kind="Kind1"},
                new Product{ProductId = 4, Name = "Product4", Kind="Kind3"},
                new Product{ProductId = 5, Name = "Product5", Kind="Kind2"}
            });

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            int res1 = ((ProductsListViewModel)controller.List("Kind1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel)controller.List("Kind2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel)controller.List("Kind3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }


    }
}
