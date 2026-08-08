namespace MediaToPacs.Core.Models
{
    /// <summary>
    /// Một quyền dùng DICOM viewer của bác sĩ — GET /risv1/staff/viewer-accesses?staffCode=...
    /// Xem docs/api/ris-v1.md §3b.
    /// </summary>
    public class PractitionerViewerAccessDto
    {
        public int id { get; set; }
        public int viewerConfigId { get; set; }
        public string viewerName { get; set; }

        /// <summary>Vd: "MedDream", "Ohif"</summary>
        public string viewerType { get; set; }
        public bool isDefault { get; set; }
    }

    /// <summary>
    /// Response của POST /risv1/order-items/by-placer-code/{code}/viewer-link - trả trực tiếp,
    /// không bọc "data". Xem docs/api/ris-v1.md §9d.
    /// </summary>
    public class ViewerLinkResult
    {
        public string launchUrl { get; set; }
    }
}
