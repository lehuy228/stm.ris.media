using MediaToPacs.Core.Auths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintToPACSDemo.Utilities
{
    public static class ServiceLocator
    {
        public static ISessionService SessionService { get; set; }
    }
}
