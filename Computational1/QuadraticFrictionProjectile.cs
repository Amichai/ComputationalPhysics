using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace Computational1 {
	public class QuadraticFrictionProjectile : MultiVariableEq{
		string _g = "g",
			_v0Mag = "v0Mag",
			_beta = "beta";
		public QuadraticFrictionProjectile(double beta, double vx0, double vy0) {
			AddEqParameter(_g, g);
			AddEqParameter(_beta, beta);
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

		public void AnimateTrajectory(double theta) {
			double v0Mag = this[_v0Mag];
			double vx0 = v0Mag * Math.Cos(theta);
			double vy0 = v0Mag * Math.Sin(theta);
			this.CurrentPosAndVelocity = new double[4] { 0, 0, vx0, vy0 };
			new Animation2(this, .1).ShowDialog();
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
	}
	//TODO: Make a rigid body animation class that takes delegates for how to manipulate the rigid bodies
	//Todo: implement a class designed for the dichotomy method
	//Todo: refactor and improve the implementation of the multivariable.rungeKutte()

	public partial class Animation2 : Form {
		public Animation2(QuadraticFrictionProjectile A, double h) {
			this.A = A;
			this.h = h;
			InitializeComponent();
		}

		private System.Windows.Forms.PictureBox redBall;
		private System.Windows.Forms.Timer timer1;
		private void InitializeComponent() {
			this.redBall = new System.Windows.Forms.PictureBox();
			this.timer1 = new System.Windows.Forms.Timer(new System.ComponentModel.Container());
			this.SuspendLayout();
			this.redBall.BackColor = Color.Red;
			this.redBall.Name = "picTarget";
			this.redBall.Size = new System.Drawing.Size(5, 5);
			this.redBall.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.redBall.TabIndex = 0;
			this.redBall.TabStop = false;
			this.timer1.Interval = (int)(1000 * h);
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(392, 341);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																  this.redBall});
			this.Name = "Form1";
			this.Text = "Crasher";
			this.ResumeLayout(false);
			background = new Bitmap(this.Width, this.Height);
			g = Graphics.FromImage(background);
			g.DrawLine(new Pen(Color.Black, .1f), new Point(0, this.Height / 2), new Point(this.Width, this.Height / 2));
			g.DrawRectangle(new Pen(Color.Red, 3f), A.PointToDraw.X, this.Height / 2 - A.PointToDraw.Y, 3, 3);
			y = A.CurrentPosAndVelocity;
		}
		Graphics g;
		QuadraticFrictionProjectile A;
		double xi = 0;
		double h;
		Point last = new Point(0, 0);
		double[] y;
		int n = 4;
		private void timer1_Tick(object sender, System.EventArgs e) {
			A.rk.UpdateYVals(xi, y, h, n);
			y = A.rk.CurrentYs;
			xi += h;

			Debug.Print(xi.ToString() + " " + y[0].ToString() + " " + y[1].ToString() + " " + y[2].ToString() + " " + y[3].ToString());

			double x1 = y[0];
			double y1 = -y[1] + this.Height /2;

			var pen = new Pen(Color.Green, .1f);
			g.DrawRectangle(pen, new Rectangle((int)x1, (int)y1, 2, 2));
			this.BackgroundImage = background;
			redBall.Location = new Point((int)x1, (int)y1);

			if (y[1] < 0)
				this.timer1.Stop();
		}

		Image background;
	}

}
