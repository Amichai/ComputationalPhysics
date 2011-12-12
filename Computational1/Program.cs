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
			
			var proj = new Projectile();
			proj.SetParameters(.1, 5, 5);
		}
	}
}
