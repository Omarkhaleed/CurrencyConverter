using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using DomainLayer;

namespace WebApi
{
    [Route("Admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public AdminController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterUser Admin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new IdentityUser
                    {
                        UserName = Admin.UserName,
                        Email = Admin.Email,
                        PhoneNumber = Admin.phoneNumber

                    };
                    var result = await userManager.CreateAsync(user, Admin.Password);

                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return Ok("Success");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error");

                }
            }
            return UnprocessableEntity(ModelState);


        }



        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginUser info)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(info.Email);
                if (user != null && await userManager.CheckPasswordAsync(user, info.Password))
                {
                    var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
                        new ClaimsPrincipal(identity));
                    return Ok("Success");
                }
                else
                    return NotFound();
            }
            return NotFound();
        }

    }

} 
