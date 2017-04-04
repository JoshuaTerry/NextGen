using System.Web.Mvc;

namespace DDI.WebApi.Controllers.General
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
