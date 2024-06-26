﻿using HangFireServiceAPI.DbContexts;
using HangFireServiceAPI.DTOs.Config;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HangFireServiceAPI.Repositories
{
    public interface IEmployeeRepository
    {
        Task<int> GetEmployeesCount();
        Task<bool> IsTableExists();
    }
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IOptionsMonitor<ConnectionStrings> _connectionString;

        public EmployeeRepository(ApplicationDbContext context, IOptionsMonitor<ConnectionStrings> connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<int> GetEmployeesCount()
        {
            try
            {
                var count = await _context.TblClaysysEmployees.CountAsync();

                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsTableExists()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString.CurrentValue.DbConnection))
                {
                    await connection.OpenAsync();

                    var tableName = nameof(_context.TblClaysysEmployees);
                    var schemaQuery = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName";

                    using (var command = new SqlCommand(schemaQuery, connection))
                    {
                        command.Parameters.AddWithValue("@TableName", tableName);

                        var result = await command.ExecuteScalarAsync();
                        var count = result is not null ? (int)result : 0;

                        return count > 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
