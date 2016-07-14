using SelfHostedWebApi.Model;
using System.Collections.Generic;

namespace SelfHostedWebApi.DataAccessLayer.Database
{
    public interface IDatabaseHandler
    {
        #region sync CRUD

        T Create<T>(T newItem) where T : BaseModel, new();

        List<T> Read<T>() where T : BaseModel, new();

        T ReadById<T>(int id) where T : BaseModel, new();

        bool Update<T>(T updatedItem) where T : BaseModel, new();

        bool Delete<T>(T itemToDelete) where T : BaseModel, new();

        bool DeleteById<T>(int id) where T : BaseModel, new();

        #endregion sync CRUD
    }
}