using Scottish_duel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Scottish_duel.Controllers
{
    public class HomeController : Controller
    {

        RegisterModelContext db = new RegisterModelContext();
        ActionPlayerContext Ap = new ActionPlayerContext();


        public ActionResult Index()
        {


            if (Request.Cookies["Login"] != null)
            {
                string str = Request.Cookies["Login"].Value;
                ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == str).FirstOrDefault();
                if (Player.outPlauer == false)
                    return RedirectToAction("ClientRoom", "Play");

                

                Request.Cookies["Login"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(Request.Cookies["Login"]);
                
                if (Player != null)
                {
                    Ap.ActionPlayers.Remove(Player);
                    Ap.SaveChanges();
                }

                

            }


            return View();
        }

        public ActionResult Play()
        {

            if (Request.Cookies["Login"] != null)
            {


                string str = Request.Cookies["Login"].Value;
                ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == str).FirstOrDefault();
                if (Player.outPlauer == false)
                    return RedirectToAction("ClientRoom", "Play");

                if (Player != null)
                {
                    Ap.ActionPlayers.Remove(Player);
                    Ap.SaveChanges();
                }

                Request.Cookies["Login"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(Request.Cookies["Login"]);
            }

            return View();
        }

        public ActionResult Contact()


        {

            if (Request.Cookies["Login"] != null)
            {


                string str = Request.Cookies["Login"].Value;
                ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == str).FirstOrDefault();
                if (Player.outPlauer == false)
                    return RedirectToAction("ClientRoom", "Play");

                if (Player != null)
                {
                    Ap.ActionPlayers.Remove(Player);
                    Ap.SaveChanges();
                }

                Request.Cookies["Login"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(Request.Cookies["Login"]);
            }

            ViewBag.Message = "Контакты владельца:";
            ViewBag.Base = db.RegisterModels;
            string ip = HttpContext.Request.UserHostAddress;
            ViewBag.ip = ip;
            string referrer = HttpContext.Request.UrlReferrer == null ? "" : HttpContext.Request.UrlReferrer.AbsoluteUri;
            ViewBag.referrer = referrer;
            string user_agent = HttpContext.Request.UserAgent;
            ViewBag.agent = user_agent;

            ViewBag.Ap = Ap.ActionPlayers;

            return View();
        }
    }


}