using SelfHostedWebApi.BuisnessLayer;
using SelfHostedWebApi.HostConfig;
using SelfHostedWebApi.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace SelfHostedWebApi.DataAccessLayer.Database
{
    public class SqliteBaseDal : IDatabaseHandler
    {
        private string dbConnection;

        public SqliteBaseDal()
        {
            dbConnection = @"Data Source = " + AppHandler.Instance.Settings.DatabaseUrl;
        }

        #region Sync CRUD

        public T Create<T>(T newItem) where T : BaseModel, new()
        {
            var queryData = BuildCreateCommand<T>(newItem);
            var id = this.ExecuteScalar(queryData);
            newItem.Id = id;
            return newItem;
        }

        public bool Delete<T>(T itemToDelete) where T : BaseModel, new()
        {
            return DeleteById<T>(itemToDelete.Id);
        }

        public bool DeleteById<T>(int id) where T : BaseModel, new()
        {
            var queryData = BuildDeleteByIdQuery<T>(id);
            return ExecuteNonQuery(queryData) == 1;
        }

        public List<T> Read<T>() where T : BaseModel, new()
        {
            var query = BuildReadCommand<T>();
            return this.ExecuteTableRead<T>(query);
        }

        public T ReadById<T>(int id) where T : BaseModel, new()
        {
            var command = BuildReadByIdCommand<T>(id);
            return ExecuteTableRead<T>(command)[0];
        }

        public bool Update<T>(T updatedItem) where T : BaseModel, new()
        {
            var queryData = BuildUpdateCommand<T>(updatedItem);
            return ExecuteNonQuery(queryData) == 1;
        }

        private SqliteCommandData BuildUpdateCommand<T>(T updatedItem) where T : BaseModel, new()
        {
            if (updatedItem == null || updatedItem.Id <= 0)
            {
                throw new ArgumentException(nameof(updatedItem), "the item to update is wrong.");
            }

            var result = new SqliteCommandData();
            result.Parameters = new List<SQLiteParameter>();
            var tableName = typeof(T).Name.ToUpper();
            var properties = typeof(T).GetProperties();
            StringBuilder stringBuilder = new StringBuilder();

            var i = 0;
            foreach (var property in properties)
            {
                if (property.Name.ToUpper() == ServerStaticValues.IdName)
                {
                    continue;
                }

                stringBuilder.Append(property.Name.ToUpper() + " = ");

                stringBuilder.Append($"@{property.Name}");

                result.Parameters.Add(new SQLiteParameter(property.Name, property.GetValue(updatedItem)));

                if (i < properties.Length - 2)
                {
                    stringBuilder.Append(", ");
                }

                i++;
            }
            result.Query = $"UPDATE {tableName} SET {stringBuilder.ToString()} WHERE {ServerStaticValues.IdName} = @{nameof(updatedItem.Id)};";
            result.Parameters.Add(new SQLiteParameter(nameof(updatedItem.Id), updatedItem.Id));
            return result;
        }

        #endregion Sync CRUD

        #region Command builders

        private SqliteCommandData BuildReadByIdCommand<T>(int id) where T : BaseModel, new()
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id), "Id must be > 0");
            }

            var result = new SqliteCommandData();
            var baseBuilder = BuildReadCommand<T>();
            var baseQuery = baseBuilder.Query;
            result.Parameters = baseBuilder.Parameters;

            baseQuery.TrimEnd(';');
            baseQuery += $" WHERE {ServerStaticValues.IdName} = @{nameof(id)};";

            result.Parameters.Add(new SQLiteParameter(nameof(id), id));
            return result;
        }

        public SqliteCommandData BuildReadCommand<T>() where T : BaseModel, new()
        {
            var result = new SqliteCommandData();
            var tableName = typeof(T).Name;
            //var query = $"SELECT * FROM {tableName};";
            var query = $"SELECT * FROM @{nameof(tableName)};";

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter($"{nameof(tableName)}", tableName));

            result.Parameters = parameters;
            result.Query = query;
            return result;
        }

        private SqliteCommandData BuildDeleteByIdQuery<T>(int id) where T : BaseModel, new()
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id), "Id must be > 0");
            }
            var result = new SqliteCommandData();
            var currentType = typeof(T);
            var tableName = currentType.Name.ToUpper();
            result.Query = $"DELETE FROM {tableName} WHERE {ServerStaticValues.IdName} = @{nameof(id)}";
            result.Parameters = new List<SQLiteParameter>();
            result.Parameters.Add(new SQLiteParameter(nameof(id), id));
            return result;
        }

        private SqliteCommandData BuildCreateCommand<T>(T newItem) where T : BaseModel, new()
        {
            if (newItem == null)
            {
                throw new ArgumentNullException(nameof(newItem), "Argument needed in Sqlite database handler");
            }

            var result = new SqliteCommandData();
            result.Parameters = new List<SQLiteParameter>();

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
                    namesBuilder.Append(currentName.ToUpper());
                    valuesBuilder.Append($"@{currentName}");
                    result.Parameters.Add(new SQLiteParameter(currentName, currentValue));

                    if (i < properties.Length - 2)
                    {
                        namesBuilder.Append(", ");
                        valuesBuilder.Append(", ");
                    }
                }

                i++;
            }

            result.Query = string.Format("INSERT INTO {0} ({1}) VALUES ({2});", tableName, namesBuilder.ToString(), valuesBuilder.ToString());
            return result;
        }

        #endregion Command builders

        public int ExecuteNonQuery(SqliteCommandData data)
        {
            if (string.IsNullOrWhiteSpace(data.Query))
            {
                throw new ArgumentNullException(nameof(data), "Query is missing");
            }

            var conn = System.Data.SQLite.Linq.SQLiteProviderFactory.Instance.CreateConnection();
            conn.ConnectionString = dbConnection;
            try
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = data.Query;
                cmd.Parameters.AddRange(data.Parameters.ToArray());
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

        public int ExecuteScalar(SqliteCommandData queryData)
        {
            if (string.IsNullOrWhiteSpace(queryData.Query))
            {
                throw new ArgumentNullException(nameof(queryData), "Query must not be null or empty");
            }

            queryData.Query += " select last_insert_rowid()";
            var conn = System.Data.SQLite.Linq.SQLiteProviderFactory.Instance.CreateConnection();
            conn.ConnectionString = dbConnection;

            try
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = queryData.Query;
                cmd.Parameters.AddRange(queryData.Parameters.ToArray());
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

        public List<T> ExecuteTableRead<T>(SqliteCommandData queryData) where T : BaseModel, new()
        {
            if (string.IsNullOrWhiteSpace(queryData.Query))
            {
                throw new ArgumentNullException(nameof(queryData), "Query must not be null or empty");
            }

            var conn = System.Data.SQLite.Linq.SQLiteProviderFactory.Instance.CreateConnection();
            conn.ConnectionString = dbConnection;
            try
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = queryData.Query;
                cmd.Parameters.AddRange(queryData.Parameters.ToArray());
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