using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Scottish_duel.Models;

namespace Scottish_duel.Hubs
{
    public class RoomHub : Hub
    {
        public static ActionPlayer BotPlayer = new ActionPlayer();
        public static List<String> BotCard = new List<string>();
        public static List<String> PlayerCard = new List<string>();
        public static ClientRoomModel BotRoom = new ClientRoomModel();
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RoomHub>();

        ActionPlayerContext Ap = new ActionPlayerContext();
        ClientRoomModelContext Rb = new ClientRoomModelContext();
        CardModelContext Cb = new CardModelContext();

        struct move
        {
            public int index;
            public int score;
        }


        move step(List<String> BotCard, ClientRoomModel BotaRoom, List<String> PlayerCard, bool flag, int kote)
        {


            ClientRoomModel Room = new ClientRoomModel();
            List<String> RoomBotCard = new List<string>();
            List<String> RoomPlayerCard = new List<string>();
            List<String> NewRoomBotCard = new List<string>();
            List<String> NewRoomPlayerCard = new List<string>();

            //Копирвание карт бота на текущую рекурсию
            for (int i = 0; i < BotCard.Count; i++)
            {
                RoomBotCard.Add(BotCard[i]);
            }
            //Копирование карт игрока на текущую рекурсию
            for (int i = 0; i < PlayerCard.Count; i++)
            {
                RoomPlayerCard.Add(PlayerCard[i]);
            }

            Room.delayedRound = BotaRoom.delayedRound;
            Room.firstPLayerActiveCard = BotaRoom.firstPLayerActiveCard;
            Room.id = BotaRoom.id;
            Room.idFirstPlayerCad = BotaRoom.idFirstPlayerCad;
            Room.idSecondPlayerCad = BotaRoom.idSecondPlayerCad;
            Room.name = BotaRoom.name;
            Room.nameFirstPlayer = BotaRoom.nameFirstPlayer;
            Room.nameGod = BotaRoom.nameGod;
            Room.nameSecondPlayer = BotaRoom.nameSecondPlayer;
            Room.numberPlayer = BotaRoom.numberPlayer;
            Room.numberRound = BotaRoom.numberRound;
            Room.P1bonusGeneral = BotaRoom.P1bonusGeneral;
            Room.P1bonusLegate = BotaRoom.P1bonusLegate;
            Room.P1bonusSpook = BotaRoom.P1bonusSpook;
            Room.P2bonusGeneral = BotaRoom.P2bonusGeneral;
            Room.P2bonusLegate = BotaRoom.P2bonusLegate;
            Room.P2bonusSpook = BotaRoom.P2bonusSpook;
            Room.password = BotaRoom.password;
            Room.secondPLayerActiveCard = BotaRoom.secondPLayerActiveCard;
            Room.vPointFerstPlayer = BotaRoom.vPointFerstPlayer;
            Room.vPointSecondPlayer = BotaRoom.vPointSecondPlayer;

            //Опеределение конца игры ( Выход из рекурсии)
            if (flag == true)
                switch (BotWinner(Room, kote))
                {
                    case 4:
                        return new move { score = -10 };
                    case 3:
                        return new move { score = 10 };
                    case 5:
                        return new move { score = 0 };
                }



            // Создание массива объектов move
            List<move> moves = new List<move>();

            //Цикл до конца игры
            for (int i = 0; i < RoomBotCard.Count; i++)
            {
                move move = new move();
                move.index = Int32.Parse(RoomBotCard[i]);
                for (int j = 0; j < RoomPlayerCard.Count; j++)
                {

                    Room.firstPLayerActiveCard = true;
                    Room.secondPLayerActiveCard = true;
                    Room.idFirstPlayerCad = Int32.Parse(RoomPlayerCard[j]);
                    Room.idSecondPlayerCad = move.index;

                    NewRoomBotCard.RemoveRange(0, NewRoomBotCard.Count);
                    NewRoomPlayerCard.RemoveRange(0, NewRoomPlayerCard.Count);

                    //Создание новой колоды бота для отправки
                    for (int k = 0; k < RoomBotCard.Count; k++)
                    {

                        if (RoomBotCard[k] == Room.idSecondPlayerCad.ToString())
                        {
                            continue;
                        }
                        NewRoomBotCard.Add(RoomBotCard[k]);
                    }

                    //Создание новой колоды игрока для отправки
                    for (int k = 0; k < RoomPlayerCard.Count; k++)
                    {

                        if (RoomPlayerCard[k] == Room.idFirstPlayerCad.ToString())
                        {
                            continue;
                        }
                        NewRoomPlayerCard.Add(RoomPlayerCard[k]);
                    }

                    int kotik = BotRoom.numberRound + 2;
                    if (kotik >= 8)
                        kotik = 8;
                    var result = step(NewRoomBotCard, Room, NewRoomPlayerCard, true, kotik);
                    move.score = move.score + result.score;
                }
                moves.Add(move);
            }

            int bestMove = 0;
            var bestScore = -10000;
            for (var i = 0; i < moves.Count; i++)
            {
                if (moves[i].score > bestScore)
                {
                    bestScore = moves[i].score;
                    bestMove = i;
                }
            }
            return moves[bestMove];
        }


