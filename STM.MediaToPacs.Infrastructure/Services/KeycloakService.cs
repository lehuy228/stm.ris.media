using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MediaToPacs.Core.Models;

namespace MediaToPacs.Infrastructure.Services
{
    public class KeycloakService 
    {
        public static async Task<KeycloakUserInfo> GetUserInfoFromToken(string accessToken)
        {
            string adminUser = "admin";
            string adminPassword = "Anphat123!";
            string clientId = "admin-cli";
            var baseurl = ConfigurationManager.AppSettings["Auth:URL"];
            var realm = ConfigurationManager.AppSettings["Auth:REALM"];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var userID = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var httpClient = new HttpClient();
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, $"{baseurl}/realms/master/protocol/openid-connect/token");
            tokenRequest.Content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("username", adminUser),
            new KeyValuePair<string, string>("password", adminPassword),
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", clientId),
            });

            var tokenResponse = await httpClient.SendAsync(tokenRequest);
            tokenResponse.EnsureSuccessStatusCode();
            var tokenJson = await tokenResponse.Content.ReadAsStringAsync();

            var tokenDoc = JsonDocument.Parse(tokenJson);
            string accessTokenAdmin = tokenDoc.RootElement.GetProperty("access_token").GetString();

            var userRequest = new HttpRequestMessage(HttpMethod.Get, $"{baseurl}/admin/realms/{realm}/users/{userID}");
            userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenAdmin);

            var userResponse = await httpClient.SendAsync(userRequest);
            userResponse.EnsureSuccessStatusCode();
            var userJson = await userResponse.Content.ReadAsStringAsync();

            var doc = JsonDocument.Parse(userJson);
            var root = doc.RootElement;

            var user = new KeycloakUserInfo
            {
                UserId = root.GetProperty("id").GetString(),
                Username = root.GetProperty("username").GetString(),
                Email = root.TryGetProperty("email", out var email) ? email.GetString() : null,
                FirstName = root.TryGetProperty("firstName", out var firstName) ? firstName.GetString() : null,
                LastName = root.TryGetProperty("lastName", out var lastName) ? lastName.GetString() : null
            };

            if (root.TryGetProperty("attributes", out var attributes))
            {
                if (attributes.TryGetProperty("employeeCode", out var empCode))
                    user.EmployeeCode = empCode[0].GetString();

                if (attributes.TryGetProperty("cccd", out var cccd))
                    user.CCCD = cccd[0].GetString();

                if (attributes.TryGetProperty("hisId", out var hisCode))
                    user.HISCode = hisCode[0].GetString();
            }

            return user;
        }
    }
}
