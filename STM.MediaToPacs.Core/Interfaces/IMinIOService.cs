using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Interfaces
{
    public interface IMinIOService
    {
        Task<string> UploadUserFileAsync(string objectName, string base64Data, string contentType);

    }
}
