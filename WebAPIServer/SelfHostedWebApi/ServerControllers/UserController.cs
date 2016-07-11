using SelfHostedWebApi.BuisnessLayer;
using SelfHostedWebApi.HostConfig;
using SelfHostedWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SelfHostedWebApi.ServerControllers
{
    public class UserController: ApiController
    {
        [HttpPost]
        [ActionName("ConnectUser")]
        public HttpResponseMessage ConnectUser(User userToConnect)
        {
            var bll = new UserBLL();
            return ControllerContext.Request.CreateResponse(System.Net.HttpStatusCode.Accepted, bll.ConnectUser(userToConnect));
        }

        [HttpGet]
        [ServerAuthorizationFilter]
        [ActionName("GetConnecteduser")]
        public List<User> GetConnecteduser()
        {
            var bll = new UserBLL();
            return bll.GetConnectedUsers();
        }
    }
}
