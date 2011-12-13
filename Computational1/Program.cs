using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Common;
using Common.ComputationHelper.Graphing;
using System.Drawing;

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
			PlotData p = new PlotData(proj, 0, 2, .01);
			proj.SetvMag(10);
			Point target = new Point(2, 1);
			var angle = proj.GetAngleToTarget(target.X, target.Y);
			p.AddPoint(target.X, target.Y, "Target to hit");
			if (angle == double.MinValue)
				throw new Exception("no solution");
			proj.SetvMagTheta(10, angle);
			p.AddParametricTrial("x", "y", "t", "Launch angle: " + angle.ToString());
			p.Graph();

		}
	}
}
