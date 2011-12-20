using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Diagnostics;

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
		public DoublePendulum(double m1, double m2, double r1, double r2, double psi1, double psi2, double phi1, double g, double l, double I1, double I2, double a11, double a12, double a22, double dpsi1dt, double dpsi2dt) {
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

		public void Evolve() {
			double[] yvals = new double[4]{.2,.2,.2,.2};
			var series = new RungeKutta().Evaluate(0, 10, .01, 4, yvals, updateYVals);
			var data = new PlotData(series); 
			data.Graph();
		}

		private double[] updateYVals(double[] yVals, double x, double h, int n){
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
}
