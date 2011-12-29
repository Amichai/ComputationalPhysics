using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Common;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace Computational1 {
	class LogisticMap {
		Series mapData = new Series("Logistic Map");
		Random rand = new Random();
		public LogisticMap(double mu, double r0, double rmax, double dr) {
			mapData.ChartType = SeriesChartType.Point ;
			mapData.MarkerSize = 1;
			mapData.MarkerColor = Color.Black;

			int numberOfThreadsInMap =1 ;
			List<double> activeThreadValues;
			double eps = .005;
			for(double r = r0; r < rmax; r+= dr ){
				activeThreadValues = new List<double>();
				Func<double, double> log = i => r * i * (1 - Math.Pow(i, mu));
				for (int i = 150; i < 260; i++) {
					var A = new Sequence(log,	.2).GetElementAt(i);
					if (A > double.MaxValue || A < double.MinValue) {
						break;	
					}
					if (activeThreadValues.Where(j => j < A + eps && j > A - eps).Count() == 0)
						activeThreadValues.Add(A);
					mapData.Points.AddXY(r, A);
				}
				if (activeThreadValues.Count > numberOfThreadsInMap) {
					Console.WriteLine("Bifurcation point: " + r.ToString() + " " + activeThreadValues.Count.ToString() + " threads.");
					numberOfThreadsInMap = activeThreadValues.Count();
					eps /= numberOfThreadsInMap;
				}
			}
		}
		public Graph Graph(){
			return new Graph(mapData);
		}
	}
}
