using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using Scottish_duel.Models;

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

        public ActionResult See ()
        {
            return View(db.RegisterModels);
        }


    }
}