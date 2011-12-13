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
using System.Diagnostics;

namespace VisualizationUI {
	public partial class Chart : Form {
		public Chart() {
			//Todo: Get rid of the visualization UI and port visualization functionality to the Common library
			LaminarFrictionProjectile proj = new LaminarFrictionProjectile(2);
			PlotData p = new PlotData(proj, 0, 5, .01);
			proj.SetvMagTheta(10, Math.PI / 4);
			var angle = proj.GetAngleToTarget(2, 2);
			p.AddPoint(2, 2, "target");
			Debug.Print(angle.ToString());
			if (angle == double.MinValue)
				throw new Exception("no solution");

			p.AddParametricTrial("x", "y", "t");


			InitializeComponent(p);
		}
	}
}
