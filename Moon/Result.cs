using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moon
{
    public partial class Result : Form
    {
        Bitmap bitmap;
        public Result(ref Bitmap bitmap, Point pt)
        {
            InitializeComponent();
            this.bitmap = bitmap;
            pictureBox1.Image = new Bitmap(bitmap);

            this.Location = pt;
            this.StartPosition = FormStartPosition.Manual;
            updateSize();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                SaveFileDialog diag = new SaveFileDialog();
                diag.Filter = "*.jpg|*.jpg|*.png|*.png";
                diag.FilterIndex = 0;
                diag.FileName = string.Format("{0}.jpg", bitmap.GetHashCode());
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    bitmap.Save(diag.FileName);
                }
            }
        }

        public void updateImage(Bitmap img)
        {
            pictureBox1.Image = img;
            pictureBox1.Refresh();
        }

        private void updateSize()
        {


            pictureBox1.Size = new Size(Width-150,Height-150);
            //pictureBox1.Size = new Size()
            pictureBox1.Left = Width / 2 - pictureBox1.Width / 2;
            pictureBox1.Top = Height / 2 - pictureBox1.Height / 2;
        }

        private void Result_Resize(object sender, EventArgs e)
        {
            updateSize();
        }

        private void Result_Load(object sender, EventArgs e)
        {

        }
    }
}
