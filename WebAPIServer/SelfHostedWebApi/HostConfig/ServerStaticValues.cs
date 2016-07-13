using System.Collections.Generic;

namespace SelfHostedWebApi.HostConfig
{
    public class ServerStaticValues
    {
        public readonly static string MethodName_UserConnect = "ConnectUser";

        public readonly static List<string> IDsNames = new List<string>() { "ID" };

        public enum AppRole
        {
            Admin,
            reader,
            nothing
        };
    }
}