using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace STM.MediaToPACS.Main.UI
{
    public sealed class SignatureCancellationReasonDialog : XtraForm
    {
        private readonly ComboBoxEdit _reasonEditor;

        private static readonly string[] QuickReasons =
        {
            "Cần chỉnh sửa nội dung kết luận",
            "Bổ sung hoặc chỉnh sửa hình ảnh",
            "Kết quả chưa chính xác, cần kiểm tra lại",
            "Chọn nhầm bệnh nhân hoặc y lệnh",
            "Lý do khác"
        };

        public string Reason => (_reasonEditor.EditValue as string ?? _reasonEditor.Text ?? string.Empty).Trim();

        public SignatureCancellationReasonDialog(IWin32Window owner)
        {
            Text = "Lý do hủy ký số";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(430, 150);

            var caption = new LabelControl
            {
                Text = "Chọn lý do hoặc tự nhập nội dung:",
                Location = new Point(18, 18),
                AutoSizeMode = LabelAutoSizeMode.Default
            };

            _reasonEditor = new ComboBoxEdit
            {
                Location = new Point(18, 48),
                Width = 394,
                TabIndex = 0
            };
            _reasonEditor.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            _reasonEditor.Properties.Items.AddRange(QuickReasons);
            _reasonEditor.SelectedIndex = 0;

            var cancelButton = new SimpleButton
            {
                Text = "Hủy",
                DialogResult = DialogResult.Cancel,
                Location = new Point(232, 98),
                Width = 85,
                TabIndex = 2
            };

            var confirmButton = new SimpleButton
            {
                Text = "Xác nhận",
                Location = new Point(327, 98),
                Width = 85,
                TabIndex = 1
            };
            confirmButton.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(Reason))
                {
                    XtraMessageBox.Show(this, "Vui lòng nhập lý do hủy ký số.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _reasonEditor.Focus();
                    return;
                }

                DialogResult = DialogResult.OK;
            };

            AcceptButton = confirmButton;
            CancelButton = cancelButton;
            Controls.Add(caption);
            Controls.Add(_reasonEditor);
            Controls.Add(cancelButton);
            Controls.Add(confirmButton);

            Shown += (s, e) =>
            {
                _reasonEditor.Focus();
                _reasonEditor.SelectAll();
            };

            if (owner != null)
                ShowInTaskbar = false;
        }
    }
}
