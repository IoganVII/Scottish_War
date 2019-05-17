using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;
using Scottish_duel.Hubs;
using Scottish_duel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace Scottish_duel.Controllers
{


    public class PlayController : Controller
    {

        public string currentUser = "";

        RegisterModelContext db = new RegisterModelContext();
        ActionPlayerContext Ap = new ActionPlayerContext();
        ClientRoomModelContext Rb = new ClientRoomModelContext();

        public ActionResult ClientRoom(ClientRoomModel model)
        {

            ViewBag.name = "";
            model.nameFirstPlayer = "";
            model.nameSecondPlayer = "";


            if (HttpContext.Request.UrlReferrer == null)
                return RedirectToAction("Register", "Account");

            //Проверка авторизации
            if (Request.Cookies["Login"] == null)
                return RedirectToAction("Register", "Account");

            currentUser = Request.Cookies["Login"].Value;


            if (Request.Cookies["Login"] != null)
            {
                ViewBag.name = Request.Cookies["Login"].Value;
            }


            




            ViewBag.RoomBase = Rb.ClientRoomModels;

            return View(model);
        }

        // [HttpPost]
        public ActionResult CreateRoom()
        {
            ViewBag.name = "";

            //Проверка авторизации
            if (Request.Cookies["Login"] == null)
                return RedirectToAction("Register", "Account");

            if (Request.Cookies["Login"] != null)
                ViewBag.name = Request.Cookies["Login"].Value;


            return View();
        }


        [HttpPost]
        public void JoinRoom(string idroom)
        {



        }

        public ActionResult CreatedRoom()
        {
            //Проверка авторизации
            if (Request.Cookies["Login"] == null)
                return RedirectToAction("Register", "Account");

            currentUser = Request.Cookies["Login"].Value;

            ActionPlayer player = Ap.ActionPlayers.Where(o => o.Name == currentUser).FirstOrDefault();

            ClientRoomModel model = Rb.ClientRoomModels.Where(o => o.id == player.idRoom).FirstOrDefault();
            return View(model);
        }


        public ActionResult ViewRoom()
        {




            return RedirectToAction("CreatedRoom", "Play");
        }

        public ActionResult Game()
        {
            return View();
        }

        public ActionResult BotGame()
        {
            return View();
        }

    }

}