using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using OSGeo.GDAL;
using OSGeo.OSR;
using OSGeo.OGR;
using liuyuzhen;


namespace Moon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Gdal.AllRegister();
            CheckForIllegalCrossThreadCalls = false;


            int x = (System.Windows.Forms.SystemInformation.WorkingArea.Width / 2 - this.Size.Width / 2);
            int y = (System.Windows.Forms.SystemInformation.WorkingArea.Height / 2 - this.Size.Height / 2);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(x, y);

            refreshLayout();
            init();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        Result[] results;
        Bitmap[] bitmaps;
        liuyuzhen.Moon _moon;
        Orbit _orbit;
        Camera _camera;

        bool _orbSet = false;
        bool _moonSet = false;
        bool _camSet = false;

        double dt;
        int times;
        double t0;
        double thrd;
        int maxiteration;


        string help = "*Exposure times* also mean the height of the image\n" +
            "*Threshold* determines when to stop iteration.\n" +
            "*Max Iteration* is the maximum number of iterations.\n\n" +
            "If you are not sure, use the default value.";

        // rubish  just for test
        /*
        private void simulate()
        {
            Dataset DEM = Gdal.Open(@"D:\刘雨臻\全月\DEM\Lunar_LRO_LOLA_Global_LDEM_118m_Mar2014.tif", Access.GA_ReadOnly);
            Dataset DOM = Gdal.Open(@"D:\刘雨臻\全月\DOM\Mosaci_Moon.tif", Access.GA_ReadOnly);

            // camera settings
            double[] angles = { 23.5, 0, -23.5 };
            double f = 224.334 / 1000;
            double pixSize = 10.1 * Math.Pow(10,-6);
            Camera camera = new Camera(f, pixSize, 100, angles);

            // orbit settings
            double h = 100000;
            double a = 1737400 + h;
            double ee = 0;
            double i = 60;
            double omega = 0;
            double w = 0;
            double n = 2 * Math.PI / (16 * 3600);//16h per round
            double t0 = 0;

            Orbit orbit = new Orbit(a, ee, i, omega, w, n, t0);

            // moon settings
            liuyuzhen.Moon moon = new liuyuzhen.Moon(DEM, DOM);
            moon.setDOMInterpolate(Interpolate.LINEAR);

            // generate solver
            Solver solver = new Solver(moon, camera, orbit);

            liuyuzhen.Observer.IObserver prg = new liuyuzhen.Observer.ProgressbarObserver(progressBar1);
            liuyuzhen.Observer.IObserver labbar = new liuyuzhen.Observer.StatusbarObserver(label1);

            solver.addObserver(prg);
            solver.addObserver(labbar);

            double ratio = h / f;
            double pixGroundSize = pixSize * ratio;
            double v = n * a;

            int height = 800;
            int tStart = 0;
            double dt = 200/v;

            button1.Enabled = false;

			
			TimeSpan t1 = new TimeSpan(DateTime.Now.Ticks);
            bitmaps = solver.simulate(bitmaps, height, tStart, dt, 5, 5);
			TimeSpan t2 = new TimeSpan(DateTime.Now.Ticks);
			label1.Text = string.Format("Time cost is {0} seconds.", (t2-t1
).Seconds);


            button1.Enabled = true;
        }
        */
        private void refreshLayout()
        {
            label1.Left = 20;
            label1.Top = 35;

            progressBar1.Width = Width - 70;
            progressBar1.Left = 35;
            progressBar1.Top = Height - progressBar1.Height - 50;
        }

        public void setMoon(liuyuzhen.Moon moon)
        {
            _moon = moon;
            _moonSet = true;
            label1.Text = "Moon set";
        }

        public void setOrbit(Orbit orbit)
        {
            _orbit = orbit;
            _orbSet = true;
            label1.Text = "Orbir set";
        }

        public void setCamera(Camera camera)
        {
            _camera = camera;
            _camSet = true;
            label1.Text = "Camera set";
        }

        private void clear()
        {
            if (bitmaps != null)
            {
                for (int i = 0; i < bitmaps.Length; i++)
                {
                    bitmaps[i].Dispose();
                    results[i].Dispose();
                }
            }
            
            bitmaps = null;
            results = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!_moonSet)
            {
                MessageBox.Show("Please set DOM and DEM in \"Setting\" -> \"Moon\" ");
                return;
            }
            if (!_camSet)
            {
                MessageBox.Show("Please set camera parameters in \"Setting\" -> \"Camera\" ");
                return;
            }
            if (!_orbSet)
            {
                MessageBox.Show("Please set orbital parameters in \"Setting\" -> \"Orbit\" ");
                return;
            }

            dt = double.Parse(textBox1.Text);
            times = int.Parse(textBox2.Text);
            t0 = double.Parse(textBox7.Text) * 3600 +
                double.Parse(textBox8.Text) * 60 +
                double.Parse(textBox9.Text);
            thrd = double.Parse(textBox3.Text);
            maxiteration = int.Parse(textBox4.Text);

            clear();
            results = new Result[_camera.count()];
            bitmaps = new Bitmap[_camera.count()];

            Rectangle rec = System.Windows.Forms.Screen.GetWorkingArea(this);
            for (int i = 0; i < _camera.count(); i++)
            {
                Bitmap btm = new Bitmap(_camera.width(), times);
                bitmaps[i] = btm;
                Result result = new Result(ref btm, 
                    new System.Drawing.Point(563 * i % rec.Width, 300));
                result.Show();
                results[i] = result;
            }
            //for () -----------------------------------------------------------------------

            Thread thread = new Thread(generate);
            thread.IsBackground = true;
            thread.Start();
        }
        private void init()
        {
            // set exposure
            textBox1.Text = "0.34743515461010475";
            textBox2.Text = "100";

            // set time to start
            textBox7.Text = "0";
            textBox8.Text = "0";
            textBox9.Text = "0";

            // set threshold and iterations
            textBox3.Text = "5";
            textBox4.Text = "10";
            
        }

        private void generate()
        {


            try
            {

                Solver solver = new Solver(_moon, _camera, _orbit);

                // observer pattern  
                // used to update statusbar and label 
                liuyuzhen.Observer.IObserver prg = 
                    new liuyuzhen.Observer.ProgressbarObserver(progressBar1);

                liuyuzhen.Observer.IObserver labbar =
                    new liuyuzhen.Observer.StatusbarObserver(label1);

                liuyuzhen.Observer.IObserver imgObsever = 
                    new liuyuzhen.Observer.ImageObserver(bitmaps, results);

                solver.addObserver(prg);
                solver.addObserver(labbar);
                solver.addObserver(imgObsever);

                button1.Enabled = false;

                TimeSpan t1 = new TimeSpan(DateTime.Now.Ticks);
                solver.simulate(bitmaps, times, t0, dt, thrd, maxiteration);
                TimeSpan t2 = new TimeSpan(DateTime.Now.Ticks);

                label1.Text = string.Format("Time cost is {0} seconds.", (t2-t1).Seconds);
                button1.Enabled = true;


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        //private void buttonDisp_Click(object sender, EventArgs e)
        //{
        //    if (bitmaps != null)
        //    {
        //        for (int i=0;i<bitmaps.GetLength(0);i++)
        //        {
        //            Result result = new Result(bitmaps[i]);
        //            result.Show();
        //        }
        //    }
        //}

        private void orbitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrbitSetting setting = new OrbitSetting(this);
            setting.ShowDialog();
        }

        private void moonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoonSetting setting = new MoonSetting(this);
            setting.ShowDialog();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            refreshLayout();
        }

        private void cameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraSetting setting = new CameraSetting(this);
            setting.Show();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(help, "Help");
        }
    }

    
}

