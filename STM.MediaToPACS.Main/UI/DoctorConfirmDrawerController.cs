using System;
using System.Drawing;
using System.Windows.Forms;

namespace STM.MediaToPACS.Main.UI
{
    /// <summary>
    /// Quản lý drawer cài đặt hệ thống được nhúng trong form xác nhận bác sĩ.
    /// </summary>
    internal sealed class DoctorConfirmDrawerController : IDisposable
    {
        private const int DrawerWidth = 520;
        private const int AnimationStep = 65;

        private readonly Form _owner;
        private readonly Panel _overlay;
        private readonly Panel _drawer;
        private readonly Panel _drawerHeader;
        private readonly Panel _drawerContent;
        private readonly Button _closeButton;
        private readonly Timer _animationTimer;
        private DoctorQuickSettingsControl _settingsControl;
        private bool _opening;

        public DoctorConfirmDrawerController(Form owner)
        {
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));

            _overlay = new Panel
            {
                BackColor = Color.FromArgb(45, 52, 60),
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
                BackColor = Color.FromArgb(15, 72, 116),
                Dock = DockStyle.Top,
                Height = 48
            };

            _drawerContent = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill
            };

            var drawerTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(18, 13),
                Text = "Cài đặt hệ thống"
            };

            _closeButton = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(DrawerWidth - 112, 6),
                Size = new Size(102, 36),
                Text = "Thu gọn  ×",
                TabStop = false
            };
            _closeButton.FlatAppearance.BorderSize = 0;
            _closeButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(196, 50, 50);
            _closeButton.Click += (sender, args) => Close();

            _drawerHeader.Controls.Add(drawerTitle);
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
