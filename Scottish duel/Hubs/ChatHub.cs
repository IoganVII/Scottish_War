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


        //Определить победителя
        int Winner(CardModel card1, CardModel card2, ClientRoomModel model)
        {

            bool win1 = false;
            bool win2 = false;
            //Матрица правил
            int[,] rule = new int[8, 8] {
            {0,1,1,1,1,1,2,0 },
            {2,0,1,1,2,1,1,0 },
            {2,2,0,1,1,1,1,1 },
            {2,2,2,0,2,1,1,0 },
            {2,1,2,1,0,2,2,0 },
            {2,2,2,2,1,0,1,0 },
            {1,2,2,2,1,2,0,0 },
            {0,0,2,0,0,0,0,0 }
        };

            var stregth1 = card1.strength;
            var stregth2 = card2.strength;

            //Если в раунде побеждает принцесса.
            if (card1.id == 2 || card2.id == 2)
            {
                if (card1.id == 2 && card2.id == 8)
                {
                    return 3;
                }

                if (card2.id == 2 && card1.id == 8)
                {
                    return 4;
                }
            }

            //Если кто-то сыграл посла, активация свойства
            if (card1.number == 4 || card2.number == 4)
            {
                if (card1.number == 4)
                {
                    model.P1bonusLegate = 2;
                }
                if (card2.number == 4)
                {
                    model.P2bonusLegate = 2;
                }
            }

            //Активация навыка шпиона
            if (card1.number == 2 || card2.number == 2)
            {
                if (card1.number == 2 && card2.number != 5)
                    model.P1bonusSpook = 1;
                if (card2.number == 2 && card1.number != 5)
                    model.P2bonusSpook = 1;
                if (card2.number == 2 && card1.number == 2)
                {
                    model.P1bonusSpook = 0;
                    model.P2bonusSpook = 0;
                }

            }





            //Просчёт раунда с учётом свойства генерала
            if (model.P1bonusGeneral != 0 || model.P2bonusGeneral != 0)
            {
                if (model.P1bonusGeneral != 0)
                {
                    stregth1 += model.P1bonusGeneral;
                    model.P1bonusGeneral = 0;
                }
                if (model.P2bonusGeneral != 0)
                {
                    stregth2 += model.P2bonusGeneral;
                    model.P2bonusGeneral = 0;
                }

                if (card1.number != 0 && card2.number != 0)

                {
                    if (stregth1 > stregth2)
                    {
                        if (card2.number == 3)
                        {
                            if (model.P2bonusLegate != 0)
                            {
                                model.vPointSecondPlayer += model.P2bonusLegate;
                                model.P2bonusLegate = 0;
                            }
                            else
                                model.vPointSecondPlayer++;

                            //Отменяем навык вражеского посла
                            if (model.P1bonusLegate != 0)
                                model.P1bonusLegate = 0;

                            if (model.delayedRound != 0)
                            {
                                model.vPointSecondPlayer = model.vPointSecondPlayer + model.delayedRound;
                                model.delayedRound = 0;
                            }
                            win2 = true;
                        }
                        else
                        {
                            if (model.P1bonusLegate != 0)
                            {
                                model.vPointFerstPlayer += model.P1bonusLegate;
                                model.P1bonusLegate = 0;
                            }
                            else
                                model.vPointFerstPlayer++;

                            //Отменяем навык вражеского посла
                            if (model.P2bonusLegate != 0)
                                model.P2bonusLegate = 0;

                            if (model.delayedRound != 0)
                            {
                                model.vPointFerstPlayer = model.vPointFerstPlayer + model.delayedRound;
                                model.delayedRound = 0;
                            }
                            win1 = true;
                        }
                    }
                    if (stregth2 > stregth1)
                    {
                        if (card1.number == 3)
                        {
                            if (model.P1bonusLegate != 0)
                            {
                                model.vPointFerstPlayer += model.P1bonusLegate;
                                model.P1bonusLegate = 0;
                            }
                            else
                                model.vPointFerstPlayer++;

                            //Отменяем навык вражеского посла
                            if (model.P2bonusLegate != 0)
                                model.P2bonusLegate = 0;

                            if (model.delayedRound != 0)
                            {
                                model.vPointFerstPlayer = model.vPointFerstPlayer + model.delayedRound;
                                model.delayedRound = 0;
                            }
                            win1 = true;
                        }
                        else
                        {
                            if (model.P2bonusLegate != 0)
                            {
                                model.vPointSecondPlayer += model.P2bonusLegate;
                                model.P2bonusLegate = 0;
                            }
                            else
                                model.vPointSecondPlayer++;

                            //Отменяем навык вражеского посла
                            if (model.P1bonusLegate != 0)
                                model.P1bonusLegate = 0;

                            if (model.delayedRound != 0)
                            {
                                model.vPointSecondPlayer = model.vPointSecondPlayer + model.delayedRound;
                                model.delayedRound = 0;
                            }
                            win2 = true;
                        }
                    }

                    if (stregth1 == stregth2)
                        model.delayedRound++;
                }
                else
                    model.delayedRound++;


            }
            //Обычный просчёт игры
            else
            {
                int win = rule[7 - stregth2, 7 - stregth1];
                switch (win)
                {
                    case 0:
                        model.delayedRound++;
                        break;
                    case 1:
                        if (model.P2bonusLegate != 0)
                        {
                            model.vPointSecondPlayer += model.P2bonusLegate;
                            model.P2bonusLegate = 0;
                        }
                        else
                            model.vPointSecondPlayer++;

                        //Отменяем навык вражеского посла
                        if (model.P1bonusLegate != 0)
                            model.P1bonusLegate = 0;

                        if (model.delayedRound != 0)
                        {
                            model.vPointSecondPlayer = model.vPointSecondPlayer + model.delayedRound;
                            model.delayedRound = 0;
                        }
                        win2 = true;
                        break;
                    case 2:
                        if (model.P1bonusLegate != 0)
                        {
                            model.vPointFerstPlayer += model.P1bonusLegate;
                            model.P1bonusLegate = 0;
                        }
                        else
                            model.vPointFerstPlayer++;

                        //Отменяем навык вражеского посла
                        if (model.P2bonusLegate != 0)
                            model.P2bonusLegate = 0;

                        if (model.delayedRound != 0)
                        {
                            model.vPointFerstPlayer = model.vPointFerstPlayer + model.delayedRound;
                            model.delayedRound = 0;
                        }
                        win1 = true;
                        break;
                }

            }

            //Если кто-то сыграл Генерала, активация свойства
            if (card1.number == 6 || card2.number == 6)
            {
                if (card1.number == 6 && card2.number != 5)
                {
                    model.P1bonusGeneral = 2;
                }

                if (card2.number == 6 && card1.number != 5)
                {
                    model.P2bonusGeneral = 2;
                }

            }


            model.numberRound++;
            model.firstPLayerActiveCard = false;
            model.secondPLayerActiveCard = false;
            Rb.SaveChanges();


            if (model.numberRound == 8)
            {
                if (model.vPointFerstPlayer > model.vPointSecondPlayer)
                    return 3;
                if (model.vPointFerstPlayer < model.vPointSecondPlayer)
                    return 4;
                if (model.vPointFerstPlayer == model.vPointSecondPlayer)
                    return 5;
            }

            if (win1 == true)
                return 1;
            if (win2 == true)
                return 2;
            if (win1 == false && win2 == false)
                return 0;
            return 0;
        }


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
        public void waitPlayer(string group, string Login)
        {
            Groups.Add(Context.ConnectionId, group);
            ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == Login).FirstOrDefault();
            if (Player.idRoom != 0)
            {
                backInTableRoom(Login);
            }

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
                model.delayedRound = 0;
                model.P1bonusLegate = 0;
                model.P2bonusLegate = 0;
                model.P1bonusGeneral = 0;
                model.P2bonusGeneral = 0;
                model.vPointFerstPlayer = 0;
                model.vPointSecondPlayer = 0;
                model.numberRound = 0;
                model.P1bonusSpook = 0;
                model.P2bonusSpook = 0;
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

                CardModel card1 = Cb.CardModels.Where(o => o.number == model.idFirstPlayerCad).FirstOrDefault();
                CardModel card2 = Cb.CardModels.Where(o => o.number == model.idSecondPlayerCad).FirstOrDefault();

                var win = Winner(card1, card2, model);

                switch (win)
                {
                    case 0:
                        Clients.Group(idRoom.ToString()).resultbattle("Раунд отложен", model.vPointFerstPlayer, model.vPointSecondPlayer);
                        break;
                    case 1:
                        Clients.Group(idRoom.ToString()).resultbattle("Раунд Победа синих", model.vPointFerstPlayer, model.vPointSecondPlayer);
                        break;
                    case 2:
                        Clients.Group(idRoom.ToString()).resultbattle("Раунд Победа красных", model.vPointFerstPlayer, model.vPointSecondPlayer);
                        break;
                    case 3:
                        Clients.Group(idRoom.ToString()).resultbattle("Конец игры: Победа синих");
                        Clients.Group(idRoom.ToString()).upDateRoom();
                        return;
                    case 4:
                        Clients.Group(idRoom.ToString()).resultbattle("Конец игры: Победа красных");
                        Clients.Group(idRoom.ToString()).upDateRoom();
                        return;
                    case 5:
                        Clients.Group(idRoom.ToString()).resultbattle("Конец игры: ничья");
                        Clients.Group(idRoom.ToString()).upDateRoom();
                        return;
                    default:
                        break;
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
                if (model.secondPLayerActiveCard == true)
                {
                    Clients.OthersInGroup(idRoom.ToString()).enemyCard(cardId, Player.ColorTeam, 1);
                    if (model.P1bonusSpook == 0)
                        Clients.Caller.enemyCard(model.idSecondPlayerCad.ToString(), "K", 1);
                    else
                        model.P1bonusSpook = 0;
                }
                else
                {
                    if (model.P2bonusSpook != 0)
                    {
                        Clients.OthersInGroup(idRoom.ToString()).enemyCard(cardId, Player.ColorTeam, 1);
                    }
                    else
                    {
                        Clients.OthersInGroup(idRoom.ToString()).enemyCard(cardId, Player.ColorTeam, 0);
                        if (model.P1bonusSpook != 0)
                            model.P1bonusSpook = 0;
                    }
                }
            }
            else
            {
                model.secondPLayerActiveCard = true;
                model.idSecondPlayerCad = Int32.Parse(cardId);
                Clients.Caller.getboolcard(model.secondPLayerActiveCard);
                if (model.firstPLayerActiveCard == true)
                {
                    Clients.OthersInGroup(idRoom.ToString()).enemyCard(cardId, Player.ColorTeam, 1);
                    if (model.P2bonusSpook == 0)
                        Clients.Caller.enemyCard(model.idFirstPlayerCad.ToString(), "С", 1);
                    else
                        model.P2bonusSpook = 0;
                }
                else
                {
                    if (model.P1bonusSpook != 0)
                    {

                        Clients.OthersInGroup(idRoom.ToString()).enemyCard(cardId, Player.ColorTeam, 1);
                    }
                    else
                    {
                        Clients.OthersInGroup(idRoom.ToString()).enemyCard(cardId, Player.ColorTeam, 0);
                        if (model.P2bonusSpook != 0)
                            model.P2bonusSpook = 0;
                    }
                }
            }
            Rb.SaveChanges();


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