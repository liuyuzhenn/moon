using System;
using OSGeo.GDAL;
using System.Windows.Forms;
using System.IO;
using Moon;


namespace liuyuzhen
{
    public partial class MoonSetting : Form
    {
        Form1 _frm;
        
        public MoonSetting(Form1 frm)
        {
            InitializeComponent();
            _frm = frm;

            int x = (System.Windows.Forms.SystemInformation.WorkingArea.Width/2 - this.Size.Width /2);
            int y = (System.Windows.Forms.SystemInformation.WorkingArea.Height/2 - this.Size.Height /2);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(x, y);

            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "*.tif|*.tif|all|*.*";
            if (diag.ShowDialog()==DialogResult.OK)
            {
                pathDOM.Text = diag.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "*.tif|*.tif|all|*.*";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                pathDEM.Text = diag.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Dataset dom = Gdal.Open(pathDOM.Text, Access.GA_ReadOnly);
                Dataset dem = Gdal.Open(pathDEM.Text, Access.GA_ReadOnly);

                liuyuzhen.Moon moon = new Moon(dem, dom);
                int idx = comboBox1.SelectedIndex;
                switch(idx)
                {
                    case 0:
                        {
                            moon.setDEMInterpolate(liuyuzhen.Interpolate.NEAREST);
                            moon.setDOMInterpolate(liuyuzhen.Interpolate.NEAREST);
                            break;
                        }

                    case 1:
                        {
                            moon.setDEMInterpolate(liuyuzhen.Interpolate.LINEAR);
                            moon.setDOMInterpolate(liuyuzhen.Interpolate.LINEAR);
                            break;
                        }
                }

                _frm.setMoon(moon);
              
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                pathDOM.Clear();
                pathDEM.Clear();
            }
        }
    }
}
