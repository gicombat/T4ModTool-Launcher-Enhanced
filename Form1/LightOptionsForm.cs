using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;

namespace Form1
{
	public class LightOptionsForm : MetroForm
	{
		private IContainer components;

		private Button LightOptionsButtonCancel;

		private Button LightOptionsButtonOK;

		private RadioButton LightOptionsExtraRadioButton;

		private GroupBox LightOptionsFastExtraGroupBox;

		private RadioButton LightOptionsFastRadioButton;

		private GroupBox LightOptionsGroupBox;

		private CheckBox LightOptionsJitterCheckBox;

		private NumericUpDown LightOptionsJitterNumericUpDown;

		private CheckBox LightOptionsMaxBouncesCheckBox;

		private NumericUpDown LightOptionsMaxBouncesNumericUpDown;

		private CheckBox LightOptionsNoModelShadowsCheckBox;

		private CheckBox LightOptionsTracesCheckBox;

		private NumericUpDown LightOptionsTracesNumericUpDown;

		private CheckBox LightOptionsVerboseCheckBox;

		public LightOptionsForm()
		{
			InitializeComponent();
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
			this.LightOptionsGroupBox = new System.Windows.Forms.GroupBox();
			this.LightOptionsFastExtraGroupBox = new System.Windows.Forms.GroupBox();
			this.LightOptionsExtraRadioButton = new System.Windows.Forms.RadioButton();
			this.LightOptionsFastRadioButton = new System.Windows.Forms.RadioButton();
			this.LightOptionsJitterNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.LightOptionsMaxBouncesNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.LightOptionsTracesNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.LightOptionsJitterCheckBox = new System.Windows.Forms.CheckBox();
			this.LightOptionsMaxBouncesCheckBox = new System.Windows.Forms.CheckBox();
			this.LightOptionsTracesCheckBox = new System.Windows.Forms.CheckBox();
			this.LightOptionsVerboseCheckBox = new System.Windows.Forms.CheckBox();
			this.LightOptionsNoModelShadowsCheckBox = new System.Windows.Forms.CheckBox();
			this.LightOptionsButtonOK = new System.Windows.Forms.Button();
			this.LightOptionsButtonCancel = new System.Windows.Forms.Button();
			this.LightOptionsGroupBox.SuspendLayout();
			this.LightOptionsFastExtraGroupBox.SuspendLayout();
			this.LightOptionsJitterNumericUpDown.BeginInit();
			this.LightOptionsMaxBouncesNumericUpDown.BeginInit();
			this.LightOptionsTracesNumericUpDown.BeginInit();
			base.SuspendLayout();
			this.LightOptionsGroupBox.Controls.Add(this.LightOptionsFastExtraGroupBox);
			this.LightOptionsGroupBox.Controls.Add(this.LightOptionsJitterNumericUpDown);
			this.LightOptionsGroupBox.Controls.Add(this.LightOptionsMaxBouncesNumericUpDown);
			this.LightOptionsGroupBox.Controls.Add(this.LightOptionsTracesNumericUpDown);
			this.LightOptionsGroupBox.Controls.Add(this.LightOptionsJitterCheckBox);
			this.LightOptionsGroupBox.Controls.Add(this.LightOptionsMaxBouncesCheckBox);
			this.LightOptionsGroupBox.Controls.Add(this.LightOptionsTracesCheckBox);
			this.LightOptionsGroupBox.Controls.Add(this.LightOptionsVerboseCheckBox);
			this.LightOptionsGroupBox.Controls.Add(this.LightOptionsNoModelShadowsCheckBox);
			this.LightOptionsGroupBox.Location = new System.Drawing.Point(8, 11);
			this.LightOptionsGroupBox.Name = "LightOptionsGroupBox";
			this.LightOptionsGroupBox.Size = new System.Drawing.Size(325, 121);
			this.LightOptionsGroupBox.TabIndex = 0;
			this.LightOptionsGroupBox.TabStop = false;
			this.LightOptionsGroupBox.Text = "Compile Light Options";
			this.LightOptionsFastExtraGroupBox.Controls.Add(this.LightOptionsExtraRadioButton);
			this.LightOptionsFastExtraGroupBox.Controls.Add(this.LightOptionsFastRadioButton);
			this.LightOptionsFastExtraGroupBox.Location = new System.Drawing.Point(12, 19);
			this.LightOptionsFastExtraGroupBox.Name = "LightOptionsFastExtraGroupBox";
			this.LightOptionsFastExtraGroupBox.Size = new System.Drawing.Size(117, 32);
			this.LightOptionsFastExtraGroupBox.TabIndex = 9;
			this.LightOptionsFastExtraGroupBox.TabStop = false;
			this.LightOptionsExtraRadioButton.AutoSize = true;
			this.LightOptionsExtraRadioButton.Location = new System.Drawing.Point(57, 9);
			this.LightOptionsExtraRadioButton.Name = "LightOptionsExtraRadioButton";
			this.LightOptionsExtraRadioButton.Size = new System.Drawing.Size(49, 17);
			this.LightOptionsExtraRadioButton.TabIndex = 1;
			this.LightOptionsExtraRadioButton.Text = "Extra";
			this.LightOptionsExtraRadioButton.UseVisualStyleBackColor = true;
			this.LightOptionsFastRadioButton.AutoSize = true;
			this.LightOptionsFastRadioButton.Checked = true;
			this.LightOptionsFastRadioButton.Location = new System.Drawing.Point(6, 9);
			this.LightOptionsFastRadioButton.Name = "LightOptionsFastRadioButton";
			this.LightOptionsFastRadioButton.Size = new System.Drawing.Size(45, 17);
			this.LightOptionsFastRadioButton.TabIndex = 0;
			this.LightOptionsFastRadioButton.TabStop = true;
			this.LightOptionsFastRadioButton.Text = "Fast";
			this.LightOptionsFastRadioButton.UseVisualStyleBackColor = true;
			this.LightOptionsJitterNumericUpDown.DecimalPlaces = 3;
			int[] bits = new int[4] { 1, 0, 0, 196608 };
			this.LightOptionsJitterNumericUpDown.Increment = new decimal(bits);
			this.LightOptionsJitterNumericUpDown.Location = new System.Drawing.Point(243, 86);
			int[] bits2 = new int[4] { 4, 0, 0, 0 };
			this.LightOptionsJitterNumericUpDown.Maximum = new decimal(bits2);
			this.LightOptionsJitterNumericUpDown.Name = "LightOptionsJitterNumericUpDown";
			this.LightOptionsJitterNumericUpDown.Size = new System.Drawing.Size(68, 20);
			this.LightOptionsJitterNumericUpDown.TabIndex = 8;
			this.LightOptionsMaxBouncesNumericUpDown.Location = new System.Drawing.Point(242, 60);
			this.LightOptionsMaxBouncesNumericUpDown.Name = "LightOptionsMaxBouncesNumericUpDown";
			this.LightOptionsMaxBouncesNumericUpDown.Size = new System.Drawing.Size(68, 20);
			this.LightOptionsMaxBouncesNumericUpDown.TabIndex = 7;
			this.LightOptionsTracesNumericUpDown.Location = new System.Drawing.Point(243, 34);
			int[] bits3 = new int[4] { 500, 0, 0, 0 };
			this.LightOptionsTracesNumericUpDown.Maximum = new decimal(bits3);
			this.LightOptionsTracesNumericUpDown.Name = "LightOptionsTracesNumericUpDown";
			this.LightOptionsTracesNumericUpDown.Size = new System.Drawing.Size(68, 20);
			this.LightOptionsTracesNumericUpDown.TabIndex = 6;
			this.LightOptionsJitterCheckBox.AutoSize = true;
			this.LightOptionsJitterCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.LightOptionsJitterCheckBox.Location = new System.Drawing.Point(147, 80);
			this.LightOptionsJitterCheckBox.Name = "LightOptionsJitterCheckBox";
			this.LightOptionsJitterCheckBox.Size = new System.Drawing.Size(46, 17);
			this.LightOptionsJitterCheckBox.TabIndex = 5;
			this.LightOptionsJitterCheckBox.Text = "Jitter";
			this.LightOptionsJitterCheckBox.UseVisualStyleBackColor = true;
			this.LightOptionsJitterCheckBox.CheckedChanged += new System.EventHandler(LightOptionsJitterCheckBox_CheckedChanged);
			this.LightOptionsMaxBouncesCheckBox.AutoSize = true;
			this.LightOptionsMaxBouncesCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.LightOptionsMaxBouncesCheckBox.Location = new System.Drawing.Point(147, 57);
			this.LightOptionsMaxBouncesCheckBox.Name = "LightOptionsMaxBouncesCheckBox";
			this.LightOptionsMaxBouncesCheckBox.Size = new System.Drawing.Size(89, 17);
			this.LightOptionsMaxBouncesCheckBox.TabIndex = 4;
			this.LightOptionsMaxBouncesCheckBox.Text = "Max Bounces";
			this.LightOptionsMaxBouncesCheckBox.UseVisualStyleBackColor = true;
			this.LightOptionsMaxBouncesCheckBox.CheckedChanged += new System.EventHandler(LightOptionsMaxBouncesCheckBox_CheckedChanged);
			this.LightOptionsTracesCheckBox.AutoSize = true;
			this.LightOptionsTracesCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.LightOptionsTracesCheckBox.Location = new System.Drawing.Point(147, 34);
			this.LightOptionsTracesCheckBox.Name = "LightOptionsTracesCheckBox";
			this.LightOptionsTracesCheckBox.Size = new System.Drawing.Size(57, 17);
			this.LightOptionsTracesCheckBox.TabIndex = 3;
			this.LightOptionsTracesCheckBox.Text = "Traces";
			this.LightOptionsTracesCheckBox.UseVisualStyleBackColor = true;
			this.LightOptionsTracesCheckBox.CheckedChanged += new System.EventHandler(LightOptionsTracesCheckBox_CheckedChanged);
			this.LightOptionsVerboseCheckBox.AutoSize = true;
			this.LightOptionsVerboseCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.LightOptionsVerboseCheckBox.Location = new System.Drawing.Point(12, 80);
			this.LightOptionsVerboseCheckBox.Name = "LightOptionsVerboseCheckBox";
			this.LightOptionsVerboseCheckBox.Size = new System.Drawing.Size(63, 17);
			this.LightOptionsVerboseCheckBox.TabIndex = 2;
			this.LightOptionsVerboseCheckBox.Text = "Verbose";
			this.LightOptionsVerboseCheckBox.UseVisualStyleBackColor = true;
			this.LightOptionsNoModelShadowsCheckBox.AutoSize = true;
			this.LightOptionsNoModelShadowsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.LightOptionsNoModelShadowsCheckBox.Location = new System.Drawing.Point(12, 57);
			this.LightOptionsNoModelShadowsCheckBox.Name = "LightOptionsNoModelShadowsCheckBox";
			this.LightOptionsNoModelShadowsCheckBox.Size = new System.Drawing.Size(117, 17);
			this.LightOptionsNoModelShadowsCheckBox.TabIndex = 1;
			this.LightOptionsNoModelShadowsCheckBox.Text = "No Model Shadows";
			this.LightOptionsNoModelShadowsCheckBox.UseVisualStyleBackColor = true;
			this.LightOptionsButtonOK.Location = new System.Drawing.Point(169, 138);
			this.LightOptionsButtonOK.Name = "LightOptionsButtonOK";
			this.LightOptionsButtonOK.Size = new System.Drawing.Size(75, 23);
			this.LightOptionsButtonOK.TabIndex = 1;
			this.LightOptionsButtonOK.Text = "OK";
			this.LightOptionsButtonOK.UseVisualStyleBackColor = true;
			this.LightOptionsButtonOK.Click += new System.EventHandler(LightOptionsButtonOK_Click);
			this.LightOptionsButtonCancel.Location = new System.Drawing.Point(258, 138);
			this.LightOptionsButtonCancel.Name = "LightOptionsButtonCancel";
			this.LightOptionsButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.LightOptionsButtonCancel.TabIndex = 2;
			this.LightOptionsButtonCancel.Text = "Cancel";
			this.LightOptionsButtonCancel.UseVisualStyleBackColor = true;
			this.LightOptionsButtonCancel.Click += new System.EventHandler(LightOptionsButtonCancel_Click);
			base.AcceptButton = this.LightOptionsButtonOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.CancelButton = this.LightOptionsButtonCancel;
			base.ClientSize = new System.Drawing.Size(345, 170);
			base.ControlBox = false;
			base.Controls.Add(this.LightOptionsButtonCancel);
			base.Controls.Add(this.LightOptionsButtonOK);
			base.Controls.Add(this.LightOptionsGroupBox);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "LightOptionsForm";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Advanced Light Options";
			base.Load += new System.EventHandler(LightOptionsForm_Load);
			this.LightOptionsGroupBox.ResumeLayout(false);
			this.LightOptionsGroupBox.PerformLayout();
			this.LightOptionsFastExtraGroupBox.ResumeLayout(false);
			this.LightOptionsFastExtraGroupBox.PerformLayout();
			this.LightOptionsJitterNumericUpDown.EndInit();
			this.LightOptionsMaxBouncesNumericUpDown.EndInit();
			this.LightOptionsTracesNumericUpDown.EndInit();
			base.ResumeLayout(false);
		}

		private void LightOptionsButtonCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void LightOptionsButtonOK_Click(object sender, EventArgs e)
		{
			Launcher.mapSettings.SetBoolean("lightoptions_extra", LightOptionsExtraRadioButton.Checked);
			Launcher.mapSettings.SetBoolean("lightoptions_nomodelshadow", LightOptionsNoModelShadowsCheckBox.Checked);
			Launcher.mapSettings.SetBoolean("lightoptions_verbose", LightOptionsVerboseCheckBox.Checked);
			Launcher.mapSettings.SetBoolean("lightoptions_traces", LightOptionsTracesCheckBox.Checked);
			Launcher.mapSettings.SetBoolean("lightoptions_maxbounces", LightOptionsMaxBouncesCheckBox.Checked);
			Launcher.mapSettings.SetBoolean("lightoptions_jitter", LightOptionsJitterCheckBox.Checked);
			Launcher.mapSettings.SetDecimal("lightoptions_traces_val", LightOptionsTracesNumericUpDown.Value);
			Launcher.mapSettings.SetDecimal("lightoptions_maxbounces_val", LightOptionsMaxBouncesNumericUpDown.Value);
			Launcher.mapSettings.SetDecimal("lightoptions_jitter_val", LightOptionsJitterNumericUpDown.Value);
			Close();
		}

		private void LightOptionsForm_Load(object sender, EventArgs e)
		{
			LightOptionsExtraRadioButton.Checked = Launcher.mapSettings.GetBoolean("lightoptions_extra");
			LightOptionsFastRadioButton.Checked = !LightOptionsExtraRadioButton.Checked;
			LightOptionsNoModelShadowsCheckBox.Checked = Launcher.mapSettings.GetBoolean("lightoptions_nomodelshadow");
			LightOptionsVerboseCheckBox.Checked = Launcher.mapSettings.GetBoolean("lightoptions_verbose");
			LightOptionsTracesCheckBox.Checked = Launcher.mapSettings.GetBoolean("lightoptions_traces");
			LightOptionsMaxBouncesCheckBox.Checked = Launcher.mapSettings.GetBoolean("lightoptions_maxbounces");
			LightOptionsJitterCheckBox.Checked = Launcher.mapSettings.GetBoolean("lightoptions_jitter");
			Launcher.SetNumericUpDownValue(LightOptionsTracesNumericUpDown, Launcher.mapSettings.GetDecimal("lightoptions_traces_val"));
			Launcher.SetNumericUpDownValue(LightOptionsMaxBouncesNumericUpDown, Launcher.mapSettings.GetDecimal("lightoptions_maxbounces_val"));
			Launcher.SetNumericUpDownValue(LightOptionsJitterNumericUpDown, Launcher.mapSettings.GetDecimal("lightoptions_jitter_val"));
			LightOptionsFormUpdate();
		}

		private void LightOptionsFormUpdate()
		{
			LightOptionsTracesNumericUpDown.Enabled = LightOptionsTracesCheckBox.Checked;
			LightOptionsMaxBouncesNumericUpDown.Enabled = LightOptionsMaxBouncesCheckBox.Checked;
			LightOptionsJitterNumericUpDown.Enabled = LightOptionsJitterCheckBox.Checked;
		}

		private void LightOptionsJitterCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			LightOptionsFormUpdate();
		}

		private void LightOptionsMaxBouncesCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			LightOptionsFormUpdate();
		}

		private void LightOptionsTracesCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			LightOptionsFormUpdate();
		}
	}
}
