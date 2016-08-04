using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostedWebApi.DataAccessLayer.Database
{
    public class SqliteCommandData
    {
        public string Query { get; set; }
        public List<SQLiteParameter> parameters { get; set; }
    }
}
