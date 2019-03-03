using Scottish_duel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Scottish_duel.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                FormsAuthentication.SignOut();

            return View();
        }

        public ActionResult Play()
        {
            return View();
        }

        public ActionResult Contact()


        {
            ViewBag.Message = "Контакты владельца:";
            RegisterModelContext db = new RegisterModelContext();
            ViewBag.Base = db.RegisterModels;
            string ip = HttpContext.Request.UserHostAddress;
            ViewBag.ip = ip;
            string referrer = HttpContext.Request.UrlReferrer == null ? "" : HttpContext.Request.UrlReferrer.AbsoluteUri;
            ViewBag.referrer = referrer;
            string user_agent = HttpContext.Request.UserAgent;
            ViewBag.agent = user_agent;


            Session["userName"] = Request.Form["userName"];
            bool isSessionNew = Session.IsNewSession;
            string sessionId = Session.SessionID;

            return View();
        }
    }


}