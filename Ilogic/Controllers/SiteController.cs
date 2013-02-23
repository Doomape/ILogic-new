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
                MailMessage msg = md.CreateMailMessage("tomi_krama@hotmail.com", replacements, body, new System.Web.UI.Control());
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

       
        public JsonResult Sendform( string ime, string broj, string email, string poraka)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            if (( ime != "") && (email != "") && (poraka != ""))
            {
                string porakadoiLogic = "Name or Company: " + ime + "\n" + "Email: " + email + "\n" + "Number: " + broj + "\n" + "Message:" + "\n" + poraka;
                SendMail("Нова порака", porakadoiLogic);
                res.Add("msg", " Успешно испратена порака ");
            }
            else
            {
                res.Add("msg", " Неуспешно испратена порака ");
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}
