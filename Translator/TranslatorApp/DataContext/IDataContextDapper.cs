using TranslatorApp.Models;

namespace TranslatorApp.DataContext
{
    public interface IDataContextDapper
    {
        public IEnumerable<T> LoadData<T>(SearchParameters searchParameters);
        public bool ExecuteSql(string sql, object parameters);
    }
}
