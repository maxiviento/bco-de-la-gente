using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using ApiBatch.Infraestructure;
using Infraestructura.Core.Filtros;
using Infraestructura.Core.Formateadores;

namespace ApiBatch
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(Formateadores.Registrar);
            GlobalConfiguration.Configure(FiltrosBatch.Registrar);
            BatchProcessTimer.Start();
            GenerateProcessTimer.Start();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            BatchProcessTimer.Stop();
            GenerateProcessTimer.Stop();
        }
    }
}