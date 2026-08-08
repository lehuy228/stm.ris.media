using MediaToPacs.Core.Auths;
using MediaToPacs.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;

namespace MediaToPacs.Infrastructure.Auths
{
    public class SessionService : ISessionService
    {
        private UserInfo _currentUser;

        public void SetToken(string accessToken, string refreshToken, DateTime expiresAt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);

            var name = token.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value;
            var familyName = token.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value;
            var email = token.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var username = token.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
            var roles = GetRolesFromJwt(token);

            _currentUser = new UserInfo
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt,
                FirstName = familyName,
                LastName = name,
                Email = email,
                Username = username,
                Roles = roles
            };
        }

        public void OpenChangePasswordPage()
        {
            var baseUrl = ConfigurationManager.AppSettings["Auth:URL"];
            var realm = ConfigurationManager.AppSettings["Auth:REALM"];

            if (string.IsNullOrWhiteSpace(baseUrl) || string.IsNullOrWhiteSpace(realm))
            {
                throw new InvalidOperationException("Auth:URL hoặc Auth:REALM chưa được cấu hình");
            }

            var changePasswordUrl = $"{baseUrl.TrimEnd('/')}/realms/{realm}/account/#/password";
            Process.Start(new ProcessStartInfo(changePasswordUrl) { UseShellExecute = true });
        }

        public UserInfo GetCurrentUser() => _currentUser;
        public bool IsLoggedIn => _currentUser != null;

        private List<string> GetRolesFromJwt(JwtSecurityToken jwt)
        {
            if (jwt.Payload.TryGetValue("realm_access", out var realmAccessObj))
            {
                if (realmAccessObj is JsonElement realmAccessElement &&
                    realmAccessElement.TryGetProperty("roles", out var rolesElement) &&
                    rolesElement.ValueKind == JsonValueKind.Array)
                {
                    return rolesElement.EnumerateArray()
                                       .Select(role => role.GetString())
                                       .Where(role => role != null)
                                       .ToList();
                }
            }

            return new List<string>();
        }
    }
}
