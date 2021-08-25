using System;
using OSGeo.GDAL;
using OSGeo.OSR;
using System.Drawing;
using System.Collections.Generic;
using liuyuzhen.Observer;
using System.Windows.Forms;
using Moon;

namespace liuyuzhen
{
    public class Solver
    {
        Moon _moon;
        Camera _camera;
        Orbit _orbit;

        List<IObserver> observers;
        
        public Solver(Moon moon, Camera camera, Orbit orbit)
        {
            _moon = moon;
            _camera = camera;
            _orbit = orbit;

            observers = new List<IObserver>();
        }
        
        //observer pattern
        public void addObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        // times: exposure times,
        // t0: time to start expose
        // dt: interval between exposures
        public void simulate(Bitmap[] bitmaps, int times, double t0, double dt, double threshold = 5, int maxiteration=10)
        {
            int count = _camera.count();
            /*
            Bitmap[] bitmaps = new Bitmap[count];
            for (int i=0;i< count; i++)
            {
                Bitmap btm = new Bitmap(_camera.width(), times);
                bitmaps[i] = btm;
            }
            */

            Color black = Color.Black;

            double f = _camera.principalDistance();

            double R = _moon.R();
            // per row
            for (int row=0;row<times;row++)
            {
                double exposeTime = t0 + row * dt;
                Point pos = _orbit.Pos3D(exposeTime);


                Matrix moon2cam = moon2camMatrix(pos);
                Matrix cam2moon = moon2cam.invert();

                Point orig = new Point(0,0,0);

                Point moonCenter = moon2cam * orig;

                // moon's center in camera's coordinate system
                double X0 = moonCenter.x;
                double Y0 = moonCenter.y;
                double Z0 = moonCenter.z;
                
                // per collum
                for (int col=0;col<_camera.width();col++)
                {
                    double[,] xy = _camera.getXY(col);

                    // per angle 
                    for (int ag=0;ag< count; ag++)
                    {
                        // highest mountain on the moon is 9848m
                        double tempR = R + 9848;

                        double x = xy[ag, 0];
                        double y = xy[ag, 1];

                        double A = x * x + y * y + f * f;
                        double B = 2 * f * Z0 - 2 * x * X0 - 2 * y * Y0;
                        

                        double difMin = R;
                        Point bestPt = new Point();
                        bool noInter = false;
                        // iteration 
                        for (int k = 0; k < maxiteration; k++)
                        {
                            double C = X0 * X0 + Y0 * Y0 + Z0 * Z0 - tempR * tempR;
                            double derta = B * B - 4 * A * C;

                            if (derta < 0)
                            {
                                noInter = true;
                                break;
                            }

                            double lambda = (- B - Math.Sqrt(derta)) / (2 * A);

                            // in camera's coordinate system
                            Point interCam = new Point(lambda * x, lambda * y, -lambda * f);


                            // in Moon's coordinate system
                            Point interMoon = cam2moon * interCam;

                            double h = _moon.getElevation(interMoon);

                            double dif = h + R - tempR;

                            if (Math.Abs(dif)< Math.Abs(difMin))
                            {
                                difMin = dif;
                                bestPt = interMoon;
                            }


                            tempR = nextR(tempR, difMin);
                        }

                        if (noInter)
                            bitmaps[ag].SetPixel(col, row, black);
                        else
                        {
                            int gray = _moon.getGray(bestPt);
                            Color color = Color.FromArgb(gray, gray, gray);
                            bitmaps[ag].SetPixel(col, row, color);
                        }

                    }

                }
                notify(times, row+1);
            }


        }

        public void notify(int total, int row)
        {
            foreach(IObserver o in observers)
            {
                o.exec(total, row);
            }
        }

        public double nextR(double R, double dif)
        {
            return R + dif;
        }

        private Matrix moon2camMatrix(Point pos)
        {
            double[] BLH = _moon.threeD2geo(pos.x, pos.y, pos.z);
            // shift the origin to x0,y0,z0
            return MatFactory.getMatrixTowadsCenterRM(BLH, pos, _orbit.inclination());
        }
    }

    
    public enum Interpolate
    {
        NEAREST,
        LINEAR,
    }

