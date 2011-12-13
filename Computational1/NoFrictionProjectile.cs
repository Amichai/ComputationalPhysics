using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Computational1 {
	public class LaminarFrictionProjectile: MultiVariableEq{
		string y = "y",
			_x = "x",
			vy = "vy",
			vx = "vx",
			_theta = "theta",
			t = "t",
			_vx0 = "vx0",
			_vy0 = "vy0",
			_g = "g",
			_x0 = "x0",
			_y0 = "y0",
			_gamma = "gamma",
			_v0Mag = "v0Mag";

		public LaminarFrictionProjectile(double gamma, double g = 9.8, double x0 = 0, double y0 = 0) {
			AddDependentVariable(vx, ()=> //vx = vx0 e^{gamma t}
				this[_vx0]() * Math.Exp( - this[_gamma]()* this[t]())
				);
			AddDependentVariable(vy, () => //vy = - g/gamma  + (vy0  + g/ gamma) e^{gamma t}
				-this[_g]() / this[_gamma]() + (this[_vy0]() + this[_g]() / this[_gamma]()) * Math.Exp(-this[_gamma]() * this[t]())
				);
			AddDependentVariable(_x, () =>
					this[_vx0]() * (1 - Math.Exp(-this[_gamma]() * this[t]())) / this[_gamma]()
					);
			AddDependentVariable(y, () =>
					-this[_g]() * this[t]() / this[_gamma]() + (this[_vy0]() + this[_g]() / this[_gamma]()) * (1 - Math.Exp(-this[_gamma]() * this[t]())) / this[_gamma]()
					);
			
			AddEqParameter(_g, g);
			AddEqParameter(_x0, x0);
			AddEqParameter(_y0, y0);
			AddEqParameter(_gamma, gamma);
		}

		public void SetVxVy(double vx0, double vy0) {
			AddEqParameter(_vx0, vx0);
			AddEqParameter(_vy0, vy0);
			AddEqParameter(_theta, Math.Atan(vy0 / vx0));
			AddDependentVariable(_v0Mag, () =>
					Math.Sqrt(Math.Pow(this[_vx0](), 2) + Math.Pow(this[_vy0](), 2))
					);
		}
		public void SetvMagTheta(double v0Mag, double theta) {
			AddEqParameter(_v0Mag, v0Mag);
			AddEqParameter(_theta, theta);
			AddDependentVariable(_vx0, () =>
				this[_v0Mag]() * Math.Cos(theta)
				);
			AddDependentVariable(_vy0, () =>
				this[_v0Mag]() * Math.Sin(theta)
				);
		}

		public  double GetAngleToTarget(double x, double y) {
			double xi = Math.Tan(this[_theta]());
			double A = this[_g]() /( x * this[_gamma]().Sqrd());
			double B = x * this[_gamma]() / this[_v0Mag]();
			if (B > 1)
				return double.MinValue;
			if (x > this[_v0Mag]() / this[_gamma]())
				return double.MinValue;
			var t = new SingleVariableEq(i => y / x - A * Math.Log(1 - B* Math.Sqrt(1 + i.Sqrd())) + B * Math.Sqrt( 1 + i.Sqrd()) - i);
			t.Graph(-15, 15, .01);
			double C = Math.Exp( - (- y / x + 1 + Math.Sqrt(1 /B.Sqrd() - 1) / A));

			double initApprox1 = Math.Sqrt((1 - C).Sqrd() / B.Sqrd() - 1);
			double initApprox2 = -Math.Sqrt((1 - C).Sqrd() / B.Sqrd() - 1);
			var deriv = new SingleVariableEq(i => (A* B.Sqrd()* i) / (1 - B * Math.Sqrt(1 + i.Sqrd())) -1);
			var theta = Math.Atan(t.NewtonRalfson(initApprox2, 400, deriv));
			return theta;
		}
	}

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
