using Domain.Abstarct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class ProductController : Controller
    {
        
        private IProductRepository repository;
        public int pageSize = 4;
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string kind, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                .Where(p => kind == null || p.Kind == kind)
                .OrderBy(product => product.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = kind == null ?
                        repository.Products.Count() :
                        repository.Products.Where(product => product.Kind == kind).Count()
                },
                CurrentKind = kind
            };

            return View(model);
        }

    }
}