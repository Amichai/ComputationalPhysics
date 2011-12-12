using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

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
			_y0  = "y0";

		public NoFrictionProjectile(double vx0, double vy0, double theta, double g = 9.8, double x0 = 0, double y0 = 0) {
			//y = x tan (theta)  - x^2 g / 2 vx0^2
			AddDependentVariable(y, () => this[x]() * Math.Tan(this[_theta]()) - (Math.Pow(this[x]() , 2) * this[_g]()) / 
					(2 * Math.Pow(this[_vx0](), 2))
				);
			AddDependentVariable(vy, () => this[_vy0]() - this[_g]() * this[t]());


			AddEqParameter(_vx0, vx0);
			AddEqParameter(_vy0, vy0);
			AddEqParameter(_theta, theta);
			AddEqParameter(_g, g);
			AddEqParameter(_x0, x0);
			AddEqParameter(_y0, y0);
		}
	}
}
