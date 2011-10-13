using System.Web.Mvc;

namespace BasicBlog.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
