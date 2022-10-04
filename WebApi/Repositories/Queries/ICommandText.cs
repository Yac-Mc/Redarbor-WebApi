using System.Collections.Generic;
using WebApi.Models.Queries;

namespace WebApi.Repositories.Queries
{
    public interface ICommandText<T>
    {
        #region GenericRepository

        /// <summary>
        /// Query to get all data from a table, select (columnNames) From (tableName).
        /// </summary>
        /// <param name="columnsNames"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string SelectAll(string columnsNames, string tableName);

        /// <summary>
        /// Query to get all the data from a table, Select (columnsNames) From (tableName) (pagination).
        /// </summary>
        /// <param name="columnsNames"></param>
        /// <param name="tableName"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        string SelectAll(string columnsNames, string tableName, Pagination<T> pagination);

        /// <summary>
        /// Query to get all the data in a table using filters with Where, Select (columnsNames) From (tableName) Where (Filters).
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filters"></param>
        /// <param name="columnsNames"></param>
        /// <returns></returns>
        string SelectAll(string columnsNames, string tableName, List<Filter> filters);

        /// <summary>
        /// Query to get the count of a table using filters with Where, Select (columnsNames) From (tableName) Where (filters).
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        string SelectAllCount(string tableName, List<Filter> filters);


        /// <summary>
        /// Statement to update the data in a table, UPDATE (tableName) SET (columnNames).
        /// </summary>
        /// <param name="columnsNames"></param>
        /// <param name="tableName"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        string Update(string columnsNames, string tableName, string keyName);
        string Delete(string tableName, List<Filter> filters);
        string Insert(string columnNames, string tableName);

        #endregion
    }
}