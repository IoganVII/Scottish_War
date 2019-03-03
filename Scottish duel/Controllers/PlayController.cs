using Scottish_duel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scottish_duel.Controllers
{
    public class PlayController : Controller
    {

        RegisterModelContext db = new RegisterModelContext();

        // GET: Play
   //     [Authorize]
        public ActionResult ClientRoom()
        {
            var userName = User.Identity.Name;

            foreach (RegisterModel b in db.RegisterModels)
            {

                if (userName == b.Login)
                {
                    ViewBag.id = b.id;
                }
            }


            return View();
        }
    }
}