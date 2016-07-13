namespace SelfHostedWebApi.Model
{
    public class User
    {
        public User(string name, string password, string role)
        {
            Pseudo = name;
            Password = password;
            Role = role;
        }

        public User()
        {
        }

        public int Id { get; private set; }
        public string Pseudo { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}