using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Repositories
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        protected BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        private string GetConnectionString()
        {
            return _connectionString ?? _configuration.GetConnectionString("DefaultConnection");
        }

        // use for buffered queries that return a type
        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            await using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();
            return await getData(connection);
        }

        // use for buffered queries that do not return a type
        protected async Task WithConnection(Func<IDbConnection, Task> getData)
        {
            await using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();
            await getData(connection);
        }

        // use for non-buffered queries that return a type
        protected async Task<TResult> WithConnection<TRead, TResult>(Func<IDbConnection, Task<TRead>> getData, Func<TRead, Task<TResult>> process)
        {
            await using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();
            var data = await getData(connection);
            return await process(data);
        }
    }
}
