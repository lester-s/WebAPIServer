﻿using SelfHostedWebApi.BuisnessLayer;
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
                );

            bool parseValue = true;

            if (args.Length > 0)
            {
                var parseResult = bool.TryParse(args[0], out parseValue);

                parseValue = parseResult ? parseValue : true;
            }

            AppHandler.Instance.IsAuthenticationActive = parseValue;

            if (AppHandler.Instance.IsAuthenticationActive)
            {
                config.MessageHandlers.Add(new AuthenticationMessageHandler());
                config.Filters.Add(new ServerAuthorizationFilter() { Role = ServerStaticValues.AppRole.Admin });
            }

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}