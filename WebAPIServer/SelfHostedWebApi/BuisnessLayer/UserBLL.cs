using SelfHostedWebApi.Model;
using System;
using System.Collections.Generic;

namespace SelfHostedWebApi.BuisnessLayer
{
    public class UserBLL
    {
        public UserBLL()
        {
        }

        public bool ConnectUser(User userToConnect)
        {
            if (userToConnect == null || string.IsNullOrWhiteSpace(userToConnect.Name))
            {
                throw new ArgumentNullException(nameof(userToConnect), "argument needed");
            }

            return AppHandler.Instance.ConnectUser(userToConnect);
        }

        public List<User> GetConnectedUsers()
        {
            return AppHandler.Instance.ConnectedUsers;
        }

        public List<User> GetAllUsers()
        {
            return AppHandler.Instance.users;
        }
    }
}