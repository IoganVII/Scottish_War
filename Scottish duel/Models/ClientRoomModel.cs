using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scottish_duel.Models
{
    public class ClientRoomModel
    {
        public int id { get; set; }
        public string name { get; set; }

        public string nameGod { get; set; }

        public string password { get; set; }

        public int numberPlayer { set; get; }

        public string nameFirstPlayer { set; get; }

        public string nameSecondPlayer { set; get; }

        public bool firstPLayerActiveCard { set; get; }

        public int idFirstPlayerCad { set; get; }

        public int idSecondPlayerCad { set; get; }

        public bool secondPLayerActiveCard { set; get; }

        public int vPointFerstPlayer { set; get; }

        public int vPointSecondPlayer { set; get; }

        public int numberRound { set; get; }

    }
}