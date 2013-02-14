using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Ilogic.Controllers
{
    public class SiteController : Controller
    {
        //
        // GET: /Site/

        //
        // GET: /Site/
       
        
        public ActionResult Electronics()
        {
            return View();
        }
        public ActionResult Products()
        {
            return View();
        }
        public ActionResult Index_aspNet()
        {
            return View();
        }
        public ActionResult Index_php()
        {
            return View();
        }

        public static void SendMail(string subject, string body)
        {

            var fromAddress = new MailAddress("ilogicmk@gmail.com", "iLogic");
            string fromPassword = "trotinet";

            try
            {
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
                };

                System.Web.UI.WebControls.MailDefinition md = new System.Web.UI.WebControls.MailDefinition();
                md.From = "ilogicmk@gmail.com";
                md.IsBodyHtml = false;
                md.Subject = subject;
                System.Collections.Specialized.ListDictionary replacements = new System.Collections.Specialized.ListDictionary();
                MailMessage msg = md.CreateMailMessage("ilogicmk@gmail.com", replacements, body, new System.Web.UI.Control());
                smtp.Send(msg);
            }
            catch
            {

            }
        }

        public ActionResult PartialConsoleWindow()
        {
            return View("PartialConsoleWindow");
        }

        [HttpPost]
        public JsonResult SendForm(string company, string name_lastname, string phone, string email, string message, bool boolEmail)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            // string firmaC = firma+ime+tel+email+poraka;
            if ((company != "" && name_lastname != "" || company == "" && name_lastname != "" || company != "" && name_lastname == "") && (phone != "" || phone == "") && (email != "") && (message != "") && (boolEmail == true))
            {
                string porakadoBelina = "Фирма: " + company + "\n" + "Име и презиме: " + name_lastname + "\n" + "Телефонски број: " + phone + "\n" + "Електронска пошта: " + email + "\n" + "Порака:" + "\n" + message;
                SendMail("Нова порака", porakadoBelina);
                res.Add("msg", "Ok");
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                res.Add("msg", "NOk");
                return Json(res, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
