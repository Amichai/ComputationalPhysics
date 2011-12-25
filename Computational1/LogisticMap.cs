using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Common;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace Computational1 {
	class LogisticMap {
		Series mapData = new Series("Logistic Map");
		public LogisticMap(double mu, double r0, double rmax, double dr) {
			mapData.ChartType = SeriesChartType.Point;
			for(double r = r0; r < rmax; r+= dr ){
				Func<double, double> log = i => r * i * (1 - Math.Pow(i, mu));
				for (int i = 10; i < 20; i++) {
					var A = new Sequence(log, 2).GetElementAt(i);
					if (A > double.MaxValue) {
						throw new Exception();
					}
					mapData.Points.Add(r, A);
				}
			}
		}
		public void Show(){
			
		}
	}
}
