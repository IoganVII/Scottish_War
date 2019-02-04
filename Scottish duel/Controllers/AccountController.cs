using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using Scottish_duel.Models;

namespace Scottish_duel.Controllers
{
    public class AccountController : Controller
    {

        private SqlConnection sqlConnection = null;

        // GET: Account
        public ActionResult Register()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);

            sqlConnection.Open();

            return View();
        }

        [HttpPost]
        public RegisterModel Reg (RegisterModel model)
        {

            model.Login = model.Login;

            return new RegisterModel()
            {
                Login = "123",
                Password = "456"
            };
        }


    }
}