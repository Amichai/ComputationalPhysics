using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Diagnostics;

namespace Computational1 {

	//TODO: make each parameter aware of its dependence
	//Do this by implementing a DAG class
	//Let every parameter be simultaneously dependent and independent
	public class LaminarFrictionProjectile: MultiVariableEq{
		string y = "y",
			_x = "x",
			vy = "vy",
			vx = "vx",
			_theta = "theta",
			_t = "t",
			_vx0 = "vx0",
			_vy0 = "vy0",
			_g = "g",
			_x0 = "x0",
			_y0 = "y0",
			_gamma = "gamma",
			_v0Mag = "v0Mag",
			_xMax = "xMax";

		//TODO: take variable sets out of the constructors
		public LaminarFrictionProjectile(double gamma, double g = 9.8, double x0 = 0, double y0 = 0) {
			AddDependentVariable(vx, ()=> //vx = vx0 e^{gamma t}
				this[_vx0]() * Math.Exp( - this[_gamma]()* this[_t]())
				);
			AddDependentVariable(vy, () => //vy = - g/gamma  + (vy0  + g/ gamma) e^{gamma t}
				-this[_g]() / this[_gamma]() + (this[_vy0]() + this[_g]() / this[_gamma]()) * Math.Exp(-this[_gamma]() * this[_t]())
				);
			AddDependentVariable(_x, () =>
					this[_vx0]() * (1 - Math.Exp(-this[_gamma]() * this[_t]())) / this[_gamma]()
					);
			AddDependentVariable(y, () =>
					-this[_g]() * this[_t]() / this[_gamma]() + (this[_vy0]() + this[_g]() / this[_gamma]()) * (1 - Math.Exp(-this[_gamma]() * this[_t]())) / this[_gamma]()
					);		
			
		//Calculate tmax from theta and gamma and use that to calculate xmax

			AddEqParameter(_g, g);
			AddEqParameter(_x0, x0);
			AddEqParameter(_y0, y0);
			AddEqParameter(_gamma, gamma);
		}

		public double GetThetaForMaxDistance() {
			//Initial approximation of an x value
			double xMax = this[_vx0]() / this[_gamma]();
			//This isn't what we need because y(t) depends on theta
			//We're really trying to maximize x(t, theta) given the condition y(t, theta) = 0
			//To solve, find t such that x'(t) = 0
			//x'(t): 
			//new SingleVariableEq(derivOfXOfT).Graph(0, 10, .1);
			double G = this[_gamma]();
			double V = this[_v0Mag]();
			double g = this[_g]();
			new SingleVariableEq(tOfVandGamma).Graph(0, Math.PI, .001);
			//Func<double, double> xMaxOfTheta = th => V * Math.Cos(th) * (1 - Math.Exp(-G * tOfVandGamma(th))) / G;
			//new SingleVariableEq(xMaxOfTheta).Graph(0, Math.PI, .001);
			return double.MinValue;
		}

		private double tOfVandGamma(double theta) {
			double G = this[_gamma]();
			double V = this[_v0Mag]();
			double g = this[_g]();			
			double Vy0 = V* Math.Sin(theta);
			double xMax = this[_vx0]() / this[_gamma]();
			Func<double, double> solveMe = t => G * g * t + G * Vy0 * Math.Exp(-g * G) + g * Math.Exp(-g * G) - G * Vy0 - g;
			return new SingleVariableEq(solveMe).NewtonRaphson(xMax - .01, .001, xMax, 1.0e-10, 400);
		}

