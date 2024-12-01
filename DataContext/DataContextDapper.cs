using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TranslatorApp.Models;

namespace TranslatorApp.DataContext
{
    public sealed class DataContextDapper : IDataContextDapper
    {
        private readonly IConfiguration _configuration;
        private readonly string _defaultConnectionString = "DefaultConnection";

        public DataContextDapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<T> LoadData<T>(SearchParameters searchParameters)
        {
            using var dbConnection = new SqlConnection(_configuration.GetConnectionString(_defaultConnectionString));
            return dbConnection.Query<T>("SearchTranslations", searchParameters, commandType: CommandType.StoredProcedure);
        }

        public bool ExecuteSql(string sql, object parameters)
        {
            using var dbConnection = new SqlConnection(_configuration.GetConnectionString(_defaultConnectionString));
            return dbConnection.Execute(sql, parameters) > 0;
        }
    }
}
