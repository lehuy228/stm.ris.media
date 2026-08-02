using System;

namespace MediaToPacs.Core.Models
{
    /// <summary>
    /// Kết quả POST /hl7/oru/resend — gửi lại ORU^R01 sang HIS (đồng bộ).
    /// ErrorCode/ErrorMessage lấy nguyên vẹn từ ACK của HIS (ERR-5/ERR-7) để người vận hành
    /// biết hướng xử lý; xem docs hl7 mục "Mã lỗi ERR-5 (Application Error Code)".
    /// </summary>
    public class OruResendResultDto
    {
        /// <summary>Mã phiếu chỉ định API echo lại (OruResendResult.OrderCode).</summary>
        public string orderCode { get; set; }

        public bool success { get; set; }

        /// <summary>
        /// HTTP 200: mã lỗi nghiệp vụ từ ACK của HIS (ERR-5: 1/2/4/8/16) hoặc mã kỹ thuật tầng giao tiếp
        /// (HTTP_EXCEPTION, INVALID_RESPONSE_JSON, NO_MSA, EMPTY_ACK).
        /// HTTP 404/409: mã lỗi phía RIS lấy từ envelope error.code (NOT_FOUND, REPORT_NOT_FINAL).
        /// </summary>
        public string errorCode { get; set; }

        /// <summary>Mô tả lỗi tương ứng errorCode (ERR-5/ERR-7 của HIS, hoặc error.message của RIS).</summary>
        public string errorMessage { get; set; }

        /// <summary>ACK gốc dạng Base64 (nếu API trả về) - chỉ dùng để log/đối chiếu.</summary>
        public string ackBase64 { get; set; }

        /// <summary>Mô tả tiếng Việt + hướng xử lý theo mã lỗi; null nếu không nhận ra mã.</summary>
        public string GetErrorCodeDescription()
        {
            if (string.IsNullOrWhiteSpace(errorCode))
                return null;

            switch (errorCode.Trim())
            {
                case "0":
                    return "Cập nhật kết quả thành công.";
                case "1":
                    return "HIS báo: không có mã thanh toán chi tiết trong gói tin.\n" +
                           "Kiểm tra lại mã phiếu chỉ định / mã chỉ định trước khi gửi lại.";
                case "2":
                    return "HIS báo: không lấy được dữ liệu (lỗi nội bộ phía HIS).\n" +
                           "Cần liên hệ HIS dò log phía họ, gửi lại nhiều lần không giải quyết được.";
                case "4":
                    return "HIS báo: không tìm thấy chỉ định tương ứng gói tin.\n" +
                           "HIS không khớp được mã phiếu chỉ định gửi sang với chỉ định nào bên họ.";
                case "8":
                    return "HIS báo: kết quả đã được ký số bên HIS.\n" +
                           "Gửi lại KHÔNG có tác dụng - HIS đã có kết quả đã ký cho chỉ định này.";
                case "16":
                    return "HIS báo: bệnh nhân đã ra viện nên HIS từ chối cập nhật.\n" +
                           "Cần xử lý ngoài luồng (liên hệ HIS), gửi lại không giải quyết được.";

                // Lỗi tầng giao tiếp RIS <-> HIS (chưa tới mức ACK nghiệp vụ) - thử lại thường có tác dụng.
                case "HTTP_EXCEPTION":
                    return "Không gọi được sang HIS (mất kết nối/timeout).\n" +
                           "Kiểm tra mạng và dịch vụ HIS rồi thử gửi lại.";
                case "INVALID_RESPONSE_JSON":
                    return "HIS trả về dữ liệu không đọc được (JSON không hợp lệ).\n" +
                           "Cần liên hệ HIS kiểm tra phía họ.";
                case "NO_MSA":
                    return "ACK của HIS không đọc được kết quả (không có MSA lẫn mã lỗi ERR-5).\n" +
                           "Cần liên hệ HIS kiểm tra bản tin phản hồi.";
                case "EMPTY_ACK":
                    return "HIS không trả về ACK.\n" +
                           "Kiểm tra dịch vụ HIS rồi thử gửi lại.";

                // Lỗi phía RIS (HTTP 404/409, khác tầng với ACK của HIS).
                case "NOT_FOUND":
                    return "RIS không tìm thấy y lệnh theo mã chỉ định này, hoặc y lệnh chưa có phiếu kết quả.";
                case "REPORT_NOT_FINAL":
                    return "Phiếu kết quả chưa ở trạng thái hoàn thành (Final) nên chưa có gì để gửi.\n" +
                           "Cần ký số/hoàn thành kết quả trước khi gửi sang HIS.";

                default:
                    return null;
            }
        }

        /// <summary>Nội dung hiển thị cho người dùng khi gửi lại thất bại.</summary>
        public string BuildFailureMessage()
        {
            var text = "Không gửi lại được kết quả sang HIS.";

            if (!string.IsNullOrWhiteSpace(errorCode))
                text += Environment.NewLine + "Mã lỗi HIS: " + errorCode.Trim();

            if (!string.IsNullOrWhiteSpace(errorMessage))
                text += Environment.NewLine + "Nội dung: " + errorMessage.Trim();

            var description = GetErrorCodeDescription();
            if (!string.IsNullOrWhiteSpace(description))
                text += Environment.NewLine + Environment.NewLine + description;

            return text;
        }
    }
}