		private double secondDerivXofT(double t) {
			double G = this[_gamma]();
			double V = this[_v0Mag]();
			double g = this[_g]();
			return -((G.Sqrd()*Math.Exp(5*t*G)-5*G.Sqrd()*Math.Exp(4*t*G)+10*G.Sqrd()*Math.Exp(3*t*G)
				-10*G.Sqrd()*Math.Exp(2*t*G)+5*G.Sqrd()*Math.Exp(t*G)-G.Sqrd())*Math.Pow(V,8)+(-2*g.Sqrd()*t.Sqrd()*G.Sqrd()*Math.Exp(5*t*G)+6*g.Sqrd()
					*t.Sqrd()*G.Sqrd()*Math.Exp(4*t*G)-6*g.Sqrd()*t.Sqrd()*G.Sqrd()*Math.Exp(3*t*G)+2*g.Sqrd()*t.Sqrd()*G.Sqrd()*Math.Exp(2*t*G))*Math.Pow(V,6)+V.Sqrd()*
						((g.Sqrd()*Math.Exp(6*t*G)+(g.Sqrd()*t.Sqrd()*G.Sqrd()-2*g.Sqrd()*t*G-4*g.Sqrd())*Math.Exp(5*t*G)+(-2*g.Sqrd()*t.Sqrd()*G.Sqrd()+6*
							g.Sqrd()*t*G+6*g.Sqrd())*Math.Exp(4*t*G)+(g.Sqrd()*t.Sqrd()*G.Sqrd()-6*g.Sqrd()*t*G-4*g.Sqrd())*Math.Exp(3*t*G)+(2*g.Sqrd()*t*G+g.Sqrd())
								*Math.Exp(2*t*G))*Math.Pow(V,4)+(-Math.Pow(g,4)*t.Sqrd()*Math.Exp(6*t*G)+(-Math.Pow(g,4)*Math.Pow(t,4)*G.Sqrd()+2*Math.Pow(g,4)*Math.Pow(t,3)
								*G+2*Math.Pow(g,4)*t.Sqrd())
								*Math.Exp(5*t*G)+(-2*Math.Pow(g,4)*Math.Pow(t,3)*G-Math.Pow(g,4)*t.Sqrd())
									*Math.Exp(4*t*G))*V.Sqrd())+(Math.Pow(g,4)*t.Sqrd()*Math.Exp(6*t*G)+(-2*Math.Pow(g,4)*Math.Pow(t,3)*G-2*Math.Pow(g,4)*t.Sqrd())*
									Math.Exp(5*t*G)+(Math.Pow(g,4)*Math.Pow(t,4)*G.Sqrd()+2*Math.Pow(g,4)*Math.Pow(t,3)*G+Math.Pow(g,4)*t.Sqrd())
										*Math.Exp(4*t*G))*Math.Pow(V,4)+(Math.Pow(g,4)*Math.Pow(t,4)*G.Sqrd()*Math.Exp(5*t*G)-Math.Pow(g,4)*Math.Pow(t,4)*G.Sqrd()*Math.Exp(4*t*G))*Math.Pow(V,4))/(Math.Sqrt((Math.Exp(t*G)-1)*V-g*t*Math.Exp(t*G))
											*Math.Sqrt((Math.Exp(t*G)-1)*V+g*t*Math.Exp(t*G))*((G*Math.Exp(5*t*G)-4*G*Math.Exp(4*t*G)+6*G*Math.Exp(3*t*G)-4*G*Math.Exp(2*t*G)+G*Math.Exp(t*G))*Math.Pow(V,5)+
												(-g.Sqrd()*t.Sqrd()*G*Math.Exp(5*t*G)+2*g.Sqrd()*t.Sqrd()*G*Math.Exp(4*t*G)-g.Sqrd()*t.Sqrd()*G*Math.Exp(3*t*G))*Math.Pow(V,3))*Math.Abs(V));
		}

		//http://www.solvemymath.com/online_math_calculator/calculus/derivative_calculator/index.php
		//input: V* cos(asin((g*x) / ((1 - exp(-G*x))*V)))*(1 - exp(-G*x)) / G
		private double derivOfXOfT(double t){
			double G = this[_gamma]();
			double V = this[_v0Mag]();
			double g = this[_g]();

			double A = Math.Exp(-G * t) * ((-G * Math.Exp(2 * t * G) + 2 * G * Math.Exp(G * t) - G) * Math.Pow(V, 4)
				+ V.Sqrd() * g.Sqrd() * t * Math.Exp(3 * t * G) + (-g.Sqrd() * t.Sqrd() * G - g.Sqrd() * t) * Math.Exp(2 * t * G))
				+ g.Sqrd() * t.Sqrd() * G * Math.Exp(2 * t * G) * V.Sqrd();
			//new SingleVariableEq(i => (Math.Exp(2 * i * G) - 2 * Math.Exp(i * G) + 1) * V.Sqrd() - g.Sqrd() * i.Sqrd() * Math.Exp(2 * i * G)).Graph(-10, 10, .1);
			double B = G * Math.Sqrt(Math.Exp(2 * t * G) - 2 * Math.Exp(t * G) + 1) * V * Math.Sqrt((Math.Exp(2 * t * G) - 2 * Math.Exp(t * G) + 1) * V.Sqrd()
				- g.Sqrd() * t.Sqrd() * Math.Exp(2 * t * G)) * Math.Abs(V);

			return A / B;
		}
			
