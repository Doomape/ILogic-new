using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace Ilogic.Controllers
{
    public class SiteController : Controller
    {
        //
        // GET: /Site/

        //
        // GET: /Site/

        public ActionResult Projects()
        {
            return View();
        }
        
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
        public ActionResult About_Us()
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
        public JsonResult getProducts(string id)
        {
            string allInfo = "";
            OleDbConnection conn1 = null;
            conn1 = new OleDbConnection(
            "Provider=Microsoft.Jet.OLEDB.4.0; " +
            "Data Source=" + System.Web.HttpContext.Current.Server.MapPath("~") + ("App_Data\\Products.mdb"));
            conn1.Open();
            OleDbDataReader dbReader1 = null;
            OleDbCommand cmd1 = conn1.CreateCommand();
            string allProductInfo = "SELECT * from Products where Products.id=1;";
            cmd1.CommandText = allProductInfo;
            dbReader1 = cmd1.ExecuteReader();
            List<string> list = new List<string>();
            while (dbReader1.Read())
            {
                allInfo = "@" + (string)dbReader1.GetValue(1) + "@" + (string)dbReader1.GetValue(2) + "@" + (string)dbReader1.GetValue(3) + "@" + (string)dbReader1.GetValue(4) + "@" + (string)dbReader1.GetValue(5);
                list.Add(allInfo);
                allInfo = "";
            }
            dbReader1.Close();
            list.Insert(0, list.Count.ToString());
            conn1.Close();

         
            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}