    public class Moon
    {
        // we assume DOM and DEM have same the geo-datum(spheroid) and projection
        Dataset _DOM, _DEM;
        Band _DOMband, _DEMband;
        SpatialReference refPro,refGeo;

        CoordinateTransformation transGeo2Pro;

        Interpolate DEMInter, DOMInter;

        double[] _DEMpro2raster;
        double[] _DOMpro2raster;

        double _R;
        public Moon(Dataset DEM, Dataset DOM)
        {
            _DEM = DEM;
            _DOM = DOM;

            double[] _DEMtrans = new double[6];
            double[] _DOMtrans = new double[6];
            DEM.GetGeoTransform(_DEMtrans);
            DOM.GetGeoTransform(_DOMtrans);


            initMatrix(_DOMtrans, _DEMtrans);


            refPro = new SpatialReference(_DOM.GetProjection());
            refGeo = refPro.CloneGeogCS();

            transGeo2Pro = new CoordinateTransformation(refGeo, refPro);
            _R = refGeo.GetSemiMajor();

            DEMInter = Interpolate.NEAREST;
            DOMInter = Interpolate.NEAREST;

            _DOMband = _DOM.GetRasterBand(1);
            _DEMband = _DEM.GetRasterBand(1);
        }
        public double R() { return _R; }
        public void setDEMInterpolate(Interpolate interpolate)
        {
            DEMInter = interpolate;
        }

        public void setDOMInterpolate(Interpolate interpolate)
        {
            DOMInter = interpolate;
        }

        public double getElevation(double x,double y, double z)
        {
            double[] geo = threeD2geo(x, y, z);//B L 
            double[] lonlat2xy = { geo[1], geo[0] };//L B:lon lat

            transGeo2Pro.TransformPoint(lonlat2xy);

            double[] xy = DEMpro2raster(lonlat2xy[0], lonlat2xy[1]);
            x = xy[0];
            y = xy[1];
            switch (DEMInter)
            {
                case Interpolate.NEAREST:
                    {
                        int[] gray = new int[1];
                        _DEMband.ReadRaster((int)Math.Round(x), (int)Math.Round(y), 1, 1,
                            gray, 1, 1, 0, 0);

                        return gray[0];
                    }
                case Interpolate.LINEAR:
                    {
                        int[] grays = new int[4];
                        _DEMband.ReadRaster((int)x, (int)y, 2, 2,
                           grays, 2, 2, 0, 0);

                        int g11, g12, g21, g22;
                        g11 = grays[0];
                        g12 = grays[1];
                        g21 = grays[2];
                        g22 = grays[3];

                        double yInv = y - (int)y;
                        double xInv = x - (int)x;

                        double g1 = (1 - yInv) * g11 + yInv * g21;
                        double g2 = (1 - yInv) * g12 + yInv * g22;

                        return ((1 - xInv) * g1 + xInv * g2);
                    }
                default:
                    break;
            }
            return 0;
        }

        public double getElevation(Point pt)
        {
            return getElevation(pt.x, pt.y, pt.z);
        }
        public int getGray(double x, double y, double z)
        {
            double[] geo = threeD2geo(x, y, z);
            double[] lonlat2xy = { geo[1], geo[0] };
            transGeo2Pro.TransformPoint(lonlat2xy);


            double[] xy = DOMpro2raster(lonlat2xy[0], lonlat2xy[1]);
            x = xy[0];
            y = xy[1];
            switch (DOMInter)
            {
                case Interpolate.NEAREST:
                    {
                        int[] gray = new int[1];
                        _DOMband.ReadRaster((int)Math.Round(x), (int)Math.Round(y), 1, 1,
                            gray, 1, 1, 0, 0);

                        return gray[0];
                    } 
                case Interpolate.LINEAR:
                    {
                        int[] grays = new int[4];
                        _DOMband.ReadRaster((int)x, (int)y, 2, 2,
                           grays, 2, 2, 0, 0);

                        int g11, g12, g21, g22;
                        g11 = grays[0];
                        g12 = grays[1];
                        g21 = grays[2];
                        g22 = grays[3];

                        double yInv = y - (int)y;
                        double xInv = x - (int)x;

                        double g1 = (1 - yInv) * g11 + yInv * g21;
                        double g2 = (1 - yInv) * g12 + yInv * g22;

                        return (int)((1 - xInv) * g1 + xInv * g2);
                    }
                default:
                    break;
            }

            return 0;
        }

