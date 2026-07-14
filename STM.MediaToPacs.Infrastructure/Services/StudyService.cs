using MediaToPacs.Core.Interfaces;
using MediaToPacs.Core.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MediaToPacs.Infrastructure.Services
{
    public class StudyService : IStudyService
    {
        private HttpClient _client;

        public StudyService()
        {
        }

        public void LoadClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<StudyOr>> FindAllStudiesAsync(
             string patientId = null,
             string patientName = null,
             string modality = null,
             string studyID = null,
             string accessionNumber = null,
             string refPhyName = null,
             int limit = 10,
             DateTime? dateFrom = null,
             DateTime? dateTo = null)
        {
            try
            {
                var queryDict = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(patientId))
                    queryDict["PatientID"] = "*" + patientId + "*";

                if (!string.IsNullOrWhiteSpace(patientName))
                    queryDict["PatientName"] = "*" + patientName + "*";

                if (!string.IsNullOrWhiteSpace(modality))
                    queryDict["ModalitiesInStudy"] = modality;

                if (!string.IsNullOrWhiteSpace(studyID))
                    queryDict["StudyID"] = studyID;

                if (!string.IsNullOrWhiteSpace(accessionNumber))
                    queryDict["AccessionNumber"] = "*" + accessionNumber + "*";

                if (!string.IsNullOrWhiteSpace(refPhyName))
                    queryDict["ReferringPhysicianName"] = refPhyName;

                if (dateFrom.HasValue && dateTo.HasValue)
                {
                    if (dateFrom.Value.Date == dateTo.Value.Date)
                    {
                        queryDict["StudyDate"] = dateFrom.Value.ToString("yyyyMMdd");
                    }
                    else
                    {
                        queryDict["StudyDate"] = dateFrom.Value.ToString("yyyyMMdd") + "-" + dateTo.Value.ToString("yyyyMMdd");
                    }
                }
                var query = new
                {
                    Level = "Study",
                    Expand = true,
                    Limit = limit,
                    Query = queryDict,
                    RequestedTags = new[] {
                        "ModalitiesInStudy",
                        "NumberOfStudyRelatedInstances",
                        "NumberOfStudyRelatedSeries"
                    },
                    OrderBy = new[] {
                    new {
                        Type = "Metadata",
                        Key = "LastUpdate",
                        Direction = "DESC"
                    }
                }
                };


                var content = new StringContent(
                   Newtonsoft.Json.JsonConvert.SerializeObject(query),
                   Encoding.UTF8,
                   "application/json");

                var response = await _client.PostAsync(ApiEndpoints.Orthanc.Find, content);
                response.EnsureSuccessStatusCode();


                var json = await response.Content.ReadAsStringAsync();


                var items = JsonSerializer.Deserialize<List<JsonElement>>(json);

                var result = new List<StudyOr>();

                foreach (var item in items)
                {
                    try
                    {
                        var study = new StudyOr
                        {
                            ID = GetString(item, "ID")
                        };

                        if (item.TryGetProperty("PatientMainDicomTags", out var patientTags))
                        {
                            study.PatientID = GetString(patientTags, "PatientID");
                            study.PatientName = GetString(patientTags, "PatientName");
                            study.PatientSex = GetString(patientTags, "PatientSex");
                            study.PatientDate = ParseDate(GetString(patientTags, "PatientBirthDate")) ?? DateTime.MinValue;
                        }

                        if (item.TryGetProperty("RequestedTags", out var requestedTags))
                        {

                            study.ModalitiesInStudy = GetString(requestedTags, "ModalitiesInStudy");
                        }
                        if (item.TryGetProperty("MainDicomTags", out var mainTags))
                        {
                            study.AccessionNumber = GetString(mainTags, "AccessionNumber");
                            study.PatientDescription = GetString(mainTags, "StudyDescription");
                            study.ReferD = GetString(mainTags, "ReferringPhysicianName");
                            study.StudyInstanceUID = GetString(mainTags, "StudyInstanceUID");

                            var studyDateStr = GetString(mainTags, "StudyDate");   // "20250813"
                            var studyTimeStr = GetString(mainTags, "StudyTime");   // "225349.000000"

                            DateTime studyDateTime = DateTime.MinValue;

                            if (!string.IsNullOrEmpty(studyDateStr))
                            {
                                if (DateTime.TryParseExact(studyDateStr, "yyyyMMdd",
                                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var datePart))
                                {
                                    if (!string.IsNullOrEmpty(studyTimeStr))
                                    {
                                        var timeOnly = studyTimeStr.Split('.')[0];
                                        TimeSpan timePart;
                                        if (TimeSpan.TryParseExact(timeOnly, "hhmmss", CultureInfo.InvariantCulture, out timePart))
                                        {
                                            studyDateTime = datePart.Date.Add(timePart);
                                        }
                                        else
                                        {
                                            studyDateTime = datePart;
                                        }
                                    }
                                    else
                                    {
                                        studyDateTime = datePart;
                                    }
                                }
                            }

                            study.StudyDate = studyDateTime;
                        }

                        result.Add(study);
                    }
                    catch (Exception ex)
                    {
                        string log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Error parsing study item: {ex}\n";

                    }
                }

                return result;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<bool> ModifyStudyAsync(
            string orthancStudyId,
            string patientName = null,
            string patientId = null,
            string patientGender = null,
            DateTime? patientBirthDate = null,
            string accessionNumber = null,
            string referringPhysicianName = null,
            DateTime? studyDate = null,
            DateTime? studyTime = null)
        {

            try
            {

                var replace = new Dictionary<string, string>();
               
                    if (!string.IsNullOrEmpty(patientName)) replace["PatientName"] = patientName;
                    if (!string.IsNullOrEmpty(patientGender)) replace["PatientSex"] = patientGender;
                    if (patientBirthDate.HasValue) replace["PatientBirthDate"] = patientBirthDate.Value.ToString("yyyyMMdd");
                    if (!string.IsNullOrEmpty(patientId)) replace["PatientID"] = patientId;
                    replace["AccessionNumber"] = accessionNumber;
                    if (!string.IsNullOrEmpty(referringPhysicianName)) replace["ReferringPhysicianName"] = referringPhysicianName;
                    if (studyDate.HasValue) replace["StudyDate"] = studyDate.Value.ToString("yyyyMMdd");
                    if (studyTime.HasValue) replace["StudyTime"] = studyTime.Value.ToString("HHmmss");
                
                var modifyBody = new
                {
                    Replace = replace,
                    Remove = new string[] { },
                    Keep = new string[] { },
                    KeepSource = false,
                    KeepLabels = true,
                    Force = true,
                    Synchronous = false
                };

                var json = JsonSerializer.Serialize(modifyBody);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(
                    $"{ApiEndpoints.Orthanc.Studies}/{orthancStudyId}{ApiEndpoints.Orthanc.Modify}", content);
                var result = await response.Content.ReadAsStringAsync();


                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private string GetString(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.String
                ? prop.GetString()
                : null;
        }

        private DateTime? ParseDate(string yyyymmdd)
        {
            return DateTime.TryParseExact(yyyymmdd, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var date)
                ? date
                : (DateTime?)null;
        }

      

        public async Task<bool> ModifyPatientAsync(
            string patientOrthancId,
            string patientName = null,
            string patientId = null,
            string patientGender = null,
            DateTime? patientBirthDate = null)
        {
            try
            {
                var replace = new Dictionary<string, string>();
                var patientExists = await CheckPatientExistsAsync(patientId: patientId);

                replace["OtherPatientIDs"] = "";
                if (!string.IsNullOrEmpty(patientName)) replace["PatientName"] = patientName;
                if (!string.IsNullOrEmpty(patientGender)) replace["PatientSex"] = patientGender;
                if (patientBirthDate.HasValue) replace["PatientBirthDate"] = patientBirthDate.Value.ToString("yyyyMMdd");
                if (!string.IsNullOrEmpty(patientId)) replace["PatientID"] = patientId;

                // Body gửi lên Orthanc
                var modifyBody = new
                {
                    Replace = replace,
                    Remove = new string[] { },
                    Keep = new string[] { },
                    KeepSource = false,
                    KeepLabels = true,
                    Force = true,
                    Synchronous = false
                };

                var json = JsonSerializer.Serialize(modifyBody);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Gọi API MODIFY
                var response = await _client.PostAsync(
                    $"{ApiEndpoints.Orthanc.Patients}/{patientExists.PatientOrthancId}{ApiEndpoints.Orthanc.Modify}", content);
                var result = await response.Content.ReadAsStringAsync();


                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<(bool Exists, string PatientOrthancId)> CheckPatientExistsAsync(string patientId)
        {
            var requestBody = new
            {
                Level = "Patient",
                Query = new { PatientID = patientId }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(ApiEndpoints.Orthanc.Find, content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var ids = JsonSerializer.Deserialize<string[]>(responseBody);

            if (ids != null && ids.Length > 0)
            {
                return (true, ids[0]);
            }

            return (false, null);
        }

        //public async Task<MainDicomTags> GetPatientMainDicomTagsAsync(string patientId)
        //{

        //    var response = await _client.GetAsync($"patients/{patientId}");
        //    response.EnsureSuccessStatusCode();

        //    string json = await response.Content.ReadAsStringAsync();

        //    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<OrthancPatientResponse>(json);

        //    return data.MainDicomTags;

        //}


        #region Lấy danh sách ảnh đánh giấu
        public async Task GetKeyImageObject(string studyId, string folderPath)
        {
            var jsonKo = await _client.GetStringAsync(
                $"{ApiEndpoints.Orthanc.Studies}/{studyId}{ApiEndpoints.Orthanc.StudySeries}");

            JArray seriesList = JArray.Parse(jsonKo);

            var koInstances = new List<string>();
            var prInstances = new List<string>();
            foreach (var series in seriesList)
            {
                string modality = series["MainDicomTags"]?["Modality"]?.ToString();
                if (modality == "KO")
                {
                    var instances = series["Instances"] as JArray;
                    if (instances != null)
                    {
                        foreach (var instanceId in instances)
                        {
                            koInstances.Add(instanceId.ToString());
                        }
                    }
                }
                else if (modality == "PR")
                {
                    var instances = series["Instances"] as JArray;
                    if (instances != null)
                    {
                        foreach (var instanceId in instances)
                        {
                            prInstances.Add(instanceId.ToString());
                        }
                    }
                }
            }

            HashSet<string> referencedSOPs = new HashSet<string>();
            foreach (var koInstance in koInstances)
            {
                // string urlUID = $"{baseUrl}/instances/{koInstance}/tags?simplify";
                // var clientUID = new HttpClient();
                // clientUID.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                //"Basic", Convert.ToBase64String(byteArray)
                // );

                var jsonUID = await _client.GetStringAsync(
                    $"{ApiEndpoints.Orthanc.Instances}/{koInstance}{ApiEndpoints.Orthanc.SimplifiedTags}");

                JObject obj = JObject.Parse(jsonUID);


                var evidenceSeq = obj["CurrentRequestedProcedureEvidenceSequence"] as JArray;

                if (evidenceSeq != null && evidenceSeq.Count > 0)
                {
                    foreach (var item in evidenceSeq)
                    {
                        var seriesArray = item["ReferencedSeriesSequence"] as JArray;
                        if (seriesArray == null) continue;

                        foreach (var series in seriesArray)
                        {
                            var sopArray = series["ReferencedSOPSequence"] as JArray;
                            if (sopArray == null) continue;

                            foreach (var sop in sopArray)
                            {
                                var sopUid = sop["ReferencedSOPInstanceUID"]?.ToString();
                                if (!string.IsNullOrEmpty(sopUid))
                                    referencedSOPs.Add(sopUid);
                            }
                        }
                    }
                }

                // 2. Nếu chưa có SOP nào, lấy từ ContentSequence
                if (referencedSOPs.Count == 0)
                {
                    var contentSeq = obj["ContentSequence"] as JArray;
                    if (contentSeq != null)
                    {
                        foreach (var content in contentSeq)
                        {
                            var sopArray = content["ReferencedSOPSequence"] as JArray;
                            if (sopArray == null) continue;

                            foreach (var sop in sopArray)
                            {
                                var sopUid = sop["ReferencedSOPInstanceUID"]?.ToString();
                                if (!string.IsNullOrEmpty(sopUid))
                                    referencedSOPs.Add(sopUid);
                            }
                        }
                    }
                }
            }

            foreach (var prInstance in prInstances)
            {
                // string urlUID = $"{baseUrl}/instances/{koInstance}/tags?simplify";
                // var clientUID = new HttpClient();
                // clientUID.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                //"Basic", Convert.ToBase64String(byteArray)
                // );

                var jsonUID = await _client.GetStringAsync(
                    $"{ApiEndpoints.Orthanc.Instances}/{prInstance}{ApiEndpoints.Orthanc.SimplifiedTags}");

                JObject obj = JObject.Parse(jsonUID);



                var seriesArray = obj["ReferencedSeriesSequence"] as JArray;
                if (seriesArray == null) continue;

                foreach (var series in seriesArray)
                {
                    var sopArray = series["ReferencedImageSequence"] as JArray;
                    if (sopArray == null) continue;

                    foreach (var sop in sopArray)
                    {
                        var sopUid = sop["ReferencedSOPInstanceUID"]?.ToString();
                        if (!string.IsNullOrEmpty(sopUid))
                            referencedSOPs.Add(sopUid);
                    }
                }




            }

            foreach (var referencedSOP in referencedSOPs)
            {
                string savePath = $"{folderPath}\\{referencedSOP}.dcm";
                if (File.Exists(savePath))
                {
                    continue;
                }



                // 1. Tìm Instance ID theo SOPInstanceUID
                var jsonFind = new JObject
                {
                    ["Level"] = "Instance",
                    ["Query"] = new JObject
                    {
                        ["SOPInstanceUID"] = referencedSOP
                    }
                };

                var content = new StringContent(jsonFind.ToString(), Encoding.UTF8, "application/json");
                //  http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                //"Basic", Convert.ToBase64String(byteArray)
                // );
                var findResp = await _client.PostAsync(ApiEndpoints.Orthanc.Find, content);
                findResp.EnsureSuccessStatusCode();

                var findResult = JArray.Parse(await findResp.Content.ReadAsStringAsync());

                if (findResult.Count == 0)
                {
                    return;
                }

                string instanceId = findResult[0].ToString();

                // Tải file DICOM
                var fileResp = await _client.GetAsync(
                    $"{ApiEndpoints.Orthanc.Instances}/{instanceId}{ApiEndpoints.Orthanc.InstanceFile}");
                fileResp.EnsureSuccessStatusCode();

                var bytes = await fileResp.Content.ReadAsByteArrayAsync();
                File.WriteAllBytes(savePath, bytes);
            }
        }
        public async Task<int> DeleteKOPRSeriesAsync(string studyId)
        {
            var json = await _client.GetStringAsync(
                $"{ApiEndpoints.Orthanc.Studies}/{studyId}{ApiEndpoints.Orthanc.StudySeries}");
            JArray seriesList = JArray.Parse(json);

            var seriesToDelete = new List<string>();

            foreach (var series in seriesList)
            {
                string modality = series["MainDicomTags"]?["Modality"]?.ToString();
                if (modality == "KO" || modality == "PR")
                {
                    string seriesId = series["ID"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(seriesId))
                        seriesToDelete.Add(seriesId);
                }
            }

            int deletedCount = 0;

            foreach (var seriesId in seriesToDelete)
            {
                try
                {
                    var resp = await _client.DeleteAsync($"{ApiEndpoints.Orthanc.Series}/{seriesId}");
                    if (resp.IsSuccessStatusCode)
                        deletedCount++;
                }
                catch
                {
                    // log warning nếu cần, không throw
                }
            }

            return deletedCount;
        }

        public async Task<int> DeleteStudyAsync(string machidinh)
        {
            var studies = await FindAllStudiesAsync(
                accessionNumber: machidinh,
                limit: 100
            );

            int count = 0;

            foreach (var study in studies)
            {
                bool ok = await ModifyStudyAsync(
                    orthancStudyId: study.ID,
                    accessionNumber: "" 
                );

                if (ok) count++;
            }

            return count;
        }


        #endregion
    }
}
