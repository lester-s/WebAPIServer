using SelfHostedWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SelfHostedWebApi.BuisnessLayer
{
    public class AppHandler
    {
        public List<User> users = new List<User>
        {
            new User(1,"simon", 26, "123"),
            new User(2,"simon27", 27, "123"),
            new User(3,"simon28", 28, "123")
        };

        private static AppHandler instance;

        private AppHandler()
        {
            ConnectedUsers = new List<User>();
        }

        public static AppHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppHandler();
                }
                return instance;
            }
        }

        internal void ConnectMultipleUser(User[] users)
        {
            foreach (var user in users)
            {
                ConnectUser(user);
            }
        }

        public List<User> ConnectedUsers { get; set; }

        private bool NameAlreadyInUse(string nameToCheck)
        {
            if (string.IsNullOrWhiteSpace(nameToCheck))
            {
                throw new ArgumentNullException(nameof(nameToCheck), "a Value is missing");
            }

            var result = instance.ConnectedUsers.Where(u => u.Name == nameToCheck.ToLower());

            if (result != null && result.Count() >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ConnectUser(User userToConnect)
        {
            if (NameAlreadyInUse(userToConnect.Name))
            {
                return false;
            }

            ConnectedUsers.Add(userToConnect);
            return true;
        }
    }
}