using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Computational1;
using System.Windows.Forms.DataVisualization.Charting;

namespace VisualizationUI {
	public partial class Chart : Form {
		public Chart() {
			//Pendulum pendulum = new Pendulum(10, 10, 9.8, 3, 3);
			//s = pendulum.TrialData("time", "dTheta", 0, 20, .1);
			//s = pendulum.TrialData("time", "theta", 0, 20, .1);
			//toPlot.Add(s);
			//s = pendulum.TrialData("time", "height", 0, 20, .1);
			//toPlot.Add(s);
			//s = pendulum.TrialData("time", "kineticEnergy", 0, 20, .1);
			//s = pendulum.TrialData("time", "potentialEnergy", 0, 20, .1);
			////toPlot.Add(s);
			//pendulum.AddDependentVariable("PE+KE", () => pendulum["potentialEnergy"]() + pendulum["kineticEnergy"]());
			//s = pendulum.TrialData("time", "PE+KE", 0, 20, .1);

			Polynomial poly = new Polynomial(1, 0, -200);
			PlotData p = new PlotData(poly, -20, 20, .1);
			p.AddTrial( "x", "y");
			p.AddTrial( "x", "dy");
			poly.AddDependentVariable("inty", () => poly.Relate("x", "y").EvaluateIntegral(-20, poly["x"](), .0001, 3000));
			p.AddTrial("x", "inty");
			//Cosine cos = new Cosine(3);
			//var s1 = cos.TrialData("x", "y", -20, 20, .1);
			//var s2 = cos.TrialData("x", "dy", -20, 20, .1);

			InitializeComponent(p);
		}
	}
}
