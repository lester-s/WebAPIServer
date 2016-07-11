namespace SelfHostedWebApi.Model
{
    public class User
    {
        public User(int id, string name, int age, string password)
        {
            Id = id;
            Name = name;
            Age = age;
            Password = password;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Password { get; set; }
    }
}