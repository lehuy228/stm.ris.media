namespace STM.MediaToPACS.Main
{
    /// <summary>
    /// Chỉ số layout UI do người dùng tùy chỉnh bằng kéo thả,
    /// lưu theo máy trong AppData (UiLayoutSettings.xml) và nạp lại mỗi lần mở app.
    /// </summary>
    public class UiLayoutSettings
    {
        /// <summary>
        /// Bề rộng cột camera trong FrmMain (px).
        /// Là giá trị "gốc" khi sidebar chỉ số đóng; lúc sidebar mở cột tự thu bớt như cũ.
        /// </summary>
        public float CameraColumnWidth { get; set; }

        /// <summary>
        /// Bề rộng sidebar form chỉ số (param) trong tab Kết luận (px), người dùng kéo bằng splitter.
        /// </summary>
        public int ParamSidebarWidth { get; set; }
    }
}
