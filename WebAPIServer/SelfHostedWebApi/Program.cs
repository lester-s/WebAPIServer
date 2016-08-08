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
        private static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration("http://localhost:8080");

            ServerLogger.SetDefaultConfiguration();

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional },
                constraints: null
                );

            if (AppHandler.Instance.Settings.IsAuthActivated)
            {
                config.MessageHandlers.Add(new AuthenticationMessageHandler());
                config.Filters.Add(new ServerAuthorizationFilter() { Role = ServerStaticValues.AppRole.Admin });
            }

            config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}