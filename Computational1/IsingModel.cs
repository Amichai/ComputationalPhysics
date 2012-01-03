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
		Color clr2 = Color.Red;

		public double GetTemp(){
			return temperature;
		}


		private void randomize() {
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < width; j++) {
					if (rand.NextDouble() < .5) {
						systemState[i][j] = true;
						Bitmap.SetPixel(i, j, clr1);
					} else {
						systemState[i][j] = false;
						Bitmap.SetPixel(i, j, clr2);
					}
				}
			}
		}

		public IsingModel() {
			//this.temperature = Ti;
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

		private void setPixel(int x, int y, bool val){
			if (val)
				Bitmap.SetPixel(x, y, clr1);
			else Bitmap.SetPixel(x, y, clr2);
		}

		public void Perturb() {
			int xIdx = rand.Next(0, width);
			int yIdx = rand.Next(0, width);
			double dV = changeInVWithFlip(new Tuple<int, int>(xIdx, yIdx));
			bool origonalColor = systemState[xIdx][yIdx];
			if (dV < 0) {
				systemState[xIdx][yIdx] = !origonalColor;
				setPixel(xIdx, yIdx, !origonalColor);
			} else if (rand.NextDouble() <= Math.Exp(-dV / temperature)) {
				systemState[xIdx][yIdx] = !systemState[xIdx][yIdx];
				setPixel(xIdx, yIdx, !origonalColor);
			} 
		}

		public void IncreaseT(double dt) {
			if(temperature < 100)
				temperature += dt;
		}
		public void DecreaseT(double dt) {
			if(temperature - dt > 0)
				temperature -= dt;
		}


		public Bitmap Bitmap = new Bitmap(width, width);

		public int ImgMagnification = 3;
	}

	public class IsingVisualization : Form {
		public IsingVisualization(IsingModel a) {
			this.A = a;
			InitializeComponents();
		}
		IsingModel A;
		int mag = 0;
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
			//Temperature Section:
			this.Heading1 = new Label();
			this.Heading1.Location = new Point(margin1, margin2);
			using (Graphics g = CreateGraphics()) {
				string text = "Temperature:";
				this.Heading1.Font = new Font(this.Heading1.Font, FontStyle.Bold);
				SizeF size = g.MeasureString(text, this.Heading1.Font, 495);
				this.Heading1.Height = (int)Math.Ceiling(size.Height);
				this.Heading1.Text = text;
			}
			margin2 += Heading1.Size.Height + 1;
			this.comboBox1 = new ComboBox();
			this.comboBox1.Items.AddRange(new object[]{Temperature.constant, Temperature.lower, Temperature.raise});
			this.comboBox1.Location = new Point(margin1, margin2);
			this.comboBox1.Width = 90;
			this.comboBox1.SelectedItem = Temperature.constant;
			this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
			margin2 += this.comboBox1.Size.Height + 1;
			this.properties1 = new Label();
			this.properties1.Location = new Point(margin1, margin2);
			//Interaction Energy Section
			this.Heading2 = new Label();
			this.Heading2.Location = new Point(margin1, margin2);
			using (Graphics g = CreateGraphics()) {
				string text = "Temperature:";
				this.Heading2.Font = new Font(this.Heading2.Font, FontStyle.Bold);
				SizeF size = g.MeasureString(text, this.Heading2.Font, 495);
				this.Heading2.Height = (int)Math.Ceiling(size.Height);
				this.Heading2.Text = text;
			}
			margin2 += Heading2.Size.Height + 1;
			this.comboBox2 = new ComboBox();
			this.comboBox2.Items.AddRange(new object[] { Temperature.constant, Temperature.lower, Temperature.raise });
			this.comboBox2.Location = new Point(margin1, margin2);
			this.comboBox2.Width = 90;
			this.comboBox2.SelectedItem = Temperature.constant;
			this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
			margin2 += this.comboBox2.Size.Height + 1;
			this.properties2 = new Label();
			this.properties2.Location = new Point(margin1, margin2);
			
			

			this.Controls.AddRange(new System.Windows.Forms.Control[] { this.systemImage, this.properties1, this.comboBox1
			, Heading1, this.properties2, this.comboBox2, this.Heading2});

			this.Name = "Form1";
			this.Text = "Crasher";
			this.ResumeLayout(false);
			this.BackgroundImageLayout = ImageLayout.Center;
		}

		private void SetLabel(string text) {
			using (Graphics g = CreateGraphics()) {
				SizeF size = g.MeasureString(text, this.properties1.Font, 495);
				this.properties1.Height = (int)Math.Ceiling(size.Height);
				this.properties1.Text = text;
			}
		}

		private System.Windows.Forms.PictureBox systemImage;
		private System.Windows.Forms.Timer timer1;
		private Label properties1;
		private Label Heading1;
		private ComboBox comboBox1;

		private Label properties2;
		private Label Heading2;
		private ComboBox comboBox2;

		enum Temperature{ raise, lower, constant}
		Temperature setting;

		private void timer1_Tick(object sender, System.EventArgs e) {
			for (int i = 0; i < 50; i++ )
				A.Perturb();
			setting = (Temperature)comboBox1.SelectedItem;
			if(setting == Temperature.lower)
				A.DecreaseT(.005);
			if (setting == Temperature.raise)
				A.IncreaseT(.005);
			var magnifiedBitmap = A.Bitmap.Magnify(mag);
			this.systemImage.Image = magnifiedBitmap;
			SetLabel("T= " + A.GetTemp().ToString());
			this.Refresh();
		}
	}
}
