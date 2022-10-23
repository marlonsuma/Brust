using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNet.Identity;

namespace DonorTracking.Data
{
    public class NiqUserStore: IUserStore<User, int>,
        IUserPasswordStore<User, int>,
        IUserSecurityStampStore<User, int>,
        IUserClaimStore<User, int>,
        IDisposable
    {
        private bool _disposed;
        private readonly IDbConnection _db;

        public NiqUserStore(IRepositoryConfigurationProvider config)
        {
            _db = new SqlConnection(config.ConnectionString);
        }

        public async Task CreateAsync(User user)
        {
            await _db.InsertAsync(user);

        }

        public async Task DeleteAsync(User user)
        {
            await _db.DeleteAsync(user);
        }

        public async Task<User> FindByIdAsync(int userId)
        {
            string sql = "Select * FROM [Identity].[User] WHERE Id = @userId";

            return await _db.QueryFirstOrDefaultAsync<User>(sql, new { userId });
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            string sql = "Select * FROM [Identity].[User] WHERE UserName = @userName";

            return await _db.QueryFirstOrDefaultAsync<User>(sql, new { userName });
        }

        public async Task<string> GetPasswordHashAsync(User user)
        {
            string sql = "SELECT PasswordHash FROM [Identity].[User] WHERE UserName = @userName";

            return await _db.QueryFirstOrDefaultAsync<string>(sql, new { user.UserName });
        }

        public async Task<string> GetSecurityStampAsync(User user)
        {
            string sql = "SELECT SecurityStamp FROM [Identity].[User] WHERE UserName = @userName";

            return await _db.ExecuteScalarAsync<string>(sql, new { user.UserName });
        }

        public async Task<bool> HasPasswordAsync(User user)
        {
            string sql = "SELECT CASE WHEN [Password] is not null THEN 1 ELSE 0 END  FROM [Identity].[User] " +
                         "WHERE UserName = @userName";

            return await _db.ExecuteScalarAsync<bool>(sql, new { user.UserName });
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            string sql = "UPDATE [Identity].[User] SET PasswordHash = @passwordHash " +
                         "WHERE UserName = @userName";
            await _db.ExecuteAsync(sql, new { passwordHash, user.UserName });
        }

        public async Task SetSecurityStampAsync(User user, string stamp)
        {
            user.SecurityStamp = stamp;
            string sql = "UPDATE [Identity].[User] SET SecurityStamp = @securityStamp " +
                         "WHERE UserName = @userName";
            await _db.ExecuteAsync(sql, new { SecurityStamp = stamp, UserName = user.UserName });
        }

        public async Task UpdateAsync(User user)
        {
            await _db.UpdateAsync(user);
        }

        public async Task<IList<Claim>> GetClaimsAsync(User user)
        {
            string sql = "SELECT * FROM [Identity].[UserClaim] " +
                         "WHERE UserId = @userId";
            List<Claim> claims = new List<Claim>();

            foreach (dynamic c in await _db.QueryAsync(sql, new { UserId = user.Id }))
            {
                claims.Add(new Claim(c.ClaimType, c.ClaimValue));
            }

            return claims;
        }

        public async Task AddClaimAsync(User user, Claim claim)
        {
            UserClaim newClaim = new UserClaim
                {
                    UserId = user.Id,
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                };
            await _db.InsertAsync(newClaim);
        }

        public async Task RemoveClaimAsync(User user, Claim claim)
        {
            string sql = "DELETE FROM [Identity].[UserClaim] "+
                         "WHERE UserId = @userId AND ClaimType = @claimType";
            await _db.ExecuteAsync(sql, new { UserId = user.Id, ClaimType = claim.Type });
        }

        public void Dispose()
         {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (_db.State == ConnectionState.Open)
                {
                    _db.Close();
                }
                _db.Dispose();
            }

            _disposed = true;
        }
    }
}