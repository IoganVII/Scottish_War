using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using Scottish_duel.Models;
using System.Web.Security;
using System.Collections.Specialized;

namespace Scottish_duel.Controllers
{
    public class AccountController : Controller
    {
        RegisterModelContext db = new RegisterModelContext();
        ActionPlayerContext Ap = new ActionPlayerContext();







        // GET: Account
        public ActionResult Register()
        {


            if (Request.Cookies["Login"] != null)
            {


                string str = Request.Cookies["Login"].Value;
                ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == str).FirstOrDefault();

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

        public ActionResult NewRegister()
        {
            return View();
        }


        //Вход пользователя
        [HttpPost]
        public ActionResult ClientRoom(RegisterModel model)
        {


            if (Request.Cookies["Login"] != null)
                return RedirectToAction("Register", "Account");

            foreach (var b in Ap.ActionPlayers)
            {
                if (b.Name == model.Login)
                    return RedirectToAction("Register", "Account");
            }

            foreach (RegisterModel b in db.RegisterModels)
            {
                //Добавляем нового пользователя в систему
                if (model.Login == b.Login && model.Password == b.Password)
                {

                    if (Request.Cookies[b.Login] != null)
                        return RedirectToAction("Register", "Account");

                    HttpCookie cookie = new HttpCookie("Login", b.Login);
                    cookie.Expires = DateTime.Now.AddHours(1);
                    Response.Cookies.Add(cookie);

                    ActionPlayer player = new ActionPlayer();

                    player.Name = model.Login;

                    Ap.ActionPlayers.Add(player);
                    Ap.SaveChanges();

                    ViewBag.Message = Ap.ActionPlayers;

                    return RedirectToAction("ClientRoom", "Play");
                }

            }
            return RedirectToAction("Register", "Account");
        }

        //Регистраиця нового пользователя
        public ActionResult NewRegisterList(RegisterModel model)
        {

            db.RegisterModels.Add(model);

            db.SaveChanges();

            return RedirectToAction("Register", "Account");
        }

    }
}