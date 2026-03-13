using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;

namespace Form1
{
	public class CreateMapForm : MetroForm
	{
		private IContainer components;

		private Button MapCreateButtonCancel;

		private Button MapCreateButtonOK;

		private GroupBox MapNameGroupBox;

		private TextBox MapNameTextBox;

		private GroupBox MapTemplatesGroupBox;

		private ButtonX LauncherMapCreateFolder;

		private ListBox MapTemplatesListBox;

		public CreateMapForm()
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
			this.MapTemplatesGroupBox = new System.Windows.Forms.GroupBox();
			this.MapTemplatesListBox = new System.Windows.Forms.ListBox();
			this.MapNameGroupBox = new System.Windows.Forms.GroupBox();
			this.MapNameTextBox = new System.Windows.Forms.TextBox();
			this.MapCreateButtonOK = new System.Windows.Forms.Button();
			this.MapCreateButtonCancel = new System.Windows.Forms.Button();
			this.LauncherMapCreateFolder = new DevComponents.DotNetBar.ButtonX();
			this.MapTemplatesGroupBox.SuspendLayout();
			this.MapNameGroupBox.SuspendLayout();
			base.SuspendLayout();
			this.MapTemplatesGroupBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.MapTemplatesGroupBox.BackColor = System.Drawing.Color.White;
			this.MapTemplatesGroupBox.Controls.Add(this.MapTemplatesListBox);
			this.MapTemplatesGroupBox.ForeColor = System.Drawing.Color.Black;
			this.MapTemplatesGroupBox.Location = new System.Drawing.Point(12, 12);
			this.MapTemplatesGroupBox.Name = "MapTemplatesGroupBox";
			this.MapTemplatesGroupBox.Size = new System.Drawing.Size(132, 145);
			this.MapTemplatesGroupBox.TabIndex = 0;
			this.MapTemplatesGroupBox.TabStop = false;
			this.MapTemplatesGroupBox.Text = "Map Templates";
			this.MapTemplatesListBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.MapTemplatesListBox.BackColor = System.Drawing.Color.White;
			this.MapTemplatesListBox.ForeColor = System.Drawing.Color.Black;
			this.MapTemplatesListBox.FormattingEnabled = true;
			this.MapTemplatesListBox.Location = new System.Drawing.Point(6, 19);
			this.MapTemplatesListBox.Name = "MapTemplatesListBox";
			this.MapTemplatesListBox.Size = new System.Drawing.Size(120, 121);
			this.MapTemplatesListBox.TabIndex = 0;
			this.MapTemplatesListBox.SelectedIndexChanged += new System.EventHandler(MapTemplatesListBox_SelectedIndexChanged);
			this.MapNameGroupBox.BackColor = System.Drawing.Color.White;
			this.MapNameGroupBox.Controls.Add(this.MapNameTextBox);
			this.MapNameGroupBox.ForeColor = System.Drawing.Color.Black;
			this.MapNameGroupBox.Location = new System.Drawing.Point(150, 12);
			this.MapNameGroupBox.Name = "MapNameGroupBox";
			this.MapNameGroupBox.Size = new System.Drawing.Size(260, 49);
			this.MapNameGroupBox.TabIndex = 1;
			this.MapNameGroupBox.TabStop = false;
			this.MapNameGroupBox.Text = "Map Name";
			this.MapNameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.MapNameTextBox.BackColor = System.Drawing.Color.White;
			this.MapNameTextBox.ForeColor = System.Drawing.Color.Black;
			this.MapNameTextBox.Location = new System.Drawing.Point(6, 19);
			this.MapNameTextBox.MaxLength = 15;
			this.MapNameTextBox.Name = "MapNameTextBox";
			this.MapNameTextBox.Size = new System.Drawing.Size(248, 20);
			this.MapNameTextBox.TabIndex = 0;
			this.MapCreateButtonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.MapCreateButtonOK.BackColor = System.Drawing.Color.White;
			this.MapCreateButtonOK.Enabled = false;
			this.MapCreateButtonOK.ForeColor = System.Drawing.Color.Black;
			this.MapCreateButtonOK.Location = new System.Drawing.Point(248, 134);
			this.MapCreateButtonOK.Name = "MapCreateButtonOK";
			this.MapCreateButtonOK.Size = new System.Drawing.Size(75, 23);
			this.MapCreateButtonOK.TabIndex = 2;
			this.MapCreateButtonOK.Text = "OK";
			this.MapCreateButtonOK.UseVisualStyleBackColor = false;
			this.MapCreateButtonOK.Click += new System.EventHandler(MapCreateButtonOK_Click);
			this.MapCreateButtonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.MapCreateButtonCancel.BackColor = System.Drawing.Color.White;
			this.MapCreateButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.MapCreateButtonCancel.ForeColor = System.Drawing.Color.Black;
			this.MapCreateButtonCancel.Location = new System.Drawing.Point(329, 134);
			this.MapCreateButtonCancel.Name = "MapCreateButtonCancel";
			this.MapCreateButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.MapCreateButtonCancel.TabIndex = 3;
			this.MapCreateButtonCancel.Text = "Cancel";
			this.MapCreateButtonCancel.UseVisualStyleBackColor = false;
			this.LauncherMapCreateFolder.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.LauncherMapCreateFolder.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
			this.LauncherMapCreateFolder.Location = new System.Drawing.Point(210, 77);
			this.LauncherMapCreateFolder.Name = "LauncherMapCreateFolder";
			this.LauncherMapCreateFolder.Size = new System.Drawing.Size(143, 39);
			this.LauncherMapCreateFolder.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.LauncherMapCreateFolder.TabIndex = 27;
			this.LauncherMapCreateFolder.Text = "Create Map Name Folder in Mods";
			this.LauncherMapCreateFolder.Click += new System.EventHandler(LauncherMapCreateFolder_Click);
			base.AcceptButton = this.MapCreateButtonOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.MapCreateButtonCancel;
			base.ClientSize = new System.Drawing.Size(416, 169);
			base.Controls.Add(this.LauncherMapCreateFolder);
			base.Controls.Add(this.MapCreateButtonCancel);
			base.Controls.Add(this.MapCreateButtonOK);
			base.Controls.Add(this.MapNameGroupBox);
			base.Controls.Add(this.MapTemplatesGroupBox);
			this.DoubleBuffered = true;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CreateMapForm";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Create a New Map";
			base.Load += new System.EventHandler(LauncherCreateMapForm_Load);
			this.MapTemplatesGroupBox.ResumeLayout(false);
			this.MapNameGroupBox.ResumeLayout(false);
			this.MapNameGroupBox.PerformLayout();
			base.ResumeLayout(false);
		}

