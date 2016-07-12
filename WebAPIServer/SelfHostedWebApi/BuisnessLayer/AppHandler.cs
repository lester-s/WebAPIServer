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
            new User(1,"simon", 26, "123", "admin"),
            new User(2,"simon27", 27, "123", "reader"),
            new User(3,"simon28", 28, "123", "reader")
        };

        public bool IsAuthenticationActive { get; set; } = true;

        private static AppHandler instance;

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

        public List<User> ConnectedUsers { get; set; }

        private AppHandler()
        {
            //ConnectedUsers = new List<User>();
        }

        public bool ConnectUser(User userToConnect)
        {
            if (userToConnect == null || string.IsNullOrWhiteSpace(userToConnect.Name))
            {
                throw new ArgumentNullException(nameof(userToConnect), "Argument null in Apphandler");
            }

            if (NameAlreadyInUse(userToConnect.Name))
            {
                return false;
            }

            ConnectedUsers.Add(userToConnect);
            return true;
        }

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

        internal void ConnectMultipleUser(User[] users)
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users), "Argument is null in Apphandler");
            }

            foreach (var user in users)
            {
                ConnectUser(user);
            }
        }
    }
}