using SelfHostedWebApi.DataAccessLayer.Database;
using System;
using System.Linq;

namespace SelfHostedWebApi.DataAccessLayer.UserDAL
{
    public class UserDal : IUserDal
    {
        public UserDal()
        {
            BaseDal = new SqliteBaseDal();
        }

        public UserDal(IDatabaseHandler baseDal)
        {
            BaseDal = baseDal;
        }

        public IDatabaseHandler BaseDal
        {
            get;
            set;
        }

        public Model.User UserExist(string pseudo, string password)
        {
            if (string.IsNullOrWhiteSpace(pseudo) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Missing arguments for user exists");
            }
            var query = $"select * from user where PSEUDO = '{pseudo}' and PASSWORD = '{password}'";
            return BaseDal.ExecuteTableRead<Model.User>(query)?.ElementAt(0);
        }
    }
}