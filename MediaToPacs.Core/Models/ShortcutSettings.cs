using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaToPacs.Core.Models
{
    public class ShortcutSettings
    {
        public Keys StartRecordingKey { get; set; } = Keys.F1;
        public Keys PauseRecordingKey { get; set; } = Keys.F2;
        public Keys SnapshotKey { get; set; } = Keys.F3;
        public Keys SaveDicomKey { get; set; } = Keys.F4;
        public Keys ReloadKey { get; set; } = Keys.F5;
        public Keys PrintKey { get; set; } = Keys.F6;
        public Keys ExitKey { get; set; } = Keys.F7;
        public Keys StoreKey { get; set; } = Keys.F8;
    }
}
