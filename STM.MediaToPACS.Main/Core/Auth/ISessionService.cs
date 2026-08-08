using MediaToPacs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Auths
{
    public interface ISessionService
    {
        void SetToken(string accessToken, string refreshToken, DateTime expiresAt);
        void OpenChangePasswordPage();
        UserInfo GetCurrentUser();
        bool IsLoggedIn { get; }
    }
}
