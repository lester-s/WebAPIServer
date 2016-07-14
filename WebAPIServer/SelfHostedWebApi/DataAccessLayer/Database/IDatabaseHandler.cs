using SelfHostedWebApi.Model;

namespace SelfHostedWebApi.DataAccessLayer.Database
{
    public interface IDatabaseHandler
    {
        #region sync CRUD

        T Create<T>(T newItem) where T : BaseModel, new();

        bool Read<T>() where T : BaseModel, new();

        bool ReadById<T>(int id) where T : BaseModel, new();

        bool Update<T>(T updatedItem) where T : BaseModel, new();

        bool Delete<T>(T itemToDelete) where T : BaseModel, new();

        bool DeleteById<T>(int id) where T : BaseModel, new();

        #endregion sync CRUD
    }
}