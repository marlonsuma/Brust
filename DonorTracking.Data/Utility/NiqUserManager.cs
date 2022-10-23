using Microsoft.AspNet.Identity;

namespace DonorTracking.Data
{
    public class NiqUserManager : UserManager<User, int>
    {
        public NiqUserManager(NiqUserStore store) : base(store)
        {
            PasswordValidator = new PasswordValidator
                {
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireNonLetterOrDigit = false,
                    RequireUppercase = true,
                    RequiredLength = 8
                };
        }
    }
}