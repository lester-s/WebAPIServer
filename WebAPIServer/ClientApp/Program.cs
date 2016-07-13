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

        private static void Main(string[] args)
        {
            client.BaseAddress = new Uri("http://localhost:8080");
            AskCredentials();
        }

        private static void AskCredentials()
        {
            Console.WriteLine("Username:");
            var userName = Console.ReadLine();

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
            HttpResponseMessage resp = client.GetAsync("api/basic").Result;
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
            var resp = client.GetAsync("api/user/CreateUser").Result;
            resp.EnsureSuccessStatusCode();

            var result = resp.Content.ReadAsAsync<List<User>>().Result;

            foreach (var user in result)
            {
                Console.WriteLine($"utilisateur {user.Pseudo} est agé de x");
            }
        }
    }
}