using MediaToPacs.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Interfaces
{
    /// <summary>
    /// Module RIS V2 — tích hợp nội bộ, không xác thực (base URL: /api/risv1).
    /// Xem docs/api/ris-v1.md.
    /// </summary>
    public interface IRisService2
    {
        /// <summary>
        /// Danh sách KTV, y tá cùng khoa với bác sĩ (theo staffCode).
        /// titleCodes mặc định ["ktv", "y_ta"] nếu không truyền.
        /// </summary>
        Task<List<PractitionerListDto>> GetColleaguesAsync(string staffCode, List<string> titleCodes = null);

        /// <summary>
        /// Danh sách device, lọc theo modality nếu có truyền.
        /// </summary>
        Task<List<DeviceDto>> GetDevicesAsync(string modality = null);
    }
}
