using MediaToPacs.Core.Interfaces;
using Minio;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace MediaToPacs.Infrastructure.Auths
{
    public class MinioService : IMinIOService
    {
        private readonly MinioClient _minio;
        private readonly string buket;

        public MinioService()
        {
            var urlAPi = ConfigurationManager.AppSettings["MINIO:API"];
            var user = ConfigurationManager.AppSettings["MINIO:USER"];
            var pass = ConfigurationManager.AppSettings["MINIO:PASSWORD"];
            buket = ConfigurationManager.AppSettings["MINIO:BUKET"];
            _minio = (MinioClient)new MinioClient()
                .WithEndpoint(urlAPi)
                .WithCredentials(user, pass)
                .Build();
        }

        public async Task<string> UploadUserFileAsync(string objectName, string base64Data, string contentType)
        {
            try
            {
                // Đảm bảo bucket tồn tại
                bool found = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(buket));
                if (!found)
                {
                    await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(buket));
                }

                byte[] fileBytes = Convert.FromBase64String(base64Data);
                using (var stream = new MemoryStream(fileBytes))
                {

                    await _minio.PutObjectAsync(new PutObjectArgs()
                        .WithBucket(buket)
                        .WithObject(objectName)
                        .WithStreamData(stream)
                        .WithObjectSize(stream.Length)
                        .WithContentType(contentType)
                    );

                    string presignedUrl = await _minio.PresignedGetObjectAsync(
                    new PresignedGetObjectArgs()
                        .WithBucket(buket)
                        .WithObject(objectName)
                        .WithExpiry(60 * 60)); // 1 giờ
                    
                    return presignedUrl;
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
