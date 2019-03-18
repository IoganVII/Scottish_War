using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Scottish_duel.Models
{
    public class ClientRoomModelContext : DbContext
    {
        public DbSet<ClientRoomModel> ClientRoomModels { set; get; }
    }
}