        //Поток бота
        void mythread1()
        {
            while (BotRoom.numberRound < 8)
            {
                if (BotRoom.secondPLayerActiveCard == false)
                {
                    var idBotCard = step(BotCard, BotRoom, PlayerCard, false, 2);
                    BotRoom.idSecondPlayerCad = idBotCard.index;
                    BotRoom.secondPLayerActiveCard = true;
                    for (int i = 0; i < BotCard.Count; i++)

                        if (BotCard[i] == idBotCard.index.ToString())
                        {
                            BotCard.RemoveAt(i);
                            break;
                        }
                    if (BotRoom.P1bonusSpook != 0)
                        Clients.Caller.enemyCard(idBotCard, BotPlayer.ColorTeam, 1);
                    else
                        Clients.Caller.enemyCard(idBotCard, BotPlayer.ColorTeam, 0);

                    battleround(idBotCard.index.ToString(), "Player");
                }
            }
            if (BotRoom.numberRound == 8)
                return;
        }

        int Winner(ClientRoomModel BotRoom)
        {

            CardModel card1 = Cb.CardModels.Where(o => o.number == BotRoom.idFirstPlayerCad).FirstOrDefault();
            CardModel card2 = Cb.CardModels.Where(o => o.number == BotRoom.idSecondPlayerCad).FirstOrDefault();

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
                    BotRoom.P1bonusLegate = 2;
                }
                if (card2.number == 4)
                {
                    BotRoom.P2bonusLegate = 2;
                }
            }

            //Активация навыка шпиона
            if (card1.number == 2 || card2.number == 2)
            {
                if (card1.number == 2 && card2.number != 5)
                    BotRoom.P1bonusSpook = 1;
                if (card2.number == 2 && card1.number != 5)
                    BotRoom.P2bonusSpook = 1;
                if (card2.number == 2 && card1.number == 2)
                {
                    BotRoom.P1bonusSpook = 0;
                    BotRoom.P2bonusSpook = 0;
                }

            }





