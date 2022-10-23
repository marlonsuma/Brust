using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace DonorTracking.Data
{
    public class NiqSignInManager: SignInManager<User, int>
    {
        public NiqSignInManager(NiqUserManager userManager, IAuthenticationManager authenticationManager) : 
            base(userManager, authenticationManager) { }

        /// <summary>
        /// Sign in the user in using the user name and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        public async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent)
        {
            if (UserManager == null)
            {
                return SignInStatus.Failure;
            }
            User user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInStatus.Failure;
            }

            if (!await UserManager.CheckPasswordAsync(user, password)) return SignInStatus.Failure;
            
            await SignInAsync(user,isPersistent,false);

            return SignInStatus.Success;
        }
    }
}