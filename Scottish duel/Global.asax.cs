using Scottish_duel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Scottish_duel
{
    public class MvcApplication : System.Web.HttpApplication
    {
        ActionPlayerContext Ap = new ActionPlayerContext();
        ClientRoomModelContext Rb = new ClientRoomModelContext();
        CardModelContext Cb = new CardModelContext();
        protected void Application_Start()
        {
            Ap.Database.Delete();
            Rb.Database.Delete();
            Cb.Database.Delete();

            string[] namescard = new string[8];
            namescard[0] = "Музыкант";
            namescard[1] = "Принцесса";
            namescard[2] = "Шпион";
            namescard[3] = "Убийца";
            namescard[4] = "Посол";
            namescard[5] = "Волшебник";
            namescard[6] = "Генерал";
            namescard[7] = "Принц";
            for (int i = 0; i < 8; i++)
            {
                CardModel model = new CardModel();
                model.id = i;
                model.number = i;
                model.strength = i;
                model.name = namescard[i];
                Cb.CardModels.Add(model);
                Cb.SaveChanges();
            }






            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }
}
