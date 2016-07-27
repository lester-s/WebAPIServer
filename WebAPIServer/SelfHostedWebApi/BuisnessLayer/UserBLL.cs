using SelfHostedWebApi.DataAccessLayer;
using SelfHostedWebApi.DataAccessLayer.Database;
using SelfHostedWebApi.DataAccessLayer.User;
using SelfHostedWebApi.Model;
using System;
using System.Collections.Generic;

namespace SelfHostedWebApi.BuisnessLayer
{
    public class UserBLL
    {

        private IUserDal dal;

        public IUserDal Dal
        {
            get
            {
                if (dal == null)
                {
                    throw new ArgumentNullException(nameof(dal), "A database handler is needed in BaseBLL");
                }

                return dal;
            }
            set { dal = value; }
        }

        public UserBLL(IUserDal _dbHandler)
        {
            dal = _dbHandler;
        }


        public UserBLL()
        {
            dal = new UserDal();
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
            return Dal.BaseDal.Read<User>();
        }

        internal User CreateUser(User userToCreate)
        {
            if (userToCreate == null)
            {
                throw new ArgumentNullException(nameof(userToCreate), "Argument cannot be null in UserBLL");
            }

            return Dal.BaseDal.Create<User>(userToCreate);
        }

        internal bool DeleteUser(User userToDelete)
        {
            if (userToDelete == null || userToDelete.Id <= 0)
            {
                throw new ArgumentException(nameof(userToDelete), "User needed for delete");
            }

            return Dal.BaseDal.Delete<User>(userToDelete);
        }

        internal bool DeleteUserById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id), "Id must be > 0");
            }

            return Dal.BaseDal.DeleteById<User>(id);
        }

        internal bool UpdateUser(User userToUpdate)
        {
            if (userToUpdate == null)
            {
                throw new ArgumentNullException(nameof(userToUpdate), "Argument cannot be null in UserBLL");
            }

            return Dal.BaseDal.Update<User>(userToUpdate);
        }

        public User UserExist(string pseudo, string password)
        {
            return dal.UserExist(pseudo, password);
        }
    }
}