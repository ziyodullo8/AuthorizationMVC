using AuthorizationMVC.Models.Domain;
using AuthorizationMVC.Models.DTO;
using AuthorizationMVC.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthorizationMVC.Repositories.Implementaion
{
    public class UserAuthService : IUserAuthService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public UserAuthService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<Status> LOginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid username";
                return status;
            }
            //we will match password
            if(!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid password";
                return status;
            }
            var sigInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if(sigInResult.Succeeded)
            {
                var userRoles=await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName)
                };
                foreach(var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Logged in succesfully";
                return status;
            }
            else if(sigInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User locked out";
                return status;
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Eror on loggin in";
                return status;
            }
        }


        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<Status> RegistrationAsynnc(RegistrationModel model)
        {
          var status= new Status();
          var userExists=await userManager.FindByNameAsync(model.UserName);
            if (userExists!=null)
            {
                status.StatusCode = 0;
                status.Message = "User already exists";
                return status;
            }

            ApplicationUser user = new ApplicationUser
            {
                SecurityStamp=Guid.NewGuid().ToString(),
                Name = model.Name,
                Email = model.Email,
                UserName = model.UserName,
                EmailConfirmed=true,

            };
            var result=await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode=0;
                status.Message = "User createion failed";
                return status;
            }
            //role managment
            if(!await roleManager.RoleExistsAsync(model.Role)) 
                await roleManager.CreateAsync(new IdentityRole(model.Role));
            if(await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Message = "User has registered succeessfully";
            return status;
        }
    }
}
