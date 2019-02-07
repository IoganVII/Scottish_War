using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scottish_duel.Controllers
{
    public class PlayController : Controller
    {
        // GET: Play
        public ActionResult ClientRoom()
        {
            return View();
        }
    }
}