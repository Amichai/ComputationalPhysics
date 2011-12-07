using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Computational1 {
	class Pendulum : MultiVariableEq {
		string	m = "mass",
				l = "length",			//suspension length
				inertia = "inertia",	//moment of inertia from the center of mass
				t = "time",
				g = "g",				//gravitational constant
				amp = "amplitude",
				theta = "theta",
				PE = "potentialEnergy",
				KE = "kineticEnergy",
				omega = "omega";

		public Pendulum() {
			IndependentParameters(m, l, inertia, t, g, amp);
			
			AddDependentVariable(PE, () =>
								-this[m]() * this[g]() * this[l]() * Math.Cos(this[theta]())
								);
			AddDependentVariable(KE, () =>
								((this[inertia]() + this[m]() * Math.Pow(this[l](), 2))
								* Math.Pow(this.Partial(theta, t), 2)) / 2
								);
			AddDependentVariable(omega, () =>
								Math.Sqrt((this[inertia]() + this[m]() * Math.Pow(this[l](), 2))
								  / (this[m]() * this[g]() * this[l]()))
								);
			AddDependentVariable(theta, () =>
								this[amp]() * Math.Cos(this[t]() * this[omega]())
								+ this[amp]() * Math.Sin(this[t]() * this[omega]())
								);
		}

		public void SetParameters(double mass, double length, double grav, double amplitude, double inertia ){
			this[m] = () => mass;
			this[l] = () => length;
			this[g] = () => grav;
			this[amp] = () => amplitude;
			this[this.inertia] = () => inertia;
		}

		public double Theta(double time) {
			this[t] = () => time;
			return this[this.theta]();
		}

		public double Evaluate(string varToSet, double val, string varToObserve) {
			this[varToSet] = () => val;
			return this[varToObserve]();
		}
		public double Evaluate(string var1, double val1, string var2, double val2, string varToObserve) {
			this[var1] = () => val1;
			this[var2] = () => val2;
			return this[varToObserve]();
		}
		public double Integrate() {
			throw new NotImplementedException();
		}
	}
}
