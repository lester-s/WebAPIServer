namespace SelfHostedWebApi.HostConfig
{
    public class ServerStaticValues
    {
        public readonly static string MethodName_UserConnect = "ConnectUser";
        public enum AppRole
        {
            Admin,
            reader,
            nothing
        };
    }
}