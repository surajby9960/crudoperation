using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace crudoperation.Repository
{
    public abstract class BaseAsyncRepository
    {
        public string sqlWriterConnectionString;
        public string sqlReaderConnectionString;
        private string databaseType;

        protected BaseAsyncRepository(IConfiguration configuration)
        {
            sqlWriterConnectionString = configuration.GetSection("DBInfo:WriterConnectionString").Value;
            sqlReaderConnectionString = configuration.GetSection("DBInfo:ReaderConnectionString").Value;
            databaseType = configuration.GetSection("DBInfo:DbType").Value;
        }
        internal DbConnection SqlWriterConnection
        {
            get
            {
                switch (databaseType)
                {
                    case "SqlServer":
                        return new System.Data.SqlClient.SqlConnection(sqlWriterConnectionString);
                    default:
                        return new SqlConnection(sqlWriterConnectionString);

                }
            }

        }

        internal DbConnection SqlReaderConnection
        {
            get
            {
                switch (databaseType)
                {
                    case "SqlServer":
                        return new System.Data.SqlClient.SqlConnection(sqlReaderConnectionString);
                    default:
                        return new SqlConnection(sqlReaderConnectionString);

                }
            }

        }
    }
}
