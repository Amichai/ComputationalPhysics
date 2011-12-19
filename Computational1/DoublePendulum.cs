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
			_t = "t",
			_xcom2 = "xcom2", //x coordinate for the center of mass of the second pendulum
			_ycom2 = "ycom2",
			_xcom1 = "xcom1",
			_ycom1 = "ycom1",
			_vxcom2 = "vxcom2",//x component of the velocity of the center of mass of the second pendulum 
			_vycom2 = "vycom2",
			_dpsi1dt = "dpsi1dt",
			_dpsi2dt = "dpsi2dt",
			_A1 = "A1",
			_A2 = "A2",
			_A = "A",
			_B = "B",
			_C = "C",
			_D = "D",
			_E = "E",
			_F = "F",
			_I1 = "I1",
			_I2 = "I2",
			_lagrange = "lagrange",
			_E2 = "E2",
			_A3 = "A3",
			_B2 = "B2",
			_d2psi1dt2 = "d2psi1dt2",
			_d2psi2dt2 = "d2psi2dt2",
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
			AddDependentVariable(_PE, () => -this[_m1]  * this[_g]  * this[_r1]  * Math.Cos(this[_psi1]  + this[_phi1] )
				- this[_m2]  * this[_g] *(this[_l] * Math.Cos(this[_psi1]  + this[_r2] * Math.Cos(this[_psi2] )))
				);
			AddDependentVariable(_xcom2, () => this[_l]  * Math.Sin(this[_psi1] ) + this[_r2]  * Math.Sin(this[_psi2] ));
			AddDependentVariable(_ycom2, () => -this[_l]  * Math.Cos(this[_psi1] ) - this[_r2]  * Math.Cos(this[_psi2] ));
			AddDependentVariable(_vxcom2, () => this[_dpsi1dt]  * this[_l]  * Math.Cos(this[_psi1] ) + this[_dpsi2dt]  * this[_r2]  * Math.Cos(this[_psi2] ));
			AddDependentVariable(_vycom2, () => this[_dpsi1dt]  * this[_l]  * Math.Sin(this[_psi1] ) + this[_dpsi2dt]  * this[_r2]  * Math.Sin(this[_psi2] ));
			AddDependentVariable(_A1, () => this[_m1] * this[_r1] * this[_g] );
			AddDependentVariable(_A2, () => this[_m2]  * this[_l]  * this[_g] );
			AddDependentVariable(_A, () => Math.Sqrt(
				this[_A1] .Sqrd() + this[_A2] .Sqrd() + 2* this[_A1] * this[_A2]  * Math.Cos(this[_phi1] )
				));
			AddDependentVariable(_B, () => this[_m2]  * this[_r2]  * this[_g] );
			AddDependentVariable(_C, () => this[_I1]  + this[_r1] .Sqrd() * this[_m1]  + this[_l] .Sqrd() * this[_m2] );
			AddDependentVariable(_D, () => this[_I2]  + this[_r2] .Sqrd() * this[_m2] );
			AddDependentVariable(_E, () => this[_r2]  * this[_l]  * this[_m2] .Sqrd());
			AddDependentVariable(_F, () => Math.Asin(this[_A1] * Math.Sin(this[_phi1] ) / this[_A] ));
			AddDependentVariable(_lagrange, () => this[_dpsi1dt] .Sqrd()*(this[_C]  / 2 + this[_dpsi2dt]  * this[_D]  / 2
				+ this[_dpsi1dt]  * this[_dpsi2dt]  * this[_E] * Math.Cos(this[_psi1]   - this[_psi2] ) 
				+ this[_A]  * Math.Cos(this[_psi1]  + this[_F] ) + this[_B]  * Math.Cos(this[_psi2] )));
			AddDependentVariable(_E2, () => this[_E] * Math.Cos(this[_psi1] - this[_psi2]));
			AddDependentVariable(_A3, () => -this[_E] * this[_dpsi2dt].Sqrd() * Math.Sin(this[_psi1] - this[_psi2]) - this[_A]* Math.Sin(this[_psi1] - this[_F]));
			AddDependentVariable(_B2, () => this[_E] * this[_dpsi1dt].Sqrd() * Math.Sin(this[_psi1] - this[_psi2]) - this[_B]* Math.Sin(this[_psi2]));

			//f_3
			AddDependentVariable(_d2psi1dt2, () => (this[_A3] * this[_D] - this[_B2] * this[_E2]) / (this[_C] * this[_D] - this[_E2].Sqrd()));
			//f_4
			AddDependentVariable(_d2psi2dt2, () => (this[_B2] * this[_C] - this[_A3] * this[_E2]) / (this[_C] * this[_D] - this[_E2].Sqrd())); 
		}

		//RungeKutta
		//y'[n]  = f_i(x, y[n]) i => [0,n]
		//For the double pendulum:
		//n = 4, x = t, y1 = psi1, y2 = psi2, y3 = dpsi1dt, y4 = dpsi2dt
		public void Run() {
			double psi1 = 1, psi2 = 1, dpsi1dt = 1, dpsi2dt = 1;
			double x_initial = 0, x_final = 1, dx = .01, dpsi1dt_initial = .5;
			double dpsidt = dpsi1dt_initial;
			double h = dx;
			double k1, k2, k3, k4, tOffset, offset2;
			double x = x_initial;
			int n = 4; //Get this from the pendulum
			double[] yVals = new double[n];
			int nstep = 10; // how often to print 
			for (int j = 0; x < x_final; j++ ){
				x += h;

				psi1 = this[_psi1];
				psi2 = this[_psi2];
				dpsi1dt = this[_dpsi1dt];
				dpsi2dt = this[_dpsi2dt];

				yVals[0] = psi1;
				yVals[1] = psi2;
				yVals[2] = dpsi1dt;
				yVals[3] = dpsi2dt;
				if(j%nstep == 0){
					Debug.Print(_psi1 + ": " + psi1.ToString());
					Debug.Print(_psi2 + ": " + psi2.ToString());
					Debug.Print(_dpsi1dt + ": "  + dpsi1dt.ToString());
					Debug.Print(_dpsi2dt + ": " + dpsi2dt.ToString());
					//Debug.Print(this[_energy].ToString());
					//Debug.Print(this[_KE].ToString());
					//Debug.Print(this[_PE].ToString());
				}

					
				k1 = Evaluate(_dpsi1dt, dpsi1dt, _t, x, _d2psi1dt2);
				tOffset = h / 2;
				offset2 = h * k1 / 2;
				k2 = Evaluate(_dpsi1dt, dpsi1dt + offset2, _t, x + tOffset, _d2psi1dt2);
				tOffset = h / 2;
				offset2 = h * k2 / 2;
				k3 = Evaluate(_dpsi1dt, dpsi1dt + offset2, _t, x + tOffset, _d2psi1dt2);
				tOffset = h;
				offset2 = h * k3;
				k4 = Evaluate(_dpsi1dt, dpsi1dt + offset2, _t, x + tOffset, _d2psi1dt2);
				dx = h * (k1 + 2 * k2 + 2 * k3 + 4) / 6;
				dpsi1dt += dx;
				Debug.Print("x: " + x.ToString() + " dsi1dt: " + dpsi1dt.ToString());

				var dyVals = new double[4];
				var temp =  updateYVals(yVals, dyVals, x, h, 4);
				yVals = temp.Item1;
				dyVals = temp.Item2;
			}
		}

		private void Test() {
			//new RungeKutta(4, )
		}

		private Tuple<double[], double[]> updateYVals(double[] yVals, double[] dyVals, double x, double h, int n){
			//Pass the functions for updating from the pendulum so this can be abstracted
			
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

			return new Tuple<double[], double[]>(yVals, dyVals);

		}
	}
}
