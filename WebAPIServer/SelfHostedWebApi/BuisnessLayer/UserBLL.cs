using SelfHostedWebApi.DataAccessLayer.Database;
using SelfHostedWebApi.Model;
using System;
using System.Collections.Generic;

namespace SelfHostedWebApi.BuisnessLayer
{
    public class UserBLL : BaseBLL
    {
        public UserBLL() : base(new SqliteDatabaseHandler())
        {
        }

        public UserBLL(IDatabaseHandler _dbHandler) : base(_dbHandler)
        {
        }

        public bool ConnectUser(User userToConnect)
        {
            if (userToConnect == null || string.IsNullOrWhiteSpace(userToConnect.Pseudo))
            {
                throw new ArgumentNullException(nameof(userToConnect), "argument needed");
            }

            return AppHandler.Instance.ConnectUser(userToConnect);
        }

        public List<User> GetConnectedUsers()
        {
            return AppHandler.Instance.users;
        }

        public List<User> GetAllUsers()
        {
            return AppHandler.Instance.users;
        }

        internal bool CreateUser(User userToCreate)
        {
            if (userToCreate == null)
            {
                throw new ArgumentNullException(nameof(userToCreate), "Argument cannot be null in UserBLL");
            }

            return DbHandler.Create<User>(userToCreate);
        }

        internal bool DeleteUser(User userToDelete)
        {
            if (userToDelete == null || userToDelete.Id <= 0)
            {
                throw new ArgumentException(nameof(userToDelete), "User needed for delete");
            }

            return DbHandler.Delete<User>(userToDelete);
        }

        internal bool DeleteUserById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id), "Id must be > 0");
            }

            return DbHandler.DeleteById<User>(id);
        }
    }
}