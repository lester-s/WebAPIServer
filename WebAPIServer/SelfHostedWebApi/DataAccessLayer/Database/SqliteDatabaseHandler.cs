using SelfHostedWebApi.HostConfig;
using SelfHostedWebApi.Model;
using System;
using System.Text;

namespace SelfHostedWebApi.DataAccessLayer.Database
{
    public class SqliteDatabaseHandler : IDatabaseHandler
    {
        private string dbConnection;

        public SqliteDatabaseHandler()
        {
            dbConnection = @"Data Source = D:\Projects\WebAPIServer\WebAPIServer\SelfHostedWebApi\DataAccessLayer\Database\ServerDatabase.s3db";
        }

        #region Sync CRUD

        public bool Create<T>(T newItem) where T : BaseModel, new()
        {
            string query = BuildCreateCommand<T>(newItem);
            return this.ExecuteNonQuery(query) == 1;
        }
        public bool Delete<T>(T itemToDelete) where T : BaseModel, new()
        {
            return DeleteById<T>(itemToDelete.Id);
        }

        public bool DeleteById<T>(int id) where T : BaseModel, new()
        {
            string query = BuildDeleteByIdQuery<T>(id);
            return ExecuteNonQuery(query) == 1;
        }

        private string BuildDeleteByIdQuery<T>(int id) where T : BaseModel, new()
        {
            var currentType = typeof(T);
            var tableName = currentType.Name.ToUpper();
            string query = $"DELETE FROM {tableName} WHERE {ServerStaticValues.IdName} = {id}";
            return query;
        }

        public bool Read<T>() where T : BaseModel, new()
        {
            throw new NotImplementedException();
        }

        public bool ReadById<T>(int id) where T : BaseModel, new()
        {
            throw new NotImplementedException();
        }

        public bool Update<T>(T updatedItem) where T : BaseModel, new()
        {
            throw new NotImplementedException();
        }

        #endregion Sync CRUD

        #region Command builders
        private string BuildCreateCommand<T>(T newItem) where T : BaseModel, new()
        {
            if (newItem == null)
            {
                throw new ArgumentNullException(nameof(newItem), "Argument needed in Sqlite database handler");
            }

            StringBuilder namesBuilder = new StringBuilder();
            StringBuilder valuesBuilder = new StringBuilder();

            var tableName = newItem.GetType().Name;

            var properties = newItem.GetType().GetProperties();
            var i = 0;
            foreach (var property in properties)
            {
                var currentValue = property.GetValue(newItem).ToString();
                var currentName = property.Name;

                if (!ServerStaticValues.IdName.Contains(currentName.ToUpper()))
                {
                    if (property.PropertyType == typeof(string))
                    {
                        namesBuilder.Append(currentName.ToUpper());
                        valuesBuilder.Append("'" + currentValue + "'");
                    }
                    else
                    {
                        namesBuilder.Append(currentName.ToUpper());
                        valuesBuilder.Append(currentValue);
                    }

                    if (i < properties.Length - 1)
                    {
                        namesBuilder.Append(", ");
                        valuesBuilder.Append(", ");
                    }
                }

                i++;
            }

            var result = string.Format("INSERT INTO {0} ({1}) VALUES ({2});", tableName, namesBuilder.ToString(), valuesBuilder.ToString());
            return result;
        }


        #endregion

        private int ExecuteNonQuery(string query)
        {
            var conn = System.Data.SQLite.Linq.SQLiteProviderFactory.Instance.CreateConnection();
            conn.ConnectionString = dbConnection;

            try
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return 0;
        }
    }
}