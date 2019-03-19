using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scottish_duel.Models
{
    public class CardModel
    {

        public int id { set; get; }

        public int number { set; get; }
        public string name { set; get; }

        public int strength { get; set; }
    }
}