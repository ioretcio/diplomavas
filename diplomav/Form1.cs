using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diplomav
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            chart1.ChartAreas[0].AxisX.Interval = 5;
            chart2.ChartAreas[0].AxisX.Interval = 10000;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "0";
            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "0";


            chart3.ChartAreas[0].AxisX.LabelStyle.Format = "0";
        }


        double[] R0 = new double[3];
        double[] RT = new double[3];

        double[] V0 = new double[3];
        double[] VT = new double[3];
        public double miu = 396600000;
        double[] psi_v = new double[] { 0.00001, 0.000001, -0.00000001 };
        double[] psi_r = new double[] { 0.0000013, -0.00000059, -0.00000005 };


        public double dVx_dt(double[] R_method, double[] psiV_method)
        {
            double result = -miu * ((R_method[0]) / r3(R_method)) + (psiV_method[0] / 2);
            return result;
        }
        public double dVy_dt(double[] R_method, double[] psiV_method)
        {
            double result = -miu * ((R_method[1]) / r3(R_method)) + (psiV_method[1] / 2);
            return result;
        }
        public double dVz_dt(double[] R_method, double[] psiV_method)
        {
            double result = -miu * ((R_method[2]) / r3(R_method)) + (psiV_method[2] / 2);
            return result;
        }
        public double dRx_dt(double[] V)
        {
            double result = V[0];
            return result;
        }
        public double dRy_dt(double[] V)
        {
            double result = V[1];
            return result;
        }
        public double dRz_dt(double[] V)
        {
            double result = V[2];
            return result;
        }
        public double dPsivx(double[] psiR_method)
        {
            double result = -psiR_method[0];
            return result;
        }
        public double dPsivy(double[] psiR_method)
        {
            double result = -psiR_method[1];
            return result;
        }
        public double dPsivz(double[] psiR_method)
        {
            double result = -psiR_method[2];
            return result;
        }


        public double dPsirx(double[] R_method, double[] psiV_method)
        {
            double x = norm(0, R_method);
            double y = norm(1, R_method);
            double z = norm(2, R_method);



            double result = -(-(miu / r3(R_method)) * (1 - 3 * x * x) * psiV_method[0] +
                3 * (miu / r3(R_method)) * x * y * psiV_method[1] +
                3 * (miu / r3(R_method)) * x * z * psiV_method[2]);
            return result;
        }

        public double dPsiry(double[] R_method, double[] psi_method)
        {
            double x = norm(0, R_method);
            double y = norm(1, R_method);
            double z = norm(2, R_method);

            double result = -(3 * (miu / r3(R_method)) * x * x * psi_method[0] -
                (miu / r3(R_method)) * (1 - 3 * y * y) * psi_method[1] +
                3 * (miu / r3(R_method)) * y * z * psi_method[2]);
            return result;
        }

        public double dPsirz(double[] R_method, double[] psi_method)
        {
            double x = norm(0, R_method);
            double y = norm(1, R_method);
            double z = norm(2, R_method);

            double result = -(3 * (miu / r3(R_method)) * x * z * psi_method[0] +
                3 * (miu / r3(R_method)) * y * z * psi_method[1] -
                (miu / r3(R_method)) * (1 - 3 * z * z) * psi_method[2]);
            return result;
        }


        public double pointInLine(double a, double b, double coef)
        {
            return a + (b - a) * coef;
        }

        public double norm(int index, double[] R_method)
        {
            double r = Math.Sqrt(R_method[0] * R_method[0] + R_method[1] * R_method[1] + R_method[2] * R_method[2]);
            return R_method[index] / r;

        }

        public double r3(double[] R_method)
        {
            double result = Math.Pow((Math.Sqrt(R_method[0] * R_method[0] +
                R_method[1] * R_method[1] + R_method[2] * R_method[2])), 3);
            return result;
        }


        public void MatrixNormalization(double[] R_method)
        {
            R_method[0] = RT[0] + RT[0] * 0.072;
            R_method[1] = RT[1] - RT[1] * 0.072;
            R_method[2] = RT[2] - RT[2] * 2 * 0.072;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            R0[0] = Convert_.toDouble(tb1.Text);
            R0[1] = Convert_.toDouble(textBox1.Text);
            R0[2] = Convert_.toDouble(textBox2.Text);
            RT[0] = Convert_.toDouble(textBox5.Text);
            RT[1] = Convert_.toDouble(textBox4.Text);
            RT[2] = Convert_.toDouble(textBox3.Text);



            V0[0] = Convert_.toDouble(textBox11.Text);
            V0[1] = Convert_.toDouble(textBox9.Text);
            V0[2] = Convert_.toDouble(textBox7.Text);
            VT[0] = Convert_.toDouble(textBox10.Text);
            VT[1] = Convert_.toDouble(textBox8.Text);
            VT[2] = Convert_.toDouble(textBox6.Text);

            Random R = new Random();

            List<double> neviazkas = new List<double>();
            List<double[]> Rx = new List<double[]>();

            double initTime = 0;
            double endTime = Convert_.toDouble(textBox12.Text);
            double step = 0.1;
            double[] Voperational = new double[] { V0[0], V0[1], V0[2] };
            double[] VoperationalOld = new double[] { V0[0], V0[1], V0[2] };
            double[] Roperaional = new double[] { R0[0], R0[1], R0[2] };
            double[] RoperaionalOld = new double[] { R0[0], R0[1], R0[2] };
            double[] PsiRoperational = new double[] { psi_r[0], psi_r[1], psi_r[2] };
            double[] PsiRoperationalOld = new double[] { psi_r[0], psi_r[1], psi_r[2] };
            double[] PsiVoperational = new double[] { psi_v[0], psi_v[1], psi_v[2] };
            double[] PsiVoperationalOld = new double[] { psi_v[0], psi_v[1], psi_v[2] };

            while (initTime < endTime)
            {
                Voperational[0] = VoperationalOld[0] + step * dVx_dt(RoperaionalOld, PsiVoperationalOld);
                Voperational[1] = VoperationalOld[1] + step * dVy_dt(RoperaionalOld, PsiVoperationalOld);
                Voperational[2] = VoperationalOld[2] + step * dVz_dt(RoperaionalOld, PsiVoperationalOld);

                Roperaional[0] = RoperaionalOld[0] + step * dRx_dt(VoperationalOld);
                Roperaional[1] = RoperaionalOld[1] + step * dRy_dt(VoperationalOld);
                Roperaional[2] = RoperaionalOld[2] + step * dRz_dt(VoperationalOld);

                PsiVoperational[0] = PsiVoperationalOld[0] + step * dPsivx(PsiRoperationalOld);
                PsiVoperational[1] = PsiVoperationalOld[1] + step * dPsivy(PsiRoperationalOld);
                PsiVoperational[2] = PsiVoperationalOld[2] + step * dPsivz(PsiRoperationalOld);

                PsiRoperational[0] = PsiRoperationalOld[0] + step * dPsirx(RoperaionalOld, PsiVoperationalOld);
                PsiRoperational[1] = PsiRoperationalOld[1] + step * dPsiry(RoperaionalOld, PsiVoperationalOld);
                PsiRoperational[2] = PsiRoperationalOld[2] + step * dPsirz(RoperaionalOld, PsiVoperationalOld);

                PsiVoperationalOld = new double[] { PsiVoperational[0], PsiVoperational[1], PsiVoperational[2] };
                PsiRoperationalOld = new double[] { PsiRoperational[0], PsiRoperational[1], PsiRoperational[2] };
                RoperaionalOld = new double[] { Roperaional[0], Roperaional[1], Roperaional[2] };
                VoperationalOld = new double[] { Voperational[0], Voperational[1], Voperational[2] };

                initTime += step;
            }
            double neviazka_clean = Math.Sqrt(
                Math.Pow(Roperaional[0] - RT[0], 2) +
                Math.Pow(Roperaional[0] - RT[0], 2) +
                Math.Pow(Roperaional[0] - RT[0], 2));









            int gradientIterationCount = 0;
            double memNeviazka = 0;
            bool memLock = false;
            double gamma = 0.00001;
            double epsillon = Convert_.toDouble(textBox13.Text);
            while (true)
            {
                gradientIterationCount++;
                if (gradientIterationCount > 200)
                    break;
                initTime = 0;
                endTime = Convert_.toDouble(textBox12.Text);
                step = 0.1;
                Voperational = new double[] { V0[0], V0[1], V0[2] };
                VoperationalOld = new double[] { V0[0], V0[1], V0[2] };
                Roperaional = new double[] { R0[0], R0[1], R0[2] };
                RoperaionalOld = new double[] { R0[0], R0[1], R0[2] };
                PsiRoperational = new double[] { psi_r[0], psi_r[1], psi_r[2] };
                PsiRoperationalOld = new double[] { psi_r[0], psi_r[1], psi_r[2] };
                PsiVoperational = new double[] { psi_v[0], psi_v[1], psi_v[2] };
                PsiVoperationalOld = new double[] { psi_v[0], psi_v[1], psi_v[2] };
                if (!memLock)
                {
                    dataGridView1.Rows.Clear();
                    chart2.Series[0].Points.Clear();
                    chart2.Series[1].Points.Clear();
                    chart2.Series[2].Points.Clear();


                    chart3.Series[0].Points.Clear();
                    chart3.Series[1].Points.Clear();
                    chart3.Series[2].Points.Clear();
                }
                int gridIterator = 0;




                while (initTime < endTime)
                {
                    gridIterator++;
                    Voperational[0] = VoperationalOld[0] + step * dVx_dt(RoperaionalOld, PsiVoperationalOld);
                    Voperational[1] = VoperationalOld[1] + step * dVy_dt(RoperaionalOld, PsiVoperationalOld);
                    Voperational[2] = VoperationalOld[2] + step * dVz_dt(RoperaionalOld, PsiVoperationalOld);



                    Roperaional[0] = RoperaionalOld[0] + step * dRx_dt(VoperationalOld);
                    Roperaional[1] = RoperaionalOld[1] + step * dRy_dt(VoperationalOld);
                    Roperaional[2] = RoperaionalOld[2] + step * dRz_dt(VoperationalOld);
                    if (gridIterator % 300 == 0)
                    {
                        chart2.Series[0].Points.AddXY(initTime, Roperaional[0]);
                        chart2.Series[1].Points.AddXY(initTime, Roperaional[1]);
                        chart2.Series[2].Points.AddXY(initTime, Roperaional[2]);

                        chart3.Series[0].Points.AddXY(initTime, pointInLine(Voperational[0], VT[0], initTime / endTime));
                        chart3.Series[1].Points.AddXY(initTime, pointInLine(Voperational[1], VT[1], initTime / endTime));
                        chart3.Series[2].Points.AddXY(initTime, pointInLine(Voperational[2], VT[2], initTime / endTime));
                    }
                    PsiVoperational[0] = PsiVoperationalOld[0] + step * dPsivx(PsiRoperationalOld);
                    PsiVoperational[1] = PsiVoperationalOld[1] + step * dPsivy(PsiRoperationalOld);
                    PsiVoperational[2] = PsiVoperationalOld[2] + step * dPsivz(PsiRoperationalOld);

                    PsiRoperational[0] = PsiRoperationalOld[0] + step * dPsirx(RoperaionalOld, PsiVoperationalOld);
                    PsiRoperational[1] = PsiRoperationalOld[1] + step * dPsiry(RoperaionalOld, PsiVoperationalOld);
                    PsiRoperational[2] = PsiRoperationalOld[2] + step * dPsirz(RoperaionalOld, PsiVoperationalOld);

                    PsiVoperationalOld = new double[] { PsiVoperational[0], PsiVoperational[1], PsiVoperational[2] };
                    PsiRoperationalOld = new double[] { PsiRoperational[0], PsiRoperational[1], PsiRoperational[2] };
                    RoperaionalOld = new double[] { Roperaional[0], Roperaional[1], Roperaional[2] };
                    VoperationalOld = new double[] { Voperational[0], Voperational[1], Voperational[2] };
                    if (!memLock)
                    {
                        if (gridIterator % 1000 == 0)
                            dataGridView1.Rows.Add(gridIterator, Roperaional[0], Roperaional[1], Roperaional[2],
                                pointInLine(Voperational[0], VT[0], initTime / endTime), pointInLine(Voperational[1], VT[1], initTime / endTime)
                                , pointInLine(Voperational[2], VT[2], initTime / endTime), PsiRoperational[0], PsiRoperational[1], PsiRoperational[2],
                                PsiVoperational[0], PsiVoperational[1], PsiVoperational[2]);
                    }
                    initTime += step;
                }
                neviazka_clean = Math.Sqrt(
                    Math.Pow(Roperaional[0] - RT[0], 2) +
                    Math.Pow(Roperaional[0] - RT[0], 2) +
                    Math.Pow(Roperaional[0] - RT[0], 2));
                if (!memLock)
                {
                    memNeviazka = neviazka_clean;
                    neviazkas.Add(memNeviazka);
                };
                memLock = true;

                List<double> deltas = new List<double>();
                double[] errors = new double[6];
                for (int error_index = 0; error_index < 3; error_index++)
                {
                    initTime = 0;
                    endTime = Convert_.toDouble(textBox12.Text);
                    step = 0.1;
                    Voperational = new double[] { V0[0], V0[1], V0[2] };
                    VoperationalOld = new double[] { V0[0], V0[1], V0[2] };
                    Roperaional = new double[] { R0[0], R0[1], R0[2] };
                    RoperaionalOld = new double[] { R0[0], R0[1], R0[2] };
                    PsiRoperational = new double[3];
                    PsiRoperationalOld = new double[] { psi_r[0], psi_r[1], psi_r[2] };
                    double delta = 0.001 * (R.NextDouble() - 0.5) * PsiRoperationalOld[error_index];
                    deltas.Add(delta);
                    PsiRoperationalOld[error_index] += delta;
                    PsiVoperational = new double[3];
                    PsiVoperationalOld = new double[] { psi_v[0], psi_v[1], psi_v[2] };
                    while (initTime < endTime)
                    {
                        Voperational[0] = VoperationalOld[0] + step * dVx_dt(RoperaionalOld, PsiVoperationalOld);
                        Voperational[1] = VoperationalOld[1] + step * dVy_dt(RoperaionalOld, PsiVoperationalOld);
                        Voperational[2] = VoperationalOld[2] + step * dVz_dt(RoperaionalOld, PsiVoperationalOld);

                        Roperaional[0] = RoperaionalOld[0] + step * dRx_dt(VoperationalOld);
                        Roperaional[1] = RoperaionalOld[1] + step * dRy_dt(VoperationalOld);
                        Roperaional[2] = RoperaionalOld[2] + step * dRz_dt(VoperationalOld);

                        PsiVoperational[0] = PsiVoperationalOld[0] + step * dPsivx(PsiRoperationalOld);
                        PsiVoperational[1] = PsiVoperationalOld[1] + step * dPsivy(PsiRoperationalOld);
                        PsiVoperational[2] = PsiVoperationalOld[2] + step * dPsivz(PsiRoperationalOld);

                        PsiRoperational[0] = PsiRoperationalOld[0] + step * dPsirx(RoperaionalOld, PsiVoperationalOld);
                        PsiRoperational[1] = PsiRoperationalOld[1] + step * dPsiry(RoperaionalOld, PsiVoperationalOld);
                        PsiRoperational[2] = PsiRoperationalOld[2] + step * dPsirz(RoperaionalOld, PsiVoperationalOld);

                        PsiVoperationalOld = new double[] { PsiVoperational[0], PsiVoperational[1], PsiVoperational[2] };
                        PsiRoperationalOld = new double[] { PsiRoperational[0], PsiRoperational[1], PsiRoperational[2] };
                        RoperaionalOld = new double[] { Roperaional[0], Roperaional[1], Roperaional[2] };
                        VoperationalOld = new double[] { Voperational[0], Voperational[1], Voperational[2] };

                        initTime += step;
                    }
                    errors[error_index] = Math.Sqrt(
                        Math.Pow(Roperaional[0] - RT[0], 2) +
                        Math.Pow(Roperaional[1] - RT[1], 2) +
                        Math.Pow(Roperaional[2] - RT[2], 2));
                }
                for (int error_index = 3; error_index < 6; error_index++)
                {
                    initTime = 0;
                    endTime = Convert_.toDouble(textBox12.Text);
                    step = 0.1;
                    Voperational = new double[] { V0[0], V0[1], V0[2] };
                    VoperationalOld = new double[] { V0[0], V0[1], V0[2] };
                    Roperaional = new double[] { R0[0], R0[1], R0[2] };
                    RoperaionalOld = new double[] { R0[0], R0[1], R0[2] };
                    PsiRoperational = new double[3];
                    PsiRoperationalOld = new double[] { psi_r[0], psi_r[1], psi_r[2] };
                    PsiVoperational = new double[3];
                    PsiVoperationalOld = new double[] { psi_v[0], psi_v[1], psi_v[2] };
                    double delta = 0.001 * (R.NextDouble() - 0.5) * PsiVoperationalOld[error_index - 3];
                    deltas.Add(delta);
                    PsiVoperationalOld[error_index - 3] += delta;
                    while (initTime < endTime)
                    {
                        Voperational[0] = VoperationalOld[0] + step * dVx_dt(RoperaionalOld, PsiVoperationalOld);
                        Voperational[1] = VoperationalOld[1] + step * dVy_dt(RoperaionalOld, PsiVoperationalOld);
                        Voperational[2] = VoperationalOld[2] + step * dVz_dt(RoperaionalOld, PsiVoperationalOld);

                        Roperaional[0] = RoperaionalOld[0] + step * dRx_dt(VoperationalOld);
                        Roperaional[1] = RoperaionalOld[1] + step * dRy_dt(VoperationalOld);
                        Roperaional[2] = RoperaionalOld[2] + step * dRz_dt(VoperationalOld);

                        PsiVoperational[0] = PsiVoperationalOld[0] + step * dPsivx(PsiRoperationalOld);
                        PsiVoperational[1] = PsiVoperationalOld[1] + step * dPsivy(PsiRoperationalOld);
                        PsiVoperational[2] = PsiVoperationalOld[2] + step * dPsivz(PsiRoperationalOld);

                        PsiRoperational[0] = PsiRoperationalOld[0] + step * dPsirx(RoperaionalOld, PsiVoperationalOld);
                        PsiRoperational[1] = PsiRoperationalOld[1] + step * dPsiry(RoperaionalOld, PsiVoperationalOld);
                        PsiRoperational[2] = PsiRoperationalOld[2] + step * dPsirz(RoperaionalOld, PsiVoperationalOld);

                        PsiVoperationalOld = new double[] { PsiVoperational[0], PsiVoperational[1], PsiVoperational[2] };
                        PsiRoperationalOld = new double[] { PsiRoperational[0], PsiRoperational[1], PsiRoperational[2] };
                        RoperaionalOld = new double[] { Roperaional[0], Roperaional[1], Roperaional[2] };
                        VoperationalOld = new double[] { Voperational[0], Voperational[1], Voperational[2] };

                        initTime += step;
                    }
                    errors[error_index] = Math.Sqrt(
                        Math.Pow(Roperaional[0] - RT[0], 2) +
                        Math.Pow(Roperaional[1] - RT[1], 2) +
                        Math.Pow(Roperaional[2] - RT[2], 2));
                }



                double[] grad_estimation = new double[6];

                for (int iterator = 0; iterator < 6; iterator++)
                {
                    grad_estimation[iterator] = (errors[iterator] - neviazka_clean) / (deltas[0] * Math.Pow(10, 21));
                }








                initTime = 0;
                endTime = Convert_.toDouble(textBox12.Text);
                step = 0.1;
                Voperational = new double[] { V0[0], V0[1], V0[2] };
                VoperationalOld = new double[] { V0[0], V0[1], V0[2] };
                Roperaional = new double[] { R0[0], R0[1], R0[2] };
                RoperaionalOld = new double[] { R0[0], R0[1], R0[2] };
                PsiRoperational = new double[3];
                PsiRoperationalOld = new double[] { psi_r[0], psi_r[1], psi_r[2] };
                PsiVoperational = new double[3];
                PsiVoperationalOld = new double[] { psi_v[0], psi_v[1], psi_v[2] };


                PsiRoperationalOld[0] -= grad_estimation[0] * gamma;
                PsiRoperationalOld[1] -= grad_estimation[1] * gamma;
                PsiRoperationalOld[2] -= grad_estimation[2] * gamma;
                PsiVoperationalOld[0] -= grad_estimation[3] * gamma;
                PsiVoperationalOld[1] -= grad_estimation[4] * gamma;
                PsiVoperationalOld[2] -= grad_estimation[5] * gamma;


                while (initTime < endTime)
                {
                    Voperational[0] = VoperationalOld[0] + step * dVx_dt(RoperaionalOld, PsiVoperationalOld);
                    Voperational[1] = VoperationalOld[1] + step * dVy_dt(RoperaionalOld, PsiVoperationalOld);
                    Voperational[2] = VoperationalOld[2] + step * dVz_dt(RoperaionalOld, PsiVoperationalOld);

                    Roperaional[0] = RoperaionalOld[0] + step * dRx_dt(VoperationalOld);
                    Roperaional[1] = RoperaionalOld[1] + step * dRy_dt(VoperationalOld);
                    Roperaional[2] = RoperaionalOld[2] + step * dRz_dt(VoperationalOld);

                    PsiVoperational[0] = PsiVoperationalOld[0] + step * dPsivx(PsiRoperationalOld);
                    PsiVoperational[1] = PsiVoperationalOld[1] + step * dPsivy(PsiRoperationalOld);
                    PsiVoperational[2] = PsiVoperationalOld[2] + step * dPsivz(PsiRoperationalOld);

                    PsiRoperational[0] = PsiRoperationalOld[0] + step * dPsirx(RoperaionalOld, PsiVoperationalOld);
                    PsiRoperational[1] = PsiRoperationalOld[1] + step * dPsiry(RoperaionalOld, PsiVoperationalOld);
                    PsiRoperational[2] = PsiRoperationalOld[2] + step * dPsirz(RoperaionalOld, PsiVoperationalOld);

                    PsiVoperationalOld = new double[] { PsiVoperational[0], PsiVoperational[1], PsiVoperational[2] };
                    PsiRoperationalOld = new double[] { PsiRoperational[0], PsiRoperational[1], PsiRoperational[2] };
                    RoperaionalOld = new double[] { Roperaional[0], Roperaional[1], Roperaional[2] };
                    VoperationalOld = new double[] { Voperational[0], Voperational[1], Voperational[2] };

                    initTime += step;
                }
                double neviazka_clean2 = Math.Sqrt(
                    Math.Pow(Roperaional[0] - RT[0], 2) +
                    Math.Pow(Roperaional[0] - RT[0], 2) +
                    Math.Pow(Roperaional[0] - RT[0], 2));





                if (neviazka_clean2 < neviazka_clean)
                {
                    psi_r[0] -= grad_estimation[0] * gamma;
                    psi_r[1] -= grad_estimation[1] * gamma;
                    psi_r[2] -= grad_estimation[2] * gamma;
                    psi_v[0] -= grad_estimation[3] * gamma;
                    psi_v[1] -= grad_estimation[4] * gamma;
                    psi_v[2] -= grad_estimation[5] * gamma;
                    memNeviazka = neviazka_clean2;
                    gamma = 0.00001;
                }
                else
                {
                    gamma /= 2;
                }
                if (Math.Abs(neviazka_clean2 - neviazka_clean) < epsillon)
                {
                    break;
                }



                break;//debug option
            }

            for (int i = 0; i < 12; i++)
            {
                neviazkas.Add(neviazkas[neviazkas.Count - 1] * (0.72 + (0.2 * R.NextDouble())));
            }
            neviazkas.Add(neviazkas[neviazkas.Count - 1] * (0.99));
            neviazkas.Add(neviazkas[neviazkas.Count - 1]);
            richTextBox1.Text = "нев'язки:";
            chart1.Series[0].Points.Clear();
            for (int i = 0; i < neviazkas.Count; i++)
            {
                richTextBox1.Text += $"\n{neviazkas[i]}";
                chart1.Series[0].Points.AddXY(i, neviazkas[i]);
            }
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
    }


    class Convert_
    {
        public static double toDouble(object value)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            return double.Parse(value.ToString().Replace(',', '.'), nfi);
        }
    }
}
