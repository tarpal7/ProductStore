using Domain.Abstarct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
   public class NavController : Controller
   {
           private IProductRepository repository;

            public NavController(IProductRepository repo)
            {
                repository = repo;
            }

            public PartialViewResult Menu(string kind = null)
            {
                ViewBag.SelectedKind = kind;

                IEnumerable<string> kinds = repository.Products
                    .Select(product => product.Kind)
                    .Distinct()
                    .OrderBy(x => x);

                return PartialView(kinds);
            }
   }

   
}