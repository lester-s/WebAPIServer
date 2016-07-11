using SelfHostedWebApi.Model;
using System;
using System.Collections.Generic;

namespace SelfHostedWebApi.BuisnessLayer
{
    public class UserBLL
    {
        private User[] users = new User[]
        {
            new User(1,"simon", 26),
            new User(2,"simon27", 27),
            new User(3,"simon28", 28)
        };

        public UserBLL()
        {
            AppHandler.Instance.ConnectMultipleUser(users);
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
    }
}