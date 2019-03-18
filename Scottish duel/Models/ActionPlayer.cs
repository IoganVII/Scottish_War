using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scottish_duel.Models
{
    public class ActionPlayer
    {

        public int id { set; get; }

        public string Name { set; get; }

        public int idRoom { set; get; }
    }
}