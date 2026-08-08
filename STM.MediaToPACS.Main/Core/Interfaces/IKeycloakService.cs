using MediaToPacs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Interfaces
{
    public interface IKeycloakService
    {
        Task<KeycloakUserInfo> GetUserInfoFromToken(string accessToken);
    }
}