        public int getGray(Point pt)
        {
            return getGray(pt.x, pt.y, pt.z);
        }

        private void initMatrix(double[] transDOM, double[] transDEM)
        {
            double[] DOM = { transDOM[1], transDOM[2], transDOM[4], transDOM[5] };
            double[] DEM = { transDEM[1], transDEM[2], transDEM[4], transDEM[5] };

            Matrix OM = new Matrix(DOM, 2, 2);
            Matrix EM = new Matrix(DEM, 2, 2);

            OM = OM.invert();
            EM = EM.invert();

            _DOMpro2raster = new double[6];
            _DOMpro2raster[0] = OM.at(0, 0);
            _DOMpro2raster[1] = OM.at(0, 1);
            _DOMpro2raster[2] = OM.at(1, 0);
            _DOMpro2raster[3] = OM.at(1, 1);
            _DOMpro2raster[4] = transDOM[0];
            _DOMpro2raster[5] = transDOM[3];

            _DEMpro2raster = new double[6];
            _DEMpro2raster[0] = EM.at(0, 0);
            _DEMpro2raster[1] = EM.at(0, 1);
            _DEMpro2raster[2] = EM.at(1, 0);
            _DEMpro2raster[3] = EM.at(1, 1);
            _DEMpro2raster[4] = transDEM[0];
            _DEMpro2raster[5] = transDEM[3];

        }

        // only appropriate for *spheroid* moon
        // from 3D Cartesian coordinate system(measured in X,Y,Z) to 
        // Geographic coordinate system(measured in B,L,H)
        public double[] threeD2geo(double x, double y, double z)
        {
            double[] BLH = new double[3];
            double r = Math.Sqrt(x * x + y * y);
            BLH[0] = Calculator.rad2d(Math.Atan(z / r));
            BLH[1] = Calculator.rad2d(Math.Acos(x / r));
            BLH[2] = Math.Sqrt(r * r + z * z) - _R;
            return BLH;
        }

        public double[] threeD2geo(Point pt)
        {
            return threeD2geo(pt.x, pt.y, pt.z);
        }


        // from projection to raster(measured in row and col)
        // DEM and DOM have different resolution, so there are 2 functions
        private double[] DEMpro2raster(double x, double y)
        {
            double x_ = x - _DEMpro2raster[4];
            double y_ = y - _DEMpro2raster[5];

            double[] xy = new double[2];
            
            xy[0] = _DEMpro2raster[0] * x_ + _DEMpro2raster[1] * y_;
            xy[1] = _DEMpro2raster[2] * x_ + _DEMpro2raster[3] * y_;

            return xy;
        }

        private double[] DOMpro2raster(double x, double y)
        {

            double x_ = x - _DOMpro2raster[4];
            double y_ = y - _DOMpro2raster[5];

            double[] xy = new double[2];
            xy[0] = _DOMpro2raster[0] * x_ + _DOMpro2raster[1] * y_;
            xy[1] = _DOMpro2raster[2] * x_ + _DOMpro2raster[3] * y_;
            return xy;
        }
    }


    public static class MatFactory
    {
        public static Matrix getMatrixTowadsCenter(double latitude, double longitude, double inclination)
        {
            Matrix z = getRMatrixZ(Calculator.d2rad(longitude));
            Matrix y = getRMatrixY(Math.PI / 2 - Calculator.d2rad(latitude));
            Matrix z_ = getRMatrixZ(Calculator.d2rad(inclination));
            return z_ * y * z;

        }

        public static Matrix getMatrixTowadsCenterRM(double[] BL, Point pos, double inclination)
        {
            Matrix move = MatFactory.getMatrixMove(pos.x, pos.y, pos.z);
            Matrix rt = MatFactory.getMatrixTowadsCenter(BL[0], BL[1], inclination);

            return rt * move;
        }
        public static Matrix getMatrixMove(double x0, double y0, double z0)
        {
            Matrix I = Matrix.identity(4);

            I.set(0, 3, -x0);
            I.set(1, 3, -y0);
            I.set(2, 3, -z0);

            return I;

        }
        public static Matrix getMatrixMove(Point pt)
        {
            return getMatrixMove(pt.x, pt.y, pt.z);
        }

