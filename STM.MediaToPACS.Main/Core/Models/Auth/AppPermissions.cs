using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public static class AppPermissions
    {
        public const string RisOrderCreate = "ris-order-create";
        public const string RisOrderEdit = "ris-order-edit";
        public const string RisOrderDelete = "ris-order-delete";
        public const string RisOrderList = "ris-order-list";
        public const string RisOrderDetail = "ris-order-detail";
        public const string RisOrderAdmin = "ris-order-admin";

        public const string RisPatientCreate = "ris-patient-create";
        public const string RisPatientEdit = "ris-patient-edit";
        public const string RisPatientDelete = "ris-patient-delete";
        public const string RisPatientList = "ris-patient-list";
        public const string RisPatientDetail = "ris-patient-detail";
        public const string RisPatientAdmin = "ris-patient-admin";

        public const string RisAdmin = "ris-admin";
        public const string RisConclusionAdmin = "ris-conclusion-admin";
        public const string CreateRealm = "create-realm";
        public const string DefaultRolesMaster = "default-roles-master";
        public const string RisDashboard = "ris-dashboard";

        public const string OfflineAccess = "offline_access";
        public const string UmaAuthorization = "uma_authorization";

        public const string RisWorklistList =  "ris-worklist-list";

        public const string Admin = "admin";
    }
}
