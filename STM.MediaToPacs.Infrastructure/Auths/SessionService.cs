using MediaToPacs.Core.Auths;
using MediaToPacs.Core.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