        // rotate around x axis
        public static Matrix getRMatrixX(double thetaX)
        {
            Matrix x = new Matrix(1, 0, 0,
                                  0, Math.Cos(thetaX), Math.Sin(thetaX),
                                  0, -Math.Sin(thetaX), Math.Cos(thetaX));

            Matrix l = Matrix.zeros(3, 1);

            x = Matrix.hconcat(x, l);

            l = Matrix.zeros(1, 4);
            l.set(0, 3, 1);

            x = Matrix.vconcat(x, l);
            return x;
        }

        // rotate around y axis
        public static Matrix getRMatrixY(double thetaY)
        {


            Matrix y = new Matrix(Math.Cos(thetaY), 0, -Math.Sin(thetaY),
                                  0, 1, 0,
                                  Math.Sin(thetaY), 0, Math.Cos(thetaY));
            Matrix l = Matrix.zeros(3, 1);

            y = Matrix.hconcat(y, l);

            l = Matrix.zeros(1, 4);
            l.set(0, 3, 1);

            y = Matrix.vconcat(y, l);
            return y;
        }

        // rotate around z axis
        public static Matrix getRMatrixZ(double thetaZ)
        {

            Matrix z = new Matrix(Math.Cos(thetaZ), Math.Sin(thetaZ), 0,
                                  -Math.Sin(thetaZ), Math.Cos(thetaZ), 0,
                                  0, 0, 1);
            Matrix l = Matrix.zeros(3, 1);

            z = Matrix.hconcat(z, l);

            l = Matrix.zeros(1, 4);
            l.set(0, 3, 1);

            z = Matrix.vconcat(z, l);
            return z;
        }
    }

    public class Orbit
    {
        private double _a, _e, _i, _Omega, _w, _n, _t0;
        public Orbit(double a, double e, double i, double Omega, double w, double n, double t0)
        {
            _a = a;
            _e = e;
            _i = Calculator.d2rad(i);
            _Omega = Calculator.d2rad(Omega);
            _w = Calculator.d2rad(w);
            _n = n;
            _t0 = t0;
        }

        public double inclination()
        {
            return Calculator.rad2d(_i);
        }
        // x,y,z
        public Point Pos3D(double t)
        {
            double x, y, z;
            double M = _n * (t - _t0);
            double f = M2f(M);//true anomaly
            double wplusf = _w + f;//w+f
            x = Math.Cos(_Omega) * Math.Cos(wplusf) - Math.Sin(_Omega) * Math.Sin(wplusf) * Math.Cos(_i);
            y = Math.Sin(_Omega) * Math.Cos(wplusf) + Math.Cos(_Omega) * Math.Sin(wplusf) * Math.Cos(_i);
            z = Math.Sin(wplusf) * Math.Sin(_i);

            double factor = _a * (1 - _e * _e) / (1 + _e * Math.Cos(f));
            double[] pos = { x,y,z };

            Point res = new Point(pos);
            //(factor * res).show();
            return factor*res;
        }
        
        // Newton descent method
        // mean anomaly to eccentric anomaly
        private double M2E(double M, int max_iteration = 100)
        {
            double xk = M, xk_;
            double threshold = 0.00000001;
            double fk = -_e * Math.Sin(xk);
            int i;
            for (i = 0; i < max_iteration; i++)
            {
                xk_ = xk - fk / (1 - _e * Math.Cos(xk));
                if (Math.Abs(fk) < threshold)
                {
                    break;
                }
                xk = xk_;
                fk = xk - _e * Math.Sin(xk) - M;
            }
            return xk;
        }

        // eccentric anomaly to ture anomaly
        private double E2f(double E)
        {
            return Math.Atan(Math.Sqrt(1 - _e * _e) * Math.Sin(E) / (Math.Cos(E) - _e));
        }

        private double M2f(double M)
        {
            return E2f(M2E(M));
        }
    }

    public class Camera
    {
        double _pixelSize;
        double _pd;// principal distance
        int _width;//pixel
        double _x;

        int _count;
        double[] _y;


