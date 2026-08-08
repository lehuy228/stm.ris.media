using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Interfaces
{
    public interface IHisService
    {
        Task<bool> KiemTraDuTienAsync(string url, string maThanhToanChiTiet);
    }
}
