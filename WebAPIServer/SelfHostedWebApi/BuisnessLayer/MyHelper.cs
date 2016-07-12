using SelfHostedWebApi.HostConfig;
using System;

namespace SelfHostedWebApi.BuisnessLayer
{
    public class MyHelper
    {
        public static class AuthorizationHelper
        {
            public static ServerStaticValues.AppRole setRoleFromString(string role)
            {
                if (string.IsNullOrWhiteSpace(role))
                {
                    throw new ArgumentNullException(nameof(role), "a role is needed");
                }

                switch (role.ToLower())
                {
                    case "admin":
                        return ServerStaticValues.AppRole.Admin;

                    case "reader":
                        return ServerStaticValues.AppRole.reader;

                    default:
                        return ServerStaticValues.AppRole.reader;
                }
            }
        }
    }
}