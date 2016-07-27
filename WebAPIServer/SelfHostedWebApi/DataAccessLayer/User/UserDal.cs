using SelfHostedWebApi.DataAccessLayer.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfHostedWebApi.DataAccessLayer.Database;

namespace SelfHostedWebApi.DataAccessLayer
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
            var query = $"select * from user where PSEUDO = '{pseudo}' and PASSWORD = '{password}'";
            return BaseDal.ExecuteTableRead<Model.User>(query)?.ElementAt(0);
        }
    }
}
