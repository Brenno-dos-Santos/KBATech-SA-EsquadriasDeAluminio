using Microsoft.AspNetCore.Mvc;

namespace SAEsquadriasDeAluminioApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
