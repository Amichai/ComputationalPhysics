using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Computational1 {
	class Program {
		static void Main(string[] args) {
			var pen = new Pendulum();
			pen.SetParameters(10, 10, 9.8, 3, 3);
			for (int i = 0; i < 1000; i++) {
				Debug.Print(pen.Theta(i).ToString());
				Debug.Print("Diff: " + (pen.Theta(i) - pen.Evaluate("t", i, "theta")).ToString());
			}

		}
	}
}
