﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scottish_duel.Models
{
    public class RegisterModel
    {

        public int id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}