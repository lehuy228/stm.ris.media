using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Auths
{
    public interface IPermissionService
    {
        bool HasPermission(string permission);
    }
}
