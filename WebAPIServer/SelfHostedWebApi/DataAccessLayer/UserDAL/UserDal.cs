using SelfHostedWebApi.DataAccessLayer.Database;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

            SqliteCommandData data = new SqliteCommandData();

            data.Query = $"select * from user where PSEUDO = @{nameof(pseudo)} and PASSWORD = @{nameof(password)}";
            data.parameters = new List<System.Data.SQLite.SQLiteParameter>();
            data.parameters.Add(new SQLiteParameter(nameof(pseudo), pseudo));
            data.parameters.Add(new SQLiteParameter(nameof(password), password));

            return BaseDal.ExecuteTableRead<Model.User>(data)?.ElementAt(0);
        }
    }
}