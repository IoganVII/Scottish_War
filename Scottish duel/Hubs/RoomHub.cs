using System;
using System.Collections.Generic;
using System.Linq;
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
        public static ClientRoomModel BotRoom = new ClientRoomModel();
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RoomHub>();

        ActionPlayerContext Ap = new ActionPlayerContext();
        ClientRoomModelContext Rb = new ClientRoomModelContext();
        CardModelContext Cb = new CardModelContext();


        public void init(string Login)
        {
            BotRoom.vPointFerstPlayer = 0;
            BotRoom.vPointSecondPlayer = 0;
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
            BotRoom.name = "BotGame";
            BotRoom.nameFirstPlayer = Login;
            BotRoom.nameSecondPlayer = "BotPlayer";
            BotRoom.numberRound = 0;
            Ap.SaveChanges();
            Clients.Caller.generatedTeam(Player.ColorTeam);
        }

        public void inputCard(string cardId, string Login)
        {
            ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == Login).FirstOrDefault();
            BotRoom.firstPLayerActiveCard = true;
            BotRoom.idFirstPlayerCad = Int32.Parse(cardId);
            Clients.Caller.getboolcard(BotRoom.firstPLayerActiveCard);
            var idBotCard = "";
            Random rnd = new Random();
            idBotCard = BotCard[rnd.Next(0, BotCard.Count)];
            BotRoom.idSecondPlayerCad = Int32.Parse(idBotCard);
            BotRoom.secondPLayerActiveCard = true;
            for (int i = 0; i < BotCard.Count; i++)

                if (BotCard[i] == idBotCard)
                {
                    BotCard.RemoveAt(i);
                    break;
                }
            Clients.Caller.enemyCard(idBotCard, BotPlayer.ColorTeam);
            battleround(cardId, Login);
        }


        public void battleround(string cardId, string Login)
        {
            ActionPlayer Player = Ap.ActionPlayers.Where(o => o.Name == Login).FirstOrDefault();
            //Если разыграын обе карты
            if ((BotRoom.firstPLayerActiveCard == true) && (BotRoom.secondPLayerActiveCard == true))
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

                CardModel card1 = Cb.CardModels.Where(o => o.number == BotRoom.idFirstPlayerCad).FirstOrDefault();
                CardModel card2 = Cb.CardModels.Where(o => o.number == BotRoom.idSecondPlayerCad).FirstOrDefault();

                var stregth1 = card1.strength;
                var stregth2 = card2.strength;

                int win = rule[7 - stregth2, 7 - stregth1];
                switch (win)
                {
                    case 0:
                        Clients.Caller.resultbattle("Раунд отложен", BotRoom.vPointFerstPlayer, BotRoom.vPointSecondPlayer);
                        break;
                    case 1:
                        BotRoom.vPointSecondPlayer++;
                        Clients.Caller.resultbattle("Раунд Победа красных", BotRoom.vPointFerstPlayer, BotRoom.vPointSecondPlayer);
                        break;
                    case 2:
                        BotRoom.vPointFerstPlayer++;
                        Clients.Caller.resultbattle("Раунд Победа синих", BotRoom.vPointFerstPlayer, BotRoom.vPointSecondPlayer);
                        break;
                }

                BotRoom.numberRound++;
                BotRoom.firstPLayerActiveCard = false;
                BotRoom.secondPLayerActiveCard = false;

                if (BotRoom.numberRound == 8)
                {
                    if (BotRoom.vPointFerstPlayer > BotRoom.vPointSecondPlayer)
                        Clients.Caller.resultbattle("Конец игры: Победа синих");
                    if (BotRoom.vPointFerstPlayer < BotRoom.vPointSecondPlayer)
                        Clients.Caller.resultbattle("Конец игры: Победа красных");
                    if (BotRoom.vPointFerstPlayer == BotRoom.vPointSecondPlayer)
                        Clients.Caller.resultbattle("Конец игры: ничья");
                    Clients.Caller.upDateRoom();
                }

            }
        }

    }
}