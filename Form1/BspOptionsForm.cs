using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;

namespace Form1
{
	public class BspOptionsForm : MetroForm
	{
		private CheckBox BspOptionsBlockSizeCheckBox;

		private NumericUpDown BspOptionsBlockSizeNumericUpDown;

		private Button BspOptionsButtonCancel;

		private Button BspOptionsButtonOK;

		private CheckBox BspOptionsDebugLightsCheckBox;

		private Label BspOptionsExtraOptionsLabelText;

		private TextBox BspOptionsExtraOptionsTextBox;

		private GroupBox BspOptionsGroupBox;

		private CheckBox BspOptionsOnlyEntsCheckBox;

		private CheckBox BspOptionsSampleScaleCheckBox;

		private NumericUpDown BspOptionsSampleScaleNumericUpDown;

		private IContainer components;

		public BspOptionsForm()
		{
			InitializeComponent();
		}

		private void BspOptionsBlockSizeCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			BspOptionsFormUpdate();
		}

		private void BspOptionsButtonCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void BspOptionsButtonOK_Click(object sender, EventArgs e)
		{
			Launcher.mapSettings.SetBoolean("bspoptions_onlyents", BspOptionsOnlyEntsCheckBox.Checked);
			Launcher.mapSettings.SetBoolean("bspoptions_blocksize", Value: false);
			Launcher.mapSettings.SetBoolean("bspoptions_samplescale", BspOptionsSampleScaleCheckBox.Checked);
			Launcher.mapSettings.SetBoolean("bspoptions_debuglightmaps", BspOptionsDebugLightsCheckBox.Checked);
			Launcher.mapSettings.SetDecimal("bspoptions_blocksize_val", BspOptionsBlockSizeNumericUpDown.Value);
			Launcher.mapSettings.SetDecimal("bspoptions_samplescale_val", BspOptionsSampleScaleNumericUpDown.Value);
			Launcher.mapSettings.SetString("bspoptions_extraoptions", BspOptionsExtraOptionsTextBox.Text);
			Close();
		}

		private void BspOptionsForm_Load(object sender, EventArgs e)
		{
			BspOptionsOnlyEntsCheckBox.Checked = Launcher.mapSettings.GetBoolean("bspoptions_onlyents");
			BspOptionsBlockSizeCheckBox.Checked = false;
			BspOptionsSampleScaleCheckBox.Checked = Launcher.mapSettings.GetBoolean("bspoptions_samplescale");
			BspOptionsDebugLightsCheckBox.Checked = Launcher.mapSettings.GetBoolean("bspoptions_debuglightmaps");
			Launcher.SetNumericUpDownValue(BspOptionsBlockSizeNumericUpDown, Launcher.mapSettings.GetDecimal("bspoptions_blocksize_val"));
			Launcher.SetNumericUpDownValue(BspOptionsSampleScaleNumericUpDown, Launcher.mapSettings.GetDecimal("bspoptions_samplescale_val"));
			BspOptionsExtraOptionsTextBox.Text = Launcher.mapSettings.GetString("bspoptions_extraoptions");
			BspOptionsFormUpdate();
		}

		private void BspOptionsFormUpdate()
		{
			BspOptionsBlockSizeNumericUpDown.Enabled = BspOptionsBlockSizeCheckBox.Checked;
			BspOptionsSampleScaleNumericUpDown.Enabled = BspOptionsSampleScaleCheckBox.Checked;
		}

		private void BspOptionsSampleScaleCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			BspOptionsFormUpdate();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.BspOptionsGroupBox = new System.Windows.Forms.GroupBox();
			this.BspOptionsExtraOptionsLabelText = new System.Windows.Forms.Label();
			this.BspOptionsDebugLightsCheckBox = new System.Windows.Forms.CheckBox();
			this.BspOptionsExtraOptionsTextBox = new System.Windows.Forms.TextBox();
			this.BspOptionsBlockSizeNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.BspOptionsSampleScaleNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.BspOptionsSampleScaleCheckBox = new System.Windows.Forms.CheckBox();
			this.BspOptionsBlockSizeCheckBox = new System.Windows.Forms.CheckBox();
			this.BspOptionsOnlyEntsCheckBox = new System.Windows.Forms.CheckBox();
			this.BspOptionsButtonOK = new System.Windows.Forms.Button();
			this.BspOptionsButtonCancel = new System.Windows.Forms.Button();
			this.BspOptionsGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.BspOptionsBlockSizeNumericUpDown).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.BspOptionsSampleScaleNumericUpDown).BeginInit();
			base.SuspendLayout();
			this.BspOptionsGroupBox.BackColor = System.Drawing.Color.White;
			this.BspOptionsGroupBox.Controls.Add(this.BspOptionsExtraOptionsLabelText);
			this.BspOptionsGroupBox.Controls.Add(this.BspOptionsDebugLightsCheckBox);
			this.BspOptionsGroupBox.Controls.Add(this.BspOptionsExtraOptionsTextBox);
			this.BspOptionsGroupBox.Controls.Add(this.BspOptionsBlockSizeNumericUpDown);
			this.BspOptionsGroupBox.Controls.Add(this.BspOptionsSampleScaleNumericUpDown);
			this.BspOptionsGroupBox.Controls.Add(this.BspOptionsSampleScaleCheckBox);
			this.BspOptionsGroupBox.Controls.Add(this.BspOptionsBlockSizeCheckBox);
			this.BspOptionsGroupBox.Controls.Add(this.BspOptionsOnlyEntsCheckBox);
			this.BspOptionsGroupBox.ForeColor = System.Drawing.Color.Black;
			this.BspOptionsGroupBox.Location = new System.Drawing.Point(12, 12);
			this.BspOptionsGroupBox.Name = "BspOptionsGroupBox";
			this.BspOptionsGroupBox.Size = new System.Drawing.Size(318, 137);
			this.BspOptionsGroupBox.TabIndex = 21;
			this.BspOptionsGroupBox.TabStop = false;
			this.BspOptionsGroupBox.Text = "Compile BSP Options";
			this.BspOptionsExtraOptionsLabelText.AutoSize = true;
			this.BspOptionsExtraOptionsLabelText.BackColor = System.Drawing.Color.White;
			this.BspOptionsExtraOptionsLabelText.ForeColor = System.Drawing.Color.Black;
			this.BspOptionsExtraOptionsLabelText.Location = new System.Drawing.Point(7, 83);
			this.BspOptionsExtraOptionsLabelText.Name = "BspOptionsExtraOptionsLabelText";
			this.BspOptionsExtraOptionsLabelText.Size = new System.Drawing.Size(97, 13);
			this.BspOptionsExtraOptionsLabelText.TabIndex = 19;
			this.BspOptionsExtraOptionsLabelText.Text = "Extra BSP Options:";
			this.BspOptionsDebugLightsCheckBox.AutoSize = true;
			this.BspOptionsDebugLightsCheckBox.BackColor = System.Drawing.Color.White;
			this.BspOptionsDebugLightsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.BspOptionsDebugLightsCheckBox.ForeColor = System.Drawing.Color.Black;
			this.BspOptionsDebugLightsCheckBox.Location = new System.Drawing.Point(10, 53);
			this.BspOptionsDebugLightsCheckBox.Name = "BspOptionsDebugLightsCheckBox";
			this.BspOptionsDebugLightsCheckBox.Size = new System.Drawing.Size(107, 17);
			this.BspOptionsDebugLightsCheckBox.TabIndex = 18;
			this.BspOptionsDebugLightsCheckBox.Tag = "\"Fills lightmaps with random colors to show seams\"";
			this.BspOptionsDebugLightsCheckBox.Text = "Debug Lightmaps";
			this.BspOptionsDebugLightsCheckBox.UseVisualStyleBackColor = false;
			this.BspOptionsDebugLightsCheckBox.CheckedChanged += new System.EventHandler(BspOptionsDebugLightsCheckBox_CheckedChanged);
			this.BspOptionsExtraOptionsTextBox.BackColor = System.Drawing.Color.White;
			this.BspOptionsExtraOptionsTextBox.ForeColor = System.Drawing.Color.Black;
			this.BspOptionsExtraOptionsTextBox.Location = new System.Drawing.Point(10, 99);
			this.BspOptionsExtraOptionsTextBox.Name = "BspOptionsExtraOptionsTextBox";
			this.BspOptionsExtraOptionsTextBox.Size = new System.Drawing.Size(295, 20);
			this.BspOptionsExtraOptionsTextBox.TabIndex = 17;
			this.BspOptionsBlockSizeNumericUpDown.BackColor = System.Drawing.Color.White;
			this.BspOptionsBlockSizeNumericUpDown.DecimalPlaces = 2;
			this.BspOptionsBlockSizeNumericUpDown.ForeColor = System.Drawing.Color.Black;
			this.BspOptionsBlockSizeNumericUpDown.Increment = new decimal(new int[4] { 1, 0, 0, 131072 });
			this.BspOptionsBlockSizeNumericUpDown.Location = new System.Drawing.Point(230, 19);
			this.BspOptionsBlockSizeNumericUpDown.Maximum = new decimal(new int[4] { 64, 0, 0, 0 });
			this.BspOptionsBlockSizeNumericUpDown.Name = "BspOptionsBlockSizeNumericUpDown";
			this.BspOptionsBlockSizeNumericUpDown.ReadOnly = true;
			this.BspOptionsBlockSizeNumericUpDown.Size = new System.Drawing.Size(71, 20);
			this.BspOptionsBlockSizeNumericUpDown.TabIndex = 16;
			this.BspOptionsBlockSizeNumericUpDown.Tag = "\"Grid size for regular BSP splits; 0 uses largest possible\"";
			this.BspOptionsBlockSizeNumericUpDown.Visible = false;
			this.BspOptionsBlockSizeNumericUpDown.ValueChanged += new System.EventHandler(BspOptionsBlockSizeNumericUpDown_ValueChanged);
			this.BspOptionsSampleScaleNumericUpDown.BackColor = System.Drawing.Color.White;
			this.BspOptionsSampleScaleNumericUpDown.DecimalPlaces = 2;
			this.BspOptionsSampleScaleNumericUpDown.ForeColor = System.Drawing.Color.Black;
			this.BspOptionsSampleScaleNumericUpDown.Increment = new decimal(new int[4] { 1, 0, 0, 131072 });
			this.BspOptionsSampleScaleNumericUpDown.Location = new System.Drawing.Point(230, 53);
			this.BspOptionsSampleScaleNumericUpDown.Minimum = new decimal(new int[4] { 1, 0, 0, 131072 });
			this.BspOptionsSampleScaleNumericUpDown.Name = "BspOptionsSampleScaleNumericUpDown";
			this.BspOptionsSampleScaleNumericUpDown.Size = new System.Drawing.Size(71, 20);
			this.BspOptionsSampleScaleNumericUpDown.TabIndex = 15;
			this.BspOptionsSampleScaleNumericUpDown.Tag = "\"Scales all lightmaps; For example 2.00 doubles pixel size, 0.50 halves it\"";
			this.BspOptionsSampleScaleNumericUpDown.Value = new decimal(new int[4] { 100, 0, 0, 131072 });
			this.BspOptionsSampleScaleNumericUpDown.ValueChanged += new System.EventHandler(BspOptionsSampleScaleNumericUpDown_ValueChanged);
			this.BspOptionsSampleScaleCheckBox.AutoSize = true;
			this.BspOptionsSampleScaleCheckBox.BackColor = System.Drawing.Color.White;
			this.BspOptionsSampleScaleCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.BspOptionsSampleScaleCheckBox.ForeColor = System.Drawing.Color.Black;
			this.BspOptionsSampleScaleCheckBox.Location = new System.Drawing.Point(135, 53);
			this.BspOptionsSampleScaleCheckBox.Name = "BspOptionsSampleScaleCheckBox";
			this.BspOptionsSampleScaleCheckBox.Size = new System.Drawing.Size(89, 17);
			this.BspOptionsSampleScaleCheckBox.TabIndex = 14;
			this.BspOptionsSampleScaleCheckBox.Text = "Sample Scale";
			this.BspOptionsSampleScaleCheckBox.UseVisualStyleBackColor = false;
			this.BspOptionsSampleScaleCheckBox.CheckedChanged += new System.EventHandler(BspOptionsSampleScaleCheckBox_CheckedChanged);
			this.BspOptionsBlockSizeCheckBox.AutoSize = true;
			this.BspOptionsBlockSizeCheckBox.BackColor = System.Drawing.Color.White;
			this.BspOptionsBlockSizeCheckBox.Enabled = false;
			this.BspOptionsBlockSizeCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.BspOptionsBlockSizeCheckBox.ForeColor = System.Drawing.Color.Black;
			this.BspOptionsBlockSizeCheckBox.Location = new System.Drawing.Point(135, 19);
			this.BspOptionsBlockSizeCheckBox.Name = "BspOptionsBlockSizeCheckBox";
			this.BspOptionsBlockSizeCheckBox.Size = new System.Drawing.Size(74, 17);
			this.BspOptionsBlockSizeCheckBox.TabIndex = 13;
			this.BspOptionsBlockSizeCheckBox.Text = "Block Size";
			this.BspOptionsBlockSizeCheckBox.UseVisualStyleBackColor = false;
			this.BspOptionsBlockSizeCheckBox.CheckedChanged += new System.EventHandler(BspOptionsBlockSizeCheckBox_CheckedChanged);
			this.BspOptionsOnlyEntsCheckBox.AutoSize = true;
			this.BspOptionsOnlyEntsCheckBox.BackColor = System.Drawing.Color.White;
			this.BspOptionsOnlyEntsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.BspOptionsOnlyEntsCheckBox.ForeColor = System.Drawing.Color.Black;
			this.BspOptionsOnlyEntsCheckBox.Location = new System.Drawing.Point(10, 19);
			this.BspOptionsOnlyEntsCheckBox.Name = "BspOptionsOnlyEntsCheckBox";
			this.BspOptionsOnlyEntsCheckBox.Size = new System.Drawing.Size(69, 17);
			this.BspOptionsOnlyEntsCheckBox.TabIndex = 12;
			this.BspOptionsOnlyEntsCheckBox.Tag = "\"Compile doesn't touch triggers, geometry, or lighting\"";
			this.BspOptionsOnlyEntsCheckBox.Text = "Only Ents";
			this.BspOptionsOnlyEntsCheckBox.UseVisualStyleBackColor = false;
			this.BspOptionsOnlyEntsCheckBox.CheckedChanged += new System.EventHandler(BspOptionsOnlyEntsCheckBox_CheckedChanged);
			this.BspOptionsButtonOK.BackColor = System.Drawing.Color.White;
			this.BspOptionsButtonOK.ForeColor = System.Drawing.Color.Black;
			this.BspOptionsButtonOK.Location = new System.Drawing.Point(174, 155);
			this.BspOptionsButtonOK.Name = "BspOptionsButtonOK";
			this.BspOptionsButtonOK.Size = new System.Drawing.Size(75, 23);
			this.BspOptionsButtonOK.TabIndex = 22;
			this.BspOptionsButtonOK.Text = "OK";
			this.BspOptionsButtonOK.UseVisualStyleBackColor = false;
			this.BspOptionsButtonOK.Click += new System.EventHandler(BspOptionsButtonOK_Click);
			this.BspOptionsButtonCancel.BackColor = System.Drawing.Color.White;
			this.BspOptionsButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BspOptionsButtonCancel.ForeColor = System.Drawing.Color.Black;
			this.BspOptionsButtonCancel.Location = new System.Drawing.Point(255, 155);
			this.BspOptionsButtonCancel.Name = "BspOptionsButtonCancel";
			this.BspOptionsButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.BspOptionsButtonCancel.TabIndex = 23;
			this.BspOptionsButtonCancel.Text = "Cancel";
			this.BspOptionsButtonCancel.UseVisualStyleBackColor = false;
			this.BspOptionsButtonCancel.Click += new System.EventHandler(BspOptionsButtonCancel_Click);
			base.AcceptButton = this.BspOptionsButtonOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.BspOptionsButtonCancel;
			base.ClientSize = new System.Drawing.Size(342, 190);
			base.ControlBox = false;
			base.Controls.Add(this.BspOptionsButtonCancel);
			base.Controls.Add(this.BspOptionsButtonOK);
			base.Controls.Add(this.BspOptionsGroupBox);
			this.DoubleBuffered = true;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "BspOptionsForm";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Advanced BSP Options";
			base.Load += new System.EventHandler(BspOptionsForm_Load);
			this.BspOptionsGroupBox.ResumeLayout(false);
			this.BspOptionsGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.BspOptionsBlockSizeNumericUpDown).EndInit();
			((System.ComponentModel.ISupportInitialize)this.BspOptionsSampleScaleNumericUpDown).EndInit();
			base.ResumeLayout(false);
		}

		private void BspOptionsDebugLightsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void BspOptionsOnlyEntsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void BspOptionsSampleScaleNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
		}

		private void BspOptionsBlockSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
		}
	}
}
