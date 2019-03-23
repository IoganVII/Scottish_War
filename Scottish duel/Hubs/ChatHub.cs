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
        CardModelContext Cb = new CardModelContext();


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
            int integerIdRoom = Int32.Parse(idroom);
            ClientRoomModel model = Rb.ClientRoomModels.Where(o => o.id == integerIdRoom).FirstOrDefault();
            if (model.numberPlayer < 2)
            {
                player.idRoom = Int32.Parse(idroom);
                Ap.SaveChanges();
                model.nameSecondPlayer = player.Name;
                model.numberPlayer += 1;
                Rb.SaveChanges();
                Clients.OthersInGroup("WaitPlayer").UpDateTableRoom();
                Clients.Caller.upDateRoom();
                Clients.Group(idroom).upDateRoom();
            }

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
                model.firstPLayerActiveCard = false;
                model.secondPLayerActiveCard = false;
                model.vPointFerstPlayer = 0;
                model.vPointSecondPlayer = 0;
                model.numberRound = 0;
                Rb.SaveChanges();
                Clients.Group(idRoom.ToString()).startGame(idRoom);

            }
            //Если начало игры
            if (flag == 1)
            {
                if (model.nameGod == Player.Name)
                {
                    Player.ColorTeam = "С";
                    Ap.SaveChanges();
                }
                else
                {
                    Player.ColorTeam = "K";
                    Ap.SaveChanges();
                }
                Groups.Add(Context.ConnectionId, idRoom.ToString());
                Clients.Caller.generatedTeam(Player.ColorTeam);
            }
        }

        //Описание битвы
        public void battleround(string cardId, string Login)
        {
            ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == Login).FirstOrDefault();
            var idRoom = Player.idRoom;
            ClientRoomModel model = Rb.ClientRoomModels.Where(o => o.id == Player.idRoom).FirstOrDefault();
            //Если разыграын обе карты
            if ((model.firstPLayerActiveCard == true) && (model.secondPLayerActiveCard == true))
            {

                int[,] rule = new int[8, 8] {
            {0,1,1,1,1,1,2,0 },
            {2,0,2,1,2,1,1,0 },
            {2,2,0,1,1,1,1,1 },
            {2,2,2,0,2,1,1,0 },
            {2,1,2,1,0,2,2,0 },
            {2,2,2,2,1,0,1,0 },
            {1,2,2,2,1,2,0,0 },
            {0,0,2,0,0,0,0,0 }
        };

                CardModel card1 = Cb.CardModels.Where(o => o.number == model.idFirstPlayerCad).FirstOrDefault();
                CardModel card2 = Cb.CardModels.Where(o => o.number == model.idSecondPlayerCad).FirstOrDefault();

                var stregth1 = card1.strength;
                var stregth2 = card2.strength;

                int win = rule[7 - stregth2, 7 - stregth1];
                switch (win)
                {
                    case 0:
                        Clients.Group(idRoom.ToString()).resultbattle("Раунд отложен");
                        break;
                    case 1:
                        Clients.Group(idRoom.ToString()).resultbattle("Раунд Победа красных");
                        model.vPointSecondPlayer++;
                        break;
                    case 2:
                        Clients.Group(idRoom.ToString()).resultbattle("Раунд Победа синих");
                        model.vPointFerstPlayer++;
                        break;
                }

                model.numberRound++;
                model.firstPLayerActiveCard = false;
                model.secondPLayerActiveCard = false;
                Rb.SaveChanges();

                if (model.numberRound == 8)
                {
                    if (model.vPointFerstPlayer > model.vPointSecondPlayer)
                        Clients.Group(idRoom.ToString()).resultbattle("Конец игры: Победа синих");
                    if (model.vPointFerstPlayer < model.vPointSecondPlayer)
                        Clients.Group(idRoom.ToString()).resultbattle("Конец игры: Победа красных");
                    if (model.vPointFerstPlayer == model.vPointSecondPlayer)
                        Clients.Group(idRoom.ToString()).resultbattle("Конец игры: ничья");
                    Clients.Group(idRoom.ToString()).upDateRoom();
                }

            }
        }

        public void inputCard(string cardId, string Login)
        {
            ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == Login).FirstOrDefault();
            var idRoom = Player.idRoom;
            ClientRoomModel model = Rb.ClientRoomModels.Where(o => o.id == Player.idRoom).FirstOrDefault();
            if (model.nameGod == Player.Name)
            {
                model.firstPLayerActiveCard = true;
                model.idFirstPlayerCad = Int32.Parse(cardId);
                Clients.Caller.getboolcard(model.firstPLayerActiveCard);
            }
            else
            {
                model.secondPLayerActiveCard = true;
                model.idSecondPlayerCad = Int32.Parse(cardId);
                Clients.Caller.getboolcard(model.secondPLayerActiveCard);
            }
            Rb.SaveChanges();
            Clients.OthersInGroup(idRoom.ToString()).enemyCard(cardId, Player.ColorTeam);

            battleround(cardId, Login);
        }

        public void getActiveMomentCard(string Login)
        {
            ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == Login).FirstOrDefault();
            var idRoom = Player.idRoom;
            ClientRoomModel model = Rb.ClientRoomModels.Where(o => o.id == Player.idRoom).FirstOrDefault();
            if (model.nameGod == Player.Name)
            {
                Clients.Caller.getMoment(model.firstPLayerActiveCard);
            }
            else
            {
                Clients.Caller.getMoment(model.secondPLayerActiveCard);
            }
        }

        public void backInTableRoom(string Login)
        {
            ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == Login).FirstOrDefault();

            if (Player.idRoom != 0)
            {
                ClientRoomModel roomModel = Rb.ClientRoomModels.Where(o => o.id == Player.idRoom).FirstOrDefault();
                if (roomModel.nameGod == Player.Name)
                {
                    Rb.ClientRoomModels.Remove(roomModel);
                    Rb.SaveChanges();
                    Clients.Group("WaitPlayer").upDateTableRoom();
                    Clients.Group(Player.idRoom.ToString()).upDateTableRoom();
                    Player.idRoom = 0;
                    if (roomModel.nameSecondPlayer != "")
                    {
                        ActionPlayer SecondPlayer = Ap.ActionPlayers.Where(o => o.Name == roomModel.nameSecondPlayer).FirstOrDefault();
                        SecondPlayer.idRoom = 0;
                    }
                    Ap.SaveChanges();
                    
                }
                else
                {
                    roomModel.nameSecondPlayer = "";
                    roomModel.numberPlayer--;
                    Rb.SaveChanges();
                    Clients.Group("WaitPlayer").upDateTableRoom();
                    Clients.OthersInGroup(Player.idRoom.ToString()).upDateRoom();
                    
                    Player.idRoom = 0;
                    Ap.SaveChanges();
                    Clients.Caller.upDateTableRoom();
                }
            }
        }

        public void outPlayer(string Login)
        {
            ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == Login).FirstOrDefault();
            Player.outPlauer = true;
            Ap.SaveChanges();
        }

        public void playerClosWindow(string Login)
        {
            ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == Login).FirstOrDefault();
            Ap.ActionPlayers.Remove(Player);
            Ap.SaveChanges();
        }



    }
}