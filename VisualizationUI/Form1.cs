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
			Pendulum pendulum = new Pendulum();
			pendulum.SetParameters(10, 10, 9.8, 3, 3);
			var series2 = pendulum.TrialData("time", "dTheta", 0, 20, .1);
			var series = pendulum.TrialData("time", "theta", 0, 20, .1);
			//var series3 = pendulum.TrialData("time", "kineticEnergy", 0, 20, .1);

			Polynomial poly = new Polynomial();
			series = poly.TrialData("x", "dy", 0, 20, .1);

			InitializeComponent(new Series[]{series});
		}
	}
}