		private void LauncherCreateMapForm_Load(object sender, EventArgs e)
		{
			MapTemplatesListBox.Items.Clear();
			MapTemplatesListBox.Items.AddRange(Launcher.GetMapTemplatesList());
			MapTemplatesListBox.SelectedIndex = 0;
		}

		private void MapCreateButtonOK_Click(object sender, EventArgs e)
		{
			string mapTemplate = MapTemplatesListBox.Items[MapTemplatesListBox.SelectedIndex].ToString();
			string text = Launcher.FilterMP(MapNameTextBox.Text);
			if (text.Length <= 3)
			{
				Launcher.WriteError("ERROR: Give your map a proper name.\n");
				return;
			}
			bool flag = true;
			string[] array = Launcher.CreateMapFromTemplate(mapTemplate, text, justCheckForOverwrite: true);
			if (array.Length > 0 && DialogResult.No == MessageBox.Show("Certain files would be overwritten:\n\n" + Launcher.StringArrayToString(array) + "\nDo you want to continue?", "Should overwrite files?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
			{
				flag = false;
			}
			if (flag)
			{
				Launcher.CreateMapFromTemplate(mapTemplate, text);
			}
			base.DialogResult = DialogResult.OK;
			Close();
		}

		private void MapTemplatesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			string text = "";
			string text2 = MapTemplatesListBox.Text;
			int selectedIndex = MapTemplatesListBox.SelectedIndex;
			MapCreateButtonOK.Enabled = selectedIndex >= 0;
			if (selectedIndex >= 0)
			{
				if (text2 == "Multiplayer")
				{
					text = "mp_";
				}
				else if (text2 == "Singleplayer")
				{
					text = "";
				}
				else if (text2.Contains("Zombies"))
				{
					text = "zm_";
				}
				MapNameTextBox.Text = text;
			}
		}

		private void LauncherMapCreateFolder_Click(object sender, EventArgs e)
		{
			string path = Path.Combine(Launcher.GetRootDirectory(), Path.Combine("mods", MapNameTextBox.Text));
			if (Directory.Exists(path) || MapNameTextBox.Text.Length <= 3)
			{
				MessageBoxEx.Show("Folder already exists or map name is too short (>3 characters)!", "Create Mod Folder");
				return;
			}
			Directory.CreateDirectory(path);
			if (Directory.Exists(path))
			{
				MessageBoxEx.Show("Folder was created succesfully!", "Create Mod Folder");
			}
		}
	}
}
