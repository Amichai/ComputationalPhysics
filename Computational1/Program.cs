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
			//CycloidTiral();
			//PendulumTrial();
			//ChaosGameTrial();
			//DoublePendulumTrial();
			QuadraticFrictionProjectileTrial();
			

			//A.GetAngleToTarget(20, 20, 200);
			
		}
		static void DoublePendulumTrial() {
			var A = new DoublePendulum(1, 1, 1, 1, Math.PI / 2, Math.PI / 8, -9.8, 30, .5, .5, 0, 0, 0, Math.PI / 4, Math.PI / 4, 0, 0);
			new Animation(A, .1).ShowDialog();
		}

		static void QuadraticFrictionProjectileTrial() {
			var A = new QuadraticFrictionProjectile(.01, 173, 173);
			new Animation2(A, .1).ShowDialog();

			A = new QuadraticFrictionProjectile(.01, 200);
			var theta = A.GetThetaForMaxDistance();
			A.AnimateTrajectory(theta);
		}

		static void CycloidTiral() {
			var brach = new Cycloid(4);
			PlotData p = new PlotData(brach, 0, 4 * Math.PI, .1);
			p.AddParametricTrial("x", "y", "phi");
			p.Graph();
		}

		static void RopeTrial() {
			new Rope(2, 2, 5, 5, 10).CalculateShape().Graph();
		}

		static void PendulumTrial() {
			var pen = new Pendulum(5, 10, 9.8, .9, .1);
			PlotData p = new PlotData(pen, 0, 10, .01);
			p.AddTrial("time", "theta");
			p.AddTrial("time", "dTheta");
			//p.AddTrial("time", "potentialEnergy");
			//p.AddTrial("time", "kineticEnergy");
			//p.AddTrial("time", "totalEnergy");
			p.Graph();
		}

		static void ChaosGameTrial() {
			int radius = 355;
			new ChaosGame(3, radius, 3.0 / 8.0);
			new ChaosGame(4, radius, 4.0 / 8.0);
			new ChaosGame(5, radius, 4.0 / 8.0);
			new ChaosGame(6, radius, 4.0 / 8.0);
			new ChaosGame(7, radius, 7.0 / 8.0);
		}

		//Homework #2
		static void ProjectileTrial() {
			ProjectileTrial(.001);
			ProjectileTrial(.01);
			ProjectileTrial(.1);
			ProjectileTrial(1);
		}

		static void ProjectileTrial(double gamma) {
			double v0 = 10;
			var target = new Tuple<double, double>(5, 2);
			var p = new PlotData(0, 2, .01);
			p.AddPoint(target.Item1, target.Item2, "Target to hit");

			var projFrict = new LaminarFrictionProjectile(gamma);
			projFrict.SetvMag(v0);
			double ang1 = projFrict.GetAngleToTarget(target.Item1, target.Item2);
			projFrict.SetvMagTheta(v0, ang1);

			p.SetNewEq(projFrict);
			p.AddParametricTrial("x", "y", "t", "Launch angle: " + ang1.ToString() + " gamma: " + gamma.ToString());

			var projNoFric = new NoFrictionProjectile();
			projNoFric.setVmag(v0);
			double ang2 = projNoFric.GetAngleToTarget(target.Item1, target.Item2);
			projNoFric.setTheta(ang2);
			p.SetNewEq(projNoFric);
			p.AddParametricTrial("x", "y", "t", "Launch angle: " + ang2.ToString() + " gamma: " + gamma.ToString());
			p.Graph();

			projFrict.GetThetaForMaxDistance();
		}

		static void LaminarFrictionProjectileTrial() {
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