		public void SetVxVy(double vx0, double vy0) {
			AddEqParameter(_vx0, vx0);
			AddEqParameter(_vy0, vy0);
			AddEqParameter(_theta, Math.Atan(vy0 / vx0));
			AddDependentVariable(_v0Mag, () =>
					Math.Sqrt(Math.Pow(this[_vx0](), 2) + Math.Pow(this[_vy0](), 2))
					);
		}


		public void SetvMag(double v0Mag) {
			AddEqParameter(_v0Mag, v0Mag);
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

		private double eqToSolve(double x) {
			  double Bs=Math.Sqrt(1+x*x)*B;
			  if(Bs>=1)
				return double.MinValue;
			  else
				return xi0-A*(Math.Log(1-Bs)+Bs);
		}
		double B, xi0, A;

		public double GetAngleToTarget(double x, double y) {
			return SolveUsingIterativeFunction(x, y);
			//return SolveWithNewtonRaphson(x, y);
		}
		private double SolveUsingIterativeFunction(double x, double y) {
			A = this[_g]() / (x * this[_gamma]().Sqrd());
			B = x * this[_gamma]() / this[_v0Mag]();
			if (B > 1)
				return double.MinValue;
			if (x > this[_v0Mag]() / this[_gamma]())
				return double.MinValue;
			double xi1 = 0;
			double xib = Math.Sqrt(1 / B.Sqrd() - 1);
			xi0 = y / x;
			//Solve using an iterative function method
			var t = new SingleVariableEq(eqToSolve);
			return Math.Atan(t.IterativeSolver(xi1, -xib, xib, 1.0e-10, 10000));
		}

		//A bug popped up in this method, not sure why.
		private double SolveWithNewtonRaphson(double x, double y) {
			var t2 = new SingleVariableEq(i => y / x - A * (Math.Log(1 - B * Math.Sqrt(1 + i.Sqrd())) + B * Math.Sqrt(1 + i.Sqrd())) - i);
			double C = Math.Exp(-(-y / x + 1 + Math.Sqrt(1 / B.Sqrd() - 1) / A));
			double initApprox1 = -Math.Sqrt((1 - C).Sqrd() / B.Sqrd() - 1);
			double initApprox2 = Math.Sqrt((1 - C).Sqrd() / B.Sqrd() - 1);
			var deriv = new SingleVariableEq(i => (A * B.Sqrd() * i) / (1 - B * Math.Sqrt(1 + i.Sqrd())) - 1);
			var theta = Math.Atan(t2.NewtonRaphson(deriv, 0, initApprox1, initApprox2, 1.0e-10, 500));
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
			_y0  = "y0",
			_vMag = "vMag";

		public void setTheta(double theta) {
			AddEqParameter(_theta, theta);
		}

		public void setVmag(double vMag){
			AddEqParameter(_vMag, vMag);
		}

		public NoFrictionProjectile(double g = 9.8, double x0 = 0, double y0 = 0) {
			//y = x tan (theta)  - x^2 g / 2 vx0^2
			AddDependentVariable(y, () => this[x]() * Math.Tan(this[_theta]()) - (Math.Pow(this[x]() , 2) * this[_g]()) / 
					(2 * Math.Pow(this[_vx0](), 2))
				);

			AddDependentVariable(x, () => this[_vMag]() * this[t]() * Math.Cos(this[_theta]()));
			AddDependentVariable(vy, () => this[_vy0]() - this[_g]() * this[t]());

			AddDependentVariable(_vx0, () => this[_vMag]() * Math.Cos(this[_theta]()));
			AddDependentVariable(_vy0, () => this[_vMag]() * Math.Sin(this[_theta]()));
			
			
			AddEqParameter(_g, g);
			AddEqParameter(_x0, x0);
			AddEqParameter(_y0, y0);
		}

		internal double GetAngleToTarget(double x, double y) {
			double g= this[_g]();
			double v0 = this[_vMag]();
			return Math.Atan(
				(v0.Sqrd() + Math.Sqrt(Math.Pow(v0, 4) - g*(g *x.Sqrd() + 2 * y * v0.Sqrd())))
				/ (g * x)
				);
		}

		internal double GetThetaForMaxDistance() {
			return Math.PI / 4;
		}
	}
}
