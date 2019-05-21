using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ClientApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ClientApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private JwtSettings _jwtSettings;
        private JwtSettings _jwtSettings2;
        private JwtSettings _jwtSettings3;

        public AuthorizeController(IOptionsMonitor<JwtSettings> jwtSettings, IOptionsSnapshot<JwtSettings> jwtSettings2, ILogger<AuthorizeController> logger)
        {
            _jwtSettings = jwtSettings.CurrentValue;
            jwtSettings.OnChange((jwt, s) =>
            {
                _jwtSettings3 = jwt;
                logger.LogWarning(nameof(IOptionsMonitor<JwtSettings>) + " is changed.[{msg}]", s);
            });
            _jwtSettings2 = jwtSettings2.Value;
        }

        [HttpGet]
        public IActionResult Show()
        {
            var m1_1 = string.Copy(_jwtSettings.SecretKey);
            var m2_1 = string.Copy(_jwtSettings2.SecretKey);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));
            var m1_2 = string.Copy(_jwtSettings.SecretKey);
            var m2_2 = string.Copy(_jwtSettings2.SecretKey);
            return Ok(new { m1_1, m2_1, m1_2, m2_2, _jwtSettings3 });
        }

        [HttpPost]
        public IActionResult Token(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.UserName != "admin" || model.Password != "123456")
                {
                    #region original jwt

                    var claims = new Claim[]
                    {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Role, "admin")
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        _jwtSettings.Issuer,
                        _jwtSettings.Audience,
                        claims,
                        null,
                        DateTime.Now.AddMinutes(30),
                        creds);
                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });

                    #endregion

                    #region costom jwt

                    #endregion
                }
            }
            return BadRequest();
        }
    }
}
