using SelfHostedWebApi.BuisnessLayer;
using SelfHostedWebApi.HostConfig;
using SelfHostedWebApi.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace SelfHostedWebApi.ServerControllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        [ActionName("ConnectUser")]
        public HttpResponseMessage ConnectUser(User userToConnect)
        {
            var bll = new UserBLL();
            return ControllerContext.Request.CreateResponse(System.Net.HttpStatusCode.Accepted, bll.ConnectUser(userToConnect));
        }

        [HttpGet]
        [OverrideAuthorization]
        [ServerAuthorizationFilter(Role = ServerStaticValues.AppRole.reader)]
        [ActionName("GetConnecteduser")]
        public List<User> GetConnecteduser()
        {
            var bll = new UserBLL();
            return bll.GetConnectedUsers();
        }

        [HttpGet]
        [ActionName("GetAlluser")]
        public List<User> GetAlluser()
        {
            var bll = new UserBLL();
            return bll.GetAllUsers();
        }

        [HttpPost]
        [ActionName("CreateUser")]
        public User CreateUser(User userToCreate)
        {
            var bll = new UserBLL();
            return bll.CreateUser(userToCreate);
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        public bool DeleteUser(User userToDelete)
        {
            var bll = new UserBLL();
            return bll.DeleteUser(userToDelete);
        }

        [HttpGet]
        [ActionName("DeleteUserById")]
        public bool DeleteUserById(int id)
        {
            var bll = new UserBLL();
            return bll.DeleteUserById(id);
        }
    }
}