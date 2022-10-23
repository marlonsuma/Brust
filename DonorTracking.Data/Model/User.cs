using Dapper.Contrib.Extensions;
using Microsoft.AspNet.Identity;

namespace DonorTracking.Data
{
    [Table("[Identity].[User]")]
    public class User  : IUser<int>
    {
        public User() { }

        public User(string userName)
        {
            UserName = userName;
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
    }

    [Table("[Identity].[UserClaim]")]
    public class UserClaim
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}