using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Diagnostics;

namespace Computational1 {
	public class NoFrictionProjectile : MultiVariableEq {
		string y = "y",
			x = "x",
			vy = "vy",
			vx = "vx",
			_theta = "theta",
			t = "t",
			_vx0 = "vx0",
			_vy0 = "vy0",
			_g = "g",
			_x0 = "x0",
			_y0  = "y0",
			_vMag = "vMag";

		public void setTheta(double theta) {
			AddEqParameter(_theta, theta);
		}

		public double GetTimeOfFlight() {
			return 2 * this[_vy0] / this[_g];
		}

		public void setVmag(double vMag){
			AddEqParameter(_vMag, vMag);
		}

		public double GetRange() {
			return (this[_vMag].Sqrd() / this[_g]) * Math.Sin(2 * this[_theta]);
		}

		public NoFrictionProjectile(double g = 9.8, double x0 = 0, double y0 = 0) {
			//y = x tan (theta)  - x^2 g / 2 vx0^2
			AddDependentVariable(y, () => this[x]  * Math.Tan(this[_theta] ) - (Math.Pow(this[x]  , 2) * this[_g] ) / 
					(2 * Math.Pow(this[_vx0] , 2))
				);

			AddDependentVariable(x, () => this[_vMag]  * this[t]  * Math.Cos(this[_theta] ));
			AddDependentVariable(vy, () => this[_vy0]  - this[_g]  * this[t] );

			AddDependentVariable(_vx0, () => this[_vMag]  * Math.Cos(this[_theta] ));
			AddDependentVariable(_vy0, () => this[_vMag]  * Math.Sin(this[_theta] ));
			
			
			AddEqParameter(_g, g);
			AddEqParameter(_x0, x0);
			AddEqParameter(_y0, y0);
		}

		public Tuple<double, double> GetAnglesToTarget(double x, double y) {
			double g= this[_g] ;
			double v0 = this[_vMag] ;
			double ang1 = Math.Atan(
				(v0.Sqrd() + Math.Sqrt(Math.Pow(v0, 4) - g*(g *x.Sqrd() + 2 * y * v0.Sqrd())))
				/ (g * x)
				);
			double ang2 = Math.Atan(
				(v0.Sqrd() - Math.Sqrt(Math.Pow(v0, 4) - g * (g * x.Sqrd() + 2 * y * v0.Sqrd())))
				/ (g * x)
				);
			return new Tuple<double, double>(ang1, ang2);
		}

		internal double GetThetaForMaxDistance() {
			return Math.PI / 4;
		}
	}
}
