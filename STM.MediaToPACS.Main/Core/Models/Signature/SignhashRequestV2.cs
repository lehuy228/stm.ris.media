using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    /// <summary>
    /// Request cho api/signatures/signHash-pdf-v2 - kế thừa SignhashRequest, thêm OrderItemCode
    /// (mã chỉ định) để đính kèm vào lịch sử ký gửi cho riscore.
    /// </summary>
    public class SignhashRequestV2 : SignhashRequest
    {
        public string OrderItemCode { get; set; }
    }
}
