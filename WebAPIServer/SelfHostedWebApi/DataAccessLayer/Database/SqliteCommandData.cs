using System.Collections.Generic;
using System.Data.SQLite;

namespace SelfHostedWebApi.DataAccessLayer.Database
{
    public class SqliteCommandData
    {
        public string Query { get; set; }
        public List<SQLiteParameter> Parameters { get; set; }
    }
}