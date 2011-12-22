using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace Computational1 {
	public class QuadraticFrictionProjectile : MultiVariableEq{
		string _g = "g",
			_v0Mag = "v0Mag",
			_beta = "beta",
			_theta = "theta";
		public QuadraticFrictionProjectile(double beta, double vx0, double vy0) {
			AddEqParameter(_g, g);
			AddEqParameter(_beta, beta);
			AddDependentVariable(_theta, () => Math.Atan(vy0/vx0));
			rk = new RungeKutta(updateCoordinates);
			this.CurrentPosAndVelocity = new double[4] { 0, 0, vx0, vy0 };
		}
		double g = 9.8;
		public QuadraticFrictionProjectile(double beta, double v0Mag) {
			AddEqParameter(_g, g);
			AddEqParameter(_beta, beta);
			AddEqParameter(_v0Mag, v0Mag);
			rk = new RungeKutta(updateCoordinates);
		}

		public double[] updateCoordinates(double[] yVals, double x, double h, int n) {
			double[] dy = new double[n];
			double beta = this[_beta];
			dy[0] = yVals[2];
			dy[1] = yVals[3];
			dy[2] = -beta * yVals[2] * Math.Sqrt(yVals[2].Sqrd() + yVals[3].Sqrd());
			dy[3] = -this[_g] - beta * yVals[3] * Math.Sqrt(yVals[2].Sqrd() + yVals[3].Sqrd());
			return dy;
		}
		
		public RungeKutta rk;
		int n = 4;
		double h = .1;
		double t = 0;
		public double[] CurrentPosAndVelocity;
		private double getYMax(double theta) {
			double v0Mag = this[_v0Mag];
			double vx0 = v0Mag * Math.Cos(theta);
			double vy0 = v0Mag * Math.Sin(theta);
			throw new NotImplementedException();
		}

		private double getXMax(double theta) {
			double v0Mag = this[_v0Mag];
			double vx0 =  v0Mag * Math.Cos(theta);
			double vy0 = v0Mag * Math.Sin(theta);
			CurrentPosAndVelocity = new double[4] { 0, 0, vx0, vy0 };
			t = 0;
			while (CurrentPosAndVelocity[1] >= 0) {
				rk.UpdateYVals(t, CurrentPosAndVelocity, h, n);
				CurrentPosAndVelocity = rk.CurrentYs;
				t += h;
			}
			return CurrentPosAndVelocity[0];
		}

		private double dthetadt(double theta) {
			double dtheta = .01;
			double xMax1 = getXMax(theta);
			double xMax2 = getXMax(theta + dtheta);
			return xMax2 - xMax1;
		}

		public double GetThetaForMaxDistance() {
			int counter = 0;
			return FindZero.DichotomyMethod(i => dthetadt(i), 0, Math.PI / 2 - .05, out counter, .01);
		}

		private double distance(double x1, double y1, double x2, double y2){
			double d = Math.Sqrt((y2 - y1).Sqrd() + (x2 - x1).Sqrd());
			return d;
		}

		public Point PointToDraw = new Point(0, 0);

		private double ShortestDistanceToTarget(double x, double y, double theta) {
			double lastD = int.MaxValue, newD;
			double v0Mag = this[_v0Mag];
			double vx0 = v0Mag * Math.Cos(theta);
			double vy0 = v0Mag * Math.Sin(theta);
			CurrentPosAndVelocity = new double[4] { 0, 0, vx0, vy0 };
			t = 0;
			newD = distance(CurrentPosAndVelocity[0], CurrentPosAndVelocity[1], x, y);
			while (newD < lastD) {
				rk.UpdateYVals(t, CurrentPosAndVelocity, h, n);
				CurrentPosAndVelocity = rk.CurrentYs;
				t += h;
				lastD = newD;
				newD = distance(CurrentPosAndVelocity[0], CurrentPosAndVelocity[1], x, y);
			}
			if (CurrentPosAndVelocity[1] < y)
				return -lastD;
			else return lastD;
		}	

		public double GetAngleToTarget(double xVal, double yVal) {
			int counter = 0;
			//new SingleVariableEq(i => ShortestDistanceToTarget(xVal, yVal, i)).Graph(0, Math.PI / 2, .01);
			PointToDraw = new Point((int)xVal, (int)yVal);
			return FindZero.DichotomyMethod(i => ShortestDistanceToTarget(xVal, yVal, i), 0, Math.PI / 2 - .05, out counter, .01);
		}

		public Series GetDataSeries(double ti, double tf, double dt, string title = null) {

			Series series;
			if(title == null)
				series = new Series("x versus t,\ntheta = " + this["theta"].ToString());
			else 
				series = new Series(title);
			double[] y = CurrentPosAndVelocity;
			for (double t = ti; t < tf; t += dt) {
				rk.UpdateYVals(t, y, dt, n);
				y = rk.CurrentYs;

				series.Points.Add(new DataPoint(y[0], y[1]));
				Debug.Print(t.ToString() + " " + y[0].ToString() + " " + y[1].ToString() + " " + y[2].ToString() + " " + y[3].ToString());
			}
			series.ChartType = SeriesChartType.Line;
			series.XAxisType = AxisType.Primary;
			series.YAxisType = AxisType.Primary;

			series.ChartArea = "ChartArea1";
			series.Legend = "Legend1";
			return series;
		}

		public Series GetDataSeries(double ti, double tf, double dt, double theta) {
			double v0Mag = this[_v0Mag];
			double vx0 = v0Mag * Math.Cos(theta);
			double vy0 = v0Mag * Math.Sin(theta);
			this.CurrentPosAndVelocity = new double[4] { 0, 0, vx0, vy0 };
			return GetDataSeries(ti, tf, dt, "x versus t,\ntheta = " + theta.ToString());
		}
	}
}
