using MediaToPacs.Core.Models;
using MediaToPacs.Core.Models.Ketluan;
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

        Task<UserDto> UploadCertToUser(CreateUserRequest input);

        Task<UserDto> GetUserCert(string cccd);


        Task<List<HisUserKySoResponse>> GetAllHisUserKySoAsync();
        Task<HisUserKySoResponse> GetByIdHisUserKySoAsync(Guid id);
        Task<HisUserKySoResponse> GetByHisUserKySoIdAsync(string hisUserId);
        Task<HisUserKySoResponse> CreateHisUserKySoAsync(HisUserKySoRequest input);
        Task<HisUserKySoResponse> UpdateHisUserKySoAsync(Guid id, HisUserKySoRequest input);
        Task<bool> DeleteHisUserKySoAsync(Guid id);

        void LoadClient(HttpClient client);
    }
}
