using MediaToPacs.Core.Models;
using MediaToPacs.Core.Models.Order;
using MediaToPacs.Core.Models.ServiceCatalog;
using MediaToPacs.Core.Models.Conclusion;
using MediaToPacs.Core.Models.Suggestion;
using MediaToPacs.Core.Models.Template;
using MediaToPacs.Core.Models.Device;
using MediaToPacs.Core.Models.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Interfaces
{
    public interface ISignatureService
    {
        Task<Dictionary<string, CertBO>> GetCertificateInfosAsync(string userId);
        Task<LoginResponseBO> GetToken(string userId);
        //Task<(string Bucket, string Key)> SignHashPdf(SignhashRequest request);
        Task<string> SignHashPdf(SignhashRequest request);
        Task<string> SignHashPdfV2(SignhashRequestV2 request);

        Task<UserDto> UploadCertToUser(CreateUserRequest input);

        Task<UserDto> GetUserCert(string cccd);


        Task<List<HisUserSignatureResponse>> GetAllHisUserKySoAsync();
        Task<HisUserSignatureResponse> GetByIdHisUserKySoAsync(Guid id);
        Task<HisUserSignatureResponse> GetByHisUserKySoIdAsync(string hisUserId);
        Task<HisUserSignatureResponse> CreateHisUserKySoAsync(HisUserSignatureRequest input);
        Task<HisUserSignatureResponse> UpdateHisUserKySoAsync(Guid id, HisUserSignatureRequest input);
        Task<bool> DeleteHisUserKySoAsync(Guid id);

        void LoadClient(HttpClient client);
    }
}
