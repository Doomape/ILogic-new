using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace I_Logic.Controllers
{
    public class SiteController : Controller
    {
        //
        // GET: /Site/
        Ilogic db = new Ilogic();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetData()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            var proekti = (from x in db.PROEKTIs where x.Ime == "babba" select x.Ime).FirstOrDefault();
            res.Add("ime_proekti", proekti);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}