            //Просчёт раунда с учётом свойства генерала
            if (BotRoom.P1bonusGeneral != 0 || BotRoom.P2bonusGeneral != 0)
            {
                if (BotRoom.P1bonusGeneral != 0)
                {
                    stregth1 += BotRoom.P1bonusGeneral;
                    BotRoom.P1bonusGeneral = 0;
                }
                if (BotRoom.P2bonusGeneral != 0)
                {
                    stregth2 += BotRoom.P2bonusGeneral;
                    BotRoom.P2bonusGeneral = 0;
                }

                if (card1.number != 0 && card2.number != 0)

                {
                    if (stregth1 > stregth2)
                    {
                        if (card2.number == 3)
                        {
                            if (BotRoom.P2bonusLegate != 0)
                            {
                                BotRoom.vPointSecondPlayer += BotRoom.P2bonusLegate;
                                BotRoom.P2bonusLegate = 0;
                            }
                            else
                                BotRoom.vPointSecondPlayer++;

                            //Отменяем навык вражеского посла
                            if (BotRoom.P1bonusLegate != 0)
                                BotRoom.P1bonusLegate = 0;

                            if (BotRoom.delayedRound != 0)
                            {
                                BotRoom.vPointSecondPlayer = BotRoom.vPointSecondPlayer + BotRoom.delayedRound;
                                BotRoom.delayedRound = 0;
                            }
                            win2 = true;
                        }
                        else
                        {
                            if (BotRoom.P1bonusLegate != 0)
                            {
                                BotRoom.vPointFerstPlayer += BotRoom.P1bonusLegate;
                                BotRoom.P1bonusLegate = 0;
                            }
                            else
                                BotRoom.vPointFerstPlayer++;

                            //Отменяем навык вражеского посла
                            if (BotRoom.P2bonusLegate != 0)
                                BotRoom.P2bonusLegate = 0;

                            if (BotRoom.delayedRound != 0)
                            {
                                BotRoom.vPointFerstPlayer = BotRoom.vPointFerstPlayer + BotRoom.delayedRound;
                                BotRoom.delayedRound = 0;
                            }
                            win1 = true;
                        }
                    }
                    if (stregth2 > stregth1)
                    {
                        if (card1.number == 3)
                        {
                            if (BotRoom.P1bonusLegate != 0)
                            {
                                BotRoom.vPointFerstPlayer += BotRoom.P1bonusLegate;
                                BotRoom.P1bonusLegate = 0;
                            }
                            else
                                BotRoom.vPointFerstPlayer++;

                            //Отменяем навык вражеского посла
                            if (BotRoom.P2bonusLegate != 0)
                                BotRoom.P2bonusLegate = 0;

                            if (BotRoom.delayedRound != 0)
                            {
                                BotRoom.vPointFerstPlayer = BotRoom.vPointFerstPlayer + BotRoom.delayedRound;
                                BotRoom.delayedRound = 0;
                            }
                            win1 = true;
                        }
                        else
                        {
                            if (BotRoom.P2bonusLegate != 0)
                            {
                                BotRoom.vPointSecondPlayer += BotRoom.P2bonusLegate;
                                BotRoom.P2bonusLegate = 0;
                            }
                            else
                                BotRoom.vPointSecondPlayer++;

                            //Отменяем навык вражеского посла
                            if (BotRoom.P1bonusLegate != 0)
                                BotRoom.P1bonusLegate = 0;

                            if (BotRoom.delayedRound != 0)
                            {
                                BotRoom.vPointSecondPlayer = BotRoom.vPointSecondPlayer + BotRoom.delayedRound;
                                BotRoom.delayedRound = 0;
                            }
                            win2 = true;
                        }
                    }

                    if (stregth1 == stregth2)
                        BotRoom.delayedRound++;
                }
                else
                    BotRoom.delayedRound++;


            }
            //Обычный просчёт игры
            else
            {
                int win = rule[7 - stregth2, 7 - stregth1];
                switch (win)
                {
                    case 0:
                        BotRoom.delayedRound++;
                        break;
                    case 1:
                        if (BotRoom.P2bonusLegate != 0)
                        {
                            BotRoom.vPointSecondPlayer += BotRoom.P2bonusLegate;
                            BotRoom.P2bonusLegate = 0;
                        }
                        else
                            BotRoom.vPointSecondPlayer++;

                        //Отменяем навык вражеского посла
                        if (BotRoom.P1bonusLegate != 0)
                            BotRoom.P1bonusLegate = 0;

                        if (BotRoom.delayedRound != 0)
                        {
                            BotRoom.vPointSecondPlayer = BotRoom.vPointSecondPlayer + BotRoom.delayedRound;
                            BotRoom.delayedRound = 0;
                        }
                        win2 = true;
                        break;
                    case 2:
                        if (BotRoom.P1bonusLegate != 0)
                        {
                            BotRoom.vPointFerstPlayer += BotRoom.P1bonusLegate;
                            BotRoom.P1bonusLegate = 0;
                        }
                        else
                            BotRoom.vPointFerstPlayer++;

                        //Отменяем навык вражеского посла
                        if (BotRoom.P2bonusLegate != 0)
                            BotRoom.P2bonusLegate = 0;

                        if (BotRoom.delayedRound != 0)
                        {
                            BotRoom.vPointFerstPlayer = BotRoom.vPointFerstPlayer + BotRoom.delayedRound;
                            BotRoom.delayedRound = 0;
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
                    BotRoom.P1bonusGeneral = 2;
                }

                if (card2.number == 6 && card1.number != 5)
                {
                    BotRoom.P2bonusGeneral = 2;
                }

            }

            for (int i = 0; i < PlayerCard.Count; i++)
            {
                if (BotRoom.idFirstPlayerCad.ToString() == PlayerCard[i])
                {
                    PlayerCard.RemoveAt(i);
                    break;
                }
            }
            BotRoom.numberRound++;
            BotRoom.firstPLayerActiveCard = false;
            BotRoom.secondPLayerActiveCard = false;
            Rb.SaveChanges();


            if (BotRoom.numberRound == 8)
            {
                if (BotRoom.vPointFerstPlayer > BotRoom.vPointSecondPlayer)
                    return 3;
                if (BotRoom.vPointFerstPlayer < BotRoom.vPointSecondPlayer)
                    return 4;
                if (BotRoom.vPointFerstPlayer == BotRoom.vPointSecondPlayer)
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




        //Подсчё победы в алгоритме минимакс.
        int BotWinner(ClientRoomModel BotRoom, int kote)
        {

            CardModel card1 = Cb.CardModels.Where(o => o.number == BotRoom.idFirstPlayerCad).FirstOrDefault();
            CardModel card2 = Cb.CardModels.Where(o => o.number == BotRoom.idSecondPlayerCad).FirstOrDefault();

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
                    BotRoom.P1bonusLegate = 2;
                }
                if (card2.number == 4)
                {
                    BotRoom.P2bonusLegate = 2;
                }
            }

            //Активация навыка шпиона
            if (card1.number == 2 || card2.number == 2)
            {
                if (card1.number == 2 && card2.number != 5)
                    BotRoom.P1bonusSpook = 1;
                if (card2.number == 2 && card1.number != 5)
                    BotRoom.P2bonusSpook = 1;
                if (card2.number == 2 && card1.number == 2)
                {
                    BotRoom.P1bonusSpook = 0;
                    BotRoom.P2bonusSpook = 0;
                }

            }





            //Просчёт раунда с учётом свойства генерала
            if (BotRoom.P1bonusGeneral != 0 || BotRoom.P2bonusGeneral != 0)
            {
                if (BotRoom.P1bonusGeneral != 0)
                {
                    stregth1 += BotRoom.P1bonusGeneral;
                    BotRoom.P1bonusGeneral = 0;
                }
                if (BotRoom.P2bonusGeneral != 0)
                {
                    stregth2 += BotRoom.P2bonusGeneral;
                    BotRoom.P2bonusGeneral = 0;
                }

                if (card1.number != 0 && card2.number != 0)

                {
                    if (stregth1 > stregth2)
                    {
                        if (card2.number == 3)
                        {
                            if (BotRoom.P2bonusLegate != 0)
                            {
                                BotRoom.vPointSecondPlayer += BotRoom.P2bonusLegate;
                                BotRoom.P2bonusLegate = 0;
                            }
                            else
                                BotRoom.vPointSecondPlayer++;

                            //Отменяем навык вражеского посла
                            if (BotRoom.P1bonusLegate != 0)
                                BotRoom.P1bonusLegate = 0;

                            if (BotRoom.delayedRound != 0)
                            {
                                BotRoom.vPointSecondPlayer = BotRoom.vPointSecondPlayer + BotRoom.delayedRound;
                                BotRoom.delayedRound = 0;
                            }
                            win2 = true;
                        }
                        else
                        {
                            if (BotRoom.P1bonusLegate != 0)
                            {
                                BotRoom.vPointFerstPlayer += BotRoom.P1bonusLegate;
                                BotRoom.P1bonusLegate = 0;
                            }
                            else
                                BotRoom.vPointFerstPlayer++;

                            //Отменяем навык вражеского посла
                            if (BotRoom.P2bonusLegate != 0)
                                BotRoom.P2bonusLegate = 0;

                            if (BotRoom.delayedRound != 0)
                            {
                                BotRoom.vPointFerstPlayer = BotRoom.vPointFerstPlayer + BotRoom.delayedRound;
                                BotRoom.delayedRound = 0;
                            }
                            win1 = true;
                        }
                    }
                    if (stregth2 > stregth1)
                    {
                        if (card1.number == 3)
                        {
                            if (BotRoom.P1bonusLegate != 0)
                            {
                                BotRoom.vPointFerstPlayer += BotRoom.P1bonusLegate;
                                BotRoom.P1bonusLegate = 0;
                            }
                            else
                                BotRoom.vPointFerstPlayer++;

                            //Отменяем навык вражеского посла
                            if (BotRoom.P2bonusLegate != 0)
                                BotRoom.P2bonusLegate = 0;

                            if (BotRoom.delayedRound != 0)
                            {
                                BotRoom.vPointFerstPlayer = BotRoom.vPointFerstPlayer + BotRoom.delayedRound;
                                BotRoom.delayedRound = 0;
                            }
                            win1 = true;
                        }
                        else
                        {
                            if (BotRoom.P2bonusLegate != 0)
                            {
                                BotRoom.vPointSecondPlayer += BotRoom.P2bonusLegate;
                                BotRoom.P2bonusLegate = 0;
                            }
                            else
                                BotRoom.vPointSecondPlayer++;

                            //Отменяем навык вражеского посла
                            if (BotRoom.P1bonusLegate != 0)
                                BotRoom.P1bonusLegate = 0;

                            if (BotRoom.delayedRound != 0)
                            {
                                BotRoom.vPointSecondPlayer = BotRoom.vPointSecondPlayer + BotRoom.delayedRound;
                                BotRoom.delayedRound = 0;
                            }
                            win2 = true;
                        }
                    }

                    if (stregth1 == stregth2)
                        BotRoom.delayedRound++;
                }
                else
                    BotRoom.delayedRound++;


            }
            //Обычный просчёт игры
            else
            {
                int win = rule[7 - stregth2, 7 - stregth1];
                switch (win)
                {
                    case 0:
                        BotRoom.delayedRound++;
                        break;
                    case 1:
                        if (BotRoom.P2bonusLegate != 0)
                        {
                            BotRoom.vPointSecondPlayer += BotRoom.P2bonusLegate;
                            BotRoom.P2bonusLegate = 0;
                        }
                        else
                            BotRoom.vPointSecondPlayer++;

                        //Отменяем навык вражеского посла
                        if (BotRoom.P1bonusLegate != 0)
                            BotRoom.P1bonusLegate = 0;

                        if (BotRoom.delayedRound != 0)
                        {
                            BotRoom.vPointSecondPlayer = BotRoom.vPointSecondPlayer + BotRoom.delayedRound;
                            BotRoom.delayedRound = 0;
                        }
                        win2 = true;
                        break;
                    case 2:
                        if (BotRoom.P1bonusLegate != 0)
                        {
                            BotRoom.vPointFerstPlayer += BotRoom.P1bonusLegate;
                            BotRoom.P1bonusLegate = 0;
                        }
                        else
                            BotRoom.vPointFerstPlayer++;

                        //Отменяем навык вражеского посла
                        if (BotRoom.P2bonusLegate != 0)
                            BotRoom.P2bonusLegate = 0;

                        if (BotRoom.delayedRound != 0)
                        {
                            BotRoom.vPointFerstPlayer = BotRoom.vPointFerstPlayer + BotRoom.delayedRound;
                            BotRoom.delayedRound = 0;
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
                    BotRoom.P1bonusGeneral = 2;
                }

                if (card2.number == 6 && card1.number != 5)
                {
                    BotRoom.P2bonusGeneral = 2;
                }

            }


            BotRoom.numberRound++;
            Rb.SaveChanges();



            if (BotRoom.numberRound == kote)
            {
                if (BotRoom.vPointFerstPlayer > BotRoom.vPointSecondPlayer)
                    return 3;
                if (BotRoom.vPointFerstPlayer < BotRoom.vPointSecondPlayer)
                    return 4;
                if (BotRoom.vPointFerstPlayer == BotRoom.vPointSecondPlayer)
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










        public void init(string Login)
        {
            BotRoom.vPointFerstPlayer = 0;
            BotRoom.vPointSecondPlayer = 0;
            BotRoom.delayedRound = 0;
            BotRoom.P1bonusLegate = 0;
            BotRoom.P2bonusLegate = 0;
            BotRoom.P1bonusGeneral = 0;
            BotRoom.P2bonusGeneral = 0;
            BotRoom.P1bonusSpook = 0;
            BotRoom.P2bonusSpook = 0;
            BotRoom.firstPLayerActiveCard = false;
            BotRoom.secondPLayerActiveCard = false;
            ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == Login).FirstOrDefault();
            BotPlayer.ColorTeam = "K";
            BotCard.Add("0");
            BotCard.Add("1");
            BotCard.Add("2");
            BotCard.Add("3");
            BotCard.Add("4");
            BotCard.Add("5");
            BotCard.Add("6");
            BotCard.Add("7");
            Player.ColorTeam = "С";
            PlayerCard.Add("0");
            PlayerCard.Add("1");
            PlayerCard.Add("2");
            PlayerCard.Add("3");
            PlayerCard.Add("4");
            PlayerCard.Add("5");
            PlayerCard.Add("6");
            PlayerCard.Add("7");
            BotRoom.name = "BotGame";
            BotRoom.nameFirstPlayer = Login;
            BotRoom.nameSecondPlayer = "BotPlayer";
            BotRoom.numberRound = 0;
            Ap.SaveChanges();
            Clients.Caller.generatedTeam(Player.ColorTeam);
            Thread thread1 = new Thread(mythread1);
            thread1.Name = "BotThread";
            thread1.Start();
        }

        public void inputCard(string cardId, string Login)
        {
            ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == Login).FirstOrDefault();
            BotRoom.firstPLayerActiveCard = true;
            BotRoom.idFirstPlayerCad = Int32.Parse(cardId);
            Clients.Caller.getboolcard(BotRoom.firstPLayerActiveCard);
            battleround(cardId, Login);
        }


        public void battleround(string cardId, string Login)
        {

            //Если разыграын обе карты
            if ((BotRoom.firstPLayerActiveCard == true) && (BotRoom.secondPLayerActiveCard == true))
            {
                if (BotRoom.P1bonusSpook != 0)
                    BotRoom.P1bonusSpook = 0;
                else
                    Clients.Caller.enemyCard(BotRoom.idSecondPlayerCad.ToString(), BotPlayer.ColorTeam, 1);


                var win = Winner(BotRoom);

                switch (win)
                {
                    case 0:
                        Clients.Caller.resultbattle("Раунд отложен", BotRoom.vPointFerstPlayer, BotRoom.vPointSecondPlayer);
                        break;
                    case 1:
                        Clients.Caller.resultbattle("Раунд Победа синих", BotRoom.vPointFerstPlayer, BotRoom.vPointSecondPlayer);
                        break;
                    case 2:
                        Clients.Caller.resultbattle("Раунд Победа красных", BotRoom.vPointFerstPlayer, BotRoom.vPointSecondPlayer);
                        break;
                    case 3:
                        Clients.Caller.resultbattle("Раунд Победа синих", BotRoom.vPointFerstPlayer, BotRoom.vPointSecondPlayer);
                        Clients.Caller.resultbattle("Конец игры: Победа синих");
                        Clients.Caller.upDateRoom();
                        return;
                    case 4:
                        Clients.Caller.resultbattle("Раунд Победа красных", BotRoom.vPointFerstPlayer, BotRoom.vPointSecondPlayer);
                        Clients.Caller.resultbattle("Конец игры: Победа красных");
                        Clients.Caller.upDateRoom();
                        return;
                    case 5:
                        Clients.Caller.resultbattle("Раунд отложен", BotRoom.vPointFerstPlayer, BotRoom.vPointSecondPlayer);
                        Clients.Caller.resultbattle("Конец игры: ничья");
                        Clients.Caller.upDateRoom();
                        return;
                    default:
                        break;
                }
            }
        }

    }
}