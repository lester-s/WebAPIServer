using SelfHostedWebApi.HostConfig;
using System;

namespace SelfHostedWebApi.Helpers
{
    public class AuthorizationHelper
    {
        public static ServerStaticValues.AppRole GetRoleFromString(string role)
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