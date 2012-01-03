using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Computational1 {


	public class PolynomialFit {
		public double[] DoubleHorner(double[] p, int n, double[]R, double[] Q, double a) {
			var r = p;
			var q = p;
			for (int i = n - 1; i > 0; i--) {
				r[i] = r[i] * a + p[i];
				q[i] = q[i] * a + r[i];
			}
			R = r;
			Q = q;
			return r;
		}

		public PolynomialFit(int m, double[] x, double[] y, int n) {
			int n1 = n + 1;
			int n2 = n1 + 1;
			double[] p = new double[n1];
			double[,] a = new double[n1,n1*n2];
			double[] b = new double[n2 * n2];
			for (int i = 0; i < n1 * n2; i++) {
				a[0, i] = 0;
			}
			for (int i = 0; i < n1; i++) {
				//a[i] = a[i - 1] + n2;
			}
			for (int i = 0; i < m; i++) {
				double xj = 1;
				for (int j = 0; j < n1; j++) {
					double xk = 1;
					a[j, n1] += xj * y[i];
					xk *= x[i];
				}
				xj *= x[i];
			}
			solveLinSys(a, n1, n2);
			for (int i = 0; i < n1; i++) {
				p[i] = a[i,n1];
			}			
		}

		private void solveLinSys(double[,] a, int n1, int n2){
			
		}
	}
}
