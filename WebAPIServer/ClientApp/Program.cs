using SelfHostedWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            client.BaseAddress = new Uri("http://localhost:8080");

            ListAllUsers();
            UserById(1);

            Console.WriteLine("Press Enter to quit.");
            Console.ReadLine();
        }


        static void ListAllUsers()
        {
            HttpResponseMessage resp = client.GetAsync("api/basic").Result;
            resp.EnsureSuccessStatusCode();

            var products = resp.Content.ReadAsAsync<IEnumerable<User>>().Result;
            foreach (var p in products)
            {
                Console.WriteLine("{0} {1} {2}", p.Id, p.Name, p.Age);
            }
        }

        static void UserById(int id)
        {
            var resp = client.GetAsync(string.Format("api/basic/{0}", id)).Result;
            resp.EnsureSuccessStatusCode();

            var product = resp.Content.ReadAsAsync<User>().Result;
            Console.WriteLine("ID {0}: {1}", id, product.Name);
        }
    }
}
