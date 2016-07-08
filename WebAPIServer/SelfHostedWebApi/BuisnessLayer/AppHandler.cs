using SelfHostedWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostedWebApi.BuisnessLayer
{
    public class AppHandler
    {

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
            foreach(var user in users)
            {
                ConnectUser(user);
            }
        }

        public List<User> ConnectedUsers { get; set; }

        private bool NameAlreadyInUse(string nameToCheck)
        {
            if(string.IsNullOrWhiteSpace(nameToCheck))
            {
                throw new ArgumentNullException(nameof(nameToCheck), "a Value is missing");
            }

            var result = instance.ConnectedUsers.Where(u => u.Name == nameToCheck.ToLower());

            if(result != null && result.Count() >= 1 )
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
            if(NameAlreadyInUse(userToConnect.Name))
            {
                return false;
            }

            ConnectedUsers.Add(userToConnect);
            return true;
        }
    }
}
