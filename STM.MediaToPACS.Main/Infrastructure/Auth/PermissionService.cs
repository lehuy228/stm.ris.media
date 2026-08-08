using MediaToPacs.Core.Auths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Infrastructure.Auths
{
    public class PermissionService : IPermissionService
    {
        private readonly ISessionService _sessionService;

        public PermissionService(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public bool HasPermission(string permission)
        {
            var roles = _sessionService.GetCurrentUser()?.Roles;
            if (roles == null) return false;

            if (roles.Contains("admin") || roles.Contains("ris-admin"))
                return true;

            return roles.Contains(permission);
        }
    }
}
