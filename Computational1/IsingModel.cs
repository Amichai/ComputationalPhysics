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
						setPixel(i, j, true);
					} else {
						systemState[i][j] = false;
						setPixel(i, j, false);
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

		private void setPixel(int x, int y, bool val){
			if (val) {
				Bitmap.SetPixel(x, y, clr1);
				Magnitization++;
			} else {
				Bitmap.SetPixel(x, y, clr2);
				Magnitization--;
			}
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
			this.heading1 = new Label();
			this.heading1.Location = new Point(margin1, margin2);
			using (Graphics g = CreateGraphics()) {
				string text = "Temperature:";
				this.heading1.Font = new Font(this.heading1.Font, FontStyle.Bold);
				SizeF size = g.MeasureString(text, this.heading1.Font, 495);
				this.heading1.Height = (int)Math.Ceiling(size.Height);
				this.heading1.Text = text;
			}
			margin2 += heading1.Size.Height + 1;
			this.comboBox1 = new ComboBox();
			this.comboBox1.Items.AddRange(new object[]{VarChanger.constant, VarChanger.lower, VarChanger.raise});
			this.comboBox1.Location = new Point(margin1, margin2);
			this.comboBox1.Width = 90;
			this.comboBox1.SelectedItem = VarChanger.constant;
			this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
			margin2 += this.comboBox1.Size.Height + 1;
			this.properties1 = new Label();
			this.properties1.Location = new Point(margin1, margin2);
			margin2 += this.properties1.Size.Height + 5;
			//Interaction Energy Section
			this.heading2 = new Label();
			this.heading2.Location = new Point(margin1, margin2);
			using (Graphics g = CreateGraphics()) {
				string text = "InteractionEnergy:";
				this.heading2.Font = new Font(this.heading2.Font, FontStyle.Bold);
				SizeF size = g.MeasureString(text, this.heading2.Font, 495);
				this.heading2.Height = (int)Math.Ceiling(size.Height);
				this.heading2.Text = text;
			}
			margin2 += heading2.Size.Height + 1;
			this.comboBox2 = new ComboBox();
			this.comboBox2.Items.AddRange(new object[] { VarChanger.constant, VarChanger.lower, VarChanger.raise });
			this.comboBox2.Location = new Point(margin1, margin2);
			this.comboBox2.Width = 90;
			this.comboBox2.SelectedItem = VarChanger.constant;
			this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
			margin2 += this.comboBox2.Size.Height + 1;
			this.properties2 = new Label();
			this.properties2.Location = new Point(margin1, margin2);
			margin2 += this.properties2.Height + 1;
			
			this.magnitization = new Label();
			this.magnitization.Location = new Point(margin1, margin2);

			this.Controls.AddRange(new System.Windows.Forms.Control[] { this.systemImage, this.properties1, this.comboBox1
			, heading1, this.properties2, this.comboBox2, this.heading2, this.magnitization});

			this.Name = "Form1";
			this.Text = "Crasher";
			this.ResumeLayout(false);
			this.BackgroundImageLayout = ImageLayout.Center;
		}

		private void SetLabel1(string text) {
			using (Graphics g = CreateGraphics()) {
				SizeF size = g.MeasureString(text, this.properties1.Font, 495);
				this.properties1.Height = (int)Math.Ceiling(size.Height);
				this.properties1.Text = text;
			}
		}

		private void SetLabel2(string text) {
			using (Graphics g = CreateGraphics()) {
				SizeF size = g.MeasureString(text, this.properties2.Font, 495);
				this.properties2.Height = (int)Math.Ceiling(size.Height);
				this.properties2.Text = text;
			}
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
		private Label properties1;
		private Label heading1;
		private ComboBox comboBox1;
		private Label properties2;
		private Label heading2;
		private ComboBox comboBox2;
		private Label magnitization;

		enum VarChanger{ raise, lower, constant}
		VarChanger tempSetting;
		VarChanger iESetting;

		private void timer1_Tick(object sender, System.EventArgs e) {
			for (int i = 0; i < 50; i++ )
				A.Perturb();
			tempSetting = (VarChanger)comboBox1.SelectedItem;
			if(tempSetting == VarChanger.lower)
				A.DecreaseT(.005);
			if (tempSetting == VarChanger.raise)
				A.IncreaseT(.005);
			SetLabel1("T= " + A.GetTemp().ToString());

			iESetting = (VarChanger)comboBox2.SelectedItem;
			if (iESetting == VarChanger.lower)
				A.DecreaseIE(.005);
			if (iESetting == VarChanger.raise)
				A.IncreaseIE(.005);
			SetLabel2("IE= " + A.GetIE().ToString());

			SetLabel3("Magnitization= " + A.Magnitization.ToString());
			var magnifiedBitmap = A.Bitmap.Magnify(mag);
			this.systemImage.Image = magnifiedBitmap;
			
			this.Refresh();
		}
	}
}
