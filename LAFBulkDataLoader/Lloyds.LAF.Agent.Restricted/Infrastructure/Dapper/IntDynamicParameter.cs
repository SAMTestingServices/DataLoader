using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Lloyds.LAF.Agent.Restricted.Infrastructure.Dapper
{
    public class IntDynamicParameter : SqlMapper.IDynamicParameters
    {
        private readonly string parameterName;
        private readonly IEnumerable<int> numbers;

        public IntDynamicParameter(string parameterName, IEnumerable<int> numbers)
        {
            this.parameterName = parameterName;
            this.numbers = numbers;
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            var sqlCommand = (SqlCommand)command;
            sqlCommand.CommandType = CommandType.StoredProcedure;

            var numberList = new List<Microsoft.SqlServer.Server.SqlDataRecord>();

            // Create an SqlMetaData object that describes our table type.
            Microsoft.SqlServer.Server.SqlMetaData[] tvpDefinition =
                {
                    new Microsoft.SqlServer.Server.SqlMetaData("n", SqlDbType.Int)
                };

            foreach (var number in numbers)
            {
                // Create a new record, using the metadata array above.
                var rec = new Microsoft.SqlServer.Server.SqlDataRecord(tvpDefinition);
                rec.SetInt32(0, number); // Set the value.
                numberList.Add(rec); // Add it to the list.
            }

            // Add the table parameter.
            var p = sqlCommand.Parameters.Add(parameterName, SqlDbType.Structured);
            p.Direction = ParameterDirection.Input;
            p.TypeName = "[LAF].[IntegerListTableType]";
            p.Value = numberList;
        }
    }
}