        // angle unit is °
        public Camera(double pd,double pixsize, int width, double[] angle)
        {
            _pixelSize = pixsize;
            _pd = pd;
            _width = width;

            _x = width / 2 * _pixelSize;

            _count = angle.GetLength(0);
            _y = new double[_count];
            for (int i=0;i< _count; i++)
                _y[i] = Math.Tan(Calculator.d2rad(angle[i])) * _pd;
        }
        public int width()
        {
            return _width;
        }
        public int count() { return _count; }
        public double principalDistance()
        {
            return _pd;
        }

        // given u(pix) return frontxy,midxy,backxy
        public double[,] getXY(int u)
        {
            double x = u * _pixelSize - _x;

            double[,] res = new double[_count, 2];

            for (int i=0;i< _count; i++)
            {
                res[i, 0] = x;
                res[i, 1] = _y[i];
            }

            return res;
        }



    }
    public class Point
    {
        public double x, y, z;

        public Point()
        {
            x = 0;y = 0;z = 0;
        }
        public Point(Matrix xyz)
        {
            x = xyz.at(0);
            y = xyz.at(1);
            z = xyz.at(2);
        }
        public Point(double[] xyz)
        {
            x = xyz[0];
            y = xyz[1];
            z = xyz[2];
        }
        public Point(double x,double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Point operator *(double k, Point pt)
        {
            return new Point(pt.x * k, pt.y * k, pt.z * k);
        }
        public static Point operator *(Point pt, double k)
        {
            return new Point(pt.x * k, pt.y * k, pt.z * k);
        }

        public static Point operator /(double k, Point pt)
        {
            if (k == 0)
                throw new Exception("0 can't be denominator!");
            return new Point(pt.x / k, pt.y / k, pt.z / k);
        }
        public void show()
        {
            Console.WriteLine("({0}, {1}, {2})", x,y,z);
        }
    }

    // only for double
    public class Matrix
    {
        private double[,] _value;

        //  --------------------------- Constructor Start --------------------------------------
        public Matrix(double x, double y, double z)
        {
            _value = new double[3, 1];
            _value[0, 0] = x;
            _value[1, 0] = y;
            _value[2, 0] = z;
        }

        public Matrix(double a00, double a01, double a02,
                      double a10, double a11, double a12,
                      double a20, double a21, double a22)
        {
            _value = new double[3, 3];
            _value[0, 0] = a00; _value[0, 1] = a01; _value[0, 2] = a02;
            _value[1, 0] = a10; _value[1, 1] = a11; _value[1, 2] = a12;
            _value[2, 0] = a20; _value[2, 1] = a21; _value[2, 2] = a22;

        }

        public double at(int n)
        {
            int row = (int)(n / cols());
            int col = n - row * cols();
            return _value[row, col];
        }
        public double at(int r, int c)
        {
            return _value[r, c];
        }

