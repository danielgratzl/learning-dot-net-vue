using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using WebApi.Models;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;

namespace WebApi.Repositiories
{
    public class AuthTokenRepository : IAuthTokenRepository
    {
        private readonly IConfiguration _config;

        public AuthTokenRepository(IConfiguration config)
        {
            _config = config;
        }
        
        public IDbConnection Connection => new MySqlConnection(_config.GetConnectionString("MyConnectionString"));

        public AuthToken FindAuthToken(string id, string pin)
        {
            using (var conn = Connection)
            {
                var sQuery = "SELECT Id, Pin, Email, DateVerified FROM AuthToken WHERE ID = @ID AND Pin = @Pin AND DateVerified IS NULL";
                return conn.QueryFirst<AuthToken>(sQuery, new { ID = id, Pin = pin });
            }
        }

        public AuthToken FindUnused(string email)
        {
            using (var conn = Connection)
            {
                var sQuery = "SELECT Id, Pin, Email, DateVerified FROM AuthToken WHERE Email = @Email AND DateVerified IS NULL";
                return conn.QueryFirstOrDefault<AuthToken>(sQuery, new { Email = email });
            }
        }

        public void Create(AuthToken authToken) {
            using (var conn = Connection)
            {
                conn.Insert(authToken);
            }
        }

        public void Update(AuthToken authToken) {
            using (var conn = Connection)
            {
                conn.Update(authToken);
            }
        }
    }
    
    
}