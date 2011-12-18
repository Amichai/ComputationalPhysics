using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Computational1 {
	public class Cycloid : MultiVariableEq{
		string _r = "r",
				_y = "y",
				_x = "x",
				_phi = "phi";
		public Cycloid(double r) {
			AddDependentVariable(_x, () => {
				double phi = this[_phi];
				return this[_r]*(phi - Math.Sin(phi));
			});
			AddDependentVariable(_y, () => {
				double phi = this[_phi];
				return this[_r]*(1 - Math.Cos(phi));
			});
			AddEqParameter(_r, r);
		}
	}
}
