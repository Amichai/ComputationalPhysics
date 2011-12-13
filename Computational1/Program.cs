using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Common;
using Common.ComputationHelper.Graphing;

namespace Computational1 {
	class Program {

		static void Main(string[] args) {

			LaminarFrictionProjectileTrial();
		}
		static void RopeTrial() {

			new Rope(2, 2, 5, 5, 10).CalculateShape().Graph();
		}

		static void LaminarFrictionProjectileTrial () {
			LaminarFrictionProjectile proj = new LaminarFrictionProjectile(2);
			proj.SetvMag(10);
			var target = new Tuple<double, double>(2, .1);
			var angle = proj.GetAngleToTarget(target.Item1, target.Item2);
			
			PlotData p = new PlotData(proj, 0, 2, .01);
			p.AddPoint(target.Item1, target.Item2, "Target to hit");
			if (angle == double.MinValue)
				throw new Exception("no solution");
			proj.SetvMagTheta(10, angle);
			p.AddParametricTrial("x", "y", "t", "Launch angle: " + angle.ToString());
			p.Graph();

		}
	}
}
