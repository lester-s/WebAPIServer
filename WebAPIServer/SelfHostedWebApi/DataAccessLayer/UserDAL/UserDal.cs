using NLog;
using SelfHostedWebApi.DataAccessLayer.Database;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace SelfHostedWebApi.DataAccessLayer.UserDAL
{
    public class UserDal : IUserDal
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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

            SqliteCommandData data = new SqliteCommandData();

            data.Query = $"select * from user where PSEUDO = @{nameof(pseudo)} and PASSWORD = @{nameof(password)}";
            data.Parameters = new List<System.Data.SQLite.SQLiteParameter>();
            data.Parameters.Add(new SQLiteParameter(nameof(pseudo), pseudo));
            data.Parameters.Add(new SQLiteParameter(nameof(password), password));

            var user = BaseDal.ExecuteTableRead<Model.User>(data);
            return user?.Count <= 0 ? null : user.ElementAt(0);
        }
    }
}