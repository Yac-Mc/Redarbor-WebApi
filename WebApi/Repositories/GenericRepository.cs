using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApi.Models.Queries;
using WebApi.Repositories.Queries;

namespace WebApi.Repositories
{
    public class GenericRepository<T> : BaseRepository, IGenericRepository<T> where T : class
    {
        private readonly ICommandText<T> _commandText;
        private readonly string _tableName;
        private string _keyName;
        private string _columnNames;

        public GenericRepository(IConfiguration configuration, ICommandText<T> commandText) : base(configuration)
        {
            _commandText = commandText;
            _tableName = GetTableName(typeof(T));
        }

        #region Methods privates
        private protected static string GetTableName(Type typeModel)
        {
            dynamic tableattr = typeModel.GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute");
            return $"{tableattr.Schema}.{tableattr.Name}";
        }

        private protected static string GetKeyName(T t)
        {
            dynamic keyatrr = t.GetType().GetProperties().First(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Any());
            return keyatrr.Name;
        }

        private protected static string GenerateListOfProperties(T t = null)
        {
            IEnumerable<PropertyInfo> listOfProperties = t == null ? typeof(T).GetProperties() : t.GetType().GetProperties();

            if (t == null)
            {
                return String.Join(",", (from prop in listOfProperties
                                         let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                         where Validators(attributes)
                                         select $"[{prop.Name}]").ToList());
            }

            return String.Join(",", (from prop in listOfProperties
                                     let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                     let type = prop.PropertyType.Name.Contains("Nullable") ? prop.PropertyType.GenericTypeArguments[0].Name : prop.PropertyType.Name
                                     let value = type.Equals("DateTime") ? Convert.ToDateTime(prop.GetValue(t, null)?.ToString()).ToString("dd/MM/yyyy") : prop.GetValue(t, null)?.ToString()
                                     where Validators(attributes, type, value)
                                     select $"[{prop.Name}]").ToList());
        }

        private protected async Task<long> GetTotalRows(Pagination<T> pagination, IDbConnection conn)
        {
            if ((pagination.Data.Any() && pagination.Data.Count() == pagination.PageSize) || (pagination.Filters == null || !pagination.Filters.Any()))
            {
                return await conn.QueryFirstAsync<long>(_commandText.SelectAllCount(_tableName, pagination.Filters));
            }
            return pagination.Data.Count();
        }

        private protected static bool Validators(object[] attributes, string type = null, string value = null)
        {
            if (type == null && value == null && (attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"))
            {
                return true;
            }
            else if (value != null && type != null
                && (attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore")
                && ((type.Equals("DateTime") && value != "01/01/0001") || (type.Contains("Int") && value != "0") || ((type.Equals("String") || type.Equals("Boolean")) && !string.IsNullOrEmpty(value))))
            {
                return true;
            }

            return false;
        }

        private protected T SetValuKey(T t, string id)
        {
            PropertyInfo propertyInfo = t.GetType().GetProperty(_keyName);
            propertyInfo.SetValue(t, Convert.ChangeType(id, propertyInfo.PropertyType), null);
            return t;
        }
        #endregion

        #region Methods publics
        public async Task<IEnumerable<T>> GetAllAsync(bool withColums = true)
        {
            _columnNames = withColums ? GenerateListOfProperties() : "*";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<T>(_commandText.SelectAll(_columnNames, _tableName));
            });
        }

        public async Task<IEnumerable<T>> SPGetAllByFilterAsync(string commandTextSP, object parameters)
        {
            IEnumerable<T> ts = null;

            await WithConnection(async conn =>
            {
                ts = await conn.QueryAsync<T>(commandTextSP, parameters);
            });

            return ts;
        }

        public async Task<T> GetByFilterAsync(List<Filter> filters, bool withColums = true)
        {
            _columnNames = withColums ? GenerateListOfProperties() : "*";
            return await WithConnection(async conn =>
            {
                return await conn.QueryFirstOrDefaultAsync<T>(_commandText.SelectAll(_columnNames, _tableName, filters));
            });
        }

        public async Task<IEnumerable<T>> GetAllByFilterAsync(List<Filter> filters, bool withColums = true)
        {
            _columnNames = withColums ? GenerateListOfProperties() : "*";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<T>(_commandText.SelectAll(_columnNames, _tableName, filters));
            });
        }

        public async Task<Pagination<T>> GetAllPaginationByFilterAsync(Pagination<T> pagination, bool withColums = true)
        {
            _columnNames = withColums ? GenerateListOfProperties() : "*";
            return await WithConnection(async conn =>
            {
                pagination.Data = await conn.QueryAsync<T>(_commandText.SelectAll(_columnNames, _tableName, pagination));
                pagination.TotalRows = await GetTotalRows(pagination, conn);
                decimal rounded = Math.Ceiling(pagination.TotalRows / Convert.ToDecimal(pagination.PageSize));
                pagination.PagesQuantity = Convert.ToInt32(rounded);
                return pagination;
            });
        }

        public async Task UpdateByIdAsync(T t, string id)
        {
            _keyName = GetKeyName(t);
            t = SetValuKey(t, id);
            _columnNames = GenerateListOfProperties(t);
            await WithConnection(async conn =>
            {
                await conn.ExecuteAsync(_commandText.Update(_columnNames, _tableName, _keyName), t);
            });
        }

        public async Task DeleteByIdAsync(List<Filter> filters)
        {
            await WithConnection(async conn =>
            {
                await conn.ExecuteAsync(_commandText.Delete(_tableName, filters));
            });
        }

        public async Task InsertAsync(T t)
        {
            _columnNames = GenerateListOfProperties(t);
            await WithConnection(async conn =>
            {
                await conn.ExecuteAsync(_commandText.Insert(_columnNames, _tableName), t);
            });
        }

        public async Task<int> InsertListAsync(IEnumerable<T> list)
        {
            _keyName = GetKeyName(list.ToList()[0]);
            List<string> lstColumnNames = GenerateListOfProperties().Split(',').ToList();
            lstColumnNames.RemoveAll(x => x.Equals($"[{_keyName}]"));
            _columnNames = String.Join(",", lstColumnNames);

            int recordByPage = 100000;
            int totalPages = list.Count() / recordByPage;

            await WithConnection(async conn =>
            {
                SqlTransaction trans = (SqlTransaction)conn.BeginTransaction();
                try
                {
                    for (int currentPage = 0; currentPage <= totalPages; currentPage++)
                    {
                        int skippedRows = currentPage * recordByPage;
                        var dataList = list.Skip(skippedRows).Take(recordByPage).ToList();
                        if (dataList.Count > 0)
                            await conn.ExecuteAsync(_commandText.Insert(_columnNames, _tableName), dataList, transaction: trans, 0);
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    trans.Dispose();
                }
            });

            return list.Count();
        }
        #endregion
    }
}
