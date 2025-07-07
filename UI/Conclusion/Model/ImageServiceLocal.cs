using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintToPACSDemo.UI.Conclusion.Model
{
    public class ImageServiceLocal 
    {
        public const string PrimaryKey = "Id";
        public int Id { get; set; }
        public string HospitalServiceCode { get; set; }
        public string HospitalServiceName { get; set; }
        public string MoHServiceCode { get; set; }
        public string MoHServiceName { get; set; }
        public string ServiceSampleDescription { get; set; }
        public string ServiceSampleConclusion { get; set; }
        public string SampleInstructions { get; set; }
        public ImageServiceLocal() { }
        public ImageServiceLocal(string HospitalServiceCode, string HospitalServiceName, string MoHServiceCode, string MoHServiceName, string ServiceSampleDescription, string ServiceSampleConclusion, string SampleInstructions)
        {
            this.HospitalServiceCode = HospitalServiceCode;
            this.HospitalServiceName = HospitalServiceName;
            this.MoHServiceCode = MoHServiceCode;
            this.MoHServiceName = MoHServiceName;
            this.ServiceSampleDescription = ServiceSampleDescription;
            this.ServiceSampleConclusion = ServiceSampleConclusion;
            this.SampleInstructions = SampleInstructions;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ImageServiceLocal other = (ImageServiceLocal)obj;
            return HospitalServiceCode == other.HospitalServiceCode;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (HospitalServiceCode != null ? HospitalServiceCode.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
