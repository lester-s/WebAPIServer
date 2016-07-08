using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SelfHostedWebApi
{
    public class BasicController: ApiController
    {
        User[] users = new User[]
        {
            new User(1,"simon", 26),
            new User(2,"simon27", 27),
            new User(3,"simon28", 28)
        };

        public IEnumerable<User> GetAllUsers()
        {
            return users;
        }

        public User GetUserById(int id)
        {
            var result = users.FirstOrDefault(u => u.Id == id);
            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return result;
        }
    }
}
