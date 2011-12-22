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
			//_vx0 = "vx0",
			//_vy0 = "vy0",
			//_theta = "theta",
			_beta = "beta";
		public QuadraticFrictionProjectile(double beta, double vx0, double vy0, double g = 9.8) {
			AddEqParameter(_g, g);
			//AddEqParameter(_vx0, vx0);
			//AddEqParameter(_vy0, vy0);
			AddEqParameter(_beta, beta);

			this.initialValues = new double[4] { 0, 0, vx0, vy0	 };
		}
		public double[] initialValues; 
		public double[] updateCoordinates(double[] yVals, double x, double h, int n) {
			double[] dy = new double[n];
			double beta = this[_beta];
			dy[0] = yVals[2];
			dy[1] = yVals[3];
			dy[2] = -beta * yVals[2] * Math.Sqrt(yVals[2].Sqrd() + yVals[3].Sqrd());
			dy[3] = -this[_g] - beta * yVals[3] * Math.Sqrt(yVals[2].Sqrd() + yVals[3].Sqrd());
			return dy;
		}
		
		RungeKutta B;
		int n = 4;
		double h = .1;
		double t = 0;
		double[] y;
		private double getXMax(double theta, double v0Mag) {
			double vx0 =  v0Mag * Math.Cos(theta);
			double vy0 = v0Mag * Math.Sin(theta);
			y = new double[4] { 0, 0, vx0, vy0 };
			//SetParameter(_vx0, vx0);
			//SetParameter(_vy0, vy0);

			while (y[1] >= 0) {
				y = B.GetNextVal(t, y, h, n);
				t += h;
			}
			return y[0];
		}

		private double dthetadt(double theta, double v0Mag) {
			double dtheta = .01;
			double xMax1 = getXMax(theta, v0Mag);
			double xMax2 = getXMax(theta + dtheta, v0Mag);
			return xMax2 - xMax1;
		}

		public double GetAngleToTarget(double xVal, double yVal, double v0Mag) {
			B = new RungeKutta(updateCoordinates);
			y = initialValues;
			int counter;
			double thetaForMaxDistance = FindZero.DichotomyMethod(i => dthetadt(i, v0Mag), 0, Math.PI /2 - .05, out counter, .01);
			SetParameter("theta", thetaForMaxDistance);
			double vx0 = v0Mag * Math.Cos(thetaForMaxDistance);
			double vy0 = v0Mag * Math.Sin(thetaForMaxDistance);
			this.initialValues = new double[4] { 0, 0, vx0, vy0 };
			new Animation2(this, .1).ShowDialog();
			thetaForMaxDistance = Math.PI / 4;
			vx0 = v0Mag * Math.Cos(thetaForMaxDistance);
			vy0 = v0Mag * Math.Sin(thetaForMaxDistance);
			this.initialValues = new double[4] { 0, 0, vx0, vy0 };
			new Animation2(this, .1).ShowDialog();

			throw new NotImplementedException();

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
			B = new RungeKutta(A.updateCoordinates);
			y = A.initialValues;
		}
		Graphics g;
		QuadraticFrictionProjectile A;
		double xi = 0;
		double h;
		Point last = new Point(0, 0);
		RungeKutta B;
		double[] y;
		int n = 4;
		private void timer1_Tick(object sender, System.EventArgs e) {
			y = B.GetNextVal(xi, y, h, n);
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
