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
		public void AddTrial(string param1, string param2) {
			if (eq == null)
				throw new NullReferenceException();
			toPlot.Add(eq.TrialData(param1, param2, startVal, endVal, stepSize));
		}
		public void AddTrial(MultiVariableEq eq, string param1, string param2){
			toPlot.Add(eq.TrialData(param1, param2, startVal, endVal, stepSize));
		}
		public IEnumerable<Series> GetData() {
			return toPlot.AsEnumerable();
		}
	}
}
