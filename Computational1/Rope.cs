using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace Computational1 {
	class Rope : MultiVariableEq{
		double  x1, x2, y2;
		string _x1 = "x1",
			_x2 = "x2",
			_y1 = "y1",
			_y2 = "y2",
			_ell = "ell",
			_dx = "dx",
			_dy = "dy",
			_dl = "dl",
			_T = "T",
			_g = "g";

		public Rope(double x1, double y1, double x2, double y2, double ell, double g = 9.8) {
			//You may need to check that x2 is greater than x1
			AddDependentVariable(_dx, () => this[_x2]  - this[_x1] );
			AddDependentVariable(_dy, () => this[_y2]  - this[_y1] );
			AddDependentVariable(_dl, () => Math.Sqrt(this[_ell] .Sqrd() - this[_dy] .Sqrd()));

			AddEqParameter(_x1, x1);
			AddEqParameter(_x2, x2);
			AddEqParameter(_y1, y1);
			AddEqParameter(_y2, y2);
			AddEqParameter(_ell, ell);
			AddEqParameter(_g, g);
		}

		public double GetTension() {
			double dl = this[_dl];
			double dx = this[_dx];
			double dy = this[_dy];
			double ell = this[_ell];
			x1 = this[_x1];
			x2 = this[_x2];
			y2 = this[_y2];
			double y1 = this[_y1];

			if (dl <= dx)
				return double.MinValue;
			//f(T) = 
			var A = new SingleVariableEq(t => 2 * t * Math.Sinh((x2- x1) / (2*t)) - Math.Sqrt(ell.Sqrd() - (y2 - y1).Sqrd()));
			//f'(T) =
			var B = new SingleVariableEq(t => (2 * Math.Sinh((x2 - x1) / 2 * t) * t - (x2 - x1) * Math.Cosh((x2 - x1) / 2 * t)) / t);			

			var lowerBoundOnT = Math.Sqrt((x2 - x1).Sqrd() / ((Math.Sqrt(ell.Sqrd() - (y2 - y1).Sqrd()) - (x2 - x1))* 24));
			var tension = A.NewtonRaphson(lowerBoundOnT + .1, lowerBoundOnT, 10, 1.0e-12, 100);
			//Todo: test the value of the tension and don't return a bad value
			return tension;
		}
		/// <summary>
		/// x value for point of zero slope
		/// </summary>
		public double GetX0() {
			x1 = this[_x1];
			x2 = this[_x2];
			y2 = this[_y2];
			double y1 = this[_y1];
			double t = GetTension();
			
			Func<double, double> toSolve = x => Math.Cosh((x1 - x) / t) - Math.Cosh((x2 - x) / t) + y2 - y1;
			int counter;
			
			return FindZero.DichotomyMethod(toSolve, x1, x2, out counter);
		}

		public double GetY0() {
			x1 = this[_x1];
			x2 = this[_x2];
			y2 = this[_y2];
			double y1 = this[_y1];
			double t = GetTension();
			double x0 = GetX0();
			return y2 - t * Math.Cosh((x2 - x0) / t);
		}

		/// <summary>
		/// Tension at the highest end of the rope
		/// </summary>
		public double GetMaxRopeTension(double rho) {
			x1 = this[_x1];
			x2 = this[_x2];
			y2 = this[_y2];
			double y1 = this[_y1];
			double t = GetTension();
			double x0 = GetX0();
			return rho * this[_g] * t * Math.Cosh((x2 - x0) / t);
		}

		public Series CalculateShape() {
			double dl = this[_dl] ;
			double dx = this[_dx] ;
			double dy = this[_dy] ;
			double ell = this[_ell] ;
			x1 = this[_x1] ;
			x2 = this[_x2] ;
			y2 = this[_y2] ;

			if (dl <= dx)
				return null;

			double C = Math.Sqrt(24 * (dl - dx) / (dx* dx * dx));

			var A = new SingleVariableEq(i => 2 * Math.Sinh(dx * i * .5) - i *dl);

			var B = new SingleVariableEq(i => dx * Math.Cosh(dx * i * .5) - dl);
			var D = A.NewtonRaphson(B, C, 0, C * 1.1, 1.0e-12, 100);
			
			double x, y = 0;
			double x0 = ((x1 + x2) - Math.Log((ell + dy) / (ell - dy)) / C) * 0.5;
			double y0 = y2 - Math.Cosh((x2 - x0) * C) / C;
			x = x1;
			Series ser = new Series("rope");
			dx = .01;
			while (x < x2) {
				y = Math.Cosh((x - x0) * C) / C + y0;
				ser.Points.Add(new DataPoint(x, y));
				x += dx;
			}
			x = x2;
			ser.Points.Add(new DataPoint(x, y));
			ser.ChartType = SeriesChartType.Point;
			return ser;
		}
	}
}
