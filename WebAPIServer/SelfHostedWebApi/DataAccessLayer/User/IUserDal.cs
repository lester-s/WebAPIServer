using SelfHostedWebApi.BuisnessLayer;
using SelfHostedWebApi.DataAccessLayer.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostedWebApi.DataAccessLayer.User
{
    public interface IUserDal
    {
        IDatabaseHandler BaseDal { get; set; }
        Model.User UserExist(string pseudo, string password);
    }
}
