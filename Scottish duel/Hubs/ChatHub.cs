using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Scottish_duel.Models;




namespace Scottish_duel.Hubs
{



    public class ChatHub : Hub
    {

        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

        ActionPlayerContext Ap = new ActionPlayerContext();
        ClientRoomModelContext Rb = new ClientRoomModelContext();


        //Отправка в чат
        public void send(string name, string message)
        {
            Clients.Group("1").broadcastMessage(name, message);
        }


        //Отобразить комнату
        public void viewRoomGroup(string user, string nameroom, string password)
        {
            ClientRoomModel model = new ClientRoomModel();

            var currentUser = user;



            model.name = nameroom;
            model.password = password;
            model.nameFirstPlayer = currentUser;
            model.nameSecondPlayer = "";
            model.nameGod = currentUser;
            model.numberPlayer = 1;
            Rb.ClientRoomModels.Add(model);
            Rb.SaveChanges();

            ActionPlayer player = Ap.ActionPlayers.Where(o => o.Name == currentUser).FirstOrDefault();
            player.idRoom = model.id;
            Ap.SaveChanges();
            Clients.Group("WaitPlayer").UpDateTableRoom();
            Clients.Caller.upDateRoom();

        }


        //присоединиться к комнате
        public void joinRoomGroup(string user, string idroom)
        {

            var currentUser = user;

            ActionPlayer player = Ap.ActionPlayers.Where(o => o.Name == currentUser).FirstOrDefault();
            player.idRoom = Int32.Parse(idroom);
            Ap.SaveChanges();
            ClientRoomModel model = Rb.ClientRoomModels.Where(o => o.id == player.idRoom).FirstOrDefault();
            model.nameSecondPlayer = player.Name;
            model.numberPlayer += 1;
            Rb.SaveChanges();
            Clients.Caller.upDateRoom();
            Clients.Group(idroom).upDateRoom();

        }

        //Группа игроков при выборе комнаты
        public void waitPlayer(string group)
        {
            Groups.Add(Context.ConnectionId, group);
        }

        //Группа игроков в комнате
        public void groupPlayerInRoom(string group)
        {
            Groups.Add(Context.ConnectionId, group);
        }

        public void startgame(string Login, int flag)
        {
            ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == Login).FirstOrDefault();
            var idRoom = Player.idRoom;
            if (flag == 0)
            {
                Clients.Group(idRoom.ToString()).startGame(idRoom);
                
            }
            if (flag == 1)
            {
                Groups.Add(Context.ConnectionId, idRoom.ToString());
            }
        }

    }
}