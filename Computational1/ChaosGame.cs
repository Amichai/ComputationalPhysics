using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Common;


namespace Computational1 {
	public class Polygon{
		private Point[] allVerticies;
		public Polygon(int numberOfSides, int radius) {
			double[] anglesToVerticies = new double[numberOfSides];
			allVerticies = new Point[numberOfSides];
			for (int i = 0; i < numberOfSides; i++) {
				double angleBetweenTwoVerticies = (2*Math.PI)/numberOfSides;
				anglesToVerticies[i] = i * angleBetweenTwoVerticies;
				allVerticies[i] = new Point((int)(radius * Math.Cos(anglesToVerticies[i])) + (int)radius, (int)(radius * Math.Sin(anglesToVerticies[i]))+ (int)radius);
			}
		}
		public Point[] GetAllVerticies() {
			return allVerticies;
		}
	}

	public class ChaosGame {
		public static bool IsInPolygon(Point[] poly, Point point) {
			var coef = poly.Skip(1).Select((p, i) => (point.Y - poly[i].Y) * (p.X - poly[i].X) - (point.X - poly[i].X) * (p.Y - poly[i].Y)).ToList();

			if (coef.Any(p => p == 0))
				return true;

			for (int i = 1; i < coef.Count(); i++) {
				if (coef[i] * coef[i - 1] < 0)
					return false;
			}
			return true;
		}
		private double getDistance(Point a, Point b) {
			return Math.Sqrt(Math.Pow((b.Y - a.Y), 2) + Math.Pow((b.X - a.X), 2));
		}
		private double getAngle(Point a, Point b) {
			double dx = b.X - a.X;
			double dy = b.Y - a.Y;
			return Math.Atan(dy / dx);
		}

		private Random rand = new Random();
		public ChaosGame(int numberOfSides, int radius, double fractionalDistance, int pointsToPlot= 1000000) {
			var poly = new Polygon(numberOfSides, radius);
			var image = new Bitmap(radius * 2 +5, radius *2 + 5);
			var g = Graphics.FromImage(image);
			var pen = new Pen(Color.Black, .1f);
			var verticies = poly.GetAllVerticies();
			g.DrawPolygon(pen, verticies);
			Point newPoint;
			int xCoord, yCoord;
			do{
				xCoord = rand.Next(radius);
				yCoord = rand.Next(radius);
				newPoint = new Point(xCoord, yCoord);
			}while(!IsInPolygon(verticies, newPoint));
			int lastVertex = int.MinValue;
			for (int i = 0; i < pointsToPlot; i++) {
				int vertexToAimAt;
				do {
					vertexToAimAt = rand.Next(numberOfSides);
				} while (vertexToAimAt == lastVertex);
				g.FillRectangle(Brushes.Black, xCoord, yCoord, 1, 1);
				var vertexToHit = verticies[vertexToAimAt];
				double distance = getDistance(newPoint, vertexToHit) * fractionalDistance;
				double angle = getAngle(newPoint, vertexToHit);
				int quad = Angle.GetQuadrant(newPoint, vertexToHit);
				angle = Math.Abs(angle);
				if (quad == 2) {
					xCoord = newPoint.X - (int)(distance * Math.Cos(angle));
					yCoord = newPoint.Y + (int)(distance * Math.Sin(angle));
				}else if (quad == 4) {
					xCoord = newPoint.X + (int)(distance * Math.Cos(angle));
					yCoord = newPoint.Y - (int)(distance * Math.Sin(angle));
				}else if (quad == 3) {
					xCoord = newPoint.X - (int)(distance * Math.Cos(angle));
					yCoord = newPoint.Y - (int)(distance * Math.Sin(angle));
				}else if (quad == 1) {
					xCoord = newPoint.X + (int)(distance * Math.Cos(angle));
					yCoord = newPoint.Y + (int)(distance * Math.Sin(angle));
				}
				newPoint = new Point(xCoord, yCoord);
				lastVertex = vertexToAimAt;
			}

			var form = new Form();
			form.Size = new Size(radius * 2, radius * 2);
			form.BackgroundImage = image;
			form.BackgroundImageLayout = ImageLayout.Center;
			form.ShowDialog();
		}
	}
}
