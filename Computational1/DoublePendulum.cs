using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

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
			_lagrange = "lagrange";
		public DoublePendulum(double m1, double m2, double r1, double r2, double theta1, double phi, double g, double l, double I1, double I2) {
			//L is the distance from the suspension point of the first and second pendulum
			AddEqParameter(_m1, m1);
			AddEqParameter(_g, g);
			AddEqParameter(_r1, r1);
			AddEqParameter(_r2, r2);
			AddEqParameter(_l, l);
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
		}
	}
}
