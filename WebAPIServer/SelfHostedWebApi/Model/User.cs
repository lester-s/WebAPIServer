using SelfHostedWebApi.BuisnessLayer;
using SelfHostedWebApi.HostConfig;

namespace SelfHostedWebApi.Model
{
    public class User
    {
        public User(int id, string name, int age, string password, string role)
        {
            Id = id;
            Name = name;
            Age = age;
            Password = password;
            Role = MyHelper.AuthorizationHelper.setRoleFromString(role);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Password { get; set; }
        public ServerStaticValues.AppRole Role { get; set; }
    }
}