using MediaToPacs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Interfaces
{
    public interface IStudyService
    {
        Task<List<StudyOr>> FindAllStudiesAsync(
             string patientId = null,
             string patientName = null,
             string modality = null,
             string studyID = null,
             string accessionNumber = null,
             string refPhyName = null,
             int limit = 10,
             DateTime? dateFrom = null,
             DateTime? dateTo = null);

        Task<bool> ModifyStudyAsync(
                string orthancStudyId,
                string patientName = null,
                string patientId = null,
                string patientGender = null,
                DateTime? patientBirthDate = null,
                string accessionNumber = null,
                string referringPhysicianName = null,
                DateTime? studyDate = null,
                DateTime? studyTime = null);

        Task<bool> ModifyPatientAsync(
                string patientOrthancId,
                string patientName = null,
                string patientId = null,
                string patientGender = null,
                DateTime? patientBirthDate = null);

        void LoadClient(HttpClient client);

        //Danh sách ảnh đánh dấu
        Task GetKeyImageObject(string studyInstanceUID, string folderPath);

        Task<(bool Exists, string PatientOrthancId)> CheckPatientExistsAsync(string patientId);
        //Task<MainDicomTags> GetPatientMainDicomTagsAsync(string patientId);

        Task<int> DeleteKOPRSeriesAsync(string studyId);

        Task<int> DeleteStudyAsync(string machidinh);
    }
}
