namespace SelfHostedWebApi.Model
{
    public class User : BaseModel
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

        public string Pseudo { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}