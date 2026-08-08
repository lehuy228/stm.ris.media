using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace STM.MediaToPACS.Main.UI
{
    /// <summary>
    /// Quản lý drawer cài đặt hệ thống được nhúng trong form xác nhận bác sĩ.
    /// </summary>
    internal sealed class DoctorConfirmDrawerController : IDisposable
    {
        private const int DrawerWidth = 500;
        private const int AnimationStep = 65;

        private readonly Form _owner;
        private readonly Panel _overlay;
        private readonly Panel _drawer;
        private readonly Panel _drawerHeader;
        private readonly Panel _drawerContent;
        private readonly SimpleButton _closeButton;
        private readonly Timer _animationTimer;
        private DoctorQuickSettingsControl _settingsControl;
        private bool _opening;

        public DoctorConfirmDrawerController(Form owner)
        {
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));

            _overlay = new Panel
            {
                BackColor = Color.FromArgb(70, 17, 24, 39),
                Bounds = owner.ClientRectangle,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Visible = false
            };
            _overlay.Click += (sender, args) => Close();

            _drawer = new Panel
            {
                BackColor = Color.White,
                Location = new Point(-DrawerWidth, 0),
                Size = new Size(DrawerWidth, owner.ClientSize.Height),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
            };

            _drawerHeader = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Height = 78
            };

            _drawerContent = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill
            };

            var drawerTitle = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Location = new Point(24, 16),
                Size = new Size(300, 24),
                Text = "Cài đặt hệ thống"
            };

            var drawerSubtitle = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.FromArgb(91, 100, 110),
                Location = new Point(24, 42),
                Size = new Size(330, 22),
                Text = "Thiết lập kết nối và cấu hình làm việc"
            };

            var drawerAccent = new Panel
            {
                BackColor = Color.FromArgb(15, 72, 116),
                Dock = DockStyle.Left,
                Width = 4
            };

            var drawerDivider = new Panel
            {
                BackColor = Color.FromArgb(226, 231, 236),
                Dock = DockStyle.Bottom,
                Height = 1
            };

            _closeButton = new SimpleButton
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AllowFocus = false,
                Cursor = Cursors.Hand,
                Location = new Point(DrawerWidth - 106, 20),
                Name = "drawerCloseButton",
                Size = new Size(82, 34),
                Text = "Đóng",
                TabStop = false
            };
            _closeButton.Appearance.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            _closeButton.Appearance.Options.UseFont = true;
            _closeButton.Click += (sender, args) => Close();

            _drawerHeader.Controls.Add(drawerDivider);
            _drawerHeader.Controls.Add(drawerAccent);
            _drawerHeader.Controls.Add(drawerTitle);
            _drawerHeader.Controls.Add(drawerSubtitle);
            _drawerHeader.Controls.Add(_closeButton);
            _drawer.Controls.Add(_drawerContent);
            _drawer.Controls.Add(_drawerHeader);

            _animationTimer = new Timer { Interval = 12 };
            _animationTimer.Tick += AnimationTimer_Tick;

            owner.Controls.Add(_overlay);
            owner.Controls.Add(_drawer);
            owner.Resize += Owner_Resize;
            owner.KeyPreview = true;
            owner.KeyDown += Owner_KeyDown;
        }

        public void Toggle()
        {
            if (_overlay.Visible && _opening)
                Close();
            else
                Open();
        }

        public void Open()
        {
            EnsureSettingsControl();
            _overlay.Visible = true;
            _overlay.BringToFront();
            _drawer.BringToFront();
            _opening = true;
            _animationTimer.Start();
        }

        public void Close()
        {
            if (!_overlay.Visible) return;
            _opening = false;
            _animationTimer.Start();
        }

        private void EnsureSettingsControl()
        {
            if (_settingsControl != null && !_settingsControl.IsDisposed) return;

            _settingsControl = new DoctorQuickSettingsControl { Dock = DockStyle.Fill };
            _settingsControl.CloseRequested += (sender, args) => Close();
            _drawerContent.Controls.Add(_settingsControl);
            _settingsControl.BringToFront();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            int targetLeft = _opening ? 0 : -DrawerWidth;
            int distance = targetLeft - _drawer.Left;

            if (Math.Abs(distance) <= AnimationStep)
            {
                _drawer.Left = targetLeft;
                _animationTimer.Stop();
                if (!_opening) _overlay.Visible = false;
                return;
            }

            _drawer.Left += Math.Sign(distance) * AnimationStep;
        }

        private void Owner_Resize(object sender, EventArgs e)
        {
            _overlay.Bounds = _owner.ClientRectangle;
            _drawer.Height = _owner.ClientSize.Height;
        }

        private void Owner_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape || !_overlay.Visible) return;

            Close();
            e.Handled = true;
        }

        public void Dispose()
        {
            _animationTimer.Stop();
            _animationTimer.Dispose();
            _owner.Resize -= Owner_Resize;
            _owner.KeyDown -= Owner_KeyDown;
            _settingsControl?.Dispose();
            _drawer.Dispose();
            _overlay.Dispose();
        }
    }
}
