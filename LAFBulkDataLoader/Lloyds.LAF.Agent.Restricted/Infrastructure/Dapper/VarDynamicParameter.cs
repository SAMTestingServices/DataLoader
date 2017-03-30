using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Lloyds.LAF.Agent.Restricted.Infrastructure.Dapper
{
    public class VarDynamicParameter : SqlMapper.IDynamicParameters
    {
        private readonly string parameterName;
        private readonly IEnumerable<string> values;
        private readonly SqlParameter[] additionalParameters;

        public VarDynamicParameter(string parameterName, IEnumerable<string> values, params SqlParameter[] additionalParameters)
        {
            this.parameterName = parameterName;
            this.values = values;
            this.additionalParameters = additionalParameters;
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            var sqlCommand = (SqlCommand)command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            var list = new List<Microsoft.SqlServer.Server.SqlDataRecord>();

            // Create an SqlMetaData object that describes our table type.
            Microsoft.SqlServer.Server.SqlMetaData[] tvpDefinition =
                {
                    new Microsoft.SqlServer.Server.SqlMetaData("n", SqlDbType.VarChar, 200)
                };

            foreach (var value in this.values)
            {
                // Create a new record, using the metadata array above.
                var rec = new Microsoft.SqlServer.Server.SqlDataRecord(tvpDefinition);
                rec.SetString(0, value); // Set the value.
                list.Add(rec); // Add it to the list.
            }

            // Add the table parameter.
            var p = sqlCommand.Parameters.Add(parameterName, SqlDbType.Structured);
            p.Direction = ParameterDirection.Input;
            p.TypeName = "[LAF].[VarcharListTableType]";
            p.Value = list;

            foreach (var additionalParameter in additionalParameters)
            {
                sqlCommand.Parameters.Add(additionalParameter);
            }
        }
    }
}