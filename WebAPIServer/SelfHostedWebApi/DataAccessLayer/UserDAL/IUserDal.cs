using SelfHostedWebApi.DataAccessLayer.Database;

namespace SelfHostedWebApi.DataAccessLayer.UserDAL
{
    public interface IUserDal
    {
        IDatabaseHandler BaseDal { get; set; }

        Model.User UserExist(string pseudo, string password);
    }
}