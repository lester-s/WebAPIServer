using System.Security.Principal;

namespace SelfHostedWebApi.HostConfig
{
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public BasicAuthenticationIdentity(string name, string password, ServerStaticValues.AppRole role) : base(name, "Basic")
        {
            this.Password = password;
            this.Role = role;
        }

        /// <summary>
        /// Basic Auth Password for custom authentication
        /// </summary>
        public string Password { get; set; }

        public ServerStaticValues.AppRole Role { get; set; }
    }
}