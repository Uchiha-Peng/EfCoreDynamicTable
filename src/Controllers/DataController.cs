using EfCoreDynamicTable.Data;
using EfCoreDynamicTable.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Api.Controllers
{
    /// <summary>
    /// 学生相关接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DataController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Table>> GetDefaultTable()
        {
            return await _context.Tables.ToListAsync();
        }

        [HttpGet]
        public async Task<IActionResult> GetDynamicTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new NoNullAllowedException();
            if (await CheckTableExists(_context, tableName))
            {
                return Ok(await _context.Tables.FromSqlRaw($"select * from `{tableName}`").ToListAsync());
            }
            return BadRequest($"不存在{tableName}数据表");
        }

        [HttpPost]
        public IActionResult CreateTable(string tableName)
        {
            MySqlParameter[] parameters = new MySqlParameter[] {
            new MySqlParameter("id", Guid.NewGuid()),
            new MySqlParameter("createAt", DateTime.Now),
            new MySqlParameter("tableName", tableName)
            };
            if (string.IsNullOrWhiteSpace(tableName))
                throw new NoNullAllowedException();
            _context.Database.ExecuteSqlRaw($"CREATE TABLE IF NOT EXISTS `{tableName}` LIKE `Tables`;");
            _context.Database.ExecuteSqlRaw($"INSERT INTO `{tableName}` (ID,TableName,CreateAt) VALUES(@id, @tableName,@createAt)", parameters);
            return Ok();

        }

        async Task<bool> CheckTableExists(DbContext context, string tableName)
        {
            var conn = context.Database.GetDbConnection();
            if (conn.State == ConnectionState.Closed)
                await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = $@"SELECT count(*) FROM information_schema.tables WHERE table_schema = @database AND table_name =@table";
            command.Parameters.Add(new MySqlParameter("database", conn.Database));
            command.Parameters.Add(new MySqlParameter("table", tableName));
            return (Int64)await command.ExecuteScalarAsync() > 0;
        }
    }
}
