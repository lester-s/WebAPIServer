using SelfHostedWebApi.HostConfig;
using SelfHostedWebApi.Model;
using System;
using System.Collections.Generic;
using System.Data;
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

        public T Create<T>(T newItem) where T : BaseModel, new()
        {
            string query = BuildCreateCommand<T>(newItem);
            var id = this.ExecuteScalar(query);
            newItem.Id = id;
            return newItem;
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

        public List<T> Read<T>() where T : BaseModel, new()
        {
            string query = BuildReadCommand<T>();
            return this.ExecuteTableRead<T>(query);
        }

        public T ReadById<T>(int id) where T : BaseModel, new()
        {
            string query = BuildReadByIdCommand<T>(id);
            return ExecuteTableRead<T>(query)[0];
        }

        public bool Update<T>(T updatedItem) where T : BaseModel, new()
        {
            string query = BuildUpdateCommand<T>(updatedItem);
            return ExecuteNonQuery(query) == 1;
        }

        private string BuildUpdateCommand<T>(T updatedItem) where T : BaseModel, new()
        {
            if (updatedItem == null || updatedItem.Id <= 0)
            {
                throw new ArgumentException(nameof(updatedItem), "the item to update is wrong.");
            }

            var tableName = typeof(T).Name.ToUpper();
            var properties = typeof(T).GetProperties();
            StringBuilder stringBuilder = new StringBuilder();

            var i = 0;
            foreach (var property in properties)
            {
                stringBuilder.Append(property.Name.ToUpper() + " = ");

                if (property.PropertyType == typeof(string))
                {
                    stringBuilder.Append("'" + property.GetValue(updatedItem) + "'");
                }
                else
                {
                    stringBuilder.Append(property.GetValue(updatedItem));
                }

                if (i < properties.Length - 1)
                {
                    stringBuilder.Append(", ");
                }

                i++;
            }
            var query = $"UPDATE {tableName} SET {stringBuilder.ToString()} WHERE {ServerStaticValues.IdName} = {updatedItem.Id};";
            return query;
        }

        #endregion Sync CRUD

        #region Command builders

        private string BuildReadByIdCommand<T>(int id) where T : BaseModel, new()
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id), "Id must be > 0");
            }
            var baseQuery = BuildReadCommand<T>();
            baseQuery.TrimEnd(';');
            baseQuery += $" WHERE {ServerStaticValues.IdName} = {id};";
            return baseQuery;
        }

        private string BuildReadCommand<T>() where T : BaseModel, new()
        {
            var tableName = typeof(T).Name;
            var query = $"SELECT * FROM {tableName};";
            return query;
        }

        private string BuildDeleteByIdQuery<T>(int id) where T : BaseModel, new()
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id), "Id must be > 0");
            }
            var currentType = typeof(T);
            var tableName = currentType.Name.ToUpper();
            string query = $"DELETE FROM {tableName} WHERE {ServerStaticValues.IdName} = {id}";
            return query;
        }

        private string BuildCreateCommand<T>(T newItem) where T : BaseModel, new()
        {
            if (newItem == null)
            {
                throw new ArgumentNullException(nameof(newItem), "Argument needed in Sqlite database handler");
            }

            StringBuilder namesBuilder = new StringBuilder();
            StringBuilder valuesBuilder = new StringBuilder();

            var tableName = newItem.GetType().Name.ToUpper();

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

                    if (i < properties.Length - 2)
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

        #endregion Command builders

        private int ExecuteNonQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query), "Query is missing");
            }

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

        private int ExecuteScalar(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query), "Query must not be null or empty");
            }

            query += " select last_insert_rowid()";
            var conn = System.Data.SQLite.Linq.SQLiteProviderFactory.Instance.CreateConnection();
            conn.ConnectionString = dbConnection;

            try
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                var result = cmd.ExecuteScalar();
                return (int)result;
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

        private List<T> ExecuteTableRead<T>(string query) where T : BaseModel, new()
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query), "Query must not be null or empty");
            }

            var conn = System.Data.SQLite.Linq.SQLiteProviderFactory.Instance.CreateConnection();
            conn.ConnectionString = dbConnection;
            try
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                var reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                reader.Close();
                List<T> tableItems = ParseDataTableToItems<T>(dataTable);
                return tableItems;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return null;
        }

        private List<T> ParseDataTableToItems<T>(DataTable dataTable) where T : BaseModel, new()
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException(nameof(dataTable), "There is no table to parse");
            }

            var properties = typeof(T).GetProperties();
            List<string> propertiesNames = new List<string>();
            List<T> result = new List<T>();

            foreach (DataRow row in dataTable.Rows)
            {
                T newItem = new T();
                foreach (var property in properties)
                {
                    var propertyValue = row[property.Name.ToUpper()];
                    if (propertyValue.GetType() == typeof(Int64))
                    {
                        var int32propertyValue = Convert.ToInt32(propertyValue);
                        property.SetValue(newItem, int32propertyValue);
                        continue;
                    }
                    property.SetValue(newItem, propertyValue);
                }
                result.Add(newItem);
            }

            return result;
        }
    }
}