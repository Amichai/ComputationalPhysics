using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace Computational1 {
	public class DoublePendulum : MultiVariableEq{
		string _PE = "PE",
			_m1 = "m1",
			_m2 = "m2",
			_r1 = "r1",
			_r2 = "r2",
			_g = "g",
			_psi1 = "psi1",
			_psi2 = "psi2",
			_l = "l",
			_I1 = "I1",
			_I2 = "I2",
			_a12 = "a12",
			_a11 = "a11",
			_a22 = "a22";
		public DoublePendulum(double m1, double m2, double r1, double r2, double psi1, double psi2, double g, double l, 
			double I1, double I2, double a11, double a12, double a22, 
			double phi1initial, double phi2initial, double dphi1dtinit, double dphi2dtinit) {
			//L is the distance from the suspension point of the first and second pendulum
			AddEqParameter(_m1, m1);
			AddEqParameter(_m2, m2);
			AddEqParameter(_g, g);
			AddEqParameter(_psi1, psi1);
			AddEqParameter(_psi2, psi2);
			AddEqParameter(_r1, r1);
			AddEqParameter(_r2, r2);
			AddEqParameter(_l, l);
			AddEqParameter(_I1, I1);
			AddEqParameter(_I2, I2);
			AddEqParameter(_a11, a11);
			AddEqParameter(_a12, a12);
			AddEqParameter(_a22, a22);
			initialValues = new double[4] { phi1initial, phi2initial, dphi1dtinit, dphi2dtinit };
		}

		//RungeKutta
		//y'[n]  = f_i(x, y[n]) i => [0,n]
		//For the double pendulum:
		//n = 4, x = t, y1 = psi1, y2 = psi2, y3 = dpsi1dt, y4 = dpsi2dt
		public double[] initialValues;
		public void Evolve(double ti, double tf, double dt) {
			double[] yvals = initialValues;
			var series = new RungeKutta().Evaluate(ti, tf, dt, 4, yvals, updateYVals);
			var data = new PlotData(series); 
			data.Graph();
		}

		public double[] updateYVals(double[] yVals, double x, double h, int n){
			//Pass the functions for updating from the pendulum so this can be abstracted
			double[] dyVals = new double[4];
			double I1 = this[_I1];
			double I2 = this[_I2];
			double M1 = this[_m1];
			double M2 = this[_m2];
			double R1 = this[_r1];
			double R2 = this[_r2];
			double L = this[_l];
			double g = this[_g];
			double psi = this[_psi1];
			I1 += M1 * R1 * R1;
			I2 += M2 * R2 * R2;
			double A1 = M1 * R1 * g;
			double A2 = M2 * L * g;
			double A = Math.Sqrt(A1 * A1 + A2 * A2 + 2 * A1 * A2 * Math.Cos(psi));
			psi = Math.Asin(A1 * Math.Sin(psi) / A);
			double B = M2 * R2 * g;
			double C = I1 + L * L * M2;
			double D = I2;
			double E = M2 * R2 * L;
			double D1, E1, B1;
			double a11 = this[_a11];
			double a12 = this[_a12];
			double a22 = this[_a22];

			dyVals[0] = yVals[2];
			dyVals[1] = yVals[3];
			E1 = E * Math.Sin(yVals[0] - yVals[1]);
			A1 = -A * Math.Sin(yVals[0] + psi) - E1 * yVals[3] * yVals[3] - a11 * yVals[2] - a12 * yVals[3];
			B1 = -B * Math.Sin(yVals[1]) + E1 * yVals[2] * yVals[2] - a22 * yVals[3] - a12 * yVals[2];
			E1 = E * Math.Cos(yVals[0] - yVals[1]);
			D1 = 1.0 / (C * D - E1 * E1);
			dyVals[2] = (A1 * D - E1 * B1) * D1;
			dyVals[3] = (B1 * C - E1 * A1) * D1;
			return dyVals;
		}
	}

	public partial class Animation : Form {
		public Animation(DoublePendulum A, double h) {
			this.A = A;
			this.h = h;  
			InitializeComponent();
		}

		private System.Windows.Forms.PictureBox redBall;
		private System.Windows.Forms.PictureBox blackBall;
		private System.Windows.Forms.PictureBox anchor;
		private System.Windows.Forms.Timer timer1;
		double l;
		private void InitializeComponent() {
			this.redBall = new System.Windows.Forms.PictureBox();
			this.blackBall = new System.Windows.Forms.PictureBox();
			this.anchor = new System.Windows.Forms.PictureBox();
			this.timer1 = new System.Windows.Forms.Timer(new System.ComponentModel.Container());
			this.SuspendLayout();
			this.l = A["l"];
			xOffset = this.Width / 2;
			yOffset = this.Height / 2;
			this.anchor.BackColor = Color.Purple;
			this.anchor.Size = new System.Drawing.Size(3, 3);
			this.anchor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			anchor.Location = new Point((int)xOffset, (int)yOffset);
			this.redBall.BackColor = Color.Red;
			this.redBall.Name = "picTarget";
			this.redBall.Size = new System.Drawing.Size(5, 5);
			this.redBall.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.redBall.TabIndex = 0;
			this.redBall.TabStop = false;
			this.blackBall.BackColor = Color.Black;
			this.blackBall.Name = "picBall";
			this.blackBall.Size = new System.Drawing.Size(5, 5);
			this.blackBall.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.blackBall.TabIndex = 1;
			this.blackBall.TabStop = false;

			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(392, 341);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                  this.blackBall,
                                                                  this.redBall, this.anchor});
			this.Name = "Form1";
			this.Text = "Crasher";
			this.ResumeLayout(false);
			background = new Bitmap(this.Width, this.Height);
			g = Graphics.FromImage(background);
			B = new RungeKutta(A.updateYVals);
			y = A.initialValues;
		}
		int xOffset, yOffset;
		Graphics g;
		DoublePendulum A;
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

			double x1 = l * Math.Sin(y[0]) + xOffset;
			double y1 = l * Math.Cos(y[0]) + yOffset;
			double x2 = l * (Math.Sin(y[0]) + Math.Sin(y[1])) + xOffset;
			double y2 = l * (Math.Cos(y[0]) - Math.Cos(y[1])) + yOffset;

			var pen = new Pen(Color.Green, .1f);
			g.DrawRectangle(pen, new Rectangle((int)x1, (int)y1, 2,2));
			pen = new Pen(Color.Red, .1f);
			if (last != new Point(0, 0))
				g.DrawLine(pen, last, new Point((int)x2, (int)y2));
			else
				throw new Exception();
			this.BackgroundImage = background;
			redBall.Location = new Point((int)x1, (int)y1);
			blackBall.Location = new Point((int)x2, (int)y2);
			last = new Point((int)x2, (int)y2);
		}

		Image background;
	}

}
