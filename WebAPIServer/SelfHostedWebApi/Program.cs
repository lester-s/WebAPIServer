using NLog;
using SelfHostedWebApi.BuisnessLayer;
using SelfHostedWebApi.HostConfig;
using SelfHostedWebApi.HostConfig.ExceptionsHandling;
using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.SelfHost;

namespace SelfHostedWebApi
{
    internal class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            ServerLogger.SetDefaultConfiguration();

            logger.Debug("Starting server config");

            var config = new HttpSelfHostConfiguration("http://localhost:8080");

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional },
                constraints: null
                );

            if (AppHandler.Instance.Settings.IsAuthActivated)
            {
                logger.Debug("Activating Auth");

                config.MessageHandlers.Add(new AuthenticationMessageHandler());
                config.Filters.Add(new ServerAuthorizationFilter() { Role = ServerStaticValues.AppRole.Admin });
            }

            config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                logger.Debug("Starting server");
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
                logger.Debug("Server started");
            }
        }
    }
}