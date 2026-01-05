namespace STM.MediaToPACS.Main.UI.CameraUI
{
    partial class MediaPlayerControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbSpeed = new DevExpress.XtraEditors.LabelControl();
            this.tbTimeline = new System.Windows.Forms.TrackBar();
            this.tbSpeed = new System.Windows.Forms.TrackBar();
            this.lbTimeline = new DevExpress.XtraEditors.LabelControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPlay = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPreviousFrame = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNextFrame = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPreviousKeyFrame = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNextKeyFrame = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFirstFrame = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLastFrame = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSnapshot = new System.Windows.Forms.ToolStripButton();
            this.videoView1 = new VisioForge.Core.UI.WinForms.VideoView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTimeline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSpeed)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.lbSpeed);
            this.panel1.Controls.Add(this.tbTimeline);
            this.panel1.Controls.Add(this.tbSpeed);
            this.panel1.Controls.Add(this.lbTimeline);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 361);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(700, 33);
            this.panel1.TabIndex = 1;
            // 
            // lbSpeed
            // 
            this.lbSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSpeed.Appearance.ForeColor = System.Drawing.Color.White;
            this.lbSpeed.Appearance.Options.UseForeColor = true;
            this.lbSpeed.Location = new System.Drawing.Point(651, 7);
            this.lbSpeed.Margin = new System.Windows.Forms.Padding(2, 4, 5, 0);
            this.lbSpeed.Name = "lbSpeed";
            this.lbSpeed.Size = new System.Drawing.Size(13, 14);
            this.lbSpeed.TabIndex = 69;
            this.lbSpeed.Text = "1x";
            // 
            // tbTimeline
            // 
            this.tbTimeline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTimeline.BackColor = System.Drawing.Color.Gray;
            this.tbTimeline.Location = new System.Drawing.Point(3, 5);
            this.tbTimeline.Maximum = 255;
            this.tbTimeline.Name = "tbTimeline";
            this.tbTimeline.Size = new System.Drawing.Size(400, 45);
            this.tbTimeline.TabIndex = 68;
            this.tbTimeline.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbTimeline.Scroll += new System.EventHandler(this.tbTimeline_Scroll);
            // 
            // tbSpeed
            // 
            this.tbSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSpeed.BackColor = System.Drawing.Color.Gray;
            this.tbSpeed.Location = new System.Drawing.Point(541, 5);
            this.tbSpeed.Minimum = 1;
            this.tbSpeed.Name = "tbSpeed";
            this.tbSpeed.Size = new System.Drawing.Size(104, 45);
            this.tbSpeed.TabIndex = 67;
            this.tbSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbSpeed.Value = 4;
            this.tbSpeed.Scroll += new System.EventHandler(this.tbSpeed_Scroll);
            // 
            // lbTimeline
            // 
            this.lbTimeline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTimeline.Appearance.ForeColor = System.Drawing.Color.White;
            this.lbTimeline.Appearance.Options.UseForeColor = true;
            this.lbTimeline.Location = new System.Drawing.Point(408, 7);
            this.lbTimeline.Margin = new System.Windows.Forms.Padding(2, 4, 5, 0);
            this.lbTimeline.Name = "lbTimeline";
            this.lbTimeline.Size = new System.Drawing.Size(105, 14);
            this.lbTimeline.TabIndex = 65;
            this.lbTimeline.Text = "00:00:00/00:00:00";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Gray;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPlay,
            this.toolStripButtonPreviousFrame,
            this.toolStripButtonNextFrame,
            this.toolStripButtonPreviousKeyFrame,
            this.toolStripButtonNextKeyFrame,
            this.toolStripButtonFirstFrame,
            this.toolStripButtonLastFrame,
            this.toolStripSeparator1,
            this.toolStripButtonSnapshot});
            this.toolStrip1.Location = new System.Drawing.Point(0, 313);
            this.toolStrip1.MinimumSize = new System.Drawing.Size(0, 48);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(700, 48);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonPlay
            // 
            this.toolStripButtonPlay.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButtonPlay.BackgroundImage = global::STM.MediaToPACS.Main.Properties.Resources.play;
            this.toolStripButtonPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripButtonPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPlay.Margin = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.toolStripButtonPlay.Name = "toolStripButtonPlay";
            this.toolStripButtonPlay.Padding = new System.Windows.Forms.Padding(31, 0, 0, 0);
            this.toolStripButtonPlay.Size = new System.Drawing.Size(35, 38);
            this.toolStripButtonPlay.ToolTipText = "Play/Stop";
            this.toolStripButtonPlay.Click += new System.EventHandler(this.toolStripButtonPlay_Click);
            // 
            // videoView1
            // 
            this.videoView1.BackColor = System.Drawing.Color.Black;
            this.videoView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoView1.Location = new System.Drawing.Point(0, 35);
            this.videoView1.Margin = new System.Windows.Forms.Padding(2);
            this.videoView1.Name = "videoView1";
            this.videoView1.Size = new System.Drawing.Size(600, 255);
            this.videoView1.StatusOverlay = null;
            this.videoView1.TabIndex = 3;
            // 
            // toolStripButtonPreviousFrame
            // 
            this.toolStripButtonPreviousFrame.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButtonPreviousFrame.BackgroundImage = global::STM.MediaToPACS.Main.Properties.Resources.back_arrow;
            this.toolStripButtonPreviousFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripButtonPreviousFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPreviousFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPreviousFrame.Margin = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.toolStripButtonPreviousFrame.Name = "toolStripButtonPreviousFrame";
            this.toolStripButtonPreviousFrame.Padding = new System.Windows.Forms.Padding(31, 0, 0, 0);
            this.toolStripButtonPreviousFrame.Size = new System.Drawing.Size(35, 38);
            this.toolStripButtonPreviousFrame.ToolTipText = "Previous Frame";
            this.toolStripButtonPreviousFrame.Click += new System.EventHandler(this.toolStripButtonPreviousFrame_Click);
            // 
            // toolStripButtonNextFrame
            // 
            this.toolStripButtonNextFrame.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButtonNextFrame.BackgroundImage = global::STM.MediaToPACS.Main.Properties.Resources.right_arrow;
            this.toolStripButtonNextFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripButtonNextFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNextFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNextFrame.Margin = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.toolStripButtonNextFrame.Name = "toolStripButtonNextFrame";
            this.toolStripButtonNextFrame.Padding = new System.Windows.Forms.Padding(31, 0, 0, 0);
            this.toolStripButtonNextFrame.Size = new System.Drawing.Size(35, 38);
            this.toolStripButtonNextFrame.ToolTipText = "Next Frame";
            this.toolStripButtonNextFrame.Click += new System.EventHandler(this.toolStripButtonNextFrame_Click);
            // 
            // toolStripButtonPreviousKeyFrame
            // 
            this.toolStripButtonPreviousKeyFrame.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButtonPreviousKeyFrame.BackgroundImage = global::STM.MediaToPACS.Main.Properties.Resources.rewind;
            this.toolStripButtonPreviousKeyFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripButtonPreviousKeyFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPreviousKeyFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPreviousKeyFrame.Margin = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.toolStripButtonPreviousKeyFrame.Name = "toolStripButtonPreviousKeyFrame";
            this.toolStripButtonPreviousKeyFrame.Padding = new System.Windows.Forms.Padding(31, 0, 0, 0);
            this.toolStripButtonPreviousKeyFrame.Size = new System.Drawing.Size(35, 38);
            this.toolStripButtonPreviousKeyFrame.ToolTipText = "Previous Keyframe";
            this.toolStripButtonPreviousKeyFrame.Click += new System.EventHandler(this.toolStripButtonPreviousKeyFrame_Click);
            // 
            // toolStripButtonNextKeyFrame
            // 
            this.toolStripButtonNextKeyFrame.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButtonNextKeyFrame.BackgroundImage = global::STM.MediaToPACS.Main.Properties.Resources.fast_forward_button;
            this.toolStripButtonNextKeyFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripButtonNextKeyFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNextKeyFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNextKeyFrame.Margin = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.toolStripButtonNextKeyFrame.Name = "toolStripButtonNextKeyFrame";
            this.toolStripButtonNextKeyFrame.Padding = new System.Windows.Forms.Padding(31, 0, 0, 0);
            this.toolStripButtonNextKeyFrame.Size = new System.Drawing.Size(35, 38);
            this.toolStripButtonNextKeyFrame.Text = "toolStripButton1";
            this.toolStripButtonNextKeyFrame.ToolTipText = "Next KeyFrame";
            this.toolStripButtonNextKeyFrame.Click += new System.EventHandler(this.toolStripButtonNextKeyFrame_Click);
            // 
            // toolStripButtonFirstFrame
            // 
            this.toolStripButtonFirstFrame.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButtonFirstFrame.BackgroundImage = global::STM.MediaToPACS.Main.Properties.Resources.back_button;
            this.toolStripButtonFirstFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripButtonFirstFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFirstFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFirstFrame.Margin = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.toolStripButtonFirstFrame.Name = "toolStripButtonFirstFrame";
            this.toolStripButtonFirstFrame.Padding = new System.Windows.Forms.Padding(31, 0, 0, 0);
            this.toolStripButtonFirstFrame.Size = new System.Drawing.Size(35, 38);
            this.toolStripButtonFirstFrame.ToolTipText = "First Frame";
            this.toolStripButtonFirstFrame.Click += new System.EventHandler(this.toolStripButtonFirstFrame_Click);
            // 
            // toolStripButtonLastFrame
            // 
            this.toolStripButtonLastFrame.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButtonLastFrame.BackgroundImage = global::STM.MediaToPACS.Main.Properties.Resources.next_button;
            this.toolStripButtonLastFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripButtonLastFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLastFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLastFrame.Margin = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.toolStripButtonLastFrame.Name = "toolStripButtonLastFrame";
            this.toolStripButtonLastFrame.Padding = new System.Windows.Forms.Padding(31, 0, 0, 0);
            this.toolStripButtonLastFrame.Size = new System.Drawing.Size(35, 38);
            this.toolStripButtonLastFrame.ToolTipText = "Last Frame";
            this.toolStripButtonLastFrame.Click += new System.EventHandler(this.toolStripButtonLastFrame_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 48);
            // 
            // toolStripButtonSnapshot
            // 
            this.toolStripButtonSnapshot.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButtonSnapshot.BackgroundImage = global::STM.MediaToPACS.Main.Properties.Resources.snapshot;
            this.toolStripButtonSnapshot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripButtonSnapshot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSnapshot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSnapshot.Margin = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.toolStripButtonSnapshot.Name = "toolStripButtonSnapshot";
            this.toolStripButtonSnapshot.Padding = new System.Windows.Forms.Padding(31, 0, 0, 0);
            this.toolStripButtonSnapshot.Size = new System.Drawing.Size(35, 38);
            this.toolStripButtonSnapshot.ToolTipText = "Snapshot";
            this.toolStripButtonSnapshot.Click += new System.EventHandler(this.toolStripButtonSnapshot_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(700, 38);
            this.panel2.TabIndex = 4;
            // 
            // MediaPlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.videoView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MediaPlayerControl";
            this.Size = new System.Drawing.Size(700, 394);
            this.Load += new System.EventHandler(this.VideoEditor_Load);
            this.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.MediaPlayerControl_ControlRemoved);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTimeline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSpeed)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonPlay;
        private System.Windows.Forms.ToolStripButton toolStripButtonPreviousFrame;
        private System.Windows.Forms.ToolStripButton toolStripButtonNextFrame;
        private System.Windows.Forms.ToolStripButton toolStripButtonPreviousKeyFrame;
        private System.Windows.Forms.ToolStripButton toolStripButtonNextKeyFrame;
        private System.Windows.Forms.ToolStripButton toolStripButtonFirstFrame;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private VisioForge.Core.UI.WinForms.VideoView videoView1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSnapshot;
        private System.Windows.Forms.ToolStripButton toolStripButtonLastFrame;
        private DevExpress.XtraEditors.LabelControl lbTimeline;
        private System.Windows.Forms.TrackBar tbSpeed;
        private DevExpress.XtraEditors.LabelControl lbSpeed;
        private System.Windows.Forms.TrackBar tbTimeline;
        private System.Windows.Forms.Panel panel2;
    }
}
