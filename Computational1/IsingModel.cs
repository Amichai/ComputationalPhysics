using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace Computational1 {
	public class IsingModel {
		static int width = 100;
		DoubleArray<bool> systemState = new DoubleArray<bool>(width, false);
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


		private void randomize() {
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < width; j++) {
					if (rand.NextDouble() < .5) {
						systemState[i][j] = true;
						setPixel(i, j, true, 1);
					} else {
						systemState[i][j] = false;
						setPixel(i, j, false,1);
					}
				}
			}
		}

		public IsingModel() {
			randomize();
			new IsingVisualization(this).ShowDialog();
		}

		private int neighborSum(int x, int y) {
			int sum = 0;
			if (y + 1 == width) {
				if (systemState[0][y]) {
					sum++;
				} else sum--;
			} else {
				if (systemState[x + 1][y]) {
					sum++;
				} else sum--;
			}
			if (y + 1 == width) {
				if (systemState[x][0]) {
					sum++;
				} else sum--;
			} else {
				if (systemState[x][y + 1]) {
					sum++;
				} else sum--;
			}
			if (x == 0) {
				if (systemState[width-1][y]) {
					sum++;
				} else sum--;
			} else {
				if (systemState[x - 1][y]) {
					sum++;
				} else sum--;
			}
			if (y == 0) {
				if (systemState[x][width-1]) {
					sum++;
				} else sum--;
			} else {
				if (systemState[x][y - 1]) {
					sum++;
				} else sum--;
			}
			return sum;
		}

		private double changeInVWithFlip(Tuple<int, int> at) {
			int xIdx = rand.Next(0, width);
			int yIdx = rand.Next(0, width);
			bool newVal= !systemState[at.Item1][at.Item2];
			double skPrime;
			if (newVal)
				skPrime = 2;
			else skPrime = -2;
			double neighborS = neighborSum(at.Item1, at.Item2);
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

		public void Perturb() {
			int xIdx = rand.Next(0, width);
			int yIdx = rand.Next(0, width);
			double dV = changeInVWithFlip(new Tuple<int, int>(xIdx, yIdx));
			bool origonalColor = systemState[xIdx][yIdx];
			if (dV < 0) {
				systemState[xIdx][yIdx] = !origonalColor;
				setPixel(xIdx, yIdx, !origonalColor,2);
			} else if (rand.NextDouble() <= Math.Exp(-dV / temperature)) {
				systemState[xIdx][yIdx] = !systemState[xIdx][yIdx];
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

	
	

	public class IsingVisualization : Form {
		private class varyParameter : Panel{
			Label heading = new Label();
			ComboBox options = new ComboBox();
			Label currentValue = new Label();

			int border= 3;

			public varyParameter(string headingText) {
				
				this.heading.Location = new Point(border, border);
				using (Graphics g = CreateGraphics()) {
					string text = headingText;
					this.heading.Font = new Font(this.heading.Font, FontStyle.Bold);
					SizeF size = g.MeasureString(text, this.heading.Font, 495);
					this.heading.Height = (int)Math.Ceiling(size.Height);
					this.heading.Text = text;
				}
				int yIdx = this.heading.Height;
				this.options.Items.AddRange(new object[] { VarChanger.constant, VarChanger.lower, VarChanger.raise });
				this.options.Location = new Point(border, yIdx + border);
				this.options.Width = 90;
				this.options.SelectedItem = VarChanger.constant;
				this.options.DropDownStyle = ComboBoxStyle.DropDownList;
				yIdx += this.options.Size.Height + border;
				this.currentValue = new Label();
				this.currentValue.Location = new Point(border, yIdx);
				this.Controls.AddRange(new Control[] { this.heading, this.options, this.currentValue });
				this.Width = 100;
				this.Height = yIdx + this.currentValue.Height;
			}
			public void SetValue(string text) {
				using (Graphics g = CreateGraphics()) {
					SizeF size = g.MeasureString(text, this.currentValue.Font, 495);
					this.currentValue.Height = (int)Math.Ceiling(size.Height);
					this.currentValue.Text = text;
				}
			}

			internal VarChanger SelectedItem() {
				return (VarChanger)options.SelectedItem;
			}
		}

		public IsingVisualization(IsingModel a) {
			this.A = a;
			InitializeComponents();
		}
		IsingModel A;
		int mag = 0;
		varyParameter temperature = new varyParameter("Temperature:");
		varyParameter interactionEnergy = new varyParameter("InteractionEnergy:");
		varyParameter externalField = new varyParameter("External field:");
		private void InitializeComponents() {
			this.systemImage = new PictureBox();
			mag = A.ImgMagnification;
			this.systemImage.Size = new Size(A.Bitmap.Width * mag, A.Bitmap.Width * mag);
			this.systemImage.Location = new Point(10, 10);
			this.timer1 = new System.Windows.Forms.Timer(new System.ComponentModel.Container());
			this.SuspendLayout();
			this.timer1.Interval = 1;
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(392, 341);
			int margin1 = systemImage.Location.X + systemImage.Size.Width + 10;
			int margin2 = 10;

			this.temperature.Location = new Point(margin1, margin2);
			margin2 += this.temperature.Height;
			this.interactionEnergy.Location = new Point(margin1, margin2);
			margin2 += this.interactionEnergy.Height;
			this.externalField.Location = new Point(margin1, margin2);
			margin2 += this.externalField.Height;

			this.magnitization = new Label();
			this.magnitization.Location = new Point(margin1, margin2);

			this.Controls.AddRange(new System.Windows.Forms.Control[] { this.systemImage, this.temperature, 
								this.externalField, this.interactionEnergy, this.magnitization});

			this.Name = "Form1";
			this.Text = "Crasher";
			this.ResumeLayout(false);
			this.BackgroundImageLayout = ImageLayout.Center;
			this.Width = 450;
		}

		private void SetLabel3(string text) {
			using (Graphics g = CreateGraphics()) {
				SizeF size = g.MeasureString(text, this.magnitization.Font, 495);
				this.magnitization.Height = (int)Math.Ceiling(size.Height);
				this.magnitization.Text = text;
			}
		}

		private System.Windows.Forms.PictureBox systemImage;
		private System.Windows.Forms.Timer timer1;
		private Label magnitization;
		enum VarChanger{ raise, lower, constant}
		VarChanger tempSetting;
		VarChanger iESetting;
		VarChanger externalFieldSetting;

		private void timer1_Tick(object sender, System.EventArgs e) {
			for (int i = 0; i < 50; i++ )
				A.Perturb();
			tempSetting = temperature.SelectedItem();
			if(tempSetting == VarChanger.lower)
				A.DecreaseT(.005);
			if (tempSetting == VarChanger.raise)
				A.IncreaseT(.005);
			this.temperature.SetValue("T= " + A.GetTemp().ToString());

			iESetting = interactionEnergy.SelectedItem();
			if (iESetting == VarChanger.lower)
				A.DecreaseIE(.005);
			if (iESetting == VarChanger.raise)
				A.IncreaseIE(.005);
			this.interactionEnergy.SetValue("IE= " + A.GetIE().ToString());

			externalFieldSetting = this.externalField.SelectedItem();
			if (externalFieldSetting == VarChanger.lower)
				A.DecreaseEF(.005);
			if (externalFieldSetting == VarChanger.raise)
				A.IncreaseEF(.005);
			this.externalField.SetValue("EF= " + A.GetEF().ToString());

			SetLabel3("Magnetization=\n " + A.Magnitization.ToString());
			var magnifiedBitmap = A.Bitmap.Magnify(mag);
			this.systemImage.Image = magnifiedBitmap;
			this.Refresh();
		}
	}
}
