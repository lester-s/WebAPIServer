using SelfHostedWebApi.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ClientApp
{
    internal class Program
    {
        private static HttpClient client = new HttpClient();
        private static string userName = string.Empty;

        private static void Main(string[] args)
        {
            client.BaseAddress = new Uri("http://localhost:8080");
            AskCredentials();
        }

        private static void AskCredentials()
        {
            Console.WriteLine("Username:");
            userName = Console.ReadLine();

            Console.WriteLine("Password:");
            var password = Console.ReadLine();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{userName.Trim().ToLower()}:{password.Trim()}")));

            try
            {
                CreateUser();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            AskCredentials();
        }

        private static void GetConnectedUser()
        {
            var resp = client.GetAsync("api/user/GetConnecteduser").Result;
            resp.EnsureSuccessStatusCode();

            var result = resp.Content.ReadAsAsync<List<User>>().Result;

            foreach (var user in result)
            {
                Console.WriteLine($"utilisateur {user.Pseudo} est agé de x");
            }
        }

        private static void ListAllUsers()
        {
            HttpResponseMessage resp = client.GetAsync("api/user/GetAlluser").Result;
            resp.EnsureSuccessStatusCode();

            var products = resp.Content.ReadAsAsync<IEnumerable<User>>().Result;
            foreach (var p in products)
            {
                Console.WriteLine("{0} {1}", p.Id, p.Pseudo);
            }
        }

        private static void UserById(int id)
        {
            var resp = client.GetAsync(string.Format("api/basic/{0}", id)).Result;
            resp.EnsureSuccessStatusCode();

            var product = resp.Content.ReadAsAsync<User>().Result;
            Console.WriteLine("ID {0}: {1}", id, product.Pseudo);
        }

        private static void ConnectUser(string userName, string password)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{userName.Trim().ToLower()}:{password.Trim()}")));
            var resp = client.GetAsync("api/user/connectuser").Result;
            resp.EnsureSuccessStatusCode();

            var result = resp.Content.ReadAsAsync<String>().Result;
            Console.WriteLine(result);
        }

        private static void GetAllUser()
        {
            var resp = client.GetAsync("api/user/GetAlluser").Result;
            resp.EnsureSuccessStatusCode();

            var result = resp.Content.ReadAsAsync<List<User>>().Result;

            foreach (var user in result)
            {
                Console.WriteLine($"utilisateur {user.Pseudo} est agé de x");
            }
        }

        private static void CreateUser()
        {
            var userToCreate = new User(userName, "s", "admin") { Id = 2 };

            var resp = client.PostAsJsonAsync<User>("api/user/CreateUser", userToCreate).Result;

            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = resp.Content.ReadAsStringAsync().Result;
                Console.WriteLine(error);
                return;
            }
            var result = resp.Content.ReadAsAsync<User>().Result;

            Console.WriteLine($"utilisateur {result.Pseudo} est agé de x");
        }

        private static async void DeleteUser()
        {
            var userToDelete = new User("s", "s", "admin") { Id = 2 };
            var result = await client.PostAsJsonAsync<User>("api/user/DeleteUser", userToDelete);
            Console.WriteLine(result.StatusCode);
        }

        private static void UpdateUser()
        {
            var userToCreate = new User(userName, "s", "admin") { Id = 3 };

            var resp = client.PostAsJsonAsync<User>("api/user/UpdateUser", userToCreate).Result;
            //resp.EnsureSuccessStatusCode();

            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = resp.Content.ReadAsStringAsync().Result;
                Console.WriteLine(error);
                return;
            }
            var result = resp.Content.ReadAsAsync<bool>().Result;
            Console.WriteLine(result);
        }
    }
}