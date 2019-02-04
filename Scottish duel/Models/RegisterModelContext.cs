using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Scottish_duel.Models
{
    public class RegisterModelContext : DbContext
    {
        public DbSet<RegisterModel> RegisterModels { get; set; }
    }
}