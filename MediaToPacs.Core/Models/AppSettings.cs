using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public class AppSettings
    {
        public CameraSettings CameraSettings { get; set; } = new CameraSettings();
        public ShortcutSettings ShortcutSettings { get; set; } = new ShortcutSettings();
    }
}
