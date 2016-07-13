using SelfHostedWebApi.HostConfig;
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

        public bool Create<T>(T newItem) where T : new()
        {
            string query = BuildCreateCommand<T>(newItem);
            return this.ExecuteNonQuery(query) == 1;
        }

        private string BuildCreateCommand<T>(T newItem) where T : new()
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

                if (!ServerStaticValues.IDsNames.Contains(currentName.ToUpper()))
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

        public void Delete<T>(T itemToDelete) where T : new()
        {
            throw new NotImplementedException();
        }

        public void DeleteById<T>(int id) where T : new()
        {
            throw new NotImplementedException();
        }

        public void Read<T>() where T : new()
        {
            throw new NotImplementedException();
        }

        public void ReadById<T>(int id) where T : new()
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T updatedItem) where T : new()
        {
            throw new NotImplementedException();
        }

        #endregion Sync CRUD

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