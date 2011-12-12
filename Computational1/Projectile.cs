using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Computational1 {
	class Projectile :  MultiVariableEq {
		string x0 = "x0",
			y0 = "y0",
			m = "m",
			t = "t",
			g = "g",
			xf = "xf",
			yf = "yf",
			x = "x",
			y = "y",
			vx0 = "vx0",
			vy0 = "vy0",
			vx = "vx",
			vy = "vy",
			vMag = "vMag",
			vxlast = "vxlast",
			vylast = "vylast",
			ax = "ax",
			ay = "ay",
			beta = "beta"; //drag coefficient


		public Projectile() {
			//v = v0 + at
			AddDependentVariable(vMag, () =>
								Math.Sqrt(Math.Pow(this[vx](), 2) + Math.Pow(this[vy](), 2))
								);
			AddDependentVariable(ax, () =>
								-this[beta]() * this[vx]() * this[vMag]()
								);
			//AddDependentVariable(vx, () => //vx = -B int from 0 to t (vx vMag) dt
			//                    -this[beta]() * 
			//                    );
			AddDependentVariable(ay, () =>
								-this[g]() - this[beta]() * this[vy]() * this[vMag]()
								);
			AddDependentVariable(vy, () =>	
								this[vy0]() + this[ay]() * this[t]()
								);
		}
		public void SetParameters(double beta, double vx0, double vy0, double x0 = 0, double y0 = 0) {
			this[this.beta] = () => beta;
			this[this.vx0] = () => vx0;
			this[this.vy0] = () => vy0;
			this[this.x0] = () => x0;
			this[this.y0] = () => y0;
		}

		public void FindAngleToTarget(double x, double y) {
			this[this.x] = () => x;
			this[this.y] = () => y;

			Relate(this.x, vx);
			//Find yMax
			//See if yMax is greater than y_target
		}
	}
}