        public void set(int r, int c, double value)
        {
            _value[r, c] = value;
        }
        public Matrix(double[,] a)
        {
            int r = a.GetLength(0);
            int c = a.GetLength(1);
            _value = new double[r, c];

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    _value[i, j] = a[i, j];
                }
            }


        }
        public Matrix(double[] val, int row, int col)
        {
            if (val.Length != row * col)
                throw new Exception("Failed to construct class Matrix!");

            _value = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    _value[i, j] = val[i * col + j];
                }
            }
        }
        public Matrix(Point pt)
        {
            _value = new double[4,1];
            _value[0,0] = pt.x;
            _value[1,0] = pt.y;
            _value[2,0] = pt.z;
            _value[3,0] = 1;
        }
        static public Matrix zeros(int row, int col)
        {
            double[,] value = new double[row, col];
            return new Matrix(value);
        }

        static public Matrix diag(double[] val)
        {
            int len = val.GetLength(0);
            Matrix res = Matrix.zeros(len, len);
            for (int i = 0; i < len; i++)
            {
                res.set(i, i, val[i]);
            }
            return res;
        }
        static public Matrix identity(int order)
        {
            Matrix res = Matrix.zeros(order, order);
            for (int i = 0; i < order; i++)
            {
                res.set(i, i, 1);
            }

            return res;
        }
        // horizontal
        // A = [1 2 3] B =[4]
        // then hconcat(A,B) = [1 2 3 4]
        static public Matrix hconcat(Matrix A, Matrix B)
        {

            if (A.rows() != B.rows())
                throw new Exception("Fail to concatenate!");

            double[,] res = new double[A.rows(), A.cols() + B.cols()];
            double[,] a = A.value();
            double[,] b = B.value();
            int r = A.rows();
            int c = A.cols();
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    res[i, j] = a[i, j];
                }
            }

            for (int i = 0; i < r; i++)
            {
                for (int j = c; j < res.GetLength(1); j++)
                {
                    res[i, j] = b[i, j - c];
                }
            }

            return new Matrix(res);
        }

        // horizontal
        // A = [1 2 3] B =[4 5 6]
        // then vconcat(A,B) = [1 2 3;4 5 6]
        static public Matrix vconcat(Matrix A, Matrix B)
        {

            if (A.cols() != B.cols())
                throw new Exception("Fail to concatenate!");

            double[,] res = new double[A.rows() + B.rows(), A.cols()];
            double[,] a = A.value();
            double[,] b = B.value();
            int r = A.rows();
            int c = A.cols();
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    res[i, j] = a[i, j];
                }
            }

            for (int i = r; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < c; j++)
                {
                    res[i, j] = b[i - r, j];
                }
            }

            return new Matrix(res);
        }
        public Matrix(Matrix A)
        {
            int r = A.rows();
            int c = A.cols();
            _value = new double[r, c];
            double[,] val = A.value();
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    _value[i, j] = val[i, j];
                }
            }
        }

        //  --------------------------- Constructor End --------------------------------------

        public static Matrix operator *(Matrix A, Matrix B)
        {
            double[,] res = new double[A.rows(), B.cols()];
            double[,] a = A.value();
            double[,] b = B.value();
            for (int m = 0; m < A.rows(); m++)
            {
                for (int n = 0; n < B.cols(); n++)
                {
                    double sum = 0;
                    for (int k = 0; k < A.cols(); k++)
                    {
                        sum += a[m, k] * b[k, n];
                    }
                    res[m, n] = sum;
                }
            }

            return new Matrix(res);
        }

        public static Point operator * (Matrix M, Point pt)
        {
            if (M.rows() != M.cols())
                throw new Exception("Failed to multiply!");
            

            Matrix ptM;
            if (M.rows()==4)
            {
                ptM = new Matrix(pt);
                Matrix tmp = M * ptM;

                double t = tmp.at(3);
                return new Point(tmp.at(0) / t, tmp.at(1) / t, tmp.at(2) / t);
            }
            else if (M.rows()==3)
            {
                ptM = new Matrix(pt.x, pt.y, pt.z);
                Matrix tmp = M * ptM;

                return new Point(tmp);
            }


            throw new Exception("Failed to multiply: Matrix must be 3x3 or 4x4");
        }

        //public static Matrix 
        public static Matrix operator +(Matrix A, Matrix B)
        {
            if ((A.rows() != B.rows()) || (A.cols() != B.cols()))
                throw new Exception("Can't add matrics having different size!");

            double[,] a = A.value();
            double[,] b = B.value();
            double[,] res = new double[A.rows(), A.cols()];
            for (int i = 0; i < A.rows(); i++)
            {
                for (int j = 0; j < A.cols(); j++)
                {
                    res[i, j] = a[i, j] + b[i, j];
                }
            }

            return new Matrix(res);
        }

        public static Matrix operator -(Matrix A, Matrix B)
        {

            if ((A.rows() != B.rows()) || (A.cols() != B.cols()))
                throw new Exception("Can't add matrics having different size!");

            double[,] a = A.value();
            double[,] b = B.value();
            double[,] res = new double[A.rows(), A.cols()];
            for (int i = 0; i < A.rows(); i++)
            {
                for (int j = 0; j < A.cols(); j++)
                {
                    res[i, j] = a[i, j] - b[i, j];
                }
            }

            return new Matrix(res);
        }

        public static Matrix operator -(Matrix A)
        {
            double[,] res = new double[A.rows(), A.cols()];
            for (int i = 0; i < A.rows(); i++)
            {
                for (int j = 0; j < A.cols(); j++)
                {
                    res[i, j] = -A.at(i, j);
                }
            }

            return new Matrix(res);
        }
        // deep copy
        public double[,] value()
        {
            int r = rows();
            int c = cols();
            double[,] res = new double[r, c];

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    res[i, j] = _value[i, j];
                }
            }

            return res;

        }
        public int rows()
        {
            return _value.GetLength(0);
        }
        public int cols()
        {
            return _value.GetLength(1);
        }

        // decimal places to show
        public void show(int number = 4)
        {
            Console.WriteLine();
            for (int i = 0; i < rows(); i++)
            {
                for (int j = 0; j < cols(); j++)
                {
                    Console.Write("{0}\t", _value[i, j].ToString("F" + number));
                }
                Console.WriteLine();
            }
        }

        public Matrix invert()
        {

            if (rows() != cols())
            {
                throw new Exception("Failed to invert!");
            }

            double[,] res = new double[rows(), cols()];
            bool ok = Invert(_value, res);
            if (ok)
                return new Matrix(res);
            else
                throw new Exception("Failed to invert!");
        }

        public double determinant()
        {
            return Det(_value);
        }

        // Subtract row m-1 and col n-1,  return a matrix
        private double[,] M(double[,] a, int m, int n)
        {
            int row = a.GetLength(0);
            int col = a.GetLength(1);
            int i, j;
            double[,] aa = new double[row - 1, col - 1];
            int offsety = 0;
            int offsetx;
            for (i = 0; i < row; i++)
            {
                offsetx = 0;
                if (i == m)
                {
                    offsety = 1;
                    continue;
                }
                for (j = 0; j < col; j++)
                {
                    if (j == n)
                    {
                        offsetx = 1;
                        continue;
                    }
                    aa[i - offsety, j - offsetx] = a[i, j];
                }
            }
            return aa;
        }

        private double Det(double[,] a)
        {
            if (a.GetLength(0) == 1)
            {
                return a[0, 0];
            }
            else if (a.GetLength(0) == 2)
            {
                return a[0, 0] * a[1, 1] -a[0, 1] * a[1, 0];
            }
            else
            {
                double sum = 0;
                int n = a.GetLength(0);
                for (int i = 0; i < n; i++)
                {
                    sum += a[0, i] * Math.Pow(-1, i) * Det(M(a, 0, i));
                }
                return sum;
            }

        }

        private bool Invert(double[,] a, double[,] result)
        {
            if (Det(a) == 0)
            {
                Console.WriteLine(" Can't compute matrix invert: Determinant of matrix is 0");
                return false;
            }
            int row = a.GetLength(0);
            int col = a.GetLength(1);
            if (row != col)
            {
                Console.WriteLine("Can't compute matrix invert: Row != Collum");
                return false;
            }
            int i, j;
            double det_inv = 1 / Det(a);
            for (i = 0; i < row; i++)
            {
                for (j = 0; j < col; j++)
                {
                    result[i, j] = det_inv * Math.Pow(-1, i + j) * Det(M(a, j, i));
                }
            }

            return true;
        }
    }
    public static class Calculator
    {
        static double pi = Math.PI;

        static public double d2rad(double d)
        {
            return d / 180.0 * pi;
        }
        static public double rad2d(double rad)
        {
            return rad / pi * 180;
        }
    }

    namespace Observer
    {
        public abstract class IObserver
        {
            public abstract void exec(int total, int row);
        }


        public class StatusbarObserver: IObserver
        {
            Label _label;
            public StatusbarObserver(Label label)
            {
                _label = label;
            }
            public override void exec(int total, int row)
            {
                if (row<total)
                {
                    _label.Text = string.Format("Status: {0} rows have been finished, " +
                    "{1} rows remain",
                    row, total - row);
                }
                else
                {
                    _label.Text = "Simulation is completed. Please click \"Display\" to continue.";
                }
                _label.Refresh();
            }
        }

        public class ProgressbarObserver: IObserver
        {
            ProgressBar _bar;
            public ProgressbarObserver(ProgressBar bar) { _bar = bar; }

            public override void exec(int total, int row)
            {
                _bar.Value = 100 * row / total;
            }
        }

        public class ImageObserver: IObserver
        {

            Result[] results;
            Bitmap[] bitmaps;
            public ImageObserver(Bitmap[] bitmaps, Result[] results)
            {
                this.results = results;
                this.bitmaps = bitmaps;
            }
            public override void exec(int total, int row)
            {
                if (row % 5 == 0)
                {
                    for (int i = 0; i < results.Length; i++)
                        results[i].updateImage(bitmaps[i]);
                }
            }
        }
    }

}
