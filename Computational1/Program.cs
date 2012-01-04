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
			//ProjectileTrial();
			//RopeTrial();
			//QuadraticFrictionProjectileTrial();
			//LogisticMapTrial();
			//new IsingModel();

			var A = new ThreeDIsing();
			Func<double, double> ising = i => A.AverageMagnitizationPerSpinAfterTime(i, 1000000, 1000, 1);
			new SingleVariableEq(ising).Graph(1,10, 1);


			//new PolynomialEquation(1, 1, -6).EliminateRoot(-2);
			//new PolynomialEquation(1, 1, -6).GetRoots();
			//Console.ReadLine();
		}

		

		static void LogisticMapTrial() {
			//new LogisticMap(1, 3.56, 3.57, .00001).Graph().ShowDialog();
			new LogisticMap(2, 2.4, 4, .001).Graph().ShowDialog();
		}

		static void DoublePendulumTrial() {
			var A = new DoublePendulum(1, 1, 1, 1, Math.PI / 2, Math.PI / 8, -9.8, 30, .5, .5, 0, 0, 0, Math.PI / 4, Math.PI / 4, 0, 0);
			new Animation(A, .1).ShowDialog();
		}

		static void QuadraticFrictionProjectileTrial() {
			var A = new QuadraticFrictionProjectile(.01, 173, 173);
			var ser1 = A.GetDataSeries(0, 11, .05);

			A = new QuadraticFrictionProjectile(.01, 200);
			var theta = A.GetThetaForMaxDistance();
			var ser2 = A.GetDataSeries(0, 11, .05, theta);

			A = new QuadraticFrictionProjectile(.01, 200);
			theta = A.GetAngleToTarget(155, 90);
			var ser3 = A.GetDataSeries(0, 11, .05, theta);
			var data = new PlotData(ser1, ser2, ser3);

			data.AddPoint(155, 90, "target");
			data.Graph();
		}

		static void CycloidTiral() {
			var brach = new Cycloid(4);
			PlotData p = new PlotData(brach, 0, 4 * Math.PI, .1);
			p.AddParametricTrial("x", "y", "phi");
			p.Graph();
		}

		static void RopeTrial() {
			//new Rope(2, 2, 5, 5, 5).CalculateShape().Graph();
			int counter;
			Func<double, double> tension = i => new Rope(2, 2, 5, 5, i).GetMaxRopeTension(1);
			var lengthForMinTension= FindZero.DichotomyMethod(i => new SingleVariableEq(tension).Derivative(i), 5, 7, out counter);
			Console.WriteLine("The rope length for minimizing the tension at the highest point of the rope:\n" + lengthForMinTension.ToString());
			Console.Read();
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

			var target = new Tuple<double, double>(5, 2);
			double vMag = 10;
			NoFrictionProjectile proj1 = new NoFrictionProjectile();
			proj1.setVmag(vMag);
			var angle = proj1.GetAnglesToTarget(target.Item1, target.Item2).Item2;
			proj1.setTheta(angle);
			double t = proj1.GetTimeOfFlight();
			PlotData p = new PlotData(proj1, 0, t, .01);
			p.AddPoint(target.Item1, target.Item2, "Target to hit");
			p.AddParametricTrial("x", "y", "t", "No friction\nLaunch angle: \n" + angle.ToString());
			LaminarFrictionProjectile proj2 = new LaminarFrictionProjectile();
			p.SetNewEq(proj2);
			double gamma = .1;

			
			proj2.SetGamma(gamma);
			proj2.SetvMag(vMag);
			angle = proj2.GetAngleToTarget(target.Item1, target.Item2);
			proj2.SetvMagTheta(vMag, angle);
			p.AddParametricTrial("x", "y", "t", "Launch angle: \n" + angle.ToString() + "\nGamma: " + gamma.ToString());
			gamma = .01;
			proj2.SetGamma(gamma);
			angle = proj2.GetAngleToTarget(target.Item1, target.Item2);
			proj2.SetvMagTheta(vMag, angle);
			p.AddParametricTrial("x", "y", "t", "Launch angle: \n" + angle.ToString() + "\nGamma: " + gamma.ToString());
			gamma = .001;
			proj2.SetGamma(gamma);
			angle = proj2.GetAngleToTarget(target.Item1, target.Item2);
			proj2.SetvMagTheta(vMag, angle);
			p.AddParametricTrial("x", "y", "t", "Launch angle: \n" + angle.ToString() + "\nGamma: " + gamma.ToString());
			//p.Graph();			

			for (double i = .3; i < .5; i+=.01) {
				proj2.SetGamma(i);
				proj2.SetvMag(vMag);
				angle = proj2.GetAngleToTarget(target.Item1, target.Item2);
				if(angle < 0){
					Console.WriteLine("Largest gamma with which the projectile can reach the target:");
					i -= .01;
					Console.WriteLine((i).ToString());
					Console.WriteLine("\nNumber of iterations to convergence:");
					proj2.SetGamma(i);
					Console.WriteLine(proj2.NumberOfIterationsToConvergence(target.Item1, target.Item2).ToString());
					break;
				}
			}
			Console.Read();

			//ProjectileTrial(.001);
			//ProjectileTrial(.01);
			//ProjectileTrial(.1);
			//ProjectileTrial(1);
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
			var ang2 = projNoFric.GetAnglesToTarget(target.Item1, target.Item2);
			projNoFric.setTheta(ang2.Item1);
			p.SetNewEq(projNoFric);
			p.AddParametricTrial("x", "y", "t", "Launch angle: " + ang2.Item1.ToString() + " gamma: " + gamma.ToString());
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
