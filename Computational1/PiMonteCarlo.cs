using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Diagnostics;

namespace Computational1 {
	public static class MonteCarlo {
		public static double GetPi(double iterations) {
			int s = 0;
			MyRandom rand = new MyRandom();
			for (int i = 0; i < iterations; i++) {
				double x = rand.NextDouble();
				double y = rand.NextDouble();
				if (x.Sqrd() + y.Sqrd() < 1) s++;
				Debug.Print((4.0 * s/(i + 1)).ToString());
			}
			return (4.0 * s/(iterations + 1));
		}
	}
	public class MyRandom {
		double mult = 1664525;
		double add = 1013904223;
		double m = 2e32;
		double rn = DateTime.Now.Second;
		public double NextDouble() {
			rn = Math.Abs(rn * mult + add) % m;
			return rn / m;
		}
	}
}
