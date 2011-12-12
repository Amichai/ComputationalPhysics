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
				dTheta = "dTheta";

		public Pendulum() {
			AddDependentVariable(PE, () =>
								-this[m]() * this[g]() * this[l]() * Math.Cos(this[theta]())
								);
			AddDependentVariable(KE, () =>
								((this[inertia]() + this[m]() * Math.Pow(this[l](), 2))
								* Math.Pow(this.Partial(theta, t, this[theta]()), 2)) / 2
								);
			AddDependentVariable(omega, () =>
								Math.Sqrt((this[inertia]() + this[m]() * Math.Pow(this[l](), 2))
								  / (this[m]() * this[g]() * this[l]()))
								);
			AddDependentVariable(theta, () =>
								this[amp]() * Math.Cos(this[t]() * this[omega]())
								+ this[amp]() * Math.Sin(this[t]() * this[omega]())
								);
			AddDependentVariable(dTheta, () =>
								this.Partial(theta, t, this[theta]()));
		}

		public void SetParameters(double mass, double length, double grav, double amplitude, double inertia ){
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
			  dy ="dy";
		public Polynomial() {
			AddDependentVariable(y, () =>
								Math.Pow(this[x](), 3)
								);
			AddDependentVariable(dy, () =>
								this.Partial(y, x, this[x]())
								);
		}
	}
}
