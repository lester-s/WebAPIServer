using SelfHostedWebApi.HostConfig;
using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace SelfHostedWebApi
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration("http://localhost:8080");

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional },
                constraints: null

                , handler: new AuthenticationMessageHandler(config)
                );

            config.Filters.Add(new ServerAuthorizationFilter());

            //config.Filters.Add(new ServerAuthorizationFilter());

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}