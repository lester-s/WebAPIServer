using NLog;
using SelfHostedWebApi.BuisnessLayer;
using SelfHostedWebApi.DataAccessLayer.Database;
using SelfHostedWebApi.DataAccessLayer.UserDAL;
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
        private UserBLL bll = new UserBLL(new UserDal(new SqliteBaseDal()));
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        [ActionName("ConnectUser")]
        public HttpResponseMessage ConnectUser(User userToConnect)
        {
            logger.Debug("enter connectuser point in user controller");
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, bll.ConnectUser(userToConnect));
        }

        [HttpGet]
        [OverrideAuthorization]
        [ServerAuthorizationFilter(Role = ServerStaticValues.AppRole.reader)]
        [ActionName("GetConnecteduser")]
        public HttpResponseMessage GetConnecteduser()
        {
            logger.Debug("enter GetConnecteduser point in user controller");
            return ControllerContext.Request.CreateResponse<List<User>>(HttpStatusCode.OK, bll.GetConnectedUsers());
        }

        [HttpGet]
        [ActionName("GetAlluser")]
        public HttpResponseMessage GetAlluser()
        {
            logger.Debug("enter GetAlluser point in user controller");
            return ControllerContext.Request.CreateResponse<List<User>>(HttpStatusCode.OK, bll.GetAllUsers());
        }

        [HttpPost]
        [OverrideAuthorization]
        [ServerAuthorizationFilter(Role = ServerStaticValues.AppRole.noConnectionNeeded)]
        [ActionName("CreateUser")]
        public HttpResponseMessage CreateUser(User userToCreate)
        {
            logger.Debug("enter CreateUser point in user controller");
            return ControllerContext.Request.CreateResponse<User>(HttpStatusCode.OK, bll.CreateUser(userToCreate));
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        public HttpResponseMessage DeleteUser(User userToDelete)
        {
            logger.Debug("enter DeleteUser point in user controller");
            return ControllerContext.Request.CreateResponse<bool>(HttpStatusCode.OK, bll.DeleteUser(userToDelete));
        }

        [HttpGet]
        [ActionName("DeleteUserById")]
        public HttpResponseMessage DeleteUserById(int id)
        {
            logger.Debug("enter DeleteUserById point in user controller");
            return ControllerContext.Request.CreateResponse<bool>(HttpStatusCode.OK, bll.DeleteUserById(id));
        }

        [HttpPost]
        [ActionName("UpdateUser")]
        public HttpResponseMessage UpdateUser(User userToUpdate)
        {
            logger.Debug("enter UpdateUser point in user controller");
            return ControllerContext.Request.CreateResponse<bool>(HttpStatusCode.OK, bll.UpdateUser(userToUpdate));
        }
    }
}