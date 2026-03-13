using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Metro.ColorTables;
using Properties;

namespace Form1
{
	public class LauncherForm : MetroForm
	{
		public delegate void ProcessFinishedDelegate(Process lastProcess);

		private IContainer components;

		private Mutex consoleMutex = new Mutex();

		private Process consoleProcess;

		private DateTime consoleProcessStartTime;

		private long consoleTicksWhenLastFocus = DateTime.Now.Ticks;

		private ComboBox[] dvarComboBoxes = new ComboBox[0];

		private LinkLabel LauncherAboutLabel;

		private FileSystemWatcher LauncherMapFilesSystemWatcher;

		private FileSystemWatcher LauncherModsDirectorySystemWatcher;

		private System.Windows.Forms.Timer LauncherTimer;

		private LinkLabel LauncherWikiLabel;

		private string mapName;

		private string modName;

		private ArrayList processList = new ArrayList();

		private BackgroundWorker backgroundWorker1;

		private SplitContainer LauncherSplitter;

		private GroupBox LauncherApplicationsGroupBox;

		private System.Windows.Forms.TabControl LauncherTab;

		private TabPage LauncherTabCompileLevel;

		private GroupBox LauncherCompileLevelOptionsGroupBox;

		private GroupBox LauncherGridFileGroupBox;

		private CheckBox LauncherGridCollectDotsCheckBox;

		private RadioButton LauncherMapFFTypeMP;

		private RadioButton LauncherMapFFTypeSP;

		private CheckBox LauncherModSpecificMapCheckBox;

		private CheckBox LauncherBspInfoCheckBox;

		private CheckBox LauncherBuildFastFilesCheckBox;

		private CheckBox LauncherCompileReflectionsCheckBox;

		private CheckBox LauncherConnectPathsCheckBox;

		private CheckBox LauncherCompileVisCheckBox;

		private CheckBox LauncherCompileLightsCheckBox;

		private CheckBox LauncherCompileBSPCheckBox;

		private ListBox LauncherMapList;

		private TabPage LauncherTabModBuilder;

		internal GroupBox LauncherIwdFileGroupBox;

		internal GroupBox LauncherFastFileCsvGroupBox;

		internal GroupBox LauncherModGroupBox;

		internal CheckBox LauncherModBuildSoundsCheckBox;

		internal CheckBox LauncherModVerboseCheckBox;

		internal CheckBox LauncherModBuildIwdFileCheckBox;

		internal CheckBox LauncherModBuildFastFilesCheckBox;

		private TabPage LauncherTabRunGame;

		private Panel LauncherGameOptionsPanel;

		private GroupBox LauncherRunGameCustomCommandLineGroupBox;

		private TextBox LauncherRunGameCustomCommandLineTextBox;

		private GroupBox LauncherRunGameCommandLineGroupBox;

		private GroupBox LauncherRunGameModGroupBox;

		private GroupBox LauncherRunGameExeTypeGroupBox;

		private RadioButton LauncherRunGameExeTypeMpRadioButton;

		private RadioButton LauncherRunGameTypeRadioButton;

		private TextBox LauncherProcessTimeElapsedTextBox;

		private TextBox LauncherProcessTextBox;

		private GroupBox LauncherProcessGroupBox;

		private ButtonX LauncherButtonRadiant;

		private ButtonX LauncherButtonAssetViewer;

		private ButtonX LauncherButtonAssetManager;

		private ButtonX LauncherButtonEffectsEd;

		private ButtonX LauncherButtonCancel;

		private ListBoxAdv LauncherProcessList;

		private ComboBoxEx LauncherModSpecificMapComboBox;

		private ComboBoxEx LauncherRunGameModComboBox;

		private ButtonX LauncherCompileLevelButton;

		private ButtonX LauncherCopyImgsToModButton;

		private GroupBox LauncherMapFFTypeGroupBox;

		private TextBoxX LauncherRunGameCommandLineTextBox;

		private ButtonX LauncherRunGameButton;

		private ButtonX LauncherClearConsoleButton;

		private ComboBoxEx LauncherModComboBox;

		private RichTextBoxEx LauncherFastFileCsvTextBox;

		private TreeView LauncherIwdFileTree;

		private ButtonX LauncherModOpenButton;

		private ButtonX LauncherModBuildButton;

		private ButtonX LauncherModBuildLocalizedButton;

		private ButtonX LauncherCompileBSPButton;

		private ButtonX LauncherCompileLightsButton;

		private ButtonX LauncherEditMapCSVButton;

		private ButtonX LauncherCreateMapButton;

		private ButtonX LauncherDeleteMapButton;

		private ButtonX LauncherGridEditExistingButton;

		private ButtonX LauncherGridMakeNewButton;

		private LabelX LauncherFFAssetsCountLabel;

		private DataGridView LauncherAssetCountGridView;

		private ButtonX LauncherSaveConsoleButton;

		private ButtonX LauncherModRefreshButton;

		private ComboBoxEx LauncherModLanguageComboBox;

		private LabelX LauncherModLanguageLabel;

		internal GroupBox LauncherLocalizedCsvGroupBox;

		private RichTextBoxEx LauncherLocalizedCsvTextBox;

		private ButtonX LauncherAssetCountReloadButton;

		private CheckBox LauncherRunMapAfterCompileCheckBox;

		private ButtonX LauncherFFAssetsHelp_Button;

		internal RichTextBoxEx LauncherConsole;

		private DataGridViewTextBoxColumn AssetType;

		private DataGridViewTextBoxColumn AssetCount;

		private DataGridViewTextBoxColumn Percentage;

		private LabelX LauncherModAssetLabel;

		//private ButtonX LauncherButtonWeaponEditor;
        private ButtonX LauncherButtonBlender;
        private ButtonX LauncherButtonGameUtilsImage;
        private ButtonX LauncherButtonSoundTools;
        
        private Hashtable processTable = new Hashtable();

		private event ProcessFinishedDelegate processFinishedDelegate;

		public LauncherForm()
		{
			InitializeComponent();
			base.MaximizeBox = false;
		}

		private void AddFilesToTreeView(string Directory, TreeNodeCollection tree, bool firstTime)
		{
			string[] source = new string[2] { ".git", ".svn" };
			TreeNode treeNode = null;
			if (!firstTime)
			{
				treeNode = tree.Add(new DirectoryInfo(Directory).Name);
				tree = treeNode.Nodes;
			}
			DirectoryInfo[] directories = new DirectoryInfo(Directory).GetDirectories();
			foreach (DirectoryInfo directoryInfo in directories)
			{
				if (!source.Contains(directoryInfo.Name.ToLower()) || !firstTime)
				{
					AddFilesToTreeView(Path.Combine(Directory, directoryInfo.Name), tree, firstTime: false);
				}
			}
			FileInfo[] files = new DirectoryInfo(Directory).GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				if (fileInfo.Extension.ToLower() != ".ff" && fileInfo.Extension.ToLower() != ".iwd" && fileInfo.Extension.ToLower() != ".files" && fileInfo.Extension.ToLower() != ".log" && fileInfo.Extension.ToLower() != ".bak" && fileInfo.Extension.ToLower() != ".txt" && fileInfo.Extension.ToLower() != ".rar" && fileInfo.Extension.ToLower() != ".bat")
				{
					TreeNode treeNode2 = tree.Add(fileInfo.Name);
					treeNode2.ForeColor = Color.Blue;
					treeNode2.Tag = fileInfo;
				}
			}
			if (treeNode != null)
			{
				if (treeNode.Nodes.Count != 0)
				{
					treeNode.ExpandAll();
				}
				else
				{
					treeNode.Remove();
				}
			}
		}

		private void BuildGridDelegate(int r_vc_makelog)
		{
			EnableControls(enabled: false);
			string path = mapName + ".grid";
			Launcher.CopyFile(Path.Combine(Launcher.GetMapSourceDirectory(), path), Path.Combine(Launcher.GetRawMapsDirectory(), Path.Combine(IsMP() ? "mp" : "", path)));
			LaunchProcessHelper(shouldRun: true, BuildGridFinishedDelegate, null, Launcher.GetGameApplication(IsMP()), "+set developer 1 +set logfile 2 + set r_vc_makelog " + r_vc_makelog + "+set r_vc_showlog 16 +set r_cullxmodel " + (Launcher.mapSettings.GetBoolean("compile_collectdots") ? "0" : "1") + " +set thereisacow 1337 +set com_introplayed 1 +set fs_game raw +set fs_usedevdir 1 +devmap " + mapName);
		}

		private void BuildGridFinishedDelegate(Process lastProcess)
		{
			string path = mapName + ".grid";
			Launcher.MoveFile(Path.Combine(Launcher.GetRawMapsDirectory(), Path.Combine(IsMP() ? "mp" : "", path)), Path.Combine(Launcher.GetMapSourceDirectory(), path));
			EnableControls(enabled: true);
		}

		private void LauncherModSpecificMapCheckBox_CheckChanged(object sender, EventArgs e)
		{
			LauncherModSpecificMapComboBox.Enabled = LauncherModSpecificMapCheckBox.Checked;
		}

		private bool CheckZoneSourceFiles()
		{
			if (!File.Exists(Launcher.GetZoneSourceFile(mapName)))
			{
				if (MessageBox.Show("There are no zone files for " + mapName + ". Would you like to create them?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					return false;
				}
				Launcher.CreateZoneSourceFiles(mapName);
			}
			return true;
		}

		private void CompileLevel()
		{
			EnableControls(enabled: false);
			UpdateMapSettings();
			CompileLevelBspDelegate(null);
		}

		private void CompileLevelBspDelegate(Process lastProcess)
		{
			CompileLevelHelper("compile_bsp", CompileLevelVisDelegate, lastProcess, "cod2map", "-platform pc -loadFrom \"" + GetSourceBsp() + ".map\"" + Launcher.GetBspOptions() + " \"" + GetDestinationBsp() + "\"");
		}

		private void CompileLevelBspInfoDelegate(Process lastProcess)
		{
			CompileLevelHelper("compile_bspinfo", CompileLevelFastFilesDelegate, lastProcess, "cod2map", "-info \"" + GetDestinationBsp() + "\"");
		}

		private void CompileLevelBuildFastFile(string name, Process lastProcess, ProcessFinishedDelegate nextStage)
		{
			string text = (Launcher.mapSettings.GetBoolean("compile_modenabled") ? ("-moddir " + Launcher.mapSettings.GetString("compile_modname") + " ") : "");
			CompileLevelHelper("compile_buildffs", nextStage, lastProcess, "linker_pc", "-nopause -language " + Launcher.GetLanguage() + " " + text + name + (File.Exists(Launcher.GetLoadZone(mapName)) ? (" " + Launcher.GetLoadZone(mapName)) : ""));
		}

		private void CompileLevelCleanupDelegate(Process lastProcess)
		{
			Launcher.CopyFileSmart(GetDestinationBsp() + ".lin", GetSourceBsp() + ".lin");
			string[] array = new string[5] { ".lin", ".map", ".d3dpoly", ".vclog", ".grid" };
			foreach (string text in array)
			{
				Launcher.DeleteFile(GetDestinationBsp() + text, verbose: false);
			}
			CompileLevelPathsDelegate(lastProcess);
		}

		private void CompileLevelFastFilesDelegate(Process lastProcess)
		{
			if (CheckZoneSourceFiles())
			{
				if (IsMP())
				{
					CompileLevelBuildFastFile(mapName, lastProcess, CompileLevelFastFilesLocalizedDelegate);
				}
				else
				{
					CompileLevelBuildFastFile(mapName, lastProcess, CompileLevelMoveFastFilesDelegate);
				}
			}
			else
			{
				CompileLevelRunGameDelegate(lastProcess);
			}
		}

		private void CompileLevelFastFilesLocalizedDelegate(Process lastProcess)
		{
			CompileLevelBuildFastFile("localized_" + mapName, lastProcess, CompileLevelMoveFastFilesDelegate);
		}

		private void CompileLevelFinished(Process lastProcess)
		{
			EnableControls(enabled: true);
			ReadMapAssetListFile(mapName);
		}

		private void CompileLevelHelper(string mapSettingsOption, ProcessFinishedDelegate nextStage, Process lastProcess, string processName, string processOptions)
		{
			LaunchProcessHelper(Launcher.mapSettings.GetBoolean(mapSettingsOption), nextStage, lastProcess, processName, processOptions);
		}

		private void CompileLevelHelper(string mapSettingsOption, ProcessFinishedDelegate nextStage, Process lastProcess, string processName, string processOptions, string workingDirectory)
		{
			LaunchProcessHelper(Launcher.mapSettings.GetBoolean(mapSettingsOption), nextStage, lastProcess, processName, processOptions, workingDirectory);
		}

		private void CompileLevelLightsDelegate(Process lastProcess)
		{
			CompileLevelHelper("compile_lights", CompileLevelCleanupDelegate, lastProcess, "cod2rad", "-platform pc " + Launcher.GetLightOptions() + " \"" + GetDestinationBsp() + "\"");
		}

		private void CompileLevelMoveFastFilesDelegate(Process lastProcess)
		{
			string zoneDirectory = Launcher.GetZoneDirectory();
			string path = (Launcher.mapSettings.GetBoolean("compile_modenabled") ? Launcher.GetModDirectory(Launcher.mapSettings.GetString("compile_modname")) : Path.Combine(Launcher.GetUsermapsDirectory(), mapName));
			string text = mapName + ".ff";
			string path2 = mapName + "_load.ff";
			Launcher.MoveFile(Path.Combine(zoneDirectory, text), Path.Combine(path, text));
			Launcher.MoveFile(Path.Combine(zoneDirectory, "localized_" + text), Path.Combine(path, "localized_" + text));
			Launcher.MoveFile(Path.Combine(zoneDirectory, path2), Path.Combine(path, path2));
			Launcher.Publish();
			CompileLevelRunGameDelegate(lastProcess);
		}

		private void CompileLevelPathsDelegate(Process lastProcess)
		{
			CompileLevelHelper("compile_paths", CompileLevelReflectionsDelegate, lastProcess, Launcher.GetGameTool(IsMP()), "allowdupe +set developer 1 +set logfile 2 +set thereisacow 1337 +set com_introplayed 1 +set r_fullscreen 0 +set fs_usedevdir 1 +set g_connectpaths 2 +set useFastFile 0 +devmap " + mapName);
		}

		private void CompileLevelReflectionsDelegate(Process lastProcess)
		{
			CompileLevelHelper("compile_reflections", CompileLevelBspInfoDelegate, lastProcess, Launcher.GetGameTool(IsMP()), "allowdupe +set developer 1 +set logfile 2 +set thereisacow 1337 +set com_introplayed 1 +set r_fullscreen 0 +set fs_usedevdir 1 +set ui_autoContinue 1 +set r_reflectionProbeGenerateExit 1 +set sys_smp_allowed 0 +set useFastFile 0 +set r_fullscreen 0 +set com_hunkMegs 512 +set r_reflectionProbeRegenerateAll 1 +set r_zFeather 1 +set r_smp_backend_allowed 1 +set r_reflectionProbeGenerate 1 +devmap " + mapName);
		}

		private void CompileLevelRunGameDelegate(Process lastProcess)
		{
			string text = (Launcher.mapSettings.GetBoolean("compile_modenabled") ? ("mods/" + Launcher.mapSettings.GetString("compile_modname")) : "raw");
			CompileLevelHelper("compile_runafter", CompileLevelFinished, lastProcess, Launcher.GetGameApplication(IsMP()), "+set useFastFile 1 +set fs_usedevdir 1 +set logfile 2 +set thereisacow 1337 +set com_introplayed 1 " + (IsMP() ? "+set sv_pure 0 +set g_gametype tdm " : "") + "+devmap " + mapName + " +set fs_game " + text + " ");
		}

		private void CompileLevelVisDelegate(Process lastProcess)
		{
			CompileLevelHelper("compile_vis", CompileLevelLightsDelegate, lastProcess, "cod2map", "-vis -platform pc \"" + GetDestinationBsp() + "\"");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void EnableControls(bool enabled)
		{
			EnableControls(enabled, null);
		}

		private void EnableControls(bool enabled, TabPage onlyForTabPage)
		{
			TabPage[] array = new TabPage[3] { LauncherTabCompileLevel, LauncherTabModBuilder, LauncherTabRunGame };
			foreach (TabPage tabPage in array)
			{
				if (onlyForTabPage != null && onlyForTabPage != tabPage)
				{
					continue;
				}
				foreach (Control control in tabPage.Controls)
				{
					control.Enabled = enabled;
				}
			}
			if (enabled)
			{
				LauncherModSpecificMapComboBox.Enabled = LauncherModSpecificMapCheckBox.Checked;
			}
		}

		private void EnableMapList()
		{
			bool enabled = LauncherMapList.SelectedItem != null;
			LauncherCompileLevelButton.Enabled = enabled;
			EnableControls(enabled, LauncherTabCompileLevel);
			LauncherMapList.Enabled = true;
			LauncherCreateMapButton.Enabled = true;
		}

		private string FormatDVar(ComboBox cb)
		{
			string text = "";
			if (cb.SelectedItem != null && cb.SelectedIndex > 0)
			{
				text = cb.SelectedItem.ToString();
			}
			else if (cb.Items[0].ToString() != cb.Text)
			{
				text = cb.Text;
			}
			text = text.Trim();
			if (!(text != ""))
			{
				return "";
			}
			if (cb.Tag.ToString() == "devmap")
			{
				return string.Concat("+", cb.Tag, " ", text, " ");
			}
			return string.Concat("+set ", cb.Tag, " ", text, " ");
		}

		private string FormatDvars()
		{
			StringBuilder stringBuilder = new StringBuilder();
			ComboBox[] array = dvarComboBoxes;
			foreach (ComboBox cb in array)
			{
				stringBuilder.Append(FormatDVar(cb));
			}
			return stringBuilder.ToString();
		}

		private string GetDestinationBsp()
		{
			return Launcher.GetRawMapsDirectory() + (IsMP() ? "mp\\" : "") + mapName;
		}

		private string GetGameOptions()
		{
			string text = "";
			return text + "+set fs_game " + ((LauncherRunGameModComboBox.SelectedIndex > 0) ? ("mods/" + LauncherRunGameModComboBox.Text) : "raw") + " " + FormatDvars() + " " + LauncherRunGameCustomCommandLineTextBox.Text + " ";
		}

		private string GetSourceBsp()
		{
			return Launcher.GetMapSourceDirectory() + mapName;
		}

		private void HashTableToTreeView(Hashtable ht, TreeNodeCollection tree)
		{
			if (tree == null)
			{
				return;
			}
			foreach (TreeNode item in tree)
			{
				if (ht.Contains(item.FullPath))
				{
					bool checkedFlag = (item.Checked = true);
					RecursiveCheckNodesUp(item, checkedFlag);
				}
				HashTableToTreeView(ht, item.Nodes);
			}
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherForm));
            this.LauncherTimer = new System.Windows.Forms.Timer(this.components);
            this.LauncherWikiLabel = new System.Windows.Forms.LinkLabel();
            this.LauncherAboutLabel = new System.Windows.Forms.LinkLabel();
            this.LauncherMapFilesSystemWatcher = new System.IO.FileSystemWatcher();
            this.LauncherModsDirectorySystemWatcher = new System.IO.FileSystemWatcher();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.LauncherProcessGroupBox = new System.Windows.Forms.GroupBox();
            this.LauncherProcessList = new DevComponents.DotNetBar.ListBoxAdv();
            this.LauncherButtonCancel = new DevComponents.DotNetBar.ButtonX();
            this.LauncherProcessTextBox = new System.Windows.Forms.TextBox();
            this.LauncherProcessTimeElapsedTextBox = new System.Windows.Forms.TextBox();
            this.LauncherTab = new System.Windows.Forms.TabControl();
            this.LauncherTabCompileLevel = new System.Windows.Forms.TabPage();
            this.LauncherCreateMapButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherDeleteMapButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherCompileLevelOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.LauncherFFAssetsHelp_Button = new DevComponents.DotNetBar.ButtonX();
            this.LauncherAssetCountReloadButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherAssetCountGridView = new System.Windows.Forms.DataGridView();
            this.AssetType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AssetCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Percentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LauncherFFAssetsCountLabel = new DevComponents.DotNetBar.LabelX();
            this.LauncherEditMapCSVButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherCompileBSPButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherCompileLightsButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherCopyImgsToModButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherCompileLevelButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherModSpecificMapComboBox = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.LauncherGridFileGroupBox = new System.Windows.Forms.GroupBox();
            this.LauncherGridEditExistingButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherGridMakeNewButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherGridCollectDotsCheckBox = new System.Windows.Forms.CheckBox();
            this.LauncherMapFFTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.LauncherMapFFTypeMP = new System.Windows.Forms.RadioButton();
            this.LauncherMapFFTypeSP = new System.Windows.Forms.RadioButton();
            this.LauncherModSpecificMapCheckBox = new System.Windows.Forms.CheckBox();
            this.LauncherRunMapAfterCompileCheckBox = new System.Windows.Forms.CheckBox();
            this.LauncherBspInfoCheckBox = new System.Windows.Forms.CheckBox();
            this.LauncherBuildFastFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.LauncherCompileReflectionsCheckBox = new System.Windows.Forms.CheckBox();
            this.LauncherConnectPathsCheckBox = new System.Windows.Forms.CheckBox();
            this.LauncherCompileVisCheckBox = new System.Windows.Forms.CheckBox();
            this.LauncherCompileLightsCheckBox = new System.Windows.Forms.CheckBox();
            this.LauncherCompileBSPCheckBox = new System.Windows.Forms.CheckBox();
            this.LauncherMapList = new System.Windows.Forms.ListBox();
            this.LauncherTabModBuilder = new System.Windows.Forms.TabPage();
            this.LauncherIwdFileGroupBox = new System.Windows.Forms.GroupBox();
            this.LauncherIwdFileTree = new System.Windows.Forms.TreeView();
            this.LauncherLocalizedCsvGroupBox = new System.Windows.Forms.GroupBox();
            this.LauncherLocalizedCsvTextBox = new DevComponents.DotNetBar.Controls.RichTextBoxEx();
            this.LauncherFastFileCsvGroupBox = new System.Windows.Forms.GroupBox();
            this.LauncherFastFileCsvTextBox = new DevComponents.DotNetBar.Controls.RichTextBoxEx();
            this.LauncherModGroupBox = new System.Windows.Forms.GroupBox();
            this.LauncherModLanguageLabel = new DevComponents.DotNetBar.LabelX();
            this.LauncherModLanguageComboBox = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.LauncherModBuildLocalizedButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherModRefreshButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherModBuildButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherModOpenButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherModComboBox = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.LauncherModBuildSoundsCheckBox = new System.Windows.Forms.CheckBox();
            this.LauncherModVerboseCheckBox = new System.Windows.Forms.CheckBox();
            this.LauncherModBuildIwdFileCheckBox = new System.Windows.Forms.CheckBox();
            this.LauncherModBuildFastFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.LauncherModAssetLabel = new DevComponents.DotNetBar.LabelX();
            this.LauncherTabRunGame = new System.Windows.Forms.TabPage();
            this.LauncherRunGameButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherGameOptionsPanel = new System.Windows.Forms.Panel();
            this.LauncherRunGameCustomCommandLineGroupBox = new System.Windows.Forms.GroupBox();
            this.LauncherRunGameCustomCommandLineTextBox = new System.Windows.Forms.TextBox();
            this.LauncherRunGameCommandLineGroupBox = new System.Windows.Forms.GroupBox();
            this.LauncherRunGameCommandLineTextBox = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.LauncherRunGameModGroupBox = new System.Windows.Forms.GroupBox();
            this.LauncherRunGameModComboBox = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.LauncherRunGameExeTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.LauncherRunGameExeTypeMpRadioButton = new System.Windows.Forms.RadioButton();
            this.LauncherRunGameTypeRadioButton = new System.Windows.Forms.RadioButton();
            this.LauncherApplicationsGroupBox = new System.Windows.Forms.GroupBox();
            this.LauncherClearConsoleButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherButtonBlender = new DevComponents.DotNetBar.ButtonX();
            this.LauncherButtonGameUtilsImage = new DevComponents.DotNetBar.ButtonX();
            this.LauncherButtonSoundTools = new DevComponents.DotNetBar.ButtonX();
            this.LauncherButtonAssetViewer = new DevComponents.DotNetBar.ButtonX();
            this.LauncherButtonAssetManager = new DevComponents.DotNetBar.ButtonX();
            this.LauncherButtonEffectsEd = new DevComponents.DotNetBar.ButtonX();
            this.LauncherButtonRadiant = new DevComponents.DotNetBar.ButtonX();
            this.LauncherSplitter = new System.Windows.Forms.SplitContainer();
            this.LauncherSaveConsoleButton = new DevComponents.DotNetBar.ButtonX();
            this.LauncherConsole = new DevComponents.DotNetBar.Controls.RichTextBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.LauncherMapFilesSystemWatcher)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LauncherModsDirectorySystemWatcher)).BeginInit();
            this.LauncherProcessGroupBox.SuspendLayout();
            this.LauncherTab.SuspendLayout();
            this.LauncherTabCompileLevel.SuspendLayout();
            this.LauncherCompileLevelOptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LauncherAssetCountGridView)).BeginInit();
            this.LauncherGridFileGroupBox.SuspendLayout();
            this.LauncherMapFFTypeGroupBox.SuspendLayout();
            this.LauncherTabModBuilder.SuspendLayout();
            this.LauncherIwdFileGroupBox.SuspendLayout();
            this.LauncherLocalizedCsvGroupBox.SuspendLayout();
            this.LauncherFastFileCsvGroupBox.SuspendLayout();
            this.LauncherModGroupBox.SuspendLayout();
            this.LauncherTabRunGame.SuspendLayout();
            this.LauncherRunGameCustomCommandLineGroupBox.SuspendLayout();
            this.LauncherRunGameCommandLineGroupBox.SuspendLayout();
            this.LauncherRunGameModGroupBox.SuspendLayout();
            this.LauncherRunGameExeTypeGroupBox.SuspendLayout();
            this.LauncherApplicationsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LauncherSplitter)).BeginInit();
            this.LauncherSplitter.Panel1.SuspendLayout();
            this.LauncherSplitter.Panel2.SuspendLayout();
            this.LauncherSplitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // LauncherTimer
            // 
            this.LauncherTimer.Enabled = true;
            this.LauncherTimer.Interval = 1000;
            this.LauncherTimer.Tick += new System.EventHandler(this.LauncherTimer_Tick);
            // 
            // LauncherWikiLabel
            // 
            this.LauncherWikiLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LauncherWikiLabel.AutoSize = true;
            this.LauncherWikiLabel.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LauncherWikiLabel.Location = new System.Drawing.Point(839, 4);
            this.LauncherWikiLabel.Name = "LauncherWikiLabel";
            this.LauncherWikiLabel.Size = new System.Drawing.Size(44, 17);
            this.LauncherWikiLabel.TabIndex = 6;
            this.LauncherWikiLabel.TabStop = true;
            this.LauncherWikiLabel.Text = "WIKI";
            this.LauncherWikiLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LauncherWikiLabel_LinkClicked);
            // 
            // LauncherAboutLabel
            // 
            this.LauncherAboutLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LauncherAboutLabel.AutoSize = true;
            this.LauncherAboutLabel.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LauncherAboutLabel.Location = new System.Drawing.Point(776, 4);
            this.LauncherAboutLabel.Name = "LauncherAboutLabel";
            this.LauncherAboutLabel.Size = new System.Drawing.Size(53, 17);
            this.LauncherAboutLabel.TabIndex = 7;
            this.LauncherAboutLabel.TabStop = true;
            this.LauncherAboutLabel.Text = "ABOUT";
            this.LauncherAboutLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LauncherAboutLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LauncherAboutLabel_LinkClicked);
            // 
            // LauncherMapFilesSystemWatcher
            // 
            this.LauncherMapFilesSystemWatcher.EnableRaisingEvents = true;
            this.LauncherMapFilesSystemWatcher.Filter = "*.map";
            this.LauncherMapFilesSystemWatcher.NotifyFilter = System.IO.NotifyFilters.FileName;
            this.LauncherMapFilesSystemWatcher.SynchronizingObject = this;
            this.LauncherMapFilesSystemWatcher.Changed += new System.IO.FileSystemEventHandler(this.LauncherMapFilesSystemWatcher_Changed);
            this.LauncherMapFilesSystemWatcher.Created += new System.IO.FileSystemEventHandler(this.LauncherMapFilesSystemWatcher_Created);
            this.LauncherMapFilesSystemWatcher.Deleted += new System.IO.FileSystemEventHandler(this.LauncherMapFilesSystemWatcher_Deleted);
            this.LauncherMapFilesSystemWatcher.Renamed += new System.IO.RenamedEventHandler(this.LauncherMapFilesSystemWatcher_Renamed);
            // 
            // LauncherModsDirectorySystemWatcher
            // 
            this.LauncherModsDirectorySystemWatcher.EnableRaisingEvents = true;
            this.LauncherModsDirectorySystemWatcher.NotifyFilter = System.IO.NotifyFilters.DirectoryName;
            this.LauncherModsDirectorySystemWatcher.SynchronizingObject = this;
            this.LauncherModsDirectorySystemWatcher.Changed += new System.IO.FileSystemEventHandler(this.LauncherModsDirectorySystemWatcher_Changed);
            this.LauncherModsDirectorySystemWatcher.Created += new System.IO.FileSystemEventHandler(this.LauncherModsDirectorySystemWatcher_Created);
            this.LauncherModsDirectorySystemWatcher.Deleted += new System.IO.FileSystemEventHandler(this.LauncherModsDirectorySystemWatcher_Deleted);
            this.LauncherModsDirectorySystemWatcher.Renamed += new System.IO.RenamedEventHandler(this.LauncherModsDirectorySystemWatcher_Renamed);
            // 
            // LauncherProcessGroupBox
            // 
            this.LauncherProcessGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LauncherProcessGroupBox.Controls.Add(this.LauncherProcessList);
            this.LauncherProcessGroupBox.Controls.Add(this.LauncherButtonCancel);
            this.LauncherProcessGroupBox.Location = new System.Drawing.Point(3, 3);
            this.LauncherProcessGroupBox.Name = "LauncherProcessGroupBox";
            this.LauncherProcessGroupBox.Size = new System.Drawing.Size(139, 303);
            this.LauncherProcessGroupBox.TabIndex = 2;
            this.LauncherProcessGroupBox.TabStop = false;
            this.LauncherProcessGroupBox.Text = "Processes";
            // 
            // LauncherProcessList
            // 
            this.LauncherProcessList.AutoScroll = true;
            this.LauncherProcessList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            // 
            // 
            // 
            this.LauncherProcessList.BackgroundStyle.Class = "ListBoxAdv";
            this.LauncherProcessList.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LauncherProcessList.ContainerControlProcessDialogKey = true;
            this.LauncherProcessList.DragDropSupport = true;
            this.LauncherProcessList.ItemHeight = 0;
            this.LauncherProcessList.ItemSpacing = 2;
            this.LauncherProcessList.Location = new System.Drawing.Point(6, 19);
            this.LauncherProcessList.Name = "LauncherProcessList";
            this.LauncherProcessList.ShowToolTips = false;
            this.LauncherProcessList.Size = new System.Drawing.Size(128, 154);
            this.LauncherProcessList.TabIndex = 5;
            this.LauncherProcessList.Text = "listBoxAdv1";
            this.LauncherProcessList.ItemClick += new System.EventHandler(this.LauncherProcessList_ItemClick);
            // 
            // LauncherButtonCancel
            // 
            this.LauncherButtonCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherButtonCancel.BackColor = System.Drawing.Color.LightCoral;
            this.LauncherButtonCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherButtonCancel.Location = new System.Drawing.Point(7, 203);
            this.LauncherButtonCancel.Name = "LauncherButtonCancel";
            this.LauncherButtonCancel.Size = new System.Drawing.Size(128, 65);
            this.LauncherButtonCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2013;
            this.LauncherButtonCancel.TabIndex = 0;
            this.LauncherButtonCancel.Text = "Cancel";
            this.LauncherButtonCancel.Click += new System.EventHandler(this.LauncherButtonCancel_Click);
            // 
            // LauncherProcessTextBox
            // 
            this.LauncherProcessTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LauncherProcessTextBox.Location = new System.Drawing.Point(149, 284);
            this.LauncherProcessTextBox.Name = "LauncherProcessTextBox";
            this.LauncherProcessTextBox.ReadOnly = true;
            this.LauncherProcessTextBox.ShortcutsEnabled = false;
            this.LauncherProcessTextBox.Size = new System.Drawing.Size(650, 22);
            this.LauncherProcessTextBox.TabIndex = 3;
            // 
            // LauncherProcessTimeElapsedTextBox
            // 
            this.LauncherProcessTimeElapsedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LauncherProcessTimeElapsedTextBox.Location = new System.Drawing.Point(810, 284);
            this.LauncherProcessTimeElapsedTextBox.Name = "LauncherProcessTimeElapsedTextBox";
            this.LauncherProcessTimeElapsedTextBox.ReadOnly = true;
            this.LauncherProcessTimeElapsedTextBox.Size = new System.Drawing.Size(64, 22);
            this.LauncherProcessTimeElapsedTextBox.TabIndex = 4;
            // 
            // LauncherTab
            // 
            this.LauncherTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LauncherTab.Controls.Add(this.LauncherTabCompileLevel);
            this.LauncherTab.Controls.Add(this.LauncherTabModBuilder);
            this.LauncherTab.Controls.Add(this.LauncherTabRunGame);
            this.LauncherTab.Location = new System.Drawing.Point(149, 1);
            this.LauncherTab.Name = "LauncherTab";
            this.LauncherTab.Padding = new System.Drawing.Point(0, 0);
            this.LauncherTab.SelectedIndex = 0;
            this.LauncherTab.Size = new System.Drawing.Size(771, 376);
            this.LauncherTab.TabIndex = 0;
            // 
            // LauncherTabCompileLevel
            // 
            this.LauncherTabCompileLevel.BackColor = System.Drawing.Color.Transparent;
            this.LauncherTabCompileLevel.Controls.Add(this.LauncherCreateMapButton);
            this.LauncherTabCompileLevel.Controls.Add(this.LauncherDeleteMapButton);
            this.LauncherTabCompileLevel.Controls.Add(this.LauncherCompileLevelOptionsGroupBox);
            this.LauncherTabCompileLevel.Controls.Add(this.LauncherMapList);
            this.LauncherTabCompileLevel.Location = new System.Drawing.Point(4, 25);
            this.LauncherTabCompileLevel.Name = "LauncherTabCompileLevel";
            this.LauncherTabCompileLevel.Padding = new System.Windows.Forms.Padding(3);
            this.LauncherTabCompileLevel.Size = new System.Drawing.Size(763, 347);
            this.LauncherTabCompileLevel.TabIndex = 0;
            this.LauncherTabCompileLevel.Text = "Compile FF";
            // 
            // LauncherCreateMapButton
            // 
            this.LauncherCreateMapButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherCreateMapButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherCreateMapButton.Location = new System.Drawing.Point(138, 306);
            this.LauncherCreateMapButton.Name = "LauncherCreateMapButton";
            this.LauncherCreateMapButton.Size = new System.Drawing.Size(72, 32);
            this.LauncherCreateMapButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherCreateMapButton.TabIndex = 6;
            this.LauncherCreateMapButton.Text = "Create Map";
            this.LauncherCreateMapButton.Click += new System.EventHandler(this.LauncherCreateMapButton_Click);
            // 
            // LauncherDeleteMapButton
            // 
            this.LauncherDeleteMapButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherDeleteMapButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherDeleteMapButton.Location = new System.Drawing.Point(6, 306);
            this.LauncherDeleteMapButton.Name = "LauncherDeleteMapButton";
            this.LauncherDeleteMapButton.Size = new System.Drawing.Size(72, 32);
            this.LauncherDeleteMapButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherDeleteMapButton.TabIndex = 5;
            this.LauncherDeleteMapButton.Text = "Delete Map";
            this.LauncherDeleteMapButton.Click += new System.EventHandler(this.LauncherDeleteMapButton_Click);
            // 
            // LauncherCompileLevelOptionsGroupBox
            // 
            this.LauncherCompileLevelOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LauncherCompileLevelOptionsGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherFFAssetsHelp_Button);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherAssetCountReloadButton);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherAssetCountGridView);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherFFAssetsCountLabel);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherEditMapCSVButton);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherCompileBSPButton);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherCompileLightsButton);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherCopyImgsToModButton);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherCompileLevelButton);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherModSpecificMapComboBox);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherGridFileGroupBox);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherMapFFTypeGroupBox);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherModSpecificMapCheckBox);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherRunMapAfterCompileCheckBox);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherBspInfoCheckBox);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherBuildFastFilesCheckBox);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherCompileReflectionsCheckBox);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherConnectPathsCheckBox);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherCompileVisCheckBox);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherCompileLightsCheckBox);
            this.LauncherCompileLevelOptionsGroupBox.Controls.Add(this.LauncherCompileBSPCheckBox);
            this.LauncherCompileLevelOptionsGroupBox.Location = new System.Drawing.Point(216, 6);
            this.LauncherCompileLevelOptionsGroupBox.MinimumSize = new System.Drawing.Size(364, 332);
            this.LauncherCompileLevelOptionsGroupBox.Name = "LauncherCompileLevelOptionsGroupBox";
            this.LauncherCompileLevelOptionsGroupBox.Size = new System.Drawing.Size(544, 332);
            this.LauncherCompileLevelOptionsGroupBox.TabIndex = 3;
            this.LauncherCompileLevelOptionsGroupBox.TabStop = false;
            this.LauncherCompileLevelOptionsGroupBox.Text = "Compile FF Options";
            // 
            // LauncherFFAssetsHelp_Button
            // 
            this.LauncherFFAssetsHelp_Button.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherFFAssetsHelp_Button.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherFFAssetsHelp_Button.Location = new System.Drawing.Point(442, 87);
            this.LauncherFFAssetsHelp_Button.Name = "LauncherFFAssetsHelp_Button";
            this.LauncherFFAssetsHelp_Button.Size = new System.Drawing.Size(50, 18);
            this.LauncherFFAssetsHelp_Button.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherFFAssetsHelp_Button.TabIndex = 35;
            this.LauncherFFAssetsHelp_Button.Text = "?";
            this.LauncherFFAssetsHelp_Button.Click += new System.EventHandler(this.LauncherFFAssetsHelp_Button_Click);
            // 
            // LauncherAssetCountReloadButton
            // 
            this.LauncherAssetCountReloadButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherAssetCountReloadButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherAssetCountReloadButton.Location = new System.Drawing.Point(374, 87);
            this.LauncherAssetCountReloadButton.Name = "LauncherAssetCountReloadButton";
            this.LauncherAssetCountReloadButton.Size = new System.Drawing.Size(50, 18);
            this.LauncherAssetCountReloadButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherAssetCountReloadButton.TabIndex = 34;
            this.LauncherAssetCountReloadButton.Text = "Reload";
            this.LauncherAssetCountReloadButton.Click += new System.EventHandler(this.LauncherAssetCountReloadButton_Click);
            // 
            // LauncherAssetCountGridView
            // 
            this.LauncherAssetCountGridView.AllowUserToAddRows = false;
            this.LauncherAssetCountGridView.AllowUserToDeleteRows = false;
            this.LauncherAssetCountGridView.AllowUserToResizeRows = false;
            this.LauncherAssetCountGridView.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.LauncherAssetCountGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.LauncherAssetCountGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.LauncherAssetCountGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LauncherAssetCountGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AssetType,
            this.AssetCount,
            this.Percentage});
            this.LauncherAssetCountGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.LauncherAssetCountGridView.Location = new System.Drawing.Point(218, 111);
            this.LauncherAssetCountGridView.Name = "LauncherAssetCountGridView";
            this.LauncherAssetCountGridView.ReadOnly = true;
            this.LauncherAssetCountGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.LauncherAssetCountGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.LauncherAssetCountGridView.RowHeadersVisible = false;
            this.LauncherAssetCountGridView.RowTemplate.Height = 20;
            this.LauncherAssetCountGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.LauncherAssetCountGridView.ShowCellErrors = false;
            this.LauncherAssetCountGridView.ShowCellToolTips = false;
            this.LauncherAssetCountGridView.ShowEditingIcon = false;
            this.LauncherAssetCountGridView.ShowRowErrors = false;
            this.LauncherAssetCountGridView.Size = new System.Drawing.Size(274, 142);
            this.LauncherAssetCountGridView.TabIndex = 33;
            // 
            // AssetType
            // 
            this.AssetType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.AssetType.HeaderText = "Asset";
            this.AssetType.Name = "AssetType";
            this.AssetType.ReadOnly = true;
            this.AssetType.Width = 66;
            // 
            // AssetCount
            // 
            this.AssetCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AssetCount.HeaderText = "Count";
            this.AssetCount.Name = "AssetCount";
            this.AssetCount.ReadOnly = true;
            // 
            // Percentage
            // 
            this.Percentage.HeaderText = "Usage";
            this.Percentage.Name = "Percentage";
            this.Percentage.ReadOnly = true;
            // 
            // LauncherFFAssetsCountLabel
            // 
            // 
            // 
            // 
            this.LauncherFFAssetsCountLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LauncherFFAssetsCountLabel.Location = new System.Drawing.Point(218, 89);
            this.LauncherFFAssetsCountLabel.Name = "LauncherFFAssetsCountLabel";
            this.LauncherFFAssetsCountLabel.Size = new System.Drawing.Size(91, 16);
            this.LauncherFFAssetsCountLabel.TabIndex = 31;
            this.LauncherFFAssetsCountLabel.Text = "FF Assets Count";
            // 
            // LauncherEditMapCSVButton
            // 
            this.LauncherEditMapCSVButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherEditMapCSVButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherEditMapCSVButton.Location = new System.Drawing.Point(218, 294);
            this.LauncherEditMapCSVButton.Name = "LauncherEditMapCSVButton";
            this.LauncherEditMapCSVButton.Size = new System.Drawing.Size(151, 32);
            this.LauncherEditMapCSVButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherEditMapCSVButton.TabIndex = 29;
            this.LauncherEditMapCSVButton.Text = "Edit CSV";
            this.LauncherEditMapCSVButton.Click += new System.EventHandler(this.LauncherEditMapCSVButton_Click);
            // 
            // LauncherCompileBSPButton
            // 
            this.LauncherCompileBSPButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherCompileBSPButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherCompileBSPButton.Location = new System.Drawing.Point(113, 19);
            this.LauncherCompileBSPButton.Name = "LauncherCompileBSPButton";
            this.LauncherCompileBSPButton.Size = new System.Drawing.Size(20, 17);
            this.LauncherCompileBSPButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherCompileBSPButton.TabIndex = 28;
            this.LauncherCompileBSPButton.Text = "...";
            this.LauncherCompileBSPButton.Click += new System.EventHandler(this.LauncherCompileBSPButton_Click);
            // 
            // LauncherCompileLightsButton
            // 
            this.LauncherCompileLightsButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherCompileLightsButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherCompileLightsButton.Location = new System.Drawing.Point(113, 42);
            this.LauncherCompileLightsButton.Name = "LauncherCompileLightsButton";
            this.LauncherCompileLightsButton.Size = new System.Drawing.Size(20, 17);
            this.LauncherCompileLightsButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherCompileLightsButton.TabIndex = 27;
            this.LauncherCompileLightsButton.Text = "...";
            this.LauncherCompileLightsButton.Click += new System.EventHandler(this.LauncherCompileLightsButton_Click);
            // 
            // LauncherCopyImgsToModButton
            // 
            this.LauncherCopyImgsToModButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherCopyImgsToModButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherCopyImgsToModButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.LauncherCopyImgsToModButton.Location = new System.Drawing.Point(218, 261);
            this.LauncherCopyImgsToModButton.Name = "LauncherCopyImgsToModButton";
            this.LauncherCopyImgsToModButton.Size = new System.Drawing.Size(151, 32);
            this.LauncherCopyImgsToModButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherCopyImgsToModButton.TabIndex = 26;
            this.LauncherCopyImgsToModButton.Text = "Copy Map Images to Mod";
            this.LauncherCopyImgsToModButton.Click += new System.EventHandler(this.LauncherCopyImgsToModButton_Click);
            // 
            // LauncherCompileLevelButton
            // 
            this.LauncherCompileLevelButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherCompileLevelButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherCompileLevelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LauncherCompileLevelButton.Location = new System.Drawing.Point(374, 261);
            this.LauncherCompileLevelButton.Name = "LauncherCompileLevelButton";
            this.LauncherCompileLevelButton.Size = new System.Drawing.Size(121, 65);
            this.LauncherCompileLevelButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherCompileLevelButton.TabIndex = 25;
            this.LauncherCompileLevelButton.Text = "Compile FF";
            this.LauncherCompileLevelButton.Click += new System.EventHandler(this.LauncherCompileLevelButton_Click);
            // 
            // LauncherModSpecificMapComboBox
            // 
            this.LauncherModSpecificMapComboBox.DisplayMember = "Text";
            this.LauncherModSpecificMapComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.LauncherModSpecificMapComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LauncherModSpecificMapComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LauncherModSpecificMapComboBox.FormattingEnabled = true;
            this.LauncherModSpecificMapComboBox.ItemHeight = 14;
            this.LauncherModSpecificMapComboBox.Location = new System.Drawing.Point(218, 38);
            this.LauncherModSpecificMapComboBox.Name = "LauncherModSpecificMapComboBox";
            this.LauncherModSpecificMapComboBox.Size = new System.Drawing.Size(274, 20);
            this.LauncherModSpecificMapComboBox.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherModSpecificMapComboBox.TabIndex = 24;
            this.LauncherModSpecificMapComboBox.Click += new System.EventHandler(this.LauncherModSpecificMapComboBox_Click);
            // 
            // LauncherGridFileGroupBox
            // 
            this.LauncherGridFileGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LauncherGridFileGroupBox.Controls.Add(this.LauncherGridEditExistingButton);
            this.LauncherGridFileGroupBox.Controls.Add(this.LauncherGridMakeNewButton);
            this.LauncherGridFileGroupBox.Controls.Add(this.LauncherGridCollectDotsCheckBox);
            this.LauncherGridFileGroupBox.Location = new System.Drawing.Point(6, 261);
            this.LauncherGridFileGroupBox.Name = "LauncherGridFileGroupBox";
            this.LauncherGridFileGroupBox.Size = new System.Drawing.Size(203, 65);
            this.LauncherGridFileGroupBox.TabIndex = 18;
            this.LauncherGridFileGroupBox.TabStop = false;
            this.LauncherGridFileGroupBox.Text = "Grid File";
            // 
            // LauncherGridEditExistingButton
            // 
            this.LauncherGridEditExistingButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherGridEditExistingButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherGridEditExistingButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.LauncherGridEditExistingButton.Location = new System.Drawing.Point(105, 37);
            this.LauncherGridEditExistingButton.Name = "LauncherGridEditExistingButton";
            this.LauncherGridEditExistingButton.Size = new System.Drawing.Size(92, 23);
            this.LauncherGridEditExistingButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherGridEditExistingButton.TabIndex = 7;
            this.LauncherGridEditExistingButton.Text = "Edit Grid";
            this.LauncherGridEditExistingButton.Click += new System.EventHandler(this.LauncherGridEditExistingButton_Click);
            // 
            // LauncherGridMakeNewButton
            // 
            this.LauncherGridMakeNewButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherGridMakeNewButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherGridMakeNewButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.LauncherGridMakeNewButton.Location = new System.Drawing.Point(6, 37);
            this.LauncherGridMakeNewButton.Name = "LauncherGridMakeNewButton";
            this.LauncherGridMakeNewButton.Size = new System.Drawing.Size(92, 23);
            this.LauncherGridMakeNewButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherGridMakeNewButton.TabIndex = 7;
            this.LauncherGridMakeNewButton.Text = "Create Grid";
            this.LauncherGridMakeNewButton.Click += new System.EventHandler(this.LauncherGridMakeNewButton_Click);
            // 
            // LauncherGridCollectDotsCheckBox
            // 
            this.LauncherGridCollectDotsCheckBox.AutoSize = true;
            this.LauncherGridCollectDotsCheckBox.Location = new System.Drawing.Point(7, 19);
            this.LauncherGridCollectDotsCheckBox.Name = "LauncherGridCollectDotsCheckBox";
            this.LauncherGridCollectDotsCheckBox.Size = new System.Drawing.Size(146, 20);
            this.LauncherGridCollectDotsCheckBox.TabIndex = 17;
            this.LauncherGridCollectDotsCheckBox.Text = "Models Collect Dots";
            this.LauncherGridCollectDotsCheckBox.UseVisualStyleBackColor = true;
            // 
            // LauncherMapFFTypeGroupBox
            // 
            this.LauncherMapFFTypeGroupBox.Controls.Add(this.LauncherMapFFTypeMP);
            this.LauncherMapFFTypeGroupBox.Controls.Add(this.LauncherMapFFTypeSP);
            this.LauncherMapFFTypeGroupBox.Location = new System.Drawing.Point(6, 210);
            this.LauncherMapFFTypeGroupBox.Name = "LauncherMapFFTypeGroupBox";
            this.LauncherMapFFTypeGroupBox.Size = new System.Drawing.Size(105, 45);
            this.LauncherMapFFTypeGroupBox.TabIndex = 23;
            this.LauncherMapFFTypeGroupBox.TabStop = false;
            this.LauncherMapFFTypeGroupBox.Text = "Show FF Type";
            // 
            // LauncherMapFFTypeMP
            // 
            this.LauncherMapFFTypeMP.AutoSize = true;
            this.LauncherMapFFTypeMP.Location = new System.Drawing.Point(6, 19);
            this.LauncherMapFFTypeMP.Name = "LauncherMapFFTypeMP";
            this.LauncherMapFFTypeMP.Size = new System.Drawing.Size(45, 20);
            this.LauncherMapFFTypeMP.TabIndex = 22;
            this.LauncherMapFFTypeMP.Text = "MP";
            this.LauncherMapFFTypeMP.UseVisualStyleBackColor = true;
            this.LauncherMapFFTypeMP.CheckedChanged += new System.EventHandler(this.LauncherMapFFTypeMP_CheckedChanged);
            // 
            // LauncherMapFFTypeSP
            // 
            this.LauncherMapFFTypeSP.AutoSize = true;
            this.LauncherMapFFTypeSP.Checked = true;
            this.LauncherMapFFTypeSP.Location = new System.Drawing.Point(62, 19);
            this.LauncherMapFFTypeSP.Name = "LauncherMapFFTypeSP";
            this.LauncherMapFFTypeSP.Size = new System.Drawing.Size(43, 20);
            this.LauncherMapFFTypeSP.TabIndex = 21;
            this.LauncherMapFFTypeSP.TabStop = true;
            this.LauncherMapFFTypeSP.Text = "SP";
            this.LauncherMapFFTypeSP.UseVisualStyleBackColor = true;
            this.LauncherMapFFTypeSP.CheckedChanged += new System.EventHandler(this.LauncherMapFFTypeSP_CheckedChanged);
            // 
            // LauncherModSpecificMapCheckBox
            // 
            this.LauncherModSpecificMapCheckBox.AutoSize = true;
            this.LauncherModSpecificMapCheckBox.Location = new System.Drawing.Point(218, 15);
            this.LauncherModSpecificMapCheckBox.Name = "LauncherModSpecificMapCheckBox";
            this.LauncherModSpecificMapCheckBox.Size = new System.Drawing.Size(134, 20);
            this.LauncherModSpecificMapCheckBox.TabIndex = 5;
            this.LauncherModSpecificMapCheckBox.Text = "Mod Specific Map";
            this.LauncherModSpecificMapCheckBox.UseVisualStyleBackColor = true;
            this.LauncherModSpecificMapCheckBox.CheckedChanged += new System.EventHandler(this.LauncherModSpecificMapCheckBox_CheckChanged);
            // 
            // LauncherRunMapAfterCompileCheckBox
            // 
            this.LauncherRunMapAfterCompileCheckBox.AutoSize = true;
            this.LauncherRunMapAfterCompileCheckBox.Location = new System.Drawing.Point(9, 191);
            this.LauncherRunMapAfterCompileCheckBox.Name = "LauncherRunMapAfterCompileCheckBox";
            this.LauncherRunMapAfterCompileCheckBox.Size = new System.Drawing.Size(163, 20);
            this.LauncherRunMapAfterCompileCheckBox.TabIndex = 8;
            this.LauncherRunMapAfterCompileCheckBox.Text = "Run Map After Compile";
            this.LauncherRunMapAfterCompileCheckBox.UseVisualStyleBackColor = true;
            // 
            // LauncherBspInfoCheckBox
            // 
            this.LauncherBspInfoCheckBox.AutoSize = true;
            this.LauncherBspInfoCheckBox.Location = new System.Drawing.Point(9, 168);
            this.LauncherBspInfoCheckBox.Name = "LauncherBspInfoCheckBox";
            this.LauncherBspInfoCheckBox.Size = new System.Drawing.Size(77, 20);
            this.LauncherBspInfoCheckBox.TabIndex = 7;
            this.LauncherBspInfoCheckBox.Text = "BSP Info";
            this.LauncherBspInfoCheckBox.UseVisualStyleBackColor = true;
            // 
            // LauncherBuildFastFilesCheckBox
            // 
            this.LauncherBuildFastFilesCheckBox.AutoSize = true;
            this.LauncherBuildFastFilesCheckBox.Location = new System.Drawing.Point(9, 145);
            this.LauncherBuildFastFilesCheckBox.Name = "LauncherBuildFastFilesCheckBox";
            this.LauncherBuildFastFilesCheckBox.Size = new System.Drawing.Size(107, 20);
            this.LauncherBuildFastFilesCheckBox.TabIndex = 6;
            this.LauncherBuildFastFilesCheckBox.Text = "Build FastFile";
            this.LauncherBuildFastFilesCheckBox.UseVisualStyleBackColor = true;
            // 
            // LauncherCompileReflectionsCheckBox
            // 
            this.LauncherCompileReflectionsCheckBox.AutoSize = true;
            this.LauncherCompileReflectionsCheckBox.Location = new System.Drawing.Point(9, 111);
            this.LauncherCompileReflectionsCheckBox.Name = "LauncherCompileReflectionsCheckBox";
            this.LauncherCompileReflectionsCheckBox.Size = new System.Drawing.Size(146, 20);
            this.LauncherCompileReflectionsCheckBox.TabIndex = 4;
            this.LauncherCompileReflectionsCheckBox.Text = "Compile Reflections";
            this.LauncherCompileReflectionsCheckBox.UseVisualStyleBackColor = true;
            // 
            // LauncherConnectPathsCheckBox
            // 
            this.LauncherConnectPathsCheckBox.AutoSize = true;
            this.LauncherConnectPathsCheckBox.Location = new System.Drawing.Point(9, 88);
            this.LauncherConnectPathsCheckBox.Name = "LauncherConnectPathsCheckBox";
            this.LauncherConnectPathsCheckBox.Size = new System.Drawing.Size(112, 20);
            this.LauncherConnectPathsCheckBox.TabIndex = 3;
            this.LauncherConnectPathsCheckBox.Text = "Connect Paths";
            this.LauncherConnectPathsCheckBox.UseVisualStyleBackColor = true;
            // 
            // LauncherCompileVisCheckBox
            // 
            this.LauncherCompileVisCheckBox.AutoSize = true;
            this.LauncherCompileVisCheckBox.Enabled = false;
            this.LauncherCompileVisCheckBox.Location = new System.Drawing.Point(9, 65);
            this.LauncherCompileVisCheckBox.Name = "LauncherCompileVisCheckBox";
            this.LauncherCompileVisCheckBox.Size = new System.Drawing.Size(98, 20);
            this.LauncherCompileVisCheckBox.TabIndex = 2;
            this.LauncherCompileVisCheckBox.Text = "Compile Vis";
            this.LauncherCompileVisCheckBox.UseVisualStyleBackColor = true;
            // 
            // LauncherCompileLightsCheckBox
            // 
            this.LauncherCompileLightsCheckBox.AutoSize = true;
            this.LauncherCompileLightsCheckBox.Location = new System.Drawing.Point(9, 42);
            this.LauncherCompileLightsCheckBox.Name = "LauncherCompileLightsCheckBox";
            this.LauncherCompileLightsCheckBox.Size = new System.Drawing.Size(114, 20);
            this.LauncherCompileLightsCheckBox.TabIndex = 1;
            this.LauncherCompileLightsCheckBox.Text = "Compile Lights";
            this.LauncherCompileLightsCheckBox.UseVisualStyleBackColor = true;
            // 
            // LauncherCompileBSPCheckBox
            // 
            this.LauncherCompileBSPCheckBox.AutoSize = true;
            this.LauncherCompileBSPCheckBox.Location = new System.Drawing.Point(9, 19);
            this.LauncherCompileBSPCheckBox.Name = "LauncherCompileBSPCheckBox";
            this.LauncherCompileBSPCheckBox.Size = new System.Drawing.Size(106, 20);
            this.LauncherCompileBSPCheckBox.TabIndex = 0;
            this.LauncherCompileBSPCheckBox.Text = "Compile BSP";
            this.LauncherCompileBSPCheckBox.UseVisualStyleBackColor = true;
            // 
            // LauncherMapList
            // 
            this.LauncherMapList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LauncherMapList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.LauncherMapList.FormattingEnabled = true;
            this.LauncherMapList.HorizontalScrollbar = true;
            this.LauncherMapList.IntegralHeight = false;
            this.LauncherMapList.Location = new System.Drawing.Point(6, 7);
            this.LauncherMapList.Name = "LauncherMapList";
            this.LauncherMapList.Size = new System.Drawing.Size(204, 291);
            this.LauncherMapList.TabIndex = 1;
            this.LauncherMapList.SelectedIndexChanged += new System.EventHandler(this.LauncherMapList_SelectedIndexChanged);
            // 
            // LauncherTabModBuilder
            // 
            this.LauncherTabModBuilder.BackColor = System.Drawing.Color.Transparent;
            this.LauncherTabModBuilder.Controls.Add(this.LauncherIwdFileGroupBox);
            this.LauncherTabModBuilder.Controls.Add(this.LauncherLocalizedCsvGroupBox);
            this.LauncherTabModBuilder.Controls.Add(this.LauncherFastFileCsvGroupBox);
            this.LauncherTabModBuilder.Controls.Add(this.LauncherModGroupBox);
            this.LauncherTabModBuilder.Controls.Add(this.LauncherModAssetLabel);
            this.LauncherTabModBuilder.Location = new System.Drawing.Point(4, 25);
            this.LauncherTabModBuilder.Name = "LauncherTabModBuilder";
            this.LauncherTabModBuilder.Padding = new System.Windows.Forms.Padding(3);
            this.LauncherTabModBuilder.Size = new System.Drawing.Size(763, 347);
            this.LauncherTabModBuilder.TabIndex = 1;
            this.LauncherTabModBuilder.Text = "Mod Builder";
            // 
            // LauncherIwdFileGroupBox
            // 
            this.LauncherIwdFileGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LauncherIwdFileGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.LauncherIwdFileGroupBox.Controls.Add(this.LauncherIwdFileTree);
            this.LauncherIwdFileGroupBox.Location = new System.Drawing.Point(351, 6);
            this.LauncherIwdFileGroupBox.Name = "LauncherIwdFileGroupBox";
            this.LauncherIwdFileGroupBox.Size = new System.Drawing.Size(414, 335);
            this.LauncherIwdFileGroupBox.TabIndex = 2;
            this.LauncherIwdFileGroupBox.TabStop = false;
            this.LauncherIwdFileGroupBox.Text = "IWD File List";
            // 
            // LauncherIwdFileTree
            // 
            this.LauncherIwdFileTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LauncherIwdFileTree.BackColor = System.Drawing.Color.White;
            this.LauncherIwdFileTree.CheckBoxes = true;
            this.LauncherIwdFileTree.Indent = 15;
            this.LauncherIwdFileTree.Location = new System.Drawing.Point(6, 20);
            this.LauncherIwdFileTree.Name = "LauncherIwdFileTree";
            this.LauncherIwdFileTree.Size = new System.Drawing.Size(405, 308);
            this.LauncherIwdFileTree.TabIndex = 2;
            this.LauncherIwdFileTree.DoubleClick += new System.EventHandler(this.LauncherIwdFileTree_DoubleClick);
            // 
            // LauncherLocalizedCsvGroupBox
            // 
            this.LauncherLocalizedCsvGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LauncherLocalizedCsvGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.LauncherLocalizedCsvGroupBox.Controls.Add(this.LauncherLocalizedCsvTextBox);
            this.LauncherLocalizedCsvGroupBox.Location = new System.Drawing.Point(6, 275);
            this.LauncherLocalizedCsvGroupBox.Name = "LauncherLocalizedCsvGroupBox";
            this.LauncherLocalizedCsvGroupBox.Size = new System.Drawing.Size(339, 66);
            this.LauncherLocalizedCsvGroupBox.TabIndex = 25;
            this.LauncherLocalizedCsvGroupBox.TabStop = false;
            this.LauncherLocalizedCsvGroupBox.Text = "Localized FF localized_mod.csv (english)";
            // 
            // LauncherLocalizedCsvTextBox
            // 
            // 
            // 
            // 
            this.LauncherLocalizedCsvTextBox.BackgroundStyle.Class = "RichTextBoxBorder";
            this.LauncherLocalizedCsvTextBox.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LauncherLocalizedCsvTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LauncherLocalizedCsvTextBox.Location = new System.Drawing.Point(7, 18);
            this.LauncherLocalizedCsvTextBox.Name = "LauncherLocalizedCsvTextBox";
            this.LauncherLocalizedCsvTextBox.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 " +
    "Segoe UI;}}\r\n{\\*\\generator Riched20 10.0.22621}\\viewkind4\\uc1 \r\n\\pard\\f0\\fs17\\pa" +
    "r\r\n}\r\n";
            this.LauncherLocalizedCsvTextBox.Size = new System.Drawing.Size(326, 42);
            this.LauncherLocalizedCsvTextBox.TabIndex = 26;
            this.LauncherLocalizedCsvTextBox.WordWrap = false;
            // 
            // LauncherFastFileCsvGroupBox
            // 
            this.LauncherFastFileCsvGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.LauncherFastFileCsvGroupBox.Controls.Add(this.LauncherFastFileCsvTextBox);
            this.LauncherFastFileCsvGroupBox.Location = new System.Drawing.Point(6, 200);
            this.LauncherFastFileCsvGroupBox.Name = "LauncherFastFileCsvGroupBox";
            this.LauncherFastFileCsvGroupBox.Size = new System.Drawing.Size(339, 147);
            this.LauncherFastFileCsvGroupBox.TabIndex = 19;
            this.LauncherFastFileCsvGroupBox.TabStop = false;
            this.LauncherFastFileCsvGroupBox.Text = "FastFile mod.csv";
            // 
            // LauncherFastFileCsvTextBox
            // 
            // 
            // 
            // 
            this.LauncherFastFileCsvTextBox.BackgroundStyle.Class = "RichTextBoxBorder";
            this.LauncherFastFileCsvTextBox.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LauncherFastFileCsvTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LauncherFastFileCsvTextBox.Location = new System.Drawing.Point(7, 18);
            this.LauncherFastFileCsvTextBox.Name = "LauncherFastFileCsvTextBox";
            this.LauncherFastFileCsvTextBox.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 " +
    "Segoe UI;}}\r\n{\\*\\generator Riched20 10.0.22621}\\viewkind4\\uc1 \r\n\\pard\\f0\\fs17\\pa" +
    "r\r\n}\r\n";
            this.LauncherFastFileCsvTextBox.Size = new System.Drawing.Size(326, 56);
            this.LauncherFastFileCsvTextBox.TabIndex = 2;
            this.LauncherFastFileCsvTextBox.WordWrap = false;
            // 
            // LauncherModGroupBox
            // 
            this.LauncherModGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.LauncherModGroupBox.Controls.Add(this.LauncherModLanguageLabel);
            this.LauncherModGroupBox.Controls.Add(this.LauncherModLanguageComboBox);
            this.LauncherModGroupBox.Controls.Add(this.LauncherModBuildLocalizedButton);
            this.LauncherModGroupBox.Controls.Add(this.LauncherModRefreshButton);
            this.LauncherModGroupBox.Controls.Add(this.LauncherModBuildButton);
            this.LauncherModGroupBox.Controls.Add(this.LauncherModOpenButton);
            this.LauncherModGroupBox.Controls.Add(this.LauncherModComboBox);
            this.LauncherModGroupBox.Controls.Add(this.LauncherModBuildSoundsCheckBox);
            this.LauncherModGroupBox.Controls.Add(this.LauncherModVerboseCheckBox);
            this.LauncherModGroupBox.Controls.Add(this.LauncherModBuildIwdFileCheckBox);
            this.LauncherModGroupBox.Controls.Add(this.LauncherModBuildFastFilesCheckBox);
            this.LauncherModGroupBox.Location = new System.Drawing.Point(6, 6);
            this.LauncherModGroupBox.Name = "LauncherModGroupBox";
            this.LauncherModGroupBox.Size = new System.Drawing.Size(339, 188);
            this.LauncherModGroupBox.TabIndex = 4;
            this.LauncherModGroupBox.TabStop = false;
            this.LauncherModGroupBox.Text = "Mod";
            // 
            // LauncherModLanguageLabel
            // 
            this.LauncherModLanguageLabel.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.LauncherModLanguageLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LauncherModLanguageLabel.Location = new System.Drawing.Point(6, 158);
            this.LauncherModLanguageLabel.Name = "LauncherModLanguageLabel";
            this.LauncherModLanguageLabel.Size = new System.Drawing.Size(70, 20);
            this.LauncherModLanguageLabel.TabIndex = 23;
            this.LauncherModLanguageLabel.Text = "Language:";
            // 
            // LauncherModLanguageComboBox
            // 
            this.LauncherModLanguageComboBox.DisplayMember = "Text";
            this.LauncherModLanguageComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.LauncherModLanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LauncherModLanguageComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LauncherModLanguageComboBox.FormattingEnabled = true;
            this.LauncherModLanguageComboBox.ItemHeight = 14;
            this.LauncherModLanguageComboBox.Items.AddRange(new object[] {
            "english",
            "french",
            "german",
            "italian",
            "spanish",
            "russian",
            "korean",
            "polish"});
            this.LauncherModLanguageComboBox.Location = new System.Drawing.Point(80, 157);
            this.LauncherModLanguageComboBox.Name = "LauncherModLanguageComboBox";
            this.LauncherModLanguageComboBox.Size = new System.Drawing.Size(253, 20);
            this.LauncherModLanguageComboBox.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherModLanguageComboBox.TabIndex = 24;
            // 
            // LauncherModBuildLocalizedButton
            // 
            this.LauncherModBuildLocalizedButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherModBuildLocalizedButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherModBuildLocalizedButton.Location = new System.Drawing.Point(6, 126);
            this.LauncherModBuildLocalizedButton.Name = "LauncherModBuildLocalizedButton";
            this.LauncherModBuildLocalizedButton.Size = new System.Drawing.Size(327, 26);
            this.LauncherModBuildLocalizedButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherModBuildLocalizedButton.TabIndex = 22;
            this.LauncherModBuildLocalizedButton.Text = "Build Localized FF";
            this.LauncherModBuildLocalizedButton.Click += new System.EventHandler(this.LauncherModBuildLocalizedButton_Click);
            // 
            // LauncherModRefreshButton
            // 
            this.LauncherModRefreshButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherModRefreshButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherModRefreshButton.Location = new System.Drawing.Point(245, 94);
            this.LauncherModRefreshButton.Name = "LauncherModRefreshButton";
            this.LauncherModRefreshButton.Size = new System.Drawing.Size(88, 26);
            this.LauncherModRefreshButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherModRefreshButton.TabIndex = 21;
            this.LauncherModRefreshButton.Text = "Reload Mod";
            this.LauncherModRefreshButton.Click += new System.EventHandler(this.LauncherModRefreshButton_Click);
            // 
            // LauncherModBuildButton
            // 
            this.LauncherModBuildButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherModBuildButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherModBuildButton.Location = new System.Drawing.Point(6, 94);
            this.LauncherModBuildButton.Name = "LauncherModBuildButton";
            this.LauncherModBuildButton.Size = new System.Drawing.Size(88, 26);
            this.LauncherModBuildButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherModBuildButton.TabIndex = 3;
            this.LauncherModBuildButton.Text = "Build Mod";
            this.LauncherModBuildButton.Click += new System.EventHandler(this.LauncherModBuildButton_Click);
            // 
            // LauncherModOpenButton
            // 
            this.LauncherModOpenButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherModOpenButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherModOpenButton.Location = new System.Drawing.Point(126, 94);
            this.LauncherModOpenButton.Name = "LauncherModOpenButton";
            this.LauncherModOpenButton.Size = new System.Drawing.Size(88, 26);
            this.LauncherModOpenButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherModOpenButton.TabIndex = 20;
            this.LauncherModOpenButton.Text = "Open Mod";
            this.LauncherModOpenButton.Click += new System.EventHandler(this.LauncherModOpenButton_Click);
            // 
            // LauncherModComboBox
            // 
            this.LauncherModComboBox.DisplayMember = "Text";
            this.LauncherModComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.LauncherModComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LauncherModComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LauncherModComboBox.FormattingEnabled = true;
            this.LauncherModComboBox.ItemHeight = 14;
            this.LauncherModComboBox.Location = new System.Drawing.Point(6, 19);
            this.LauncherModComboBox.Name = "LauncherModComboBox";
            this.LauncherModComboBox.Size = new System.Drawing.Size(327, 20);
            this.LauncherModComboBox.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherModComboBox.TabIndex = 1;
            this.LauncherModComboBox.SelectedIndexChanged += new System.EventHandler(this.LauncherModComboBox_SelectedIndexChanged);
            this.LauncherModComboBox.Click += new System.EventHandler(this.LauncherModComboBox_Click);
            // 
            // LauncherModBuildSoundsCheckBox
            // 
            this.LauncherModBuildSoundsCheckBox.AutoSize = true;
            this.LauncherModBuildSoundsCheckBox.Location = new System.Drawing.Point(152, 45);
            this.LauncherModBuildSoundsCheckBox.Name = "LauncherModBuildSoundsCheckBox";
            this.LauncherModBuildSoundsCheckBox.Size = new System.Drawing.Size(105, 20);
            this.LauncherModBuildSoundsCheckBox.TabIndex = 17;
            this.LauncherModBuildSoundsCheckBox.Text = "Build Sounds";
            this.LauncherModBuildSoundsCheckBox.UseVisualStyleBackColor = true;
            // 
            // LauncherModVerboseCheckBox
            // 
            this.LauncherModVerboseCheckBox.AutoSize = true;
            this.LauncherModVerboseCheckBox.Location = new System.Drawing.Point(152, 71);
            this.LauncherModVerboseCheckBox.Name = "LauncherModVerboseCheckBox";
            this.LauncherModVerboseCheckBox.Size = new System.Drawing.Size(78, 20);
            this.LauncherModVerboseCheckBox.TabIndex = 15;
            this.LauncherModVerboseCheckBox.Text = "Verbose";
            this.LauncherModVerboseCheckBox.UseVisualStyleBackColor = true;
            // 
            // LauncherModBuildIwdFileCheckBox
            // 
            this.LauncherModBuildIwdFileCheckBox.AutoSize = true;
            this.LauncherModBuildIwdFileCheckBox.Location = new System.Drawing.Point(7, 71);
            this.LauncherModBuildIwdFileCheckBox.Name = "LauncherModBuildIwdFileCheckBox";
            this.LauncherModBuildIwdFileCheckBox.Size = new System.Drawing.Size(110, 20);
            this.LauncherModBuildIwdFileCheckBox.TabIndex = 14;
            this.LauncherModBuildIwdFileCheckBox.Text = "Build IWD File";
            this.LauncherModBuildIwdFileCheckBox.UseVisualStyleBackColor = true;
            // 
            // LauncherModBuildFastFilesCheckBox
            // 
            this.LauncherModBuildFastFilesCheckBox.AutoSize = true;
            this.LauncherModBuildFastFilesCheckBox.Location = new System.Drawing.Point(7, 45);
            this.LauncherModBuildFastFilesCheckBox.Name = "LauncherModBuildFastFilesCheckBox";
            this.LauncherModBuildFastFilesCheckBox.Size = new System.Drawing.Size(146, 20);
            this.LauncherModBuildFastFilesCheckBox.TabIndex = 13;
            this.LauncherModBuildFastFilesCheckBox.Text = "Build mod.ff FastFile";
            this.LauncherModBuildFastFilesCheckBox.UseVisualStyleBackColor = true;
            // 
            // LauncherModAssetLabel
            // 
            // 
            // 
            // 
            this.LauncherModAssetLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LauncherModAssetLabel.Location = new System.Drawing.Point(633, 192);
            this.LauncherModAssetLabel.Name = "LauncherModAssetLabel";
            this.LauncherModAssetLabel.Size = new System.Drawing.Size(91, 16);
            this.LauncherModAssetLabel.TabIndex = 36;
            // 
            // LauncherTabRunGame
            // 
            this.LauncherTabRunGame.BackColor = System.Drawing.Color.Transparent;
            this.LauncherTabRunGame.Controls.Add(this.LauncherRunGameButton);
            this.LauncherTabRunGame.Controls.Add(this.LauncherGameOptionsPanel);
            this.LauncherTabRunGame.Controls.Add(this.LauncherRunGameCustomCommandLineGroupBox);
            this.LauncherTabRunGame.Controls.Add(this.LauncherRunGameCommandLineGroupBox);
            this.LauncherTabRunGame.Controls.Add(this.LauncherRunGameModGroupBox);
            this.LauncherTabRunGame.Controls.Add(this.LauncherRunGameExeTypeGroupBox);
            this.LauncherTabRunGame.Location = new System.Drawing.Point(4, 25);
            this.LauncherTabRunGame.Name = "LauncherTabRunGame";
            this.LauncherTabRunGame.Padding = new System.Windows.Forms.Padding(3);
            this.LauncherTabRunGame.Size = new System.Drawing.Size(763, 347);
            this.LauncherTabRunGame.TabIndex = 2;
            this.LauncherTabRunGame.Text = "Run Game";
            // 
            // LauncherRunGameButton
            // 
            this.LauncherRunGameButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherRunGameButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherRunGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.LauncherRunGameButton.Location = new System.Drawing.Point(589, 249);
            this.LauncherRunGameButton.Name = "LauncherRunGameButton";
            this.LauncherRunGameButton.Size = new System.Drawing.Size(122, 95);
            this.LauncherRunGameButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherRunGameButton.TabIndex = 0;
            this.LauncherRunGameButton.Text = "Run Game";
            this.LauncherRunGameButton.Click += new System.EventHandler(this.LauncherRunGameButton_Click);
            // 
            // LauncherGameOptionsPanel
            // 
            this.LauncherGameOptionsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LauncherGameOptionsPanel.AutoScroll = true;
            this.LauncherGameOptionsPanel.BackColor = System.Drawing.Color.Transparent;
            this.LauncherGameOptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LauncherGameOptionsPanel.Location = new System.Drawing.Point(6, 59);
            this.LauncherGameOptionsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.LauncherGameOptionsPanel.Name = "LauncherGameOptionsPanel";
            this.LauncherGameOptionsPanel.Size = new System.Drawing.Size(751, 176);
            this.LauncherGameOptionsPanel.TabIndex = 5;
            this.LauncherGameOptionsPanel.Click += new System.EventHandler(this.LauncherGameOptionsFlowPanel_Click);
            // 
            // LauncherRunGameCustomCommandLineGroupBox
            // 
            this.LauncherRunGameCustomCommandLineGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LauncherRunGameCustomCommandLineGroupBox.Controls.Add(this.LauncherRunGameCustomCommandLineTextBox);
            this.LauncherRunGameCustomCommandLineGroupBox.Location = new System.Drawing.Point(6, 246);
            this.LauncherRunGameCustomCommandLineGroupBox.Name = "LauncherRunGameCustomCommandLineGroupBox";
            this.LauncherRunGameCustomCommandLineGroupBox.Size = new System.Drawing.Size(623, 44);
            this.LauncherRunGameCustomCommandLineGroupBox.TabIndex = 4;
            this.LauncherRunGameCustomCommandLineGroupBox.TabStop = false;
            this.LauncherRunGameCustomCommandLineGroupBox.Text = "Custom Command Line";
            // 
            // LauncherRunGameCustomCommandLineTextBox
            // 
            this.LauncherRunGameCustomCommandLineTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LauncherRunGameCustomCommandLineTextBox.Location = new System.Drawing.Point(6, 19);
            this.LauncherRunGameCustomCommandLineTextBox.Name = "LauncherRunGameCustomCommandLineTextBox";
            this.LauncherRunGameCustomCommandLineTextBox.Size = new System.Drawing.Size(611, 22);
            this.LauncherRunGameCustomCommandLineTextBox.TabIndex = 0;
            // 
            // LauncherRunGameCommandLineGroupBox
            // 
            this.LauncherRunGameCommandLineGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LauncherRunGameCommandLineGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.LauncherRunGameCommandLineGroupBox.Controls.Add(this.LauncherRunGameCommandLineTextBox);
            this.LauncherRunGameCommandLineGroupBox.Location = new System.Drawing.Point(6, 297);
            this.LauncherRunGameCommandLineGroupBox.Name = "LauncherRunGameCommandLineGroupBox";
            this.LauncherRunGameCommandLineGroupBox.Size = new System.Drawing.Size(623, 44);
            this.LauncherRunGameCommandLineGroupBox.TabIndex = 3;
            this.LauncherRunGameCommandLineGroupBox.TabStop = false;
            this.LauncherRunGameCommandLineGroupBox.Text = "Command Line";
            // 
            // LauncherRunGameCommandLineTextBox
            // 
            this.LauncherRunGameCommandLineTextBox.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.LauncherRunGameCommandLineTextBox.Border.Class = "TextBoxBorder";
            this.LauncherRunGameCommandLineTextBox.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LauncherRunGameCommandLineTextBox.DisabledBackColor = System.Drawing.Color.White;
            this.LauncherRunGameCommandLineTextBox.ForeColor = System.Drawing.Color.Black;
            this.LauncherRunGameCommandLineTextBox.Location = new System.Drawing.Point(6, 19);
            this.LauncherRunGameCommandLineTextBox.Name = "LauncherRunGameCommandLineTextBox";
            this.LauncherRunGameCommandLineTextBox.PreventEnterBeep = true;
            this.LauncherRunGameCommandLineTextBox.ReadOnly = true;
            this.LauncherRunGameCommandLineTextBox.Size = new System.Drawing.Size(565, 22);
            this.LauncherRunGameCommandLineTextBox.TabIndex = 5;
            // 
            // LauncherRunGameModGroupBox
            // 
            this.LauncherRunGameModGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LauncherRunGameModGroupBox.Controls.Add(this.LauncherRunGameModComboBox);
            this.LauncherRunGameModGroupBox.Location = new System.Drawing.Point(109, 6);
            this.LauncherRunGameModGroupBox.Name = "LauncherRunGameModGroupBox";
            this.LauncherRunGameModGroupBox.Size = new System.Drawing.Size(648, 47);
            this.LauncherRunGameModGroupBox.TabIndex = 1;
            this.LauncherRunGameModGroupBox.TabStop = false;
            this.LauncherRunGameModGroupBox.Text = "Mod";
            // 
            // LauncherRunGameModComboBox
            // 
            this.LauncherRunGameModComboBox.DisplayMember = "Text";
            this.LauncherRunGameModComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.LauncherRunGameModComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LauncherRunGameModComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LauncherRunGameModComboBox.FormattingEnabled = true;
            this.LauncherRunGameModComboBox.ItemHeight = 14;
            this.LauncherRunGameModComboBox.Location = new System.Drawing.Point(6, 19);
            this.LauncherRunGameModComboBox.Name = "LauncherRunGameModComboBox";
            this.LauncherRunGameModComboBox.Size = new System.Drawing.Size(590, 20);
            this.LauncherRunGameModComboBox.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherRunGameModComboBox.TabIndex = 0;
            this.LauncherRunGameModComboBox.SelectedIndexChanged += new System.EventHandler(this.LauncherRunGameModComboBox_SelectedIndexChanged);
            this.LauncherRunGameModComboBox.Click += new System.EventHandler(this.LauncherRunGameModComboBox_Click);
            // 
            // LauncherRunGameExeTypeGroupBox
            // 
            this.LauncherRunGameExeTypeGroupBox.Controls.Add(this.LauncherRunGameExeTypeMpRadioButton);
            this.LauncherRunGameExeTypeGroupBox.Controls.Add(this.LauncherRunGameTypeRadioButton);
            this.LauncherRunGameExeTypeGroupBox.Location = new System.Drawing.Point(6, 6);
            this.LauncherRunGameExeTypeGroupBox.Name = "LauncherRunGameExeTypeGroupBox";
            this.LauncherRunGameExeTypeGroupBox.Size = new System.Drawing.Size(97, 47);
            this.LauncherRunGameExeTypeGroupBox.TabIndex = 0;
            this.LauncherRunGameExeTypeGroupBox.TabStop = false;
            this.LauncherRunGameExeTypeGroupBox.Text = "Exe Type";
            // 
            // LauncherRunGameExeTypeMpRadioButton
            // 
            this.LauncherRunGameExeTypeMpRadioButton.AutoSize = true;
            this.LauncherRunGameExeTypeMpRadioButton.Location = new System.Drawing.Point(50, 19);
            this.LauncherRunGameExeTypeMpRadioButton.Name = "LauncherRunGameExeTypeMpRadioButton";
            this.LauncherRunGameExeTypeMpRadioButton.Size = new System.Drawing.Size(45, 20);
            this.LauncherRunGameExeTypeMpRadioButton.TabIndex = 1;
            this.LauncherRunGameExeTypeMpRadioButton.Text = "MP";
            this.LauncherRunGameExeTypeMpRadioButton.UseVisualStyleBackColor = true;
            // 
            // LauncherRunGameTypeRadioButton
            // 
            this.LauncherRunGameTypeRadioButton.AutoSize = true;
            this.LauncherRunGameTypeRadioButton.Checked = true;
            this.LauncherRunGameTypeRadioButton.Location = new System.Drawing.Point(6, 19);
            this.LauncherRunGameTypeRadioButton.Name = "LauncherRunGameTypeRadioButton";
            this.LauncherRunGameTypeRadioButton.Size = new System.Drawing.Size(43, 20);
            this.LauncherRunGameTypeRadioButton.TabIndex = 0;
            this.LauncherRunGameTypeRadioButton.TabStop = true;
            this.LauncherRunGameTypeRadioButton.Text = "SP";
            this.LauncherRunGameTypeRadioButton.UseVisualStyleBackColor = true;
            // 
            // LauncherApplicationsGroupBox
            // 
            this.LauncherApplicationsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LauncherApplicationsGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.LauncherApplicationsGroupBox.Controls.Add(this.LauncherClearConsoleButton);
            // this.LauncherApplicationsGroupBox.Controls.Add(this.LauncherButtonWeaponEditor);
            this.LauncherApplicationsGroupBox.Controls.Add(this.LauncherButtonBlender);
            this.LauncherApplicationsGroupBox.Controls.Add(this.LauncherButtonGameUtilsImage);
            this.LauncherApplicationsGroupBox.Controls.Add(this.LauncherButtonSoundTools);
            this.LauncherApplicationsGroupBox.Controls.Add(this.LauncherButtonAssetViewer);
            this.LauncherApplicationsGroupBox.Controls.Add(this.LauncherButtonAssetManager);
            this.LauncherApplicationsGroupBox.Controls.Add(this.LauncherButtonEffectsEd);
            this.LauncherApplicationsGroupBox.Controls.Add(this.LauncherButtonRadiant);
            this.LauncherApplicationsGroupBox.ForeColor = System.Drawing.Color.Black;
            this.LauncherApplicationsGroupBox.Location = new System.Drawing.Point(4, 0);
            this.LauncherApplicationsGroupBox.Name = "LauncherApplicationsGroupBox";
            this.LauncherApplicationsGroupBox.Size = new System.Drawing.Size(139, 377);
            this.LauncherApplicationsGroupBox.TabIndex = 1;
            this.LauncherApplicationsGroupBox.TabStop = false;
            this.LauncherApplicationsGroupBox.Text = "Tools";
            // 
            // LauncherClearConsoleButton
            // 
            this.LauncherClearConsoleButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherClearConsoleButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherClearConsoleButton.FocusCuesEnabled = false;
            this.LauncherClearConsoleButton.Location = new System.Drawing.Point(6, 330);
            this.LauncherClearConsoleButton.Name = "LauncherClearConsoleButton";
            this.LauncherClearConsoleButton.Size = new System.Drawing.Size(128, 32);
            this.LauncherClearConsoleButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherClearConsoleButton.TabIndex = 14;
            this.LauncherClearConsoleButton.Text = "Clear Console";
            this.LauncherClearConsoleButton.Click += new System.EventHandler(this.LauncherClearConsoleButton_Click);
			// 
			// LauncherButtonWeaponEditor /* Disabled this shit, we don't need it
			// 
			//this.LauncherButtonWeaponEditor.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			//this.LauncherButtonWeaponEditor.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
			//this.LauncherButtonWeaponEditor.FocusCuesEnabled = false;
			//this.LauncherButtonWeaponEditor.Location = new System.Drawing.Point(6, 171);
			//this.LauncherButtonWeaponEditor.Name = "LauncherButtonWeaponEditor";
			//this.LauncherButtonWeaponEditor.Size = new System.Drawing.Size(128, 32);
			//this.LauncherButtonWeaponEditor.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			//this.LauncherButtonWeaponEditor.TabIndex = 11;
			//this.LauncherButtonWeaponEditor.Text = "Weapon Editor";
			//this.LauncherButtonWeaponEditor.Visible = true;
			//this.LauncherButtonWeaponEditor.Click += new System.EventHandler(this.LauncherButtonWeaponEditor_Click);
			// 
			// LauncherButtonBlender
			// 
			this.LauncherButtonBlender.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.LauncherButtonBlender.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
			this.LauncherButtonBlender.FocusCuesEnabled = false;
			this.LauncherButtonBlender.Location = new System.Drawing.Point(6, 171);
			this.LauncherButtonBlender.Name = "LauncherButtonWeaponEditor";
			this.LauncherButtonBlender.Size = new System.Drawing.Size(128, 32);
			this.LauncherButtonBlender.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.LauncherButtonBlender.TabIndex = 11;
			this.LauncherButtonBlender.Text = "Blender";
			this.LauncherButtonBlender.Click += new System.EventHandler(this.LauncherButtonWeaponEditor_Click);
			// 
			// LauncherButtonGameUtilsImage
			//
			this.LauncherButtonGameUtilsImage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.LauncherButtonGameUtilsImage.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
			this.LauncherButtonGameUtilsImage.FocusCuesEnabled = false;
			this.LauncherButtonGameUtilsImage.Location = new System.Drawing.Point(6, 209);
			this.LauncherButtonGameUtilsImage.Name = "LauncherButtonWeaponEditor";
			this.LauncherButtonGameUtilsImage.Size = new System.Drawing.Size(128, 32);
			this.LauncherButtonGameUtilsImage.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.LauncherButtonGameUtilsImage.TabIndex = 12;
			this.LauncherButtonGameUtilsImage.Text = "Game Utils Image";
			this.LauncherButtonGameUtilsImage.Visible = true;
			this.LauncherButtonGameUtilsImage.Click += new System.EventHandler(this.LauncherButtonWeaponEditor_Click);
			// 
			// LauncherButtonSoundTools
			// 
			this.LauncherButtonSoundTools.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.LauncherButtonSoundTools.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
			this.LauncherButtonSoundTools.FocusCuesEnabled = false;
			this.LauncherButtonSoundTools.Location = new System.Drawing.Point(6, 247);
			this.LauncherButtonSoundTools.Name = "LauncherButtonWeaponEditor";
			this.LauncherButtonSoundTools.Size = new System.Drawing.Size(128, 32);
			this.LauncherButtonSoundTools.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.LauncherButtonSoundTools.TabIndex = 13;
			this.LauncherButtonSoundTools.Text = "Sound Tools";
			this.LauncherButtonSoundTools.Visible = true;
			this.LauncherButtonSoundTools.Click += new System.EventHandler(this.LauncherButtonWeaponEditor_Click);
			// 
			// LauncherButtonAssetViewer
			// 
			this.LauncherButtonAssetViewer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherButtonAssetViewer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherButtonAssetViewer.FocusCuesEnabled = false;
            this.LauncherButtonAssetViewer.Location = new System.Drawing.Point(6, 133);
            this.LauncherButtonAssetViewer.Name = "LauncherButtonAssetViewer";
            this.LauncherButtonAssetViewer.Size = new System.Drawing.Size(128, 32);
            this.LauncherButtonAssetViewer.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherButtonAssetViewer.TabIndex = 10;
            this.LauncherButtonAssetViewer.Text = "Asset Viewer";
            this.LauncherButtonAssetViewer.Click += new System.EventHandler(this.LauncherButtonAssetViewer_Click);
            // 
            // LauncherButtonAssetManager
            // 
            this.LauncherButtonAssetManager.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherButtonAssetManager.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherButtonAssetManager.FocusCuesEnabled = false;
            this.LauncherButtonAssetManager.Location = new System.Drawing.Point(6, 95);
            this.LauncherButtonAssetManager.Name = "LauncherButtonAssetManager";
            this.LauncherButtonAssetManager.Size = new System.Drawing.Size(128, 32);
            this.LauncherButtonAssetManager.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherButtonAssetManager.TabIndex = 9;
            this.LauncherButtonAssetManager.Text = "Asset Manager";
            this.LauncherButtonAssetManager.TextColor = System.Drawing.Color.Black;
            this.LauncherButtonAssetManager.Click += new System.EventHandler(this.LauncherButtonAssetManager_Click);
            // 
            // LauncherButtonEffectsEd
            // 
            this.LauncherButtonEffectsEd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherButtonEffectsEd.AntiAlias = true;
            this.LauncherButtonEffectsEd.BackColor = System.Drawing.Color.Transparent;
            this.LauncherButtonEffectsEd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherButtonEffectsEd.FocusCuesEnabled = false;
            this.LauncherButtonEffectsEd.Location = new System.Drawing.Point(6, 57);
            this.LauncherButtonEffectsEd.Name = "LauncherButtonEffectsEd";
            this.LauncherButtonEffectsEd.Size = new System.Drawing.Size(128, 32);
            this.LauncherButtonEffectsEd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherButtonEffectsEd.TabIndex = 8;
            this.LauncherButtonEffectsEd.Text = "Effects Editor";
            this.LauncherButtonEffectsEd.Click += new System.EventHandler(this.LauncherButtonEffectsEd_Click);
            // 
            // LauncherButtonRadiant
            // 
            this.LauncherButtonRadiant.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherButtonRadiant.AntiAlias = true;
            this.LauncherButtonRadiant.BackColor = System.Drawing.Color.Black;
            this.LauncherButtonRadiant.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherButtonRadiant.Cursor = System.Windows.Forms.Cursors.Default;
            this.LauncherButtonRadiant.FocusCuesEnabled = false;
            this.LauncherButtonRadiant.Location = new System.Drawing.Point(6, 19);
            this.LauncherButtonRadiant.Name = "LauncherButtonRadiant";
            this.LauncherButtonRadiant.Size = new System.Drawing.Size(128, 32);
            this.LauncherButtonRadiant.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherButtonRadiant.TabIndex = 7;
            this.LauncherButtonRadiant.Text = "Radiant";
            this.LauncherButtonRadiant.TextColor = System.Drawing.Color.Black;
            this.LauncherButtonRadiant.Click += new System.EventHandler(this.LauncherButtonRadiant_Click);
            // 
            // LauncherSplitter
            // 
            this.LauncherSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LauncherSplitter.BackColor = System.Drawing.Color.Transparent;
            this.LauncherSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.LauncherSplitter.IsSplitterFixed = true;
            this.LauncherSplitter.Location = new System.Drawing.Point(9, 23);
            this.LauncherSplitter.Name = "LauncherSplitter";
            this.LauncherSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // LauncherSplitter.Panel1
            // 
            this.LauncherSplitter.Panel1.Controls.Add(this.LauncherApplicationsGroupBox);
            this.LauncherSplitter.Panel1.Controls.Add(this.LauncherTab);
            this.LauncherSplitter.Panel1MinSize = 380;
            // 
            // LauncherSplitter.Panel2
            // 
            this.LauncherSplitter.Panel2.Controls.Add(this.LauncherSaveConsoleButton);
            this.LauncherSplitter.Panel2.Controls.Add(this.LauncherConsole);
            this.LauncherSplitter.Panel2.Controls.Add(this.LauncherProcessTimeElapsedTextBox);
            this.LauncherSplitter.Panel2.Controls.Add(this.LauncherProcessTextBox);
            this.LauncherSplitter.Panel2.Controls.Add(this.LauncherProcessGroupBox);
            this.LauncherSplitter.Panel2MinSize = 100;
            this.LauncherSplitter.Size = new System.Drawing.Size(917, 693);
            this.LauncherSplitter.SplitterDistance = 380;
            this.LauncherSplitter.TabIndex = 1;
            // 
            // LauncherSaveConsoleButton
            // 
            this.LauncherSaveConsoleButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LauncherSaveConsoleButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LauncherSaveConsoleButton.ImageFixedSize = new System.Drawing.Size(15, 15);
            this.LauncherSaveConsoleButton.ImagePosition = DevComponents.DotNetBar.eImagePosition.Right;
            this.LauncherSaveConsoleButton.Location = new System.Drawing.Point(836, 257);
            this.LauncherSaveConsoleButton.Name = "LauncherSaveConsoleButton";
            this.LauncherSaveConsoleButton.Size = new System.Drawing.Size(38, 20);
            this.LauncherSaveConsoleButton.StopPulseOnMouseOver = false;
            this.LauncherSaveConsoleButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LauncherSaveConsoleButton.TabIndex = 6;
            this.LauncherSaveConsoleButton.Tooltip = "Save current console log";
            this.LauncherSaveConsoleButton.Click += new System.EventHandler(this.LauncherSaveConsoleButton_Click);
            // 
            // LauncherConsole
            // 
            // 
            // 
            // 
            this.LauncherConsole.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.LauncherConsole.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.LauncherConsole.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.LauncherConsole.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.LauncherConsole.BackgroundStyle.Class = "RichTextBoxBorder";
            this.LauncherConsole.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LauncherConsole.DetectUrls = false;
            this.LauncherConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LauncherConsole.Location = new System.Drawing.Point(150, 3);
            this.LauncherConsole.Margin = new System.Windows.Forms.Padding(0);
            this.LauncherConsole.Name = "LauncherConsole";
            this.LauncherConsole.ReadOnly = true;
            this.LauncherConsole.Rtf = resources.GetString("LauncherConsole.Rtf");
            this.LauncherConsole.Size = new System.Drawing.Size(765, 275);
            this.LauncherConsole.TabIndex = 5;
            this.LauncherConsole.WordWrap = false;
            this.LauncherConsole.ZoomFactor = 0.93F;
            // 
            // LauncherForm
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(938, 728);
            this.Controls.Add(this.LauncherAboutLabel);
            this.Controls.Add(this.LauncherWikiLabel);
            this.Controls.Add(this.LauncherSplitter);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LauncherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Launcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LauncherForm_FormClosing);
            this.Load += new System.EventHandler(this.LauncherForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LauncherMapFilesSystemWatcher)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LauncherModsDirectorySystemWatcher)).EndInit();
            this.LauncherProcessGroupBox.ResumeLayout(false);
            this.LauncherTab.ResumeLayout(false);
            this.LauncherTabCompileLevel.ResumeLayout(false);
            this.LauncherCompileLevelOptionsGroupBox.ResumeLayout(false);
            this.LauncherCompileLevelOptionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LauncherAssetCountGridView)).EndInit();
            this.LauncherGridFileGroupBox.ResumeLayout(false);
            this.LauncherGridFileGroupBox.PerformLayout();
            this.LauncherMapFFTypeGroupBox.ResumeLayout(false);
            this.LauncherMapFFTypeGroupBox.PerformLayout();
            this.LauncherTabModBuilder.ResumeLayout(false);
            this.LauncherIwdFileGroupBox.ResumeLayout(false);
            this.LauncherLocalizedCsvGroupBox.ResumeLayout(false);
            this.LauncherFastFileCsvGroupBox.ResumeLayout(false);
            this.LauncherModGroupBox.ResumeLayout(false);
            this.LauncherModGroupBox.PerformLayout();
            this.LauncherTabRunGame.ResumeLayout(false);
            this.LauncherRunGameCustomCommandLineGroupBox.ResumeLayout(false);
            this.LauncherRunGameCustomCommandLineGroupBox.PerformLayout();
            this.LauncherRunGameCommandLineGroupBox.ResumeLayout(false);
            this.LauncherRunGameModGroupBox.ResumeLayout(false);
            this.LauncherRunGameExeTypeGroupBox.ResumeLayout(false);
            this.LauncherRunGameExeTypeGroupBox.PerformLayout();
            this.LauncherApplicationsGroupBox.ResumeLayout(false);
            this.LauncherSplitter.Panel1.ResumeLayout(false);
            this.LauncherSplitter.Panel2.ResumeLayout(false);
            this.LauncherSplitter.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LauncherSplitter)).EndInit();
            this.LauncherSplitter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private bool IsMP()
		{
			return Launcher.IsMP(mapName);
		}

		private void LauncherAboutLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			MessageBoxEx.Show("Original launcher by\n     Mike Denny\n\nPC Programming Lead\n     Krassimir Touevsky\n\nPC Programming Team\n     Yanbing Chen\n     Juan Morelli\n     Ewan Oughton\n     Valeria Pelova\n     Dimiter \"malkia\" Stanev\n\nPC Production Team\n     Adam Saslow\n     Cesar Stastny\n\nPC Modding Team\n     Tony Kramer\n     Gavin Niebel\n     Alex 'Sparks' Romo\n***********************************\nUpdates by\n     DidUknowiPwn\n     Elfenlied\n     Icedream\n     master131\n     momo5502\n     SE2Dev\n***********************************\nSpecial Thanks to\n     Treyarch", "About Launcher V1.1");
		}

		private void LauncherButtonAssetManager_Click(object sender, EventArgs e)
		{
			LaunchProcess("asset_manager", "", null, consoleAttached: false, null);
		}

		private void LauncherButtonAssetViewer_Click(object sender, EventArgs e)
		{
			LaunchProcess("AssetViewer", "", null, consoleAttached: false, null);
		}

		private void LauncherButtonCancel_Click(object sender, EventArgs e)
		{
			int selectedIndex = LauncherProcessList.SelectedIndex;
			if (selectedIndex >= 0)
			{
				((Process)((DictionaryEntry)processList[selectedIndex]).Key).Kill();
			}
		}

		private void LauncherCreateMapButton_Click(object sender, EventArgs e)
		{
			if (new CreateMapForm().ShowDialog() == DialogResult.OK)
			{
				UpdateMapList();
				EnableMapList();
			}
		}

		private void LauncherButtonEffectsEd_Click(object sender, EventArgs e)
		{
			LaunchProcess("EffectsEd3", "", null, consoleAttached: false, null);
		}

		private void LauncherButtonRadiant_Click(object sender, EventArgs e)
		{
			LaunchProcess("CoDWaWRadiant", (mapName != null) ? Path.Combine(Launcher.GetMapSourceDirectory(), mapName + ".map") : "", null, consoleAttached: false, null);
		}

		private void LauncherButtonWeaponEditor_Click(object sender, EventArgs eventArgs)
		{
			LaunchProcess("WeaponEditor", null, null, consoleAttached: false, null);
		}

		private void LauncherButtonTest_Click(object sender, EventArgs e)
		{
			LaunchProcess("cmd.exe", "/c dir c:\\", null, consoleAttached: true, TestProcessFinishedDelegate);
		}

		private void LauncherClearConsoleButton_Click(object sender, EventArgs e)
		{
			LauncherConsole.Clear();
		}

		private void LauncherCompileBSPButton_Click(object sender, EventArgs e)
		{
			new BspOptionsForm().ShowDialog();
		}

		private void LauncherCompileLevelButton_Click(object sender, EventArgs e)
		{
			CompileLevel();
		}

		private void LauncherCompileLightsButton_Click(object sender, EventArgs e)
		{
			new LightOptionsForm().ShowDialog();
		}

		private void LauncherDeleteMapButton_Click(object sender, EventArgs e)
		{
			string[] mapFiles = Launcher.GetMapFiles(mapName);
			if (DialogResult.Yes == MessageBoxEx.Show("The following files would be deleted:\n\n" + Launcher.StringArrayToString(mapFiles), "Are you sure you want to delete these files?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
			{
				string[] array = mapFiles;
				foreach (string fileName in array)
				{
					Launcher.DeleteFile(fileName);
				}
				string[] mapFiles2 = Launcher.GetMapFiles(mapName);
				if (mapFiles2.Length > 0)
				{
					MessageBoxEx.Show("Could not delete the following files:\n\n" + Launcher.StringArrayToString(mapFiles2), "Error deleting files", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				UpdateMapList();
				EnableMapList();
			}
		}

		private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (processTable.Count != 0)
			{
				switch (MessageBox.Show("But there are still processes running!\n\nDo you want to close them, or cancel exiting from the application?", "Application will exit!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
				{
				case DialogResult.Cancel:
					e.Cancel = true;
					return;
				case DialogResult.Yes:
					foreach (DictionaryEntry item in processTable)
					{
						((Process)item.Key).Kill();
					}
					break;
				default:
				{
					string[] array = new string[processTable.Count];
					int num = 0;
					foreach (DictionaryEntry item2 in processTable)
					{
						try
						{
							array[num] = ((Process)item2.Key).MainModule.FileName;
						}
						catch
						{
							array[num] = (string)item2.Value;
						}
						num++;
					}
					if (array.Length > 0)
					{
						MessageBoxEx.Show("The following processes are still active:\n\n" + Launcher.StringArrayToString(array) + "\nPlease close them if neccessary using the Task Manager, or similar program!\n", "Note before exiting the application", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					break;
				}
				}
			}
			UpdateMapSettings();
		}

		private void LauncherForm_Load(object sender, EventArgs e)
		{
			StyleManager.MetroColorGeneratorParameters = new MetroColorGeneratorParameters(Color.White, Color.Black);
			UpdateDVars();
			UpdateMapList();
			UpdateModList();
			EnableMapList();
			UpdateStopProcessButton();
			LauncherMapFilesSystemWatcher.Path = Launcher.GetMapSourceDirectory();
			LauncherModsDirectorySystemWatcher.Path = Launcher.GetModsDirectory();
			LauncherMapFilesSystemWatcher.EnableRaisingEvents = true;
			LauncherModsDirectorySystemWatcher.EnableRaisingEvents = true;
			Text = Text + " V1.1 - " + Launcher.GetRootDirectory();
			LauncherModLanguageComboBox.SelectedIndex = 0;
			LauncherModLanguageComboBox.SelectedIndexChanged += LauncherModLanguageComboBox_SelectedIndexChanged;
		}

		private void LauncherModLanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (LauncherModLanguageComboBox.SelectedItem != null)
			{
				Launcher.selectedLanguage = LauncherModLanguageComboBox.SelectedItem.ToString();
				LauncherLocalizedCsvGroupBox.Text = "Localized FF localized_mod.csv (" + Launcher.selectedLanguage + ")";
			}
		}

		private void LauncherGameOptionsFlowPanel_Click(object sender, EventArgs e)
		{
		}

		private void LauncherGridEditExistingButton_Click(object sender, EventArgs e)
		{
			BuildGridDelegate(2);
		}

		private void LauncherGridMakeNewButton_Click(object sender, EventArgs e)
		{
			BuildGridDelegate(1);
		}

		private void LauncherIwdFileTree_AfterCheck(object sender, TreeViewEventArgs e)
		{
			LauncherIwdFileTreeBeginUpdate();
			RecursiveCheckNodesDown(e.Node.Nodes, e.Node.Checked);
			if (e.Node.Checked)
			{
				RecursiveCheckNodesUp(e.Node.Parent, e.Node.Checked);
			}
			LauncherIwdFileTreeEndUpdate();
		}

		private void LauncherIwdFileTree_DoubleClick(object sender, EventArgs e)
		{
			if (LauncherIwdFileTree.SelectedNode != null)
			{
				try
				{
					Process process = new Process();
					process.StartInfo.ErrorDialog = true;
					process.StartInfo.FileName = Path.Combine(Launcher.GetModDirectory(modName), LauncherIwdFileTree.SelectedNode.FullPath);
					process.Start();
				}
				catch
				{
				}
			}
		}

		private void LauncherIwdFileTreeBeginUpdate()
		{
			LauncherIwdFileTree.BeginUpdate();
			LauncherIwdFileTree.AfterCheck -= LauncherIwdFileTree_AfterCheck;
		}

		private void LauncherIwdFileTreeEndUpdate()
		{
			LauncherIwdFileTree.AfterCheck += LauncherIwdFileTree_AfterCheck;
			LauncherIwdFileTree.EndUpdate();
		}

		private void LauncherMapFilesSystemWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			UpdateMapList();
		}

		private void LauncherMapFilesSystemWatcher_Created(object sender, FileSystemEventArgs e)
		{
			UpdateMapList();
		}

		private void LauncherMapFilesSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
		{
			UpdateMapList();
		}

		private void LauncherMapFilesSystemWatcher_Renamed(object sender, RenamedEventArgs e)
		{
			UpdateMapList();
		}

		private void LauncherMapList_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateMapSettings();
			EnableMapList();
			ReadMapAssetListFile(mapName);
			if (mapName.StartsWith("localized") || mapName.EndsWith("patch") || mapName.EndsWith("load"))
			{
				LauncherCompileVisCheckBox.Enabled = false;
				LauncherCompileBSPCheckBox.Enabled = false;
				LauncherConnectPathsCheckBox.Enabled = false;
				LauncherCompileLightsCheckBox.Enabled = false;
				LauncherCompileReflectionsCheckBox.Enabled = false;
				LauncherBspInfoCheckBox.Enabled = false;
				LauncherRunMapAfterCompileCheckBox.Enabled = false;
				return;
			}
			if (LauncherMapFFTypeMP.Checked)
			{
				LauncherCompileVisCheckBox.Enabled = true;
			}
			else
			{
				LauncherCompileVisCheckBox.Enabled = false;
			}
			LauncherCompileBSPCheckBox.Enabled = true;
			LauncherConnectPathsCheckBox.Enabled = true;
			LauncherCompileLightsCheckBox.Enabled = true;
			LauncherCompileReflectionsCheckBox.Enabled = true;
			LauncherBspInfoCheckBox.Enabled = true;
			LauncherRunMapAfterCompileCheckBox.Enabled = true;
		}

		private void LauncherModBuildButton_Click(object sender, EventArgs e)
		{
			if (LauncherModBuildFastFilesCheckBox.Checked || LauncherModBuildIwdFileCheckBox.Checked || LauncherModBuildSoundsCheckBox.Checked)
			{
				LauncherModComboBoxApplySettings();
				ModBuildStart();
			}
		}

		private void LauncherModComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			LauncherModComboBoxApplySettings();
		}

		private void LauncherModComboBoxApplySettings()
		{
			LauncherIwdFileTreeBeginUpdate();
			if (modName != null)
			{
				string textFile = Path.Combine(Launcher.GetModDirectory(modName), modName + ".files");
				string textFile2 = Path.Combine(Launcher.GetModDirectory(modName), "mod.csv");
				string localizedFrenchCsvFile = Path.Combine(Launcher.GetModDirectory(modName), "localized_french_mod.csv");
				string localizedEnglishCsvFile = Path.Combine(Launcher.GetModDirectory(modName), "localized_english_mod.csv");
				string localizedGermanCsvFile = Path.Combine(Launcher.GetModDirectory(modName), "localized_german_mod.csv");
				string localizedItalianCsvFile = Path.Combine(Launcher.GetModDirectory(modName), "localized_italian_mod.csv");
				string localizedSpanishCsvFile = Path.Combine(Launcher.GetModDirectory(modName), "localized_spanish_mod.csv");
				string localizedRussianCsvFile = Path.Combine(Launcher.GetModDirectory(modName), "localized_russian_mod.csv");
				string localizedKoreanCsvFile = Path.Combine(Launcher.GetModDirectory(modName), "localized_korean_mod.csv");
				string localizedPolishCsvFile = Path.Combine(Launcher.GetModDirectory(modName), "localized_polish_mod.csv");
				Launcher.SaveTextFile(textFile, Launcher.HashTableToStringArray(TreeViewToHashTable(LauncherIwdFileTree.Nodes)));
				Launcher.SaveTextFile(textFile2, LauncherFastFileCsvTextBox.Lines);
				Launcher.SaveTextFile(localizedFrenchCsvFile, LauncherLocalizedCsvTextBox.Lines);
				Launcher.SaveTextFile(localizedEnglishCsvFile, LauncherLocalizedCsvTextBox.Lines);
				Launcher.SaveTextFile(localizedGermanCsvFile, LauncherLocalizedCsvTextBox.Lines);
				Launcher.SaveTextFile(localizedItalianCsvFile, LauncherLocalizedCsvTextBox.Lines);
				Launcher.SaveTextFile(localizedSpanishCsvFile, LauncherLocalizedCsvTextBox.Lines);
				Launcher.SaveTextFile(localizedRussianCsvFile, LauncherLocalizedCsvTextBox.Lines);
				Launcher.SaveTextFile(localizedKoreanCsvFile, LauncherLocalizedCsvTextBox.Lines);
				Launcher.SaveTextFile(localizedPolishCsvFile, LauncherLocalizedCsvTextBox.Lines);
			}
			if (LauncherModComboBox.SelectedItem != null)
			{
				modName = LauncherModComboBox.SelectedItem.ToString();
				string textFile3 = Path.Combine(Launcher.GetModDirectory(modName), modName + ".files");
				string textFile4 = Path.Combine(Launcher.GetModDirectory(modName), "mod.csv");
				string localizedCsvFile = Path.Combine(Launcher.GetModDirectory(modName), "localized_"+Launcher.selectedLanguage +"_mod.csv");
				LauncherIwdFileTree.Nodes.Clear();
				AddFilesToTreeView(Launcher.GetModDirectory(modName), LauncherIwdFileTree.Nodes, firstTime: true);
				HashTableToTreeView(Launcher.StringArrayToHashTable(Launcher.LoadTextFile(textFile3)), LauncherIwdFileTree.Nodes);
				LauncherFastFileCsvTextBox.Lines = Launcher.LoadTextFile(textFile4);
				LauncherLocalizedCsvTextBox.Lines = Launcher.LoadTextFile(localizedCsvFile);
			}
			LauncherIwdFileTreeEndUpdate();
		}

		private void LauncherModsDirectorySystemWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			UpdateModList();
		}

		private void LauncherModsDirectorySystemWatcher_Created(object sender, FileSystemEventArgs e)
		{
			UpdateModList();
		}

		private void LauncherModsDirectorySystemWatcher_Deleted(object sender, FileSystemEventArgs e)
		{
			UpdateModList();
		}

		private void LauncherModsDirectorySystemWatcher_Renamed(object sender, RenamedEventArgs e)
		{
			UpdateModList();
		}

		private void LauncherProcessList_ItemClick(object sender, EventArgs e)
		{
			UpdateStopProcessButton();
		}

		private void LauncherRunGameButton_Click(object sender, EventArgs e)
		{
			ComboBox[] array = dvarComboBoxes;
			foreach (ComboBox comboBox in array)
			{
				string text = comboBox.Text.Trim();
				if (text != "")
				{
					foreach (string item in comboBox.Items)
					{
						if (text.ToLower() == item.ToLower())
						{
							text = "";
							break;
						}
					}
				}
				if (text != "")
				{
					comboBox.Items.Add(comboBox.Text);
				}
			}
			LaunchProcess(Launcher.GetGameApplication(!LauncherRunGameTypeRadioButton.Checked), GetGameOptions(), null, consoleAttached: false, null);
		}

		private void LauncherTimer_Tick(object sender, EventArgs e)
		{
			if (consoleProcess != null)
			{
				TimeSpan timeSpan = DateTime.Now - consoleProcessStartTime;
				LauncherProcessTimeElapsedTextBox.Text = timeSpan.ToString().Substring(0, 8);
			}
			string gameOptions = GetGameOptions();
			if (LauncherRunGameCommandLineTextBox.Text != gameOptions)
			{
				LauncherRunGameCommandLineTextBox.Text = gameOptions;
			}
		}

		private void LauncherWikiLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://wiki.modsrepository.com/");
		}

		private void LaunchProcessHelper(bool shouldRun, ProcessFinishedDelegate nextStage, Process lastProcess, string processName, string processOptions, string workingDirectory)
		{
			if ((lastProcess != null && lastProcess.ExitCode != 0) || !shouldRun)
			{
				nextStage(lastProcess);
			}
			else
			{
				LaunchProcess(processName, processOptions, workingDirectory, consoleAttached: true, nextStage);
			}
		}

		private void LaunchProcessHelper(bool shouldRun, ProcessFinishedDelegate nextStage, Process lastProcess, string processName, string processOptions)
		{
			LaunchProcessHelper(shouldRun, nextStage, lastProcess, processName, processOptions, null);
		}

		private void LaunchProcess(string processFileName, string arguments, string workingDirectory, bool consoleAttached, ProcessFinishedDelegate theProcessFinishedDelegate)
		{
			if (consoleProcess != null && consoleAttached)
			{
				LauncherConsole.Invoke((MethodInvoker)delegate
				{
					string text = ((processFileName == (string)processTable[consoleProcess]) ? ("Console process (" + processFileName + ") is already running!") : string.Concat("Cannot start console process ", processFileName, "!\n\nAnother console process (", processTable[consoleProcess], ") is already running"));
					MessageBox.Show(text, "Console Busy", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				});
				return;
			}
			try
			{
				Process p = new Process();
				p.StartInfo.FileName = Path.Combine(Launcher.GetStartupDirectory(), processFileName);
				p.StartInfo.CreateNoWindow = true;
				p.StartInfo.Arguments = arguments;
				p.StartInfo.UseShellExecute = false;
				p.StartInfo.WorkingDirectory = ((workingDirectory != null) ? workingDirectory : Path.GetDirectoryName(p.StartInfo.FileName));
				p.EnableRaisingEvents = true;
				p.Exited += delegate
				{
					processTable.Remove(p);
					UpdateProcessList();
				};
				if (consoleAttached)
				{
					this.processFinishedDelegate = theProcessFinishedDelegate;
					p.StartInfo.RedirectStandardError = true;
					p.StartInfo.RedirectStandardOutput = true;
					p.OutputDataReceived += delegate(object s, DataReceivedEventArgs e)
					{
						WriteConsole(e.Data, isStdError: false);
					};
					p.ErrorDataReceived += delegate(object s, DataReceivedEventArgs e)
					{
						WriteConsole(e.Data, isStdError: true);
					};
					p.Exited += delegate
					{
						LauncherButtonCancel.Invoke((MethodInvoker)delegate
						{
							LauncherProcessTimeElapsedTextBox.Text = ((p.ExitCode != 0) ? ("Error " + p.ExitCode) : "Success");
							LauncherConsole.Focus();
							consoleProcess = null;
							UpdateConsoleColor();
							if (this.processFinishedDelegate != null)
							{
								ProcessFinishedDelegate processFinishedDelegate2 = this.processFinishedDelegate;
								this.processFinishedDelegate = null;
								processFinishedDelegate2(p);
							}
						});
					};
				}
				p.Exited += delegate
				{
					p.Dispose();
				};
				p.Start();
				if (consoleAttached)
				{
					consoleProcess = p;
					consoleProcessStartTime = DateTime.Now;
					UpdateConsoleColor();
					LauncherProcessTextBox.Text = ((workingDirectory != null) ? (workingDirectory + "> ") : "") + processFileName + " " + arguments;
					p.BeginOutputReadLine();
					p.BeginErrorReadLine();
				}
				processTable.Add(p, processFileName);
				UpdateProcessList();
			}
			catch
			{
				WriteConsole("FAILED TO EXECUTE: " + processFileName + " " + arguments, isStdError: true);
				if (this.processFinishedDelegate != null)
				{
					ProcessFinishedDelegate processFinishedDelegate = this.processFinishedDelegate;
					this.processFinishedDelegate = null;
					processFinishedDelegate(null);
				}
			}
		}

		private void ModBuildFastFileDelegate(Process lastProcess)
		{
			if (LauncherModBuildFastFilesCheckBox.Checked)
			{
				if (modName == null)
				{
					return;
				}
				string text = Path.Combine(Launcher.GetModDirectory(modName), "mod.csv");
				string text2 = Path.Combine(Launcher.GetZoneSourceDirectory(), "mod.csv");
				if (text == null || text2 == null)
				{
					return;
				}
				Launcher.CopyFileSmart(text, text2);
			}
			LaunchProcessHelper(LauncherModBuildFastFilesCheckBox.Checked, ModBuildMoveModFastFileDelegate, lastProcess, "linker_pc", "-nopause -language " + Launcher.GetLanguage() + " -moddir " + modName + " mod");
		}

		private void ModBuildFinishedDelegate(Process lastProcess)
		{
			Launcher.PublishModOnly();
			EnableControls(enabled: true);
		}

		private void ModBuildIwdFileDelegate(Process lastProcess)
		{
			string text = Path.Combine(Launcher.GetModDirectory(modName), modName + ".iwd");
			if (LauncherModBuildIwdFileCheckBox.Checked)
			{
				Launcher.DeleteFile(text, verbose: false);
			}
			LaunchProcessHelper(LauncherModBuildIwdFileCheckBox.Checked, ModBuildFinishedDelegate, lastProcess, "7za", "a \"" + text + "\" -tzip -r \"@" + Path.Combine(Launcher.GetModDirectory(modName), modName + ".files") + "\"", Launcher.GetModDirectory(modName));
		}

		private void ModBuildMoveModFastFileDelegate(Process lastProcess)
		{
			if (LauncherModBuildFastFilesCheckBox.Checked)
			{
				Launcher.MoveFile(Path.Combine(Launcher.GetZoneDirectory(), "mod.ff"), Path.Combine(Launcher.GetModDirectory(modName), "mod.ff"));
			}
			ModBuildIwdFileDelegate(lastProcess);
		}

		private void LauncherModBuildLocalizedButton_Click(object sender, EventArgs e)
		{
			if (modName != null)
			{
				LauncherModComboBoxApplySettings();
				EnableControls(enabled: false);
				ModBuildLocalizedFastFileDelegate(null);
			}
		}

		private void ModBuildLocalizedFastFileDelegate(Process lastProcess)
		{
			if (modName == null)
			{
				EnableControls(enabled: true);
				return;
			}

			if (File.Exists(Path.Combine(Launcher.GetModDirectory(modName), "localized_english_mod.csv")))
			{
				string localizedCsvSrc = Path.Combine(Launcher.GetModDirectory(modName), "localized_english_mod.csv");
				string localizedCsvDst = Path.Combine(Launcher.GetZoneSourceDirectory(), "localized_english_mod.csv");
				Launcher.SaveTextFile(localizedCsvSrc, LauncherLocalizedCsvTextBox.Lines);
				if (localizedCsvSrc != null && localizedCsvDst != null)
				{
					Launcher.CopyFileSmart(localizedCsvSrc, localizedCsvDst);
				}
				LaunchProcessHelper(true, BuildFrenchModLocalized, lastProcess, "linker_pc", "-nopause -language " + "english" + " -moddir " + modName + " localized_english_mod");
			}
			
		}


		private void BuildFrenchModLocalized(Process lastProcess)
		{
			if (File.Exists(Path.Combine(Launcher.GetModDirectory(modName), "localized_french_mod.csv")))
			{
				string localizedCsvSrc = Path.Combine(Launcher.GetModDirectory(modName), "localized_french_mod.csv");
				string localizedCsvDst = Path.Combine(Launcher.GetZoneSourceDirectory(), "localized_french_mod.csv");
				Launcher.SaveTextFile(localizedCsvSrc, LauncherLocalizedCsvTextBox.Lines);
				if (localizedCsvSrc != null && localizedCsvDst != null)
				{
					Launcher.CopyFileSmart(localizedCsvSrc, localizedCsvDst);
				}
				LaunchProcessHelper(true, BuildItalianLocalized, lastProcess, "linker_pc", "-nopause -language french -moddir " + modName + " localized_french_mod");
			}
		}
		
		private void BuildItalianLocalized(Process lastProcess)
		{
			if (File.Exists(Path.Combine(Launcher.GetModDirectory(modName), "localized_italian_mod.csv")))
			{
				string localizedCsvSrc = Path.Combine(Launcher.GetModDirectory(modName), "localized_italian_mod.csv");
				string localizedCsvDst = Path.Combine(Launcher.GetZoneSourceDirectory(), "localized_italian_mod.csv");
				Launcher.SaveTextFile(localizedCsvSrc, LauncherLocalizedCsvTextBox.Lines);
				if (localizedCsvSrc != null && localizedCsvDst != null)
				{
					Launcher.CopyFileSmart(localizedCsvSrc, localizedCsvDst);
				}
				LaunchProcessHelper(true, BuildSpanishLocalized, lastProcess, "linker_pc", "-nopause -language italian -moddir " + modName + " localized_italian_mod");
			}
		}

		private void BuildSpanishLocalized(Process lastProcess)
		{
			if (File.Exists(Path.Combine(Launcher.GetModDirectory(modName), "localized_spanish_mod.csv")))
			{
				string localizedCsvSrc = Path.Combine(Launcher.GetModDirectory(modName), "localized_spanish_mod.csv");
				string localizedCsvDst = Path.Combine(Launcher.GetZoneSourceDirectory(), "localized_spanish_mod.csv");
				Launcher.SaveTextFile(localizedCsvSrc, LauncherLocalizedCsvTextBox.Lines);
				if (localizedCsvSrc != null && localizedCsvDst != null)
				{
					Launcher.CopyFileSmart(localizedCsvSrc, localizedCsvDst);
				}
				LaunchProcessHelper(true, BuildGermanLocalized, lastProcess, "linker_pc", "-nopause -language spanish -moddir " + modName + " localized_spanish_mod");
			}
		}

		private void BuildGermanLocalized(Process lastProcess)
		{
			if (File.Exists(Path.Combine(Launcher.GetModDirectory(modName), "localized_german_mod.csv")))
			{
				string localizedCsvSrc = Path.Combine(Launcher.GetModDirectory(modName), "localized_german_mod.csv");
				string localizedCsvDst = Path.Combine(Launcher.GetZoneSourceDirectory(), "localized_german_mod.csv");
				Launcher.SaveTextFile(localizedCsvSrc, LauncherLocalizedCsvTextBox.Lines);
				if (localizedCsvSrc != null && localizedCsvDst != null)
				{
					Launcher.CopyFileSmart(localizedCsvSrc, localizedCsvDst);
				}
				LaunchProcessHelper(true, BuildRussianLocalized, lastProcess, "linker_pc", "-nopause -language german -moddir " + modName + " localized_german_mod");
			}
		}
		
		private void BuildRussianLocalized(Process lastProcess)
		{
			if (File.Exists(Path.Combine(Launcher.GetModDirectory(modName), "localized_russian_mod.csv")))
			{
				string localizedCsvSrc = Path.Combine(Launcher.GetModDirectory(modName), "localized_russian_mod.csv");
				string localizedCsvDst = Path.Combine(Launcher.GetZoneSourceDirectory(), "localized_russian_mod.csv");
				Launcher.SaveTextFile(localizedCsvSrc, LauncherLocalizedCsvTextBox.Lines);
				if (localizedCsvSrc != null && localizedCsvDst != null)
				{
					Launcher.CopyFileSmart(localizedCsvSrc, localizedCsvDst);
				}
				LaunchProcessHelper(true, BuildPolishLocalized, lastProcess, "linker_pc", "-nopause -language russian -moddir " + modName + " localized_russian_mod");
			}
		}

		private void BuildPolishLocalized(Process lastProcess)
		{
			if (File.Exists(Path.Combine(Launcher.GetModDirectory(modName), "localized_polish_mod.csv")))
			{
				string localizedCsvSrc = Path.Combine(Launcher.GetModDirectory(modName), "localized_polish_mod.csv");
				string localizedCsvDst = Path.Combine(Launcher.GetZoneSourceDirectory(), "localized_polish_mod.csv");
				Launcher.SaveTextFile(localizedCsvSrc, LauncherLocalizedCsvTextBox.Lines);
				if (localizedCsvSrc != null && localizedCsvDst != null)
				{
					Launcher.CopyFileSmart(localizedCsvSrc, localizedCsvDst);
				}
				LaunchProcessHelper(true, BuildKoreanLocalized, lastProcess, "linker_pc", "-nopause -language polish -moddir " + modName + " localized_polish_mod");
			}
		}
		
		private void BuildKoreanLocalized(Process lastProcess)
		{
			if (File.Exists(Path.Combine(Launcher.GetModDirectory(modName), "localized_korean_mod.csv")))
			{
				string localizedCsvSrc = Path.Combine(Launcher.GetModDirectory(modName), "localized_korean_mod.csv");
				string localizedCsvDst = Path.Combine(Launcher.GetZoneSourceDirectory(), "localized_korean_mod.csv");
				Launcher.SaveTextFile(localizedCsvSrc, LauncherLocalizedCsvTextBox.Lines);
				if (localizedCsvSrc != null && localizedCsvDst != null)
				{
					Launcher.CopyFileSmart(localizedCsvSrc, localizedCsvDst);
				}
				LaunchProcessHelper(true, ModBuildMoveLocalizedModFastFileDelegate, lastProcess, "linker_pc", "-nopause -language korean -moddir " + modName + " localized_korean_mod");
			}
		}
		
		private void ModBuildMoveLocalizedModFastFileDelegate(Process lastProcess)
		{
			Launcher.MoveFile(Path.Combine(Launcher.GetZoneDirectory("french"), "localized_french_mod.ff"), Path.Combine(Launcher.GetModDirectory(modName), "localized_french_mod.ff"));
			Launcher.MoveFile(Path.Combine(Launcher.GetZoneDirectory("english"), "localized_english_mod.ff"), Path.Combine(Launcher.GetModDirectory(modName), "localized_english_mod.ff"));
			Launcher.MoveFile(Path.Combine(Launcher.GetZoneDirectory("italian"), "localized_italian_mod.ff"), Path.Combine(Launcher.GetModDirectory(modName), "localized_italian_mod.ff"));
			Launcher.MoveFile(Path.Combine(Launcher.GetZoneDirectory("spanish"), "localized_spanish_mod.ff"), Path.Combine(Launcher.GetModDirectory(modName), "localized_spanish_mod.ff"));
			Launcher.MoveFile(Path.Combine(Launcher.GetZoneDirectory("german"), "localized_german_mod.ff"), Path.Combine(Launcher.GetModDirectory(modName), "localized_german_mod.ff"));
			Launcher.MoveFile(Path.Combine(Launcher.GetZoneDirectory("polish"), "localized_polish_mod.ff"), Path.Combine(Launcher.GetModDirectory(modName), "localized_polish_mod.ff"));
			Launcher.MoveFile(Path.Combine(Launcher.GetZoneDirectory("russian"), "localized_russian_mod.ff"), Path.Combine(Launcher.GetModDirectory(modName), "localized_russian_mod.ff"));
			Launcher.MoveFile(Path.Combine(Launcher.GetZoneDirectory("korean"), "localized_korean_mod.ff"), Path.Combine(Launcher.GetModDirectory(modName), "localized_korean_mod.ff"));
			Launcher.PublishModOnly();
			EnableControls(enabled: true);
		}

		private void ModBuildSoundDelegate(Process lastProcess)
		{
			LaunchProcessHelper(LauncherModBuildSoundsCheckBox.Checked, ModBuildFastFileDelegate, lastProcess, "MODSound", "-pc -ignore_orphans " + (LauncherModVerboseCheckBox.Checked ? "-verbose " : ""));
		}

		private void ModBuildStart()
		{
			if (modName != null)
			{
				EnableControls(enabled: false);
				ModBuildSoundDelegate(null);
			}
		}

		private void RecursiveCheckNodesDown(TreeNodeCollection tree, bool checkedFlag)
		{
			if (tree == null)
			{
				return;
			}
			foreach (TreeNode item in tree)
			{
				TreeNodeCollection nodes = item.Nodes;
				bool checkedFlag2 = (item.Checked = checkedFlag);
				RecursiveCheckNodesDown(nodes, checkedFlag2);
			}
		}

		private void RecursiveCheckNodesUp(TreeNode node, bool checkedFlag)
		{
			if (node != null)
			{
				TreeNode node2 = node.Parent;
				bool checkedFlag2 = (node.Checked = checkedFlag);
				RecursiveCheckNodesUp(node2, checkedFlag2);
			}
		}

		private void TestProcessFinishedDelegate(Process p)
		{
			LaunchProcess("help.exe", "", null, consoleAttached: true, null);
		}

		private Hashtable TreeViewToHashTable(TreeNodeCollection tree)
		{
			Hashtable hashtable = new Hashtable();
			TreeViewToHashTable(tree, hashtable);
			return hashtable;
		}

		private void TreeViewToHashTable(TreeNodeCollection tree, Hashtable ht)
		{
			if (tree == null)
			{
				return;
			}
			foreach (TreeNode item in tree)
			{
				if (item.Checked && item.Tag != null)
				{
					ht.Add(item.FullPath, null);
				}
				else
				{
					ht.Remove(item.FullPath);
				}
				TreeViewToHashTable(item.Nodes, ht);
			}
		}

		private void UpdateConsoleColor()
		{
			LauncherConsole.BackColor = ((consoleProcess == null) ? Color.White : Color.LightGoldenrodYellow);
		}

		private void UpdateDVars()
		{
			Panel launcherGameOptionsPanel = LauncherGameOptionsPanel;
			int num = 34;
			int num2 = -num;
			int num3 = 0;
			bool flag = true;
			Color backColor = launcherGameOptionsPanel.BackColor;
			Color color = Color.FromArgb(backColor.R * 14 / 15, backColor.G * 14 / 15, backColor.B * 14 / 15);
			dvarComboBoxes = new ComboBox[Launcher.dvars.Length];
			DVar[] dvars = Launcher.dvars;
			for (int i = 0; i < dvars.Length; i++)
			{
				DVar dVar = dvars[i];
				Panel panel = new Panel();
				panel.SetBounds(0, num2 += num, launcherGameOptionsPanel.ClientSize.Width, num);
				panel.BackColor = ((flag = !flag) ? backColor : color);
				panel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				panel.Click += LauncherGameOptionsFlowPanel_Click;
				Label label = new Label();
				label.SetBounds(4, 0, launcherGameOptionsPanel.ClientSize.Width - 220, num);
				label.TextAlign = ContentAlignment.MiddleLeft;
				label.Text = dVar.name + " (" + dVar.description + ")";
				label.Click += LauncherGameOptionsFlowPanel_Click;
				panel.Controls.Add(label);
				ComboBox comboBox = new ComboBox();
				comboBox.Tag = dVar.name;
				ComboBox comboBox2 = comboBox;
				comboBox2.Items.Add("(not set)");
				comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
				comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
				if (dVar.isDecimal)
				{
					for (decimal decimalMin = dVar.decimalMin; decimalMin <= dVar.decimalMax; decimalMin += dVar.decimalIncrement)
					{
						comboBox2.Items.Add(decimalMin.ToString());
					}
				}
				else if (dVar.name == "devmap")
				{
					comboBox2.Sorted = true;
					comboBox2.Items.AddRange(Launcher.GetZoneFastFiles());
					comboBox2.Items.AddRange(Launcher.GetCompiledMapList(ignoreBuilds: true));
				}
				comboBox2.SelectedIndex = 0;
				comboBox2.SetBounds(launcherGameOptionsPanel.ClientSize.Width - 205, 8, 200, num);
				comboBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
				panel.Controls.Add(comboBox2);
				comboBox2.BringToFront();
				launcherGameOptionsPanel.Controls.Add(panel);
				dvarComboBoxes[num3++] = comboBox2;
				comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
			}
		}

		private void UpdateMapList()
		{
			object selectedItem = LauncherMapList.SelectedItem;
			int selectedIndex = LauncherMapList.SelectedIndex;
			LauncherMapList.Items.Clear();
			LauncherMapList.Items.AddRange(Launcher.GetCompiledMapList(ignoreBuilds: false, !LauncherMapFFTypeMP.Checked));
			if (LauncherMapList.Items.Count != 0)
			{
				LauncherMapList.SelectedItem = selectedItem;
				if (LauncherMapList.SelectedItem == null)
				{
					LauncherMapList.SelectedIndex = Math.Max(0, Math.Min(selectedIndex, LauncherMapList.Items.Count - 1));
				}
			}
		}

		private void UpdateMapSettings()
		{
			if (mapName != null)
			{
				Launcher.mapSettings.SetBoolean("compile_bsp", LauncherCompileBSPCheckBox.Checked);
				Launcher.mapSettings.SetBoolean("compile_lights", LauncherCompileLightsCheckBox.Checked);
				Launcher.mapSettings.SetBoolean("compile_vis", LauncherCompileVisCheckBox.Checked);
				Launcher.mapSettings.SetBoolean("compile_paths", LauncherConnectPathsCheckBox.Checked);
				Launcher.mapSettings.SetBoolean("compile_reflections", LauncherCompileReflectionsCheckBox.Checked);
				Launcher.mapSettings.SetBoolean("compile_buildffs", LauncherBuildFastFilesCheckBox.Checked);
				Launcher.mapSettings.SetBoolean("compile_bspinfo", LauncherBspInfoCheckBox.Checked);
				Launcher.mapSettings.SetBoolean("compile_runafter", LauncherRunMapAfterCompileCheckBox.Checked);
				Launcher.mapSettings.SetBoolean("compile_modenabled", LauncherModSpecificMapCheckBox.Checked);
				Launcher.mapSettings.SetString("compile_modname", LauncherModSpecificMapComboBox.Text);
				Launcher.mapSettings.SetBoolean("compile_collectdots", LauncherGridCollectDotsCheckBox.Checked);
				Launcher.SaveMapSettings(mapName, Launcher.mapSettings.Get());
				mapName = null;
			}
			if (LauncherMapList.SelectedItem != null)
			{
				mapName = LauncherMapList.SelectedItem.ToString();
				Launcher.mapSettings.Set(Launcher.LoadMapSettings(mapName));
				LauncherCompileBSPCheckBox.Checked = Launcher.mapSettings.GetBoolean("compile_bsp");
				LauncherCompileLightsCheckBox.Checked = Launcher.mapSettings.GetBoolean("compile_lights");
				LauncherCompileVisCheckBox.Checked = Launcher.mapSettings.GetBoolean("compile_vis");
				LauncherConnectPathsCheckBox.Checked = Launcher.mapSettings.GetBoolean("compile_paths");
				LauncherCompileReflectionsCheckBox.Checked = Launcher.mapSettings.GetBoolean("compile_reflections");
				LauncherBuildFastFilesCheckBox.Checked = Launcher.mapSettings.GetBoolean("compile_buildffs");
				LauncherBspInfoCheckBox.Checked = Launcher.mapSettings.GetBoolean("compile_bspinfo");
				LauncherRunMapAfterCompileCheckBox.Checked = Launcher.mapSettings.GetBoolean("compile_runafter");
				LauncherModSpecificMapCheckBox.Checked = Launcher.mapSettings.GetBoolean("compile_modenabled");
				LauncherModSpecificMapComboBox.Text = Launcher.mapSettings.GetString("compile_modname");
				LauncherGridCollectDotsCheckBox.Checked = Launcher.mapSettings.GetBoolean("compile_collectdots");
			}
		}

		private void UpdateModList()
		{
			ComboBox[] array = new ComboBox[3] { LauncherRunGameModComboBox, LauncherModComboBox, LauncherModSpecificMapComboBox };
			string[] modList = Launcher.GetModList();
			ComboBox[] array2 = array;
			foreach (ComboBox comboBox in array2)
			{
				comboBox.Items.Clear();
			}
			LauncherRunGameModComboBox.Items.Add("(not set)");
			ComboBox[] array3 = array;
			foreach (ComboBox comboBox2 in array3)
			{
				comboBox2.Items.AddRange(modList);
				if (comboBox2.Items.Count > 0)
				{
					comboBox2.SelectedIndex = 0;
				}
			}
		}

		private void UpdateProcessList()
		{
			LauncherProcessList.Invoke((MethodInvoker)delegate
			{
				processList.Clear();
				LauncherProcessList.Items.Clear();
				foreach (DictionaryEntry item in processTable)
				{
					processList.Add(item);
					LauncherProcessList.Items.Add(Path.GetFileNameWithoutExtension((string)item.Value));
				}
				if (LauncherProcessList.SelectedIndex < 0 && LauncherProcessList.Items.Count > 0)
				{
					LauncherProcessList.SelectedIndex = 0;
				}
				UpdateStopProcessButton();
			});
		}

		private void UpdateRunGameCommandLine()
		{
		}

		private void UpdateStopProcessButton()
		{
			int selectedIndex = LauncherProcessList.SelectedIndex;
			if (selectedIndex < 0)
			{
				LauncherButtonCancel.Enabled = false;
				LauncherButtonCancel.Text = "No Active Process\n\nStart one and then use this button to stop it";
				return;
			}
			LauncherButtonCancel.Enabled = true;
			if (((DictionaryEntry)processList[selectedIndex]).Key == consoleProcess)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)processList[selectedIndex];
				LauncherButtonCancel.Text = "Stop Console Process\n\n" + Path.GetFileNameWithoutExtension(dictionaryEntry.Value.ToString());
			}
			else
			{
				DictionaryEntry dictionaryEntry2 = (DictionaryEntry)processList[selectedIndex];
				LauncherButtonCancel.Text = "Stop Application\n\n" + Path.GetFileNameWithoutExtension(dictionaryEntry2.Value.ToString());
			}
		}

		private void WriteConsole(string s, bool isStdError)
		{
			if (s == null)
			{
				return;
			}
			long ticks = DateTime.Now.Ticks;
			bool doFocus = ticks - consoleTicksWhenLastFocus > 10000000;
			if (doFocus)
			{
				consoleTicksWhenLastFocus = ticks;
			}
			LauncherConsole.Invoke((MethodInvoker)delegate
			{
				Color selectionColor = LauncherConsole.SelectionColor;
				Font selectionFont = LauncherConsole.SelectionFont;
				bool flag = isStdError || s.Contains("ERROR:");
				bool flag2 = s.Contains("WARNING:");
				if (flag || flag2)
				{
					LauncherConsole.SelectionFont = new Font(LauncherConsole.SelectionFont, FontStyle.Bold);
					LauncherConsole.SelectionColor = (flag ? Color.Red : Color.Green);
				}
				LauncherConsole.AppendText(s + "\n");
				if (doFocus)
				{
					LauncherConsole.Focus();
				}
				if (flag || flag2)
				{
					LauncherConsole.SelectionColor = selectionColor;
					LauncherConsole.SelectionFont = selectionFont;
				}
			});
		}

		private void LauncherRunGameModComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			LauncherModComboBoxApplySettings();
		}

		private void editMapCSV_Click(object sender, EventArgs e)
		{
			if (!File.Exists(Path.Combine(Launcher.GetZoneSourceDirectory(), LauncherMapList.SelectedItem.ToString()) + ".csv"))
			{
				WriteConsole("ERROR: File zone_source/" + LauncherMapList.SelectedItem.ToString() + ".csv cannot be found!", isStdError: true);
				return;
			}
			try
			{
				Process process = new Process();
				process.StartInfo.ErrorDialog = true;
				process.StartInfo.FileName = Path.Combine(Launcher.GetZoneSourceDirectory(), LauncherMapList.SelectedItem.ToString()) + ".csv";
				process.Start();
			}
			catch
			{
			}
		}

		private void LauncherCopyImgsToModButton_Click(object sender, EventArgs e)
		{
			string text = LauncherModSpecificMapComboBox.Text;
			if (text == null || text == "(not set)" || mapName == null)
			{
				WriteConsole("ERROR: Can not copy images from this map as it is not valid!", isStdError: true);
				return;
			}
			if (!LauncherModSpecificMapCheckBox.Checked)
			{
				WriteConsole("ERROR: Mod to copy to is not selected!", isStdError: true);
				return;
			}
			string rawDirectory = Launcher.GetRawDirectory();
			string text2 = Path.Combine(rawDirectory, "images");
			string modDirectory = Launcher.GetModDirectory(text);
			string text3 = Path.Combine(modDirectory, "images");
			EnableControls(enabled: false);
			string[] imagesToCopy = Launcher.GetImagesToCopy(mapName);
			if (imagesToCopy == null)
			{
				WriteConsole("ERROR: No zone_source/assetlist/" + mapName + ".csv! Compile the map first!\nABORTING COPYING IMAGES!", isStdError: true);
				EnableControls(enabled: true);
				return;
			}
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
				WriteConsole("ERROR: You didn't have a raw/images folder, make sure to go through Asset Manager/converter first!\nABORTING COPYING IMAGES!", isStdError: true);
				EnableControls(enabled: true);
				return;
			}
			if (!Directory.Exists(text3))
			{
				Directory.CreateDirectory(text3);
			}
			string[] array = imagesToCopy;
			foreach (string text4 in array)
			{
				if (File.Exists(Path.Combine(text2, text4)))
				{
					File.Copy(Path.Combine(text2, text4), Path.Combine(text3, text4), overwrite: true);
					WriteConsole("INFO: Copying image " + text4 + " to mod " + text + "!", isStdError: false);
				}
				else
				{
					WriteConsole("ERROR: " + text4 + " does not exist in raw/images!", isStdError: true);
				}
			}
			EnableControls(enabled: true);
		}

		private void LauncherMapFFTypeMP_CheckedChanged(object sender, EventArgs e)
		{
			LauncherCompileVisCheckBox.Enabled = true;
			UpdateMapList();
		}

		private void LauncherMapFFTypeSP_CheckedChanged(object sender, EventArgs e)
		{
			UpdateMapList();
			LauncherCompileVisCheckBox.Enabled = false;
			LauncherCompileVisCheckBox.Checked = false;
		}

		private void LauncherModOpenButton_Click(object sender, EventArgs e)
		{
			Process.Start("explorer.exe", Launcher.GetModDirectory(modName));
		}

		private void LauncherEditMapCSVButton_Click(object sender, EventArgs e)
		{
			if (!File.Exists(Path.Combine(Launcher.GetZoneSourceDirectory(), LauncherMapList.SelectedItem.ToString()) + ".csv"))
			{
				WriteConsole("ERROR: File zone_source/" + LauncherMapList.SelectedItem.ToString() + ".csv cannot be found!", isStdError: true);
				return;
			}
			try
			{
				Process process = new Process();
				process.StartInfo.ErrorDialog = true;
				process.StartInfo.FileName = Path.Combine(Launcher.GetZoneSourceDirectory(), LauncherMapList.SelectedItem.ToString()) + ".csv";
				process.Start();
			}
			catch
			{
			}
		}

		private void ReadMapAssetListFile(string ffName)
		{
			LauncherAssetCountGridView.Rows.Clear();
			string zoneSourceDirectory = Launcher.GetZoneSourceDirectory();
			string path = Path.Combine(Launcher.GetLanguage(), "assetlist");
			string path2 = Path.Combine(zoneSourceDirectory, Path.Combine(path, ffName)) + ".csv";
			if (!File.Exists(path2))
			{
				return;
			}
			string text = File.ReadAllText(path2);
			char[] separator = new char[2] { '\n', ',' };
			string[] array = text.Split(separator);
			string[] stringArray = new string[0];
			for (int i = 0; i < array.Length; i += 2)
			{
				if (!string.IsNullOrWhiteSpace(array[i]))
				{
					string stringItem = array[i];
					Launcher.StringArrayAdd(ref stringArray, stringItem);
				}
			}
			string text2 = "";
			string text3 = "";
			int num = 0;
			for (int j = 0; j < stringArray.Length; j++)
			{
				text2 = stringArray[j];
				if (j == 0)
				{
					text3 = text2;
				}
				int assetLimit = GetAssetLimit(text3);
				if (assetLimit != 0)
				{
					if (text2 == text3)
					{
						num++;
						continue;
					}
					double num2 = Math.Round((double)num / (double)assetLimit, 3);
					LauncherAssetCountGridView.Rows.Add(text3, num + "/" + assetLimit, num2 * 100.0 + "%");
					int index = LauncherAssetCountGridView.Rows.Count - 1;
					int index2 = LauncherAssetCountGridView.Columns.Count - 1;
					LauncherAssetCountGridView.Rows[index].Cells[index2].Style.BackColor = FakeGtR(num2);
					text3 = text2;
					num = 1;
				}
			}
		}

		public int GetAssetLimit(string assetName)
		{
			return Launcher.assetLimit[assetName];
		}

		private static bool isStringInArray(string[] strArray, string key)
		{
			if (strArray.Contains(key))
			{
				return true;
			}
			return false;
		}

		private void LauncherSaveConsoleButton_Click(object sender, EventArgs e)
		{
			if (LauncherConsole.Text == "")
			{
				LauncherProcessTextBox.Text = "Console has nothing written!";
				LauncherProcessTimeElapsedTextBox.Text = "WARNING";
				return;
			}
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = Launcher.GetBinDirectory();
			saveFileDialog.Filter = "Log File|*.log|Rich Rext File|*.rtf";
			saveFileDialog.FileName = "LauncherOutput";
			saveFileDialog.Title = "Save console log";
			DialogResult dialogResult = saveFileDialog.ShowDialog();
			if (saveFileDialog.FileName != "" && dialogResult == DialogResult.OK)
			{
				WriteConsole("Saving Console Log from time " + DateTime.Now.ToString("hh:mm:ss tt", DateTimeFormatInfo.InvariantInfo), isStdError: false);
				if (Path.GetExtension(saveFileDialog.FileName) == ".rtf")
				{
					LauncherConsole.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.RichText);
				}
				else
				{
					LauncherConsole.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
				}
				LauncherProcessTextBox.Text = "Saved Log to: " + saveFileDialog.FileName;
				LauncherProcessTimeElapsedTextBox.Text = "Success";
			}
		}

		private void LauncherModRefreshButton_Click(object sender, EventArgs e)
		{
			LauncherModComboBoxApplySettings();
		}

		private void LauncherAssetCountReloadButton_Click(object sender, EventArgs e)
		{
			ReadMapAssetListFile(mapName);
		}

		private void LauncherFFAssetsHelp_Button_Click(object sender, EventArgs e)
		{
			string text = "Every Call of Duty has predefined limits in terms of assets. If you were to surpass a certain asset limit an error will be thrown.\nYou can tell it by it saying \"Exceeded limit of ### XXX assets.\"\nTo fix this remove any unused assets from your map and rebuild your map.\nThis list shows the limits for World at War.\n";
			string[] array = Launcher.assetLimit.Keys.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				object obj = text;
				text = string.Concat(obj, "Asset: ", array[i], " -> ", GetAssetLimit(array[i]), "\n");
			}
			MessageBoxEx.Show(text, "About Asset Limits", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private Color FakeGtR(double amount)
		{
			Color color = default(Color);
			int num = (int)((double)(float)amount * 255.0);
			if (amount < 0.34)
			{
				return Color.White;
			}
			if (amount < 0.67)
			{
				return Color.FromArgb(255, 255 - num, 79);
			}
			if (amount < 1.0)
			{
				return Color.DarkOrange;
			}
			return Color.FromArgb(236, 20, 20);
		}

		private void LauncherModSpecificMapComboBox_Click(object sender, EventArgs e)
		{
			LauncherModSpecificMapComboBox.DroppedDown = true;
		}

		private void LauncherModComboBox_Click(object sender, EventArgs e)
		{
			LauncherModComboBox.DroppedDown = true;
		}

		private void LauncherRunGameModComboBox_Click(object sender, EventArgs e)
		{
			LauncherRunGameModComboBox.DroppedDown = true;
		}
    }
}
