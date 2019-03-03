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

namespace Scottish_duel.Controllers
{
    public class AccountController : Controller
    {
        RegisterModelContext db = new RegisterModelContext();

        // GET: Account
        public ActionResult Register()
        {
            

            return View();
        }

        public ActionResult NewRegister()
        {
            return View();
        }


        //Вход пользователя
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClientRoom(RegisterModel model)
        {

            

            foreach (RegisterModel b in db.RegisterModels) {

                if (model.Login == b.Login && model.Password == b.Password)
                {
               //     FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("ClientRoom", "Play");
                }
            }

            return RedirectToAction("Register","Account");

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