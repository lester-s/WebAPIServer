using SelfHostedWebApi.BuisnessLayer;
using SelfHostedWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SelfHostedWebApi.ServerControllers
{
    public class UserController: ApiController
    {
        [HttpPost]
        [ActionName("ConnectUser")]
        public bool ConnectUser(User userToConnect)
        {
            var bll = new UserBLL();
            return bll.ConnectUser(userToConnect);
        }

        [HttpGet]
        [ActionName("GetConnecteduser")]
        public List<User> GetConnecteduser()
        {
            var bll = new UserBLL();
            return bll.GetConnectedUsers();
        }
    }
}
