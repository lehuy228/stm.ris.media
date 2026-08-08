using System;
using System.Collections.Generic;

namespace MediaToPacs.Core.Models
{
    /// <summary>
    /// GET /risv1/organizations/departments — trả trực tiếp mảng, không bọc ApiResponse
    /// </summary>
    public class OrganizationDto
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string nameEn { get; set; }
        public string aliases { get; set; }
        public string orgTypeCode { get; set; }
        public string orgTypeDisplay { get; set; }
        public int? parentId { get; set; }
        public string parentName { get; set; }
        public int? level { get; set; }
        public string path { get; set; }
    }


    /// <summary>
    /// GET /risv1/staff/colleagues — trả trực tiếp mảng, không bọc ApiResponse
    /// </summary>
    public class PractitionerListDto
    {
        public int id { get; set; }
        public string staffCode { get; set; }
        public string fullName { get; set; }
        public string displayName { get; set; }
        public string gender { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string nationality { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string addressJson { get; set; }
        public string nationalId { get; set; }
        public int? orgId { get; set; }
        public List<string> titleCodes { get; set; }
    }

    /// <summary>
    /// GET /risv1/devices — trả trực tiếp mảng, không bọc ApiResponse
    /// </summary>
    public class DeviceDto
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string deviceType { get; set; }
        public string imageBase64 { get; set; }
        public string modalityCode { get; set; }
        public int? locationId { get; set; }
        public string locationName { get; set; }
        public int? orgId { get; set; }
        public string orgName { get; set; }
        public string manufacturer { get; set; }
        public string modelName { get; set; }
        public string modelNumber { get; set; }
        public string serialNumber { get; set; }
        public string partNumber { get; set; }
        public string softwareVersion { get; set; }
        public string hardwareVersion { get; set; }
        public string hostName { get; set; }
        public string dicomAeTitle { get; set; }
        public bool isWorklistEnabled { get; set; }
        public string status { get; set; }
        public string operationalStatus { get; set; }
        public DateTime? purchaseDate { get; set; }
        public DateTime? installationDate { get; set; }
        public DateTime? lastMaintenanceDate { get; set; }
        public DateTime? nextMaintenanceDate { get; set; }
        public DateTime? warrantyExpiryDate { get; set; }
        public DateTime? decommissionDate { get; set; }
        public string notes { get; set; }
        public string metadataJson { get; set; }
        public string fhirId { get; set; }
        public bool isDeleted { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? modifiedAt { get; set; }
    }
}
