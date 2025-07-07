using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintToPACSDemo.UI.Conclusion
{
    public partial class ImageControl : UserControl
    {
        public EventHandler ButtonClickDelete;
        public string imagePath;
        public ImageControl()
        {
            InitializeComponent();
        }
        public void AddImage(Image image)
        {
            pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImage.Image = image;
        }
        public Image GetImageToPicturebox()
        {
            return pictureBoxImage.Image;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (null != ButtonClickDelete)
            {
                ButtonClickDelete(this, EventArgs.Empty);
            }

        }
    }
}
