using SelfHostedWebApi.BuisnessLayer;
using SelfHostedWebApi.HostConfig;
using SelfHostedWebApi.Model;
using System.Collections.Generic;
using System.Net;
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
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, bll.ConnectUser(userToConnect));
        }

        [HttpGet]
        [OverrideAuthorization]
        [ServerAuthorizationFilter(Role = ServerStaticValues.AppRole.reader)]
        [ActionName("GetConnecteduser")]
        public HttpResponseMessage GetConnecteduser()
        {
            var bll = new UserBLL();
            return ControllerContext.Request.CreateResponse<List<User>>(HttpStatusCode.OK, bll.GetConnectedUsers());
        }

        [HttpGet]
        [ActionName("GetAlluser")]
        public HttpResponseMessage GetAlluser()
        {
            var bll = new UserBLL();
            return ControllerContext.Request.CreateResponse<List<User>>(HttpStatusCode.OK, bll.GetAllUsers());
        }

        [HttpPost]
        [ActionName("CreateUser")]
        public HttpResponseMessage CreateUser(User userToCreate)
        {
            var bll = new UserBLL();
            return ControllerContext.Request.CreateResponse<User>(HttpStatusCode.OK, bll.CreateUser(userToCreate));
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        public HttpResponseMessage DeleteUser(User userToDelete)
        {
            var bll = new UserBLL();
            return ControllerContext.Request.CreateResponse<bool>(HttpStatusCode.OK, bll.DeleteUser(userToDelete));
        }

        [HttpGet]
        [ActionName("DeleteUserById")]
        public HttpResponseMessage DeleteUserById(int id)
        {
            var bll = new UserBLL();
            return ControllerContext.Request.CreateResponse<bool>(HttpStatusCode.OK, bll.DeleteUserById(id));
        }

        [HttpPost]
        [ActionName("UpdateUser")]
        public HttpResponseMessage UpdateUser(User userToUpdate)
        {
            var bll = new UserBLL();
            var result = bll.UpdateUser(userToUpdate);
            return ControllerContext.Request.CreateResponse<bool>(HttpStatusCode.OK, result);
            //return ControllerContext.Request.CreateResponse<bool>(HttpStatusCode.OK, bll.UpdateUser(userToUpdate));
        }
    }
}