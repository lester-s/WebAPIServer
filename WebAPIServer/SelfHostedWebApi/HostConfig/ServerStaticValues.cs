namespace SelfHostedWebApi.HostConfig
{
    public class ServerStaticValues
    {
        public readonly static string MethodName_UserConnect = "ConnectUser";

        public readonly static string IdName = "ID";

        public enum AppRole
        {
            Admin,
            reader,
            nothing
        };
    }
}