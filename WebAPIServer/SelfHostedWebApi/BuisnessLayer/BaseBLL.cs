using SelfHostedWebApi.DataAccessLayer.Database;
using System;

namespace SelfHostedWebApi.BuisnessLayer
{
    public class BaseBLL
    {
        private IDatabaseHandler dbHandler;

        public IDatabaseHandler DbHandler
        {
            get
            {
                if (dbHandler == null)
                {
                    throw new ArgumentNullException(nameof(dbHandler), "A database handler is needed in BaseBLL");
                }

                return dbHandler;
            }
            set { dbHandler = value; }
        }

        public BaseBLL(IDatabaseHandler _dbHandler)
        {
            dbHandler = _dbHandler;
        }
    }
}