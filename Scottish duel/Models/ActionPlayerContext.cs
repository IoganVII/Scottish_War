using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Scottish_duel.Models
{
    public class ActionPlayerContext : DbContext
    {
        public DbSet<ActionPlayer> ActionPlayers { get; set; }
    }
}