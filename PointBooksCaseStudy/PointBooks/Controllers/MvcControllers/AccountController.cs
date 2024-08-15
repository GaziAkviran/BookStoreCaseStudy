using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PointBooks.Models;
using PointBooks.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Text.Json;


namespace PointBooks.Controllers.MvcControllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AccountController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:7137/api/AccountApi/login", model);

            if (response.IsSuccessStatusCode)
            {
                var jsonToken = await response.Content.ReadAsStringAsync();
                var token = ExtractToken(jsonToken);

                var userRole = GetUserRoleFromToken(token);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                if (userRole == "1")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Geçersiz giriş bilgileri.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Account/Register.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:7137/api/AccountApi/register", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kayıt başarısız oldu.");
                return View(model);
            }
        }

        private string ExtractToken(string jsonToken)
        {
            var tokenObj = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonToken);
            return tokenObj["token"];
        }

        private string GetUserRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
            {
                throw new SecurityTokenMalformedException("JWT is not well-formed.");
            }

            var jwtToken = handler.ReadJwtToken(token);
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (roleClaim == null)
            {
                throw new Exception("Role claim not found in token.");
            }

            return roleClaim.Value;
        }
    }
}
