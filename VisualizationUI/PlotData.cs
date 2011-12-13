using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using Common;

namespace VisualizationUI {
	class PlotData {
		List<Series> toPlot = new List<Series>();
		double startVal, endVal, stepSize;
		public PlotData(double startVal, double endVal, double stepSize) {
			this.startVal = startVal;
			this.endVal = endVal;
			this.stepSize = stepSize;
		}
		MultiVariableEq eq = null;
		public PlotData(MultiVariableEq eq, double startVal, double endVal, double stepSize) {
			this.eq = eq;
			this.startVal = startVal;
			this.endVal = endVal;
			this.stepSize = stepSize;
		}

		public void AddParametricTrial(string p1, string p2, string paremeticVar) {
			if (eq == null)
				throw new NullReferenceException();
			toPlot.Add(eq.ParemetricTrial(p1, p2, paremeticVar, startVal, endVal, stepSize));
		}
		public void AddParametricTrial(string p1, string p2, string paremeticVar, string lineName) {
			if (eq == null)
				throw new NullReferenceException();
			toPlot.Add(eq.ParemetricTrial(p1, p2, paremeticVar, startVal, endVal, stepSize, lineName));
		}

		public void AddTrial(string param1, string param2, string seriesName = null) {
			if (eq == null)
				throw new NullReferenceException();
			toPlot.Add(eq.TrialData(param1, param2, startVal, endVal, stepSize, seriesName));
		}
		public void AddTrial(MultiVariableEq eq, string param1, string param2){
			toPlot.Add(eq.TrialData(param1, param2, startVal, endVal, stepSize));
		}
		public IEnumerable<Series> GetData() {
			return toPlot.AsEnumerable();
		}

		internal void AddPoint(double x, double y, string name) {
			Series ptSer = new Series();
			ptSer.Points.Add(new DataPoint(x, y));
			ptSer.ChartType = SeriesChartType.Point;
			toPlot.Add(ptSer);
		}
	}
}
