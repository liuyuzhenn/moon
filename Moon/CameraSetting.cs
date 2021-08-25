using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using liuyuzhen;

namespace Moon
{
    public partial class CameraSetting : Form
    {
        public Form1 _frm;
        string help = "PS: Pixel size(μm)\n\n" +
                "PD: Principal distance(mm)\n\n" +
                "N: Number of pixels of one line\n\n" +
                "Ang: Solid angles of the lences\n\n";
        public CameraSetting(Form1 frm)
        {
            InitializeComponent();
            _frm = frm;
            init();

            int x = (System.Windows.Forms.SystemInformation.WorkingArea.Width / 2 - this.Size.Width / 2);
            int y = (System.Windows.Forms.SystemInformation.WorkingArea.Height / 2 - this.Size.Height / 2);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(x, y);
        }

        private void init()
        {
            textBox1.Text = "100.1";
            textBox2.Text = "224.34";
            textBox3.Text = "150";
            angleBox.Items.Add(23.5);
            angleBox.Items.Add(0);
            angleBox.Items.Add(-23.5);
        }


        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(help,"Help"
                );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double pixSize = double.Parse(textBox1.Text)/1000000;
                double pd = double.Parse(textBox2.Text)/1000;
                int w = int.Parse(textBox3.Text);

                double[] angs = new double[angleBox.Items.Count];
                for (int i=0;i< angleBox.Items.Count;i++)
                {
                    angs[i] = double.Parse(angleBox.Items[i].ToString());
                }
                Camera cam = new Camera(pd, pixSize, w, angs);
                _frm.setCamera(cam);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // delete angle
        private void button2_Click(object sender, EventArgs e)
        {
            int index = angleBox.SelectedIndex;

            if (index != -1)
            {
                angleBox.Items.RemoveAt(index);
            }

        }


        // add angle
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                double angle = double.Parse(angleBox.Text);

                for (int i=0;i<angleBox.Items.Count;i++)
                {
                    if (angle == double.Parse(angleBox.Items[i].ToString()))
                    {
                        MessageBox.Show(string.Format("{0} already exists!", angle));
                        return;
                    }
                    
                }
                


                if (angle >-90 && angle < 90)
                {
                    int count = angleBox.Items.Count;
                    angleBox.Items.Add(angle);
                }
                else
                {
                    MessageBox.Show("Angle should be in (-90, 90)", "Warning");
                }

            }
            catch
            {
                MessageBox.Show("Please make sure valid input!");
            }
        }
    }
}
