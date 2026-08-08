namespace MediaToPacs.Core.Models
{
    public class SystemUpdateConfig
    {
        public string Link { get; set; }
        public string Token { get; set; }
        public string PacsUsername { get; set; }
        public string PacsPassword { get; set; }

        public bool HasUpdateConfiguration => !string.IsNullOrWhiteSpace(Link);
    }
}
