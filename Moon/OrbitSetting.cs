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
using Moon;

namespace liuyuzhen
{
    public partial class OrbitSetting : Form
    {
        Form1 _frm;
        string help = "a: Semi-Major axis(meter)\n\n" +
                "e: Eccentricity\n\n" +
                "i: Inclination(decimal degree)\n\n" +
                "Ω: Longitude of ascending node\n\n" +
                "ω: Argument of periapsis\n\n" +
                "n: Mean angular velocity\n\n" +
                "t0: Time of periapsis(hour, minute, second)";


        public OrbitSetting(Form1 frm)
        {
            InitializeComponent();
            _frm = frm;
            init();

            int x = (System.Windows.Forms.SystemInformation.WorkingArea.Width / 2 - this.Size.Width / 2);
            int y = (System.Windows.Forms.SystemInformation.WorkingArea.Height / 2 - this.Size.Height / 2);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(x, y);
        }


        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(help, "Help");
            
            
        }

        private void setPara(double a, double e, double i, double Omega, double w, double n, double t0)
        {
            textBox1.Text = a.ToString();
            textBox2.Text = e.ToString();
            textBox3.Text = i.ToString();
            textBox4.Text = Omega.ToString();
            textBox5.Text = w.ToString();
            textBox6.Text = n.ToString();
        }


        private void init()
        {
            double h = 100000;
            double a = 1737400 + h;
            double ee = 0;
            double i = 60;
            double omega = 0;
            double w = 0;
            double n = 2 * Math.PI / (16 * 3600);//16h per round
            double t0 = 0;

            textBox7.Text = "0";
            textBox8.Text = "0";
            textBox9.Text = "0";
            setPara(a,ee,i,omega,w,n,t0);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double a = double.Parse(textBox1.Text);
                double ee = double.Parse(textBox2.Text);
                double i = double.Parse(textBox3.Text);
                double omega = double.Parse(textBox4.Text);
                double w = double.Parse(textBox5.Text);
                double n = double.Parse(textBox6.Text);//16h per round
                double t0 = double.Parse(textBox7.Text) * 3600 +
                    double.Parse(textBox8.Text) * 60 +
                    double.Parse(textBox9.Text);

                Orbit orbit = new Orbit(a, ee, i, omega, w, n, t0);
                _frm.setOrbit(orbit);
                Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
    }
}
