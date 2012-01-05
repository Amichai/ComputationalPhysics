using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Drawing;
using System.Diagnostics;

namespace Computational1 {
	public class ThreeDIsing {
		static int width = 50;
		public double NumberOfSpins = Math.Pow(width, 3);
		List<DoubleArray<bool>> systemState = new List<DoubleArray<bool>>(width);
		
		double interactionEnergy = 10;
		double temperature = 10;
		double externalField = 0;
		Random rand = new Random();
		/// <summary>Color for value = "true"</summary>
		Color clr1 = Color.Green;
		/// <summary>Color for value = "false"</summary>
		Color clr2 = Color.LightBlue;

		public int Magnitization = 0;

		public double GetTemp(){
			return temperature;
		}
		public double GetIE() { return interactionEnergy; }

		private void initializeToZero() {
			for (int i = 0; i < width; i++) {
				systemState.Add(new DoubleArray<bool>(width, false));
			}
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < width; j++) {
					for (int k = 0; k < width; k++) {
						systemState[i][j][k] = true;
						setPixel(i, j, true, 1);
					}
				}
			}
		}

		/// <summary>Experimentally determined value for 3d Ising Model critical temp.</summary>
		public const double CriticalBeta = 4.511;

		public double AutoCorrelation(int radiusToTest, int numberOfSamples) {
			List<int> samples1 = new List<int>(numberOfSamples);
			List<int> samples2 = new List<int>(numberOfSamples);
			for (int i = 0; i < numberOfSamples; i++) {
				int xIdx = rand.Next(0, width - radiusToTest);
				int yIdx = rand.Next(0, width);
				int zIdx = rand.Next(0, width);
				if(systemState[xIdx][yIdx][zIdx])
					samples1.Add(1);
				else samples1.Add(-1);
				if (systemState[xIdx+radiusToTest][yIdx][zIdx])
					samples2.Add(1);
				else samples2.Add(-1);
			}
			double mean1 = samples1.Average();
			double mean2 = samples2.Average();
			List<double> nextSet = new List<double>(numberOfSamples);
			for (int i = 0; i < numberOfSamples; i++) {
				nextSet.Add((samples1[i] - mean1) * (samples2[i] - mean2));
			}
			return nextSet.Average();
		}

		public void Equilibrate(int numberOfPertubations) {
			for (int i = 0; i < numberOfPertubations; i++) {
				Perturb();
			}
		}

		public double CorrelationLength(int maxRadius, int autoCorrelationSamples) {
			List<double> correlationLengthResults = new List<double>();
			for(int i=1; i < maxRadius; i++){
				var autoCorrelation = AutoCorrelation(i, autoCorrelationSamples);
				var result = (-i) / (Math.Log(autoCorrelation));
				if (result < width) {
					correlationLengthResults.Add(result);
				}
			}
			return correlationLengthResults.Average();
		}

		public double ReducedTemperatureVal() {
			return (GetBeta() - CriticalBeta) / GetBeta();
		}

		public void Randomize() {
			Magnitization = 0;
			for (int i = 0; i < width; i++) {
				systemState.Add(new DoubleArray<bool>(width, false));
			}
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < width; j++) {
					for (int k = 0; k < width; k++) {
						if (rand.NextDouble() < .5) {
							systemState[i][j][k] = true;
							setPixel(i, j, true, 1);
						} else {
							systemState[i][j][k] = false;
							setPixel(i, j, false, 1);
						}
					}
				}
			}
		}

		public void SetT(double T){
			temperature = T;
		}
		public void SetInteractionEnergy(double J){
			interactionEnergy = J;
		}
		public double GetBeta() {
			return temperature / interactionEnergy;
		}
		public double MagnitizationPerSpin() {
			return Magnitization / NumberOfSpins;
		}

		public void SetBeta(double beta) {
			interactionEnergy = 10;
			temperature = beta * 10;
		}

		public ThreeDIsing(int size) {
			//randomize();
			width = size;
			initializeToZero();
		}

		public ThreeDIsing() {
			//randomize();
			initializeToZero();
		}

		public double AverageMagnitizationPerSpinAfterTime(double beta, int initialSteps = 100000, int statisticalTrials = 50000, int pertubationsPerTrial = 1) {
			Magnitization = 0;
			//randomize();
			initializeToZero();
			SetBeta(beta);
			
			Perturb(initialSteps);
			var magnitizationPerSpinsReading = new List<double>(statisticalTrials);
			for (int i = 0; i < statisticalTrials; i++) {
				Perturb(pertubationsPerTrial);
				magnitizationPerSpinsReading.Add(MagnitizationPerSpin());
			}
			return magnitizationPerSpinsReading.Average();
		}

		private int neighborSum(int x, int y, int z) {
			int sum = 0;
			if (z == width - 1) {
				if (systemState[x][y][0]) {
					sum++;
				} else sum--;
			} else {
				if (systemState[x][y][z + 1]) {
					sum++;
				} else sum--;
			}
			if (z == 0) {
				if (systemState[x][y][width - 1]) {
					sum++;
				} else sum--;	
			} else {
				if (systemState[x][y][z - 1]) {
					sum++;
				} else sum--;
			}
			if (x + 1 == width) {
				if (systemState[0][y][z]) {
					sum++;
				} else sum--;
			} else {
				if (systemState[x + 1][y][z]) {
					sum++;
				} else sum--;
			}
			if (y + 1 == width) {
				if (systemState[x][0][z]) {
					sum++;
				} else sum--;
			} else {
				if (systemState[x][y + 1][z]) {
					sum++;
				} else sum--;
			}
			if (x == 0) {
				if (systemState[width-1][y][z]) {
					sum++;
				} else sum--;
			} else {
				if (systemState[x - 1][y][z]) {
					sum++;
				} else sum--;
			}
			if (y == 0) {
				if (systemState[x][width-1][z]) {
					sum++;
				} else sum--;
			} else {
				if (systemState[x][y - 1][z]) {
					sum++;
				} else sum--;
			}
			return sum;
		}

		private double changeInVWithFlip(Tuple<int, int, int> at) {
			int xIdx = rand.Next(0, width);
			int yIdx = rand.Next(0, width);
			bool newVal= !systemState[at.Item1][at.Item2][at.Item3];
			double skPrime;
			if (newVal)
				skPrime = 2;
			else skPrime = -2;
			double neighborS = neighborSum(at.Item1, at.Item2, at.Item3);
			return -skPrime*(interactionEnergy * neighborS + externalField);
		}

		private void setPixel(int x, int y, bool val, int dCharge){
			if (val) {
				Bitmap.SetPixel(x, y, clr1);
				Magnitization+=dCharge;
			} else {
				Bitmap.SetPixel(x, y, clr2);
				Magnitization-=dCharge;
			}
		}

		public void Perturb(int numberOfTimes) {
			for (int i = 0; i < numberOfTimes; i++)
				Perturb();
		}

		public void Perturb() {
			int xIdx = rand.Next(0, width);
			int yIdx = rand.Next(0, width);
			int zIdx = rand.Next(0, width);
			double dV = changeInVWithFlip(new Tuple<int, int, int>(xIdx, yIdx, zIdx));
			bool origonalColor = systemState[xIdx][yIdx][zIdx];
			if (dV < 0) {
				systemState[xIdx][yIdx][zIdx] = !origonalColor;
				setPixel(xIdx, yIdx, !origonalColor,2);
			} else if (rand.NextDouble() <= Math.Exp(-dV / temperature)) {
				systemState[xIdx][yIdx][zIdx] = !systemState[xIdx][yIdx][zIdx];
				setPixel(xIdx, yIdx, !origonalColor,2);
			} 
		}

		public void IncreaseT(double dt) {
			if(temperature < 10)
				temperature += dt;
		}
		public void DecreaseT(double dt) {
			if(temperature - dt > 0)
				temperature -= dt;
		}

		public void IncreaseIE(double dIE) {
			if (interactionEnergy < 10)
				interactionEnergy += dIE;
		}
		public void DecreaseIE(double dIE) {
			if (interactionEnergy - dIE > 0)
				interactionEnergy -= dIE;
		}

		public void IncreaseEF(double dIE) {
			if (externalField < 10)
				externalField += dIE;
		}
		public void DecreaseEF(double dIE) {
			if (externalField - dIE > -10)
				externalField -= dIE;
		}


		public Bitmap Bitmap = new Bitmap(width, width);

		public int ImgMagnification = 3;

		internal double GetEF() {
			return externalField;
		}
	}
}
