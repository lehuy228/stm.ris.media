using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Enums
{
    public enum Genders
    {
        [Description("Nam")]
        Male,

        [Description("Nữ")]
        Female,

        [Description("Khác")]
        Other
    }
}
