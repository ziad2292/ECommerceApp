using ECommerceApp.Controllers._Common;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{

    public class ShoppingCartController : CustomControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
