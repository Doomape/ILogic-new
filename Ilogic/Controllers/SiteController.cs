using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
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
        public ActionResult Software()
        {
            return View();
        }
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
        public ActionResult Asp()
        {
            return View();
        }
        public ActionResult Php()
        {
            return View();
        }
        public ActionResult AboutUs()
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
        public JsonResult getProducts()
        {
            string allInfo = "";
            OleDbConnection conn1 = null;
            conn1 = new OleDbConnection(
            "Provider=Microsoft.Jet.OLEDB.4.0; " +
            "Data Source=" + System.Web.HttpContext.Current.Server.MapPath("~") + ("App_Data\\Products.mdb"));
            conn1.Open();
            OleDbDataReader dbReader1 = null;
            OleDbCommand cmd1 = conn1.CreateCommand();
            string allProductInfo = "SELECT * from Products order by id asc;";
            cmd1.CommandText = allProductInfo;
            dbReader1 = cmd1.ExecuteReader();
            List<string> list = new List<string>();
            while (dbReader1.Read())
            {
                allInfo = "@" + (string)dbReader1.GetValue(1) + "@" + 
                    (string)dbReader1.GetValue(2) + "@" + (string)dbReader1.GetValue(3) + "@" +
                    (string)dbReader1.GetValue(4) + "@" + (string)dbReader1.GetValue(5) + "@" + (string)dbReader1.GetValue(6);
                list.Add(allInfo);
                allInfo = "";
            }
            dbReader1.Close();
            list.Insert(0, list.Count.ToString());
            conn1.Close();

         
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult changeProduct(string productName)
        {
            string allInfo = "";
            OleDbConnection conn1 = null;
            conn1 = new OleDbConnection(
            "Provider=Microsoft.Jet.OLEDB.4.0; " +
            "Data Source=" + System.Web.HttpContext.Current.Server.MapPath("~") + ("App_Data\\Products.mdb"));
            conn1.Open();
            OleDbDataReader dbReader1 = null;
            OleDbCommand cmd1 = conn1.CreateCommand();
            string allProductInfo = "SELECT * from Products where Products.product_name='" + productName + "';";
            cmd1.CommandText = allProductInfo;
            dbReader1 = cmd1.ExecuteReader();
            List<string> list = new List<string>();
            while (dbReader1.Read())
            {
                allInfo = "@" + (string)dbReader1.GetValue(1) + "@" + (string)dbReader1.GetValue(2) + "@" + 
                    (string)dbReader1.GetValue(3) + "@" + (string)dbReader1.GetValue(4) + "@" +
                    (string)dbReader1.GetValue(5) + "@" + (string)dbReader1.GetValue(6);
                list.Add(allInfo);
                allInfo = "";
            }
            dbReader1.Close();
            list.Reverse();
            list.Insert(0, list.Count.ToString());
            conn1.Close();


            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getProjects()
        {
            Dictionary<string, List<string>> res = new Dictionary<string, List<string>>();
            OleDbConnection conn1 = null;
            conn1 = new OleDbConnection(
            "Provider=Microsoft.Jet.OLEDB.4.0; " +
            "Data Source=" + System.Web.HttpContext.Current.Server.MapPath("~") + ("App_Data\\Products.mdb"));
            conn1.Open();
            OleDbDataReader dbReader1 = null;
            OleDbCommand cmd1 = conn1.CreateCommand();
            string allProductInfo = "SELECT * from Projects order by id asc;";
            cmd1.CommandText = allProductInfo;
            dbReader1 = cmd1.ExecuteReader();
            string root_path = System.Web.HttpContext.Current.Server.MapPath("~");

            List<string> list_id = new List<string>();
            List<string> list_imgsrc = new List<string>();
            List<string> screen_counter = new List<string>();
            List<string> logos = new List<string>();
            List<string> description = new List<string>();
            List<string> thumb_description = new List<string>();
            string[] extensions = new[] { ".jpg", ".tiff", ".bmp" , ".png" };

            while (dbReader1.Read())
            {
                int counter = 0;
                string project_id = dbReader1.GetValue(0).ToString();
                list_id.Add(project_id);

                DirectoryInfo screenshotsdir = new DirectoryInfo(root_path + ("\\Content\\images\\projects\\" + project_id + "\\thumbs"));
                DirectoryInfo logodir = new DirectoryInfo(root_path+("\\Content\\images\\projects\\" + project_id + "\\logo"));
                FileInfo[] logo = logodir.EnumerateFiles()
         .Where(f => extensions.Contains(f.Extension.ToLower()))
         .ToArray();
                FileInfo[] screenshots = screenshotsdir.EnumerateFiles()
         .Where(f => extensions.Contains(f.Extension.ToLower()))
         .ToArray();

                logos.Add(logo[0].FullName.Replace(root_path, "").Insert(0,@"\"));
                foreach (var screen in screenshots)
                {
                    list_imgsrc.Add(screen.FullName.Replace(root_path, "").Insert(0, @"\"));
                    counter++;
                }
                screen_counter.Add(counter.ToString());
                description.Add(dbReader1.GetValue(2).ToString());
                thumb_description.Add(dbReader1.GetValue(3).ToString());
            }
            
            dbReader1.Close();
            conn1.Close();
            res.Add("id",list_id);
            res.Add("img", list_imgsrc);
            res.Add("screen_counter", screen_counter);
            res.Add("logos", logos);
            res.Add("desc", description);
            res.Add("thumb_desc", thumb_description);

            return Json(res,JsonRequestBehavior.AllowGet);
        }



        public JsonResult getSoftware()
        {
            Dictionary<string, List<string>> res = new Dictionary<string, List<string>>();
            OleDbConnection conn1 = null;
            conn1 = new OleDbConnection(
            "Provider=Microsoft.Jet.OLEDB.4.0; " +
            "Data Source=" + System.Web.HttpContext.Current.Server.MapPath("~") + ("App_Data\\Products.mdb"));
            conn1.Open();
            OleDbDataReader dbReader1 = null;
            OleDbCommand cmd1 = conn1.CreateCommand();
            string allProductInfo = "SELECT * from Software order by id asc;";
            cmd1.CommandText = allProductInfo;
            dbReader1 = cmd1.ExecuteReader();
            string root_path = System.Web.HttpContext.Current.Server.MapPath("~");

            List<string> list_id = new List<string>();
            List<string> list_imgsrc = new List<string>();
            List<string> screen_counter = new List<string>();
            List<string> logos = new List<string>();
            List<string> description = new List<string>();
            List<string> thumb_description = new List<string>();
            List<string> software_name = new List<string>();
            string[] extensions = new[] { ".jpg", ".tiff", ".bmp", ".png" };

            while (dbReader1.Read())
            {
                int counter = 0;
                string project_id = dbReader1.GetValue(0).ToString();
                list_id.Add(project_id);
                software_name.Add(dbReader1.GetValue(2).ToString());
                DirectoryInfo screenshotsdir = new DirectoryInfo(root_path + ("\\Content\\images\\software\\" + project_id + "\\thumbs"));
                DirectoryInfo logodir = new DirectoryInfo(root_path + ("\\Content\\images\\software\\" + project_id + "\\logo"));
                FileInfo[] logo = logodir.EnumerateFiles()
         .Where(f => extensions.Contains(f.Extension.ToLower()))
         .ToArray();
                FileInfo[] screenshots = screenshotsdir.EnumerateFiles()
         .Where(f => extensions.Contains(f.Extension.ToLower()))
         .ToArray();

                logos.Add(logo[0].FullName.Replace(root_path, "").Insert(0, @"\"));
                foreach (var screen in screenshots)
                {
                    list_imgsrc.Add(screen.FullName.Replace(root_path, "").Insert(0, @"\"));
                    counter++;
                }
                screen_counter.Add(counter.ToString());
                description.Add(dbReader1.GetValue(3).ToString());
                thumb_description.Add(dbReader1.GetValue(1).ToString());
            }

            dbReader1.Close();
            conn1.Close();
            res.Add("id", list_id);
            res.Add("img", list_imgsrc);
            res.Add("screen_counter", screen_counter);
            res.Add("logos", logos);
            res.Add("desc", description);
            res.Add("thumb_desc", thumb_description);
            res.Add("software_name", software_name);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}
