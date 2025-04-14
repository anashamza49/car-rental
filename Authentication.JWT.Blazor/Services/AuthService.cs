using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using Authentification.JWT.Service.DTOs;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Authentication.JWT.Blazor.Services
{
    public class AuthService(HttpClient http,
        AuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage)
    {

        public async Task<LoginResult> Login(LoginDto loginDto)
        {
            var response = await http.PostAsJsonAsync("api/auth/login", loginDto);

            if (!response.IsSuccessStatusCode)
                return new LoginResult { Success = false, Error = "Login failed" };

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result != null && !string.IsNullOrEmpty(result.Token))
            {
                await localStorage.SetItemAsync("authToken", result.Token);

                var role = ExtractRoleFromToken(result.Token);

                await localStorage.SetItemAsync("role", role);

                ((CustomAuthStateProvider)authStateProvider).NotifyUserAuthentication(result.Token);

                return new LoginResult { Success = true, Token = result.Token };
            }

            return new LoginResult { Success = false, Error = "Token not received from API." };
        }

        private string ExtractRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            return jsonToken?.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
        }


        public async Task<RegisterResult> Register(RegisterDto registerDto)
        {
            var response = await http.PostAsJsonAsync("api/auth/register", registerDto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new RegisterResult { Success = false, Error = error };
            }

            return new RegisterResult { Success = true };
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("authToken");
            ((CustomAuthStateProvider)authStateProvider).NotifyUserLogout();
        }

        public class LoginResult
        {
            public bool Success { get; set; }
            public string Error { get; set; }
            public string Token { get; set; }
        }

        public class RegisterResult
        {
            public bool Success { get; set; }
            public string Error { get; set; }
        }

        public class LoginResponse
        {
            public string Token { get; set; }
        }
    }
}
