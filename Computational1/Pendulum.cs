using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Drawing;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;


namespace Computational1 {
	public class Pendulum : MultiVariableEq {
		string	m = "mass",
				l = "length",			//suspension length
				inertia = "inertia",	//moment of inertia from the center of mass
				t = "time",
				g = "g",				//gravitational constant
				amp = "amplitude",
				theta = "theta",
				PE = "potentialEnergy",
				KE = "kineticEnergy",
				omega = "omega",
				dTheta = "dTheta",
				h = "height",
				totalEnergy = "totalEnergy",
				lagrangian = "lagrangian";

		public Pendulum(double mass, double length, double grav, double amplitude, double inertia) {
			AddDependentVariable(h, () => this[l]() * Math.Cos(this[theta]()));
			AddDependentVariable(PE, () =>
								-this[m]() * this[g]() *this[h]()
								);
			AddDependentVariable(KE, () =>
								((this[this.inertia]() + this[m]() * this[l]().Sqrd())
								* (this.Partial(theta, t, this[theta]()).Sqrd())) / 2
								);
			AddDependentVariable(omega, () =>
								Math.Sqrt((this[this.inertia]() + this[m]() * this[l]().Sqrd())
								  / (this[m]() * this[g]() * this[l]()))
								);
			AddDependentVariable(theta, () =>
								this[amp]() * Math.Cos(this[t]() * this[omega]())
								+ this[amp]() * Math.Sin(this[t]() * this[omega]())
								);
			AddDependentVariable(dTheta, () =>
								this.Partial(theta, t, this[t]()));
			AddDependentVariable(totalEnergy, () => this[PE]() + this[KE]());
			AddDependentVariable(lagrangian, () => this[KE]() - this[PE]()
								);
			

			this[m] = () => mass;
			this[l] = () => length;
			this[g] = () => grav;
			this[amp] = () => amplitude;
			this[this.inertia] = () => inertia;
		}
	}

	public class Polynomial : MultiVariableEq {
		string x = "x",
			  y = "y",
			  dy ="dy",
			  a = "a",
			  b = "b",
			  c = "c";
		public Polynomial(double A, double B, double C) {
			AddDependentVariable(y, () =>
								this[a]() * Math.Pow(this[x](), 2) + this[b]() * this[x]() + this[c]()
								);
			AddDependentVariable(dy, () =>
								this.Partial(y, x, this[x]())
								);
			this[a] = () => A;
			this[b] = () => B;
			this[c] = () => C;
		}
	}

	public class Cosine : MultiVariableEq {
		string x = "x",
			  y = "y",
			  dy = "dy",
			  a = "A";
		public Cosine(double A){
				 AddDependentVariable(y, () =>
							Math.Cos(this[x]())
							);
					AddDependentVariable(dy, () =>
							this.Partial(y, x, this[x]())
							);
			this[a] = () => A;
		}
	}
}
