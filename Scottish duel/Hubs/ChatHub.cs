using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == name).FirstOrDefault();
            if (Player.idRoom == 0)
                Clients.Group("WaitPlayer").broadcastMessage(name, message);
            else
                Clients.Group(Player.idRoom.ToString()).broadcastMessage(name, message);
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
            ClientRoomModel model = Rb.ClientRoomModels.Where(o => o.id == Player.idRoom).FirstOrDefault();

            if ((flag == 0) && (model.nameGod == Player.Name) && (model.numberPlayer == 2))
            {
                Clients.Group(idRoom.ToString()).startGame(idRoom);

            }
            //Если начало игры
            if (flag == 1)
            {
                string[] namescard = new string[8];
                namescard[0] = "Музыкант";
                namescard[1] = "Принцесса";
                namescard[2] = "Шпион";
                namescard[3] = "Убийца";
                namescard[4] = "Посол";
                namescard[5] = "Волшебник";
                namescard[6] = "Генерал";
                namescard[7] = "Принц";
                if (model.nameGod == Player.Name)
                {
                    List<CardModel> listcard = new List<CardModel>();
                    for (int i = 0; i < 8; i++)
                    {
                        CardModel Card = new CardModel();
                        Card.number = i;
                        Card.name = namescard[i];
                        Card.strength = i;
                        listcard.Add(Card);
                    }
                    Player.deckCard = listcard;
                    Player.ColorTeam = "С";
                    Ap.SaveChanges();
                }
                else
                {
                    List<CardModel> listcard = new List<CardModel>();
                    for (int i = 0; i < 8; i++)
                    {
                        CardModel Card = new CardModel();
                        Card.number = i;
                        Card.name = namescard[i];
                        Card.strength = i;
                        listcard.Add(Card);
                    }
                    Player.deckCard = listcard;


                    Player.ColorTeam = "K";
                    Ap.SaveChanges();
                }
                Groups.Add(Context.ConnectionId, idRoom.ToString());
                Clients.Caller.generatedTeam(Player.ColorTeam);
            }
        }

        public void inputCard(string cardId, string Login)
        {
            ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == Login).FirstOrDefault();
            var idRoom = Player.idRoom;
            Clients.OthersInGroup(idRoom.ToString()).enemyCard(cardId, Player.ColorTeam);
        }

    }
}