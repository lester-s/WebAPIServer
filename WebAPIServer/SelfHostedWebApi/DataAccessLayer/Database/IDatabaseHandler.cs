namespace SelfHostedWebApi.DataAccessLayer.Database
{
    public interface IDatabaseHandler
    {
        #region sync CRUD

        void Create<T>(T newItem) where T : new();

        void Read<T>() where T : new();

        void ReadById<T>(int id) where T : new();

        void Update<T>(T updatedItem) where T : new();

        void Delete<T>(T itemToDelete) where T : new();

        void DeleteById<T>(int id) where T : new();

        #endregion sync CRUD
    }
}