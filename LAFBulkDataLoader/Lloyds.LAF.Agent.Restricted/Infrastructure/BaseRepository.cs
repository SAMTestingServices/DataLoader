using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Lloyds.LAF.Agent.Restricted.Infrastructure
{
    internal abstract class BaseRepository
    {
        protected IEnumerable<T> RunQuery<T>(string storedProcedureName, object parameters)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                var result = connection.Query<T>(storedProcedureName, parameters, CommandType.StoredProcedure);
                return result;
            }
        }

        private static string GetConnectionString()
        {
            const string ConnectingName = "LAFRestricted";
            var connectionStringSetting = ConfigurationManager.ConnectionStrings[ConnectingName];
            return connectionStringSetting == null ? null : connectionStringSetting.ConnectionString;
        }
    }
}