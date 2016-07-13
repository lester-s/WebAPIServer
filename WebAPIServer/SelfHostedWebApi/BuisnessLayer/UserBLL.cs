﻿using SelfHostedWebApi.DataAccessLayer.Database;
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

        internal List<User> CreateUser()
        {
            var newUser = new User("s", "s", "admin");
            if (newUser == null)
            {
                throw new ArgumentNullException(nameof(newUser), "Argument cannot be null in UserBLL");
            }

            DbHandler.Create<User>(newUser);

            return null;
        }
    }
}