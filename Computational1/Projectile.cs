using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Computational1 {
	class Projectile :  MultiVariableEq {
		string _x0 = "x0",
			_y0 = "y0",
			m = "m",
			t = "t",
			g = "g",
			xf = "xf",
			yf = "yf",
			x = "x",
			y = "y",
			_vx0 = "vx0",
			_vy0 = "vy0",
			vx = "vx",
			vy = "vy",
			vMag = "vMag",
			vxlast = "vxlast",
			vylast = "vylast",
			ax = "ax",
			ay = "ay",
			_beta = "beta",
			dvxdt = "dvxdt",
			dvydt = "dvydt"; //drag coefficient


		public Projectile(double beta, double vx0, double vy0, double x0 = 0, double y0 = 0) {
			//v = v0 + at
			AddDependentVariable(vMag, () =>
								Math.Sqrt(Math.Pow(this[vx] , 2) + Math.Pow(this[vy] , 2))
								);
			AddDependentVariable(ax, () =>
								-this[_beta]  * this[vx]  * this[vMag] 
								);
			AddDependentVariable(dvxdt, () => //vx = -B int from 0 to t (vx vMag) dt
								-this[_beta]  * this[vx]  * this[vMag] 
			                    );
			AddDependentVariable(dvydt, () => //vx = -B int from 0 to t (vx vMag) dt
								- this[g]  - this[_beta]  * this[vy]  * this[vMag] 
								);
			AddDependentVariable(ay, () =>
								-this[g]  - this[_beta]  * this[vy]  * this[vMag] 
								);
			AddDependentVariable(t, () => 
			                    Relate(vx, t).EvaluateIntegral(this[_vx0] , this[vx] )
			                    );
			
			AddDependentVariable(vy, () =>	
								this[_vy0]  + this[ay]  * this[t] 
								);
			AddEqParameter(_beta, beta);
			AddEqParameter(_vx0, vx0);
			AddEqParameter(_vy0, vy0);
			AddEqParameter(_x0, x0);
			AddEqParameter(_y0, y0);
		}

		public void FindAngleToTarget(double x, double y) {
			this[this.x] = x;
			this[this.y] = y;

			Relate(this.x, vx);
			//Find yMax
			//See if yMax is greater than y_target
		}
	}
}
