using SelfHostedWebApi.DataAccessLayer;
using SelfHostedWebApi.DataAccessLayer.Database;
using SelfHostedWebApi.HostConfig.Settings;
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
            new User("simon", "123", "admin"),
            new User("simon27", "123", "reader"),
            new User("simon28", "123", "reader")
        };

        public bool IsAuthenticationActive
        {
            get
            {
                return Settings.IsAuthActivated;
            }
        }

        private static AppHandler instance;

        public static AppHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    ServerSettings settings = FileToJsonHelper.ReadJsonFile<ServerSettings>("serverSettings.txt");
                    instance = new AppHandler();
                    instance.Settings = settings;
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
            if (userToConnect == null || string.IsNullOrWhiteSpace(userToConnect.Pseudo))
            {
                throw new ArgumentNullException(nameof(userToConnect), "Argument null in Apphandler");
            }

            if (NameAlreadyInUse(userToConnect.Pseudo))
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

            var result = instance.ConnectedUsers.Where(u => u.Pseudo == nameToCheck.ToLower());

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

        public ServerSettings Settings { get; private set; }

        public User UserExist(string pseudo, string password)
        {
            var bll = new UserBLL(new UserDal(new SqliteBaseDal()));
            return bll.UserExist(pseudo, password);
        }
    }
}