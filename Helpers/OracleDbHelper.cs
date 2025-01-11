using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace APIRest.Helpers
{
    public class OracleDbHelper
    {
        private readonly string _connectionString;

        public OracleDbHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            return new OracleConnection(_connectionString);
        }
    }
}
