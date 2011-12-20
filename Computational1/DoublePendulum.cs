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
			_phi1 = "phi1",
			_psi1 = "psi1",
			_psi2 = "psi2",
			_l = "l",
			_dpsi1dt = "dpsi1dt",
			_dpsi2dt = "dpsi2dt",
			_I1 = "I1",
			_I2 = "I2",
			_a12 = "a12",
			_a11 = "a11",
			_a22 = "a22";
		public DoublePendulum(double m1, double m2, double r1, double r2, double psi1, double psi2, double phi1, double g, double l, 
			double I1, double I2, double a11, double a12, double a22, double dpsi1dt, double dpsi2dt) {
			//L is the distance from the suspension point of the first and second pendulum
			AddEqParameter(_m1, m1);
			AddEqParameter(_m2, m2);
			AddEqParameter(_g, g);
			AddEqParameter(_psi1, psi1);
			AddEqParameter(_psi2, psi2);
			AddEqParameter(_dpsi1dt, dpsi1dt);
			AddEqParameter(_dpsi2dt, dpsi2dt);
			AddEqParameter(_phi1, phi1);
			AddEqParameter(_r1, r1);
			AddEqParameter(_r2, r2);
			AddEqParameter(_l, l);
			AddEqParameter(_I1, I1);
			AddEqParameter(_I2, I2);
			AddEqParameter(_a11, a11);
			AddEqParameter(_a12, a12);
			AddEqParameter(_a22, a22);
		}

		//RungeKutta
		//y'[n]  = f_i(x, y[n]) i => [0,n]
		//For the double pendulum:
		//n = 4, x = t, y1 = psi1, y2 = psi2, y3 = dpsi1dt, y4 = dpsi2dt
		public double[] initialValues = new double[4] { .2, .2, .2, .2 };
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
		public Animation(double phi1, double phi2) {
			InitializeComponent(phi1, phi2);
		}

		private int dx = 4;
		private System.Windows.Forms.PictureBox picTarget;
		private System.Windows.Forms.PictureBox picBall;
		private System.Windows.Forms.PictureBox anchor;
		private System.Windows.Forms.Timer timer1;
		double l;
		private void InitializeComponent(double phi1, double phi2) {
			double l = 30;
			this.anchor = new System.Windows.Forms.PictureBox();
			this.picTarget = new System.Windows.Forms.PictureBox();
			this.picBall = new System.Windows.Forms.PictureBox();
			this.timer1 = new System.Windows.Forms.Timer(new System.ComponentModel.Container());
			this.SuspendLayout();
			this.l = l;
			this.anchor.BackColor = Color.Black;
			this.picTarget.BackColor = Color.Red;
			double x1 = l * Math.Sin(phi1) + this.Width / 2;
			double y1 = -l * Math.Cos(phi1) + this.Height / 2;
			double x2 = l *( Math.Sin(phi1) + Math.Sin(phi2)) + this.Width / 2;
			double y2 = -l *( Math.Cos(phi1) + Math.Cos(phi2)) + this.Height / 2;
			this.picTarget.Location = new System.Drawing.Point((int)x1, (int)y1);
			this.picTarget.Name = "picTarget";
			this.picTarget.Size = new System.Drawing.Size(5, 5);
			this.picTarget.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picTarget.TabIndex = 0;
			this.picTarget.TabStop = false;
			this.anchor.Location = new Point(this.Width / 2, this.Height / 2);
			this.anchor.Size = new Size(5, 5);
			this.picBall.BackColor = Color.Black;
			this.picBall.Location = new System.Drawing.Point((int)x2, (int)y2);
			this.picBall.Name = "picBall";
			this.picBall.Size = new System.Drawing.Size(5, 5);
			this.picBall.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picBall.TabIndex = 1;
			this.picBall.TabStop = false;

			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(392, 341);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                  this.picBall,
                                                                  this.picTarget, this.anchor});
			this.Name = "Form1";
			this.Text = "Crasher";
			this.ResumeLayout(false);
			xOffset = this.Width / 2;
			yOffset = this.Height / 2;
			//(double m1, double m2, double r1, double r2, double psi1, double psi2, double phi1, double g, double l, 
			//double I1, double I2, double a11, double a12, double a22, double dpsi1dt, double dpsi2dt) {

			A = new DoublePendulum(1, 1, 15, 15, 0, 0, Math.PI / 8, 9.8, l, .5, .5, .1, .1, .1, .5, .51);
		}
		int xOffset, yOffset;

		DoublePendulum A;
		double xi = 0;
		private void timer1_Tick(object sender, System.EventArgs e) {
			RungeKutta B = new RungeKutta(A.updateYVals);
			double h = .5;
			double[] y = A.initialValues;
			int n = 4;
			
			y = B.GetNextVal(xi, y, h, n);
			xi += h;
			int quad = Angle.GetQuadrant(y[0]);

			double phi1 = y[0];

			double x1 = -l * Math.Sin(y[0]) + xOffset;
			double y1 = l * Math.Cos(y[0]) + yOffset;
			double x2 = -l * (Math.Sin(y[0]) + Math.Sin(y[1])) + xOffset;
			double y2 = l * (Math.Cos(y[0]) + Math.Cos(y[1])) + yOffset;

			picTarget.Location = new Point((int)x1, (int)y1);
			picBall.Location = new Point((int)x2, (int)y2);
		}
	}

}
