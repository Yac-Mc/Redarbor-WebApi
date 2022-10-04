using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Models.Queries;

namespace WebApi.Repositories.Queries
{
    public class CommandText<T> : ICommandText<T>
    {
        #region GenericRepository
        public string SelectAll(string columnsNames, string tableName) => $"SELECT {columnsNames} FROM {tableName}";
        public string SelectAll(string columnsNames, string tableName, List<Filter> filters) => $"SELECT {columnsNames} FROM {tableName} {GenerateFilters(filters)}";
        public string SelectAll(string columnsNames, string tableName, Pagination<T> pagination) => $"SELECT {columnsNames} FROM {tableName} {GenerateFilters(pagination.Filters)}ORDER BY {pagination.Sort} {pagination.SortDirection} OFFSET {(pagination.Page - 1) * pagination.PageSize} ROWS FETCH NEXT {pagination.PageSize} ROWS ONLY";
        public string SelectAllCount(string tableName, List<Filter> filters) => $"SELECT COUNT(*) FROM {tableName} {GenerateFilters(filters)}";
        public string Update(string columnsNames, string tableName, string keyName) => $"UPDATE {tableName} {GenerateSetUpdate(columnsNames, keyName)}";
        public string Delete(string tableName, List<Filter> filters) => $"DELETE FROM {tableName} {GenerateFilters(filters)}";
        public string Insert(string columnNames, string tableName) => $"INSERT INTO {tableName} {GenerateInsertValues(columnNames)}";
        #endregion

        #region Methods privates
        private protected static string GenerateFilters(List<Filter> filters)
        {
            StringBuilder where = new StringBuilder();
            if (filters != null && filters.Any())
            {
                where.Append("WHERE ");
                foreach (var filter in filters.OrderBy(x => x.Order).ToList())
                {
                    switch (filter.Operator.Trim().ToUpper())
                    {
                        case "LIKE":
                            where.Append($"({filter.Column} {filter.Operator} '%{filter.Value}%') {filter.Condition} ");
                            break;
                        case "BETWEEN":
                            where.Append($"({filter.Column} {filter.Operator} '{filter.Value.Split('|')[0]}' AND '{filter.Value.Split('|')[1]}') {filter.Condition} ");
                            break;
                        case "IN":
                            where.Append($"({filter.Column} {filter.Operator} ('{filter.Value}')) {filter.Condition} ");
                            break;
                        default:
                            where.Append($"({filter.Column} {filter.Operator} '{filter.Value}') {filter.Condition} ");
                            break;
                    }
                }
            }

            return $"{where}";
        }
        private protected static string GenerateSetUpdate(string columnsNames, string keyName)
        {
            string where = "";
            StringBuilder set = new StringBuilder().Append("SET ");
            foreach (string column in columnsNames.Split(','))
            {
                if (column.Equals($"[{keyName}]"))
                {
                    where = $" WHERE {keyName}=@{keyName}";
                }
                else
                {
                    set.Append($"{column}=@{column.Replace('[', ' ').Replace(']', ' ').Trim()},");
                }
            }
            set.Remove(set.Length - 1, 1);
            set.Append(where);

            return $"{set}";
        }
        private protected static string GenerateInsertValues(string columnNames)
        {
            StringBuilder fields = new StringBuilder().Append('(');
            StringBuilder values = new StringBuilder().Append("VALUES (");
            foreach (string column in columnNames.Split(','))
            {
                fields.Append($"{column},");
                values.Append($"@{column.Replace('[', ' ').Replace(']', ' ').Trim()},");
            }
            fields.Remove(fields.Length - 1, 1).Append(')');
            values.Remove(values.Length - 1, 1).Append(')');

            return $"{fields} {values}";
        }
        #endregion
    }
}
