using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public class CameraSettings
    {
        public string VideoInputDevice { get; set; }
        public string InphutFormat { get; set; }

        public string FrameRate { get; set; }
        public string OutputFormat { get; set; }
        public string AudioInputDevice { get; set; }
        public string AudioInputFormat { get; set; }
        public string AudioInputLine { get; set; }

        public bool Greyscale { get; set; } = false;
        public bool Invert { get; set; } = false;
        public bool FlipX { get; set; } = false;
        public bool FlipY { get; set; } = false;

        public bool EnableZoom { get; set; } = false;
        public double Zoom { get; set; } = 1.0;
        public int ZoomShiftX { get; set; } = 0;
        public int ZoomShiftY { get; set; } = 0;

        public bool EnablePan { get; set; } = false;
        public int PanStartTime { get; set; } = 5000;
        public int PanStopTime { get; set; } = 15000;
        public int PanSourceLeft { get; set; } = 0;
        public int PanSourceWidth { get; set; } = 640;
        public int PanSourceHeight { get; set; } = 480;
        public int PanSourceTop { get; set; } = 0;
        public int PanDestLeft { get; set; } = 0;
        public int PanDestWidth { get; set; } = 320;
        public int PanDestHeight { get; set; } = 480;
        public int PanDestTop { get; set; } = 0;

        public bool EnableLiveRotation { get; set; } = false;
        public int LiveRotationAngle { get; set; } = 0;
    }
}
