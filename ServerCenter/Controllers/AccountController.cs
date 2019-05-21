using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServerCenter.Models;

namespace ServerCenter.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignIn signIn)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = signIn.Name
                };
                var result = await _userManager.CreateAsync(user, signIn.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(
                        user,
                        new AuthenticationProperties
                        {
                            IsPersistent = true
                        });
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["Message"] = string.Join("", result.Errors.Select(error => error.Description));
                }
            }
            else
            {
                ViewData["Message"] = string.Join("", ModelState.Where(model => model.Value.ValidationState == ModelValidationState.Invalid).SelectMany(model => model.Value.Errors.Select(error => error.ErrorMessage)));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl = null)
        {
            if (HttpContext.User.Identity.IsAuthenticated) return Redirect(ReturnUrl);
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(login.Name, login.Password, isPersistent : true, lockoutOnFailure : false);
                if (result.Succeeded)
                {
                    return Redirect(login.ReturnUrl);
                }
                else
                {
                    ViewData["Message"] = "登录失败。";
                }
            }
            else
            {
                ViewData["Message"] = string.Join("", ModelState.Where(model => model.Value.ValidationState == ModelValidationState.Invalid).SelectMany(model => model.Value.Errors.Select(error => error.ErrorMessage)));
            }
            return View();
        }

        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            //DoLogout();
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        #region Original IdentityServer

        private void DoLogin()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Role, "admin")
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));
        }

        private void DoLogout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        #endregion
    }
}
