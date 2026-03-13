using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Ionic.Zip;

namespace Form1
{
	internal static class Launcher
	{
		public static DVar[] dvars = new DVar[11]
		{
			new DVar("devmap", "Run the map in developer mode on startup [Must be compiled & exist in mod/zone folder]"),
			new DVar("developer", "Used to give more debug information, also allows cheat dvars to be set.", 0m, 2m),
			new DVar("developer_script", "Script code in developer comments /# #/ is parsed.", 0m, 1m),
			new DVar("fs_usedevdir", "Run the game with raw folder contents being loaded.", 0m, 1m),
			new DVar("logfile", "Write info from console to console.log [in fs_game location]", 0m, 2m),
			new DVar("ai_disablespawn", "Disable AI from spawning [Requires developer > 0]", 0m, 1m),
			new DVar("ai_shownodes", "Show AI pathnodes and their connections [Requires developer > 0]", 0m, 4m),
			new DVar("ai_showpaths", "Show AI's pathing [Requires developer > 0]", 0m, 2m),
			new DVar("com_introplayed", "Allow the intro movie to be played, \"0\" to play, \"0\" to skip.", 0m, 1m),
			new DVar("thereisacow", "Enables Cheats (unknown)", 0m, 1m),
			new DVar("r_fullscreen", "Set to \"1\" to make the game fullscreen, \"0\" for windowed.", 0m, 1m)
		};

		public static Dictionary<string, int> assetLimit = new Dictionary<string, int>
		{
			{ "aitype", 0 },
			{ "character", 0 },
			{ "col_map_mp", 1 },
			{ "col_map_sp", 1 },
			{ "com_map", 1 },
			{ "destructibledef", 64 },
			{ "font", 16 },
			{ "fx", 400 },
			{ "game_map_mp", 1 },
			{ "game_map_sp", 1 },
			{ "gfx_map", 1 },
			{ "image", 2400 },
			{ "impactfx", 4 },
			{ "lightdef", 32 },
			{ "loaded_sound", 1600 },
			{ "localize", 8342 },
			{ "map_ents", 2 },
			{ "material", 2048 },
			{ "menu", 600 },
			{ "menufile", 128 },
			{ "mptype", 0 },
			{ "packindex", 16 },
			{ "physconstraints", 64 },
			{ "physpreset", 64 },
			{ "pixelshader", 2048 },
			{ "rawfile", 1024 },
			{ "snddriverglobals", 1 },
			{ "sound", 16000 },
			{ "stringtable", 50 },
			{ "techset", 512 },
			{ "ui_map", 0 },
			{ "vertexshader", 1024 },
			{ "weapon", 128 },
			{ "xanim", 4096 },
			{ "xmodel", 1000 },
			{ "xmodelalias", 0 },
			{ "xmodelpieces", 64 }
		};

		public static Settings launcherSettings = default(Settings);

		public static Settings mapSettings = default(Settings);

		public static LauncherForm TheLauncherForm = null;

		public static string[] MainImages = GetMainImages();

		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			Application.Run(TheLauncherForm = new LauncherForm());
		}

		public static string CanonicalDirectory(string path)
		{
			FileInfo fileInfo = new FileInfo(path + "." + Path.DirectorySeparatorChar);
			MakeDirectory(fileInfo.DirectoryName);
			return fileInfo.DirectoryName + Path.DirectorySeparatorChar;
		}

		public static bool CopyFile(string sourceFileName, string destinationFileName)
		{
			return CopyFile(sourceFileName, destinationFileName, smartCopy: false);
		}

		public static bool CopyFile(string sourceFileName, string destinationFileName, bool smartCopy)
		{
			if (!File.Exists(sourceFileName))
			{
				if (smartCopy)
				{
					DeleteFile(destinationFileName, verbose: false);
				}
				return false;
			}
			FileInfo fileInfo = new FileInfo(sourceFileName);
			if (smartCopy)
			{
				FileInfo fileInfo2 = new FileInfo(destinationFileName);
				if (fileInfo.Exists && fileInfo2.Exists && fileInfo.CreationTime == fileInfo2.CreationTime && fileInfo.LastWriteTime == fileInfo2.LastWriteTime && fileInfo.Length == fileInfo2.Length)
				{
					return true;
				}
			}
			WriteMessage("Copying  " + sourceFileName + "\n     to  " + destinationFileName + "\n");
			if (!DeleteFile(destinationFileName, verbose: false))
			{
				return false;
			}
			MakeDirectory(Path.GetDirectoryName(destinationFileName));
			try
			{
				File.Copy(sourceFileName, destinationFileName);
				if (smartCopy)
				{
					File.SetCreationTime(destinationFileName, fileInfo.CreationTime);
					File.SetLastWriteTime(destinationFileName, fileInfo.LastWriteTime);
				}
			}
			catch (Exception ex)
			{
				WriteError("ERROR: " + ex.Message + "\n");
				return false;
			}
			return true;
		}

		public static bool CopyFileSmart(string sourceFileName, string destinationFileName)
		{
			return CopyFile(sourceFileName, destinationFileName, smartCopy: true);
		}

		public static string[] CreateMapFromTemplate(string mapTemplate, string mapName)
		{
			return CreateMapFromTemplate(mapTemplate, mapName, justCheckForOverwrite: false);
		}

		public static string[] CreateMapFromTemplate(string mapTemplate, string mapName, bool justCheckForOverwrite)
		{
			string[] stringArray = new string[0];
			string text = CanonicalDirectory(Path.Combine(GetMapTemplatesDirectory(), mapTemplate));
			string[] filesRecursively = GetFilesRecursively(text, "*template*");
			foreach (string text2 in filesRecursively)
			{
				string text3 = text2.Substring(text.Length).Replace("template", mapName);
				string text4 = Path.Combine(GetRootDirectory(), text3);
				if (justCheckForOverwrite)
				{
					if (File.Exists(text4))
					{
						StringArrayAdd(ref stringArray, text3);
					}
					continue;
				}
				string[] array = LoadTextFile(text2);
				int num = 0;
				string[] array2 = array;
				foreach (string text5 in array2)
				{
					array[num++] = text5.Replace("template", mapName);
				}
				SaveTextFile(text4, array);
			}
			return stringArray;
		}

		public static void CreateZoneSourceFiles(string mapName)
		{
			using StreamWriter streamWriter = new StreamWriter(GetZoneSourceFile(mapName));
			if (IsMP(mapName))
			{
				streamWriter.WriteLine("ignore,code_post_gfx_mp");
				streamWriter.WriteLine("ignore,common_mp");
				streamWriter.WriteLine("col_map_mp,maps/mp/" + mapName + ".d3dbsp");
				streamWriter.WriteLine("rawfile,maps/mp/" + mapName + ".gsc");
				streamWriter.WriteLine("rawfile,maps/mp/" + mapName + "_fx.gsc");
				streamWriter.WriteLine("sound,common," + mapName + ",all_mp");
				streamWriter.WriteLine("sound,generic," + mapName + ",all_mp");
				streamWriter.WriteLine("sound,voiceovers," + mapName + ",all_mp");
				streamWriter.WriteLine("sound,multiplayer," + mapName + ",all_mp");
			}
			else
			{
				streamWriter.WriteLine("ignore,code_post_gfx");
				streamWriter.WriteLine("ignore,common");
				streamWriter.WriteLine("col_map_sp,maps/" + mapName + ".d3dbsp");
				streamWriter.WriteLine("rawfile,maps/" + mapName + ".gsc");
				streamWriter.WriteLine("rawfile,maps/" + mapName + "_anim.gsc");
				streamWriter.WriteLine("rawfile,maps/" + mapName + "_amb.gsc");
				streamWriter.WriteLine("rawfile,maps/" + mapName + "_fx.gsc");
				streamWriter.WriteLine("sound,common," + mapName + ",all_sp");
				streamWriter.WriteLine("sound,generic," + mapName + ",all_sp");
				streamWriter.WriteLine("sound,voiceovers," + mapName + ",all_sp");
				streamWriter.WriteLine("sound,requests," + mapName + ",all_sp");
			}
		}

		public static bool DeleteFile(string fileName)
		{
			return DeleteFile(fileName, verbose: true);
		}

		public static bool DeleteFile(string fileName, bool verbose)
		{
			if (File.Exists(fileName))
			{
				if (verbose)
				{
					WriteMessage("Deleting " + fileName + "\n");
				}
				try
				{
					File.SetAttributes(fileName, FileAttributes.Normal);
					File.Delete(fileName);
				}
				catch (Exception ex)
				{
					if (verbose)
					{
						WriteError("ERROR: " + ex.Message + "\n");
					}
					return false;
				}
			}
			return true;
		}

		public static string FilterMP(string name)
		{
			if (!IsMP(name))
			{
				return name;
			}
			return name.Substring(3);
		}

		public static string GetBinDirectory()
		{
			return CanonicalDirectory(Path.Combine(GetRootDirectory(), "bin"));
		}

		public static string GetBspOptions()
		{
			return (mapSettings.GetBoolean("bspoptions_onlyents") ? " -onlyents" : "") + (mapSettings.GetBoolean("bspoptions_blocksize") ? (" -blocksize " + mapSettings.GetDecimal("lightoptions_blocksize_val")) : "") + (mapSettings.GetBoolean("bspoptions_samplescale") ? (" -samplescale " + mapSettings.GetDecimal("bspoptions_samplescale_val")) : "") + (mapSettings.GetBoolean("bspoptions_debuglightmaps") ? " -debugLightmaps" : "") + mapSettings.GetString("bspoptions_extraoptions") + " ";
		}

		public static string[] GetDirs(string directory)
		{
			string[] stringArray = new string[0];
			DirectoryInfo[] directories = new DirectoryInfo(directory).GetDirectories();
			foreach (DirectoryInfo directoryInfo in directories)
			{
				StringArrayAdd(ref stringArray, directoryInfo.Name);
			}
			return stringArray;
		}

		public static string[] GetZoneFastFiles()
		{
			string[] stringArray = new string[0];
			GetMapList();
			FileInfo[] files = new DirectoryInfo(Path.Combine(GetRootDirectory(), Path.Combine("zone", GetLanguage()))).GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				string text = fileInfo.Name.ToString();
				if (isValidFastFile(text))
				{
					StringArrayAdd(ref stringArray, text.Substring(0, text.Length - 3));
				}
			}
			return stringArray;
		}

		public static bool isValidFastFile(string ffName)
		{
			string text = ffName.ToString();
			text = text.ToLower();
			text = text.Substring(0, text.Length - 3);
			if (!ffName.EndsWith(".ff"))
			{
				return false;
			}
			if (text.StartsWith("localized") || text.Contains("code_post") || text.StartsWith("common") || text.EndsWith("load") || text.Contains("patch") || text.Equals("ui") || text.Equals("ui_mp") || text.Equals("default") || text.Equals("credits"))
			{
				return false;
			}
			return true;
		}

		public static string[] GetFiles(string directory, string searchFilter)
		{
			string[] stringArray = new string[0];
			FileInfo[] files = new DirectoryInfo(directory).GetFiles(searchFilter);
			foreach (FileInfo fileInfo in files)
			{
				StringArrayAdd(ref stringArray, Path.GetFileName(fileInfo.Name));
			}
			return stringArray;
		}

		public static string[] GetFilesRecursively(string directory)
		{
			return GetFilesRecursively(directory, "*");
		}

		public static string[] GetFilesRecursively(string directory, string filesToIncludeFilter)
		{
			string[] files = new string[0];
			GetFilesRecursively(directory, filesToIncludeFilter, ref files);
			return files;
		}

		public static void GetFilesRecursively(string directory, string filesToIncludeFilter, ref string[] files)
		{
			DirectoryInfo[] directories = new DirectoryInfo(directory).GetDirectories();
			foreach (DirectoryInfo directoryInfo in directories)
			{
				GetFilesRecursively(Path.Combine(directory, directoryInfo.Name), filesToIncludeFilter, ref files);
			}
			FileInfo[] files2 = new DirectoryInfo(directory).GetFiles(filesToIncludeFilter);
			foreach (FileInfo fileInfo in files2)
			{
				StringArrayAdd(ref files, Path.Combine(directory, fileInfo.Name.ToLower()));
			}
		}

		public static string[] GetFilesWithoutExtension(string directory, string searchFilter)
		{
			string[] stringArray = new string[0];
			FileInfo[] files = new DirectoryInfo(directory).GetFiles(searchFilter);
			foreach (FileInfo fileInfo in files)
			{
				StringArrayAdd(ref stringArray, Path.GetFileNameWithoutExtension(fileInfo.Name));
			}
			return stringArray;
		}

		public static string GetGameApplication(bool mpVersion)
		{
			if (!mpVersion)
			{
				return "../CoDWaW";
			}
			if (Directory.GetCurrentDirectory().ToLower().Contains("steamapps"))
			{
				WriteMessage("INFO: You have the Steam version of World at War, this won't start the MP application.");
			}
			return "../CoDWaWmp";
		}

		public static string GetGameTool(bool mpVersion)
		{
			if (!mpVersion)
			{
				return "../sp_tool";
			}
			return "../mp_tool";
		}

		public static string selectedLanguage = "english";

		public static string GetLanguage()
		{
			return "english";
		}

		public static string GetLightOptions()
		{
			return (mapSettings.GetBoolean("lightoptions_extra") ? " -extra" : " -fast") + (mapSettings.GetBoolean("lightoptions_nomodelshadow") ? " -nomodelshadow" : " -modelshadow") + (mapSettings.GetBoolean("lightoptions_traces") ? (" -traces " + mapSettings.GetDecimal("lightoptions_traces_val")) : "") + (mapSettings.GetBoolean("lightoptions_maxbounces") ? (" -maxbounces " + mapSettings.GetDecimal("lightoptions_maxbounces_val")) : "") + (mapSettings.GetBoolean("lightoptions_maxbounces") ? (" -jitter " + mapSettings.GetDecimal("lightoptions_jitter_val")) : "") + (mapSettings.GetBoolean("lightoptions_verbose") ? " -verbose" : "") + " ";
		}

		public static string GetLoadZone(string mapName)
		{
			return mapName + "_load";
		}

		private static string GetLocalApplicationDirectory()
		{
			return CanonicalDirectory(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Activision"), "CodWaW"));
		}

		private static string GetLocalApplicationModDirectory(string modName)
		{
			return CanonicalDirectory(Path.Combine(GetLocalApplicationModsDirectory(), modName));
		}

		public static string[] GetLocalApplicationModFiles(string modName)
		{
			return GetModFilesByDirectory(GetLocalApplicationModDirectory(modName));
		}

		private static string GetLocalApplicationModsDirectory()
		{
			return CanonicalDirectory(Path.Combine(GetLocalApplicationDirectory(), "mods"));
		}

		private static string GetLocalApplicationUsermapsDirectory()
		{
			return CanonicalDirectory(Path.Combine(GetLocalApplicationDirectory(), "usermaps"));
		}

		public static string[] GetMainImages()
		{
			string[] stringArray = new string[0];
			string[] iWDFiles = GetIWDFiles();
			foreach (string text in iWDFiles)
			{
				using ZipFile zipFile = ZipFile.Read(text.ToString());
				foreach (ZipEntry item in zipFile)
				{
					if (item.FileName.Contains(".iwi") && item.FileName.Contains("images/"))
					{
						string text2 = item.FileName.ToString();
						text2 = text2.Substring(7);
						StringArrayAdd(ref stringArray, text2);
					}
				}
			}
			return stringArray;
		}

		public static string[] GetImagesToCopy(string modName)
		{
			string zoneSourceDirectory = GetZoneSourceDirectory();
			string path = Path.Combine(GetLanguage(), "assetlist");
			string text = Path.Combine(zoneSourceDirectory, Path.Combine(path, modName)) + ".csv";
			if (!File.Exists(text))
			{
				return null;
			}
			return GetNonStockImages(LoadCSVFile(text, "image"));
		}

		public static string[] GetNonStockImages(string[] modImages)
		{
			List<string> list = new List<string>(MainImages);
			List<string> list2 = new List<string>();
			foreach (string item in modImages)
			{
				if (!list.Contains(item))
				{
					list2.Add(item);
				}
			}
			return list2.ToArray();
		}

		public static string[] GetMapFiles(string mapName)
		{
			string[] stringArray = new string[0];
			string[] array = new string[3]
			{
				GetMapSourceDirectory(),
				GetRawMapsDirectory(),
				GetZoneSourceDirectory()
			};
			foreach (string text in array)
			{
				string[] array2 = new string[2] { ".*", "_*.*" };
				foreach (string text2 in array2)
				{
					string[] array3 = new string[2] { "", "localized_" };
					foreach (string text3 in array3)
					{
						FileInfo[] files = new DirectoryInfo(text).GetFiles(text3 + mapName + text2);
						foreach (FileInfo fileInfo in files)
						{
							StringArrayAdd(ref stringArray, Path.Combine(text, fileInfo.Name));
						}
					}
				}
			}
			return stringArray;
		}

		public static string[] GetCompiledMapList(bool ignoreBuilds, bool ignoreMP)
		{
			string[] mapList = GetMapList();
			return GetProperMapList(mapList, ignoreBuilds, ignoreMP);
		}

		public static string[] GetCompiledMapList(bool ignoreBuilds)
		{
			string[] mapList = GetMapList();
			return GetProperMapList(mapList, ignoreBuilds);
		}

		public static string[] GetProperMapList(string[] mapList, bool ignoreBuilds, bool ignoreMP)
		{
			List<string> list = new List<string>();
			foreach (string text in mapList)
			{
				if ((!ignoreMP || !text.StartsWith("mp_")) && (ignoreMP || text.StartsWith("mp_")) && ((!text.Contains("load") && !text.Contains("localized") && !text.Contains("patch")) || !ignoreBuilds) && !text.Contains("audio") && !text.Contains("geo"))
				{
					list.Add(text);
				}
			}
			return list.ToArray();
		}

		public static string[] GetProperMapList(string[] mapList, bool ignoreBuilds)
		{
			List<string> list = new List<string>();
			foreach (string text in mapList)
			{
				if (((!text.Contains("load") && !text.Contains("localized") && !text.Contains("patch")) || !ignoreBuilds) && !text.Contains("audio") && !text.Contains("geo"))
				{
					list.Add(text);
				}
			}
			return list.ToArray();
		}

		public static string[] GetIWDFiles()
		{
			return GetFilesRecursively(GetMainDirectory(), "*.iwd");
		}

		public static string[] GetMapList()
		{
			return GetFilesWithoutExtension(GetMapSourceDirectory(), "*.map");
		}

		public static string GetMapSettingsDirectory()
		{
			return CanonicalDirectory(Path.Combine(GetStartupDirectory(), Path.Combine(Application.ProductName, "map_settings")));
		}

		private static string GetMapSettingsFilename(string mapName)
		{
			return Path.Combine(GetMapSettingsDirectory(), mapName + ".cfg");
		}

		public static string GetMainDirectory()
		{
			return CanonicalDirectory(Path.Combine(GetRootDirectory(), "main"));
		}

		public static string GetMapSourceDirectory()
		{
			return CanonicalDirectory(Path.Combine(GetRootDirectory(), "map_source"));
		}

		public static string GetMapTemplatesDirectory()
		{
			return CanonicalDirectory(Path.Combine(GetStartupDirectory(), Path.Combine(Application.ProductName, "map_templates")));
		}

		public static string[] GetMapTemplatesList()
		{
			return GetDirs(GetMapTemplatesDirectory());
		}

		public static string GetModDirectory(string modName)
		{
			if (modName == null)
			{
				return null;
			}
			return CanonicalDirectory(Path.Combine(GetModsDirectory(), modName));
		}

		public static string[] GetModFiles(string modName)
		{
			return GetModFilesByDirectory(GetModDirectory(modName));
		}

		public static string[] GetModFilesByDirectory(string directory)
		{
			string[] filesRecursively = GetFilesRecursively(directory, "*.ff");
			string[] filesRecursively2 = GetFilesRecursively(directory, "*.iwd");
			string[] filesRecursively3 = GetFilesRecursively(directory, "*.arena");
			string[] array = new string[filesRecursively.Length + filesRecursively2.Length + filesRecursively3.Length];
			filesRecursively.CopyTo(array, 0);
			filesRecursively2.CopyTo(array, filesRecursively.Length);
			filesRecursively3.CopyTo(array, filesRecursively.Length + filesRecursively2.Length);
			return array;
		}

		public static string[] GetModList()
		{
			return GetDirs(GetModsDirectory());
		}

		public static string GetModsDirectory()
		{
			return CanonicalDirectory(Path.Combine(GetRootDirectory(), "mods"));
		}

		public static string GetRawDirectory()
		{
			return CanonicalDirectory(Path.Combine(GetRootDirectory(), "raw"));
		}

		public static string GetRawMapsDirectory()
		{
			return CanonicalDirectory(Path.Combine(GetRawDirectory(), "maps"));
		}

		public static string GetRootDirectory()
		{
			return CanonicalDirectory(Path.Combine(GetStartupDirectory(), ".."));
		}

		public static string GetStartupDirectory()
		{
			return CanonicalDirectory(Path.GetFullPath("."));
		}

		public static string GetUsermapsDirectory()
		{
			return CanonicalDirectory(Path.Combine(GetRootDirectory(), "usermaps"));
		}

		public static string GetZoneDirectory()
		{
			return CanonicalDirectory(Path.Combine(Path.Combine(GetRootDirectory(), "zone"), GetLanguage()));
		}

		public static string GetZoneDirectory(string language)
		{
			return CanonicalDirectory(Path.Combine(Path.Combine(GetRootDirectory(), "zone"), language));
		}

		public static string GetZoneSourceDirectory()
		{
			return CanonicalDirectory(Path.Combine(GetRootDirectory(), "zone_source"));
		}

		public static string GetZoneSourceFile(string mapName)
		{
			return Path.Combine(GetZoneSourceDirectory(), mapName + ".csv");
		}

		public static string GetZoneSourceLoadCSVFile(string mapName)
		{
			return Path.Combine(GetZoneSourceDirectory(), GetLoadZone(mapName) + ".csv");
		}

		public static string[] HashTableToStringArray(Hashtable hashTable)
		{
			int num = 0;
			string[] array = new string[hashTable.Count];
			foreach (DictionaryEntry item in hashTable)
			{
				array[num++] = string.Concat(item.Key, (item.Value != null) ? ("," + item.Value) : "");
			}
			Array.Sort(array);
			return array;
		}

		public static bool IsMP(string name)
		{
			return name.ToLower().StartsWith("mp_");
		}

		public static bool IsMultiplayerMapTemplate(string mapTemplate)
		{
			return File.Exists(Path.Combine(Path.Combine(GetMapTemplatesDirectory(), mapTemplate), "mp.txt"));
		}

		public static Hashtable LoadMapSettings(string mapName)
		{
			return StringArrayToHashTable(LoadTextFile(GetMapSettingsFilename(mapName)));
		}

		public static string[] LoadCSVFile(string csvFile)
		{
			return LoadCSVFile(csvFile, null, null);
		}

		public static string[] LoadCSVFile(string csvFile, string findsWordsWith)
		{
			return LoadCSVFile(csvFile, findsWordsWith, null);
		}

		public static string[] LoadCSVFile(string textFile, string findsWordsWith, string skipCommentLinesStartingWith)
		{
			string[] stringArray = new string[0];
			string text = "";
			string text2;
			if ((text2 = findsWordsWith) != null && text2 == "image")
			{
				text = ".iwi";
			}
			try
			{
				using StreamReader streamReader = new StreamReader(textFile);
				string text3;
				while ((text3 = streamReader.ReadLine()) != null)
				{
					text3.Trim();
					if (text3 != "" && (skipCommentLinesStartingWith == null || !text3.StartsWith(skipCommentLinesStartingWith)) && text3.StartsWith(findsWordsWith))
					{
						StringArrayAdd(ref stringArray, text3.Substring(findsWordsWith.Length + 1) + text);
					}
				}
			}
			catch
			{
			}
			return stringArray;
		}

		public static string[] LoadTextFile(string textFile)
		{
			return LoadTextFile(textFile, null);
		}

		public static string[] LoadTextFile(string textFile, string skipCommentLinesStartingWith)
		{
			string[] stringArray = new string[0];
			try
			{
				using StreamReader streamReader = new StreamReader(textFile);
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					text.Trim();
					if (text != "" && (skipCommentLinesStartingWith == null || !text.StartsWith(skipCommentLinesStartingWith)))
					{
						StringArrayAdd(ref stringArray, text);
					}
				}
			}
			catch
			{
			}
			return stringArray;
		}

		public static void removeWeaponFileFX(string weapon)
		{
			if (weapon == null)
			{
				return;
			}
			string rawDirectory = GetRawDirectory();
			string path = (weapon.Contains("sp") ? "sp" : "mp");
			string path2 = Path.Combine(rawDirectory, Path.Combine("weapons", path));
			string text = Path.Combine(path2, "backup");
			bool flag = false;
			string text2 = File.ReadAllText(weapon);
			if (!text2.StartsWith("WEAPONFILE\\"))
			{
				return;
			}
			string[] array = text2.Split('\\');
			for (int i = 1; i < array.Length; i += 2)
			{
				if ((array[i] == "worldFlashEffect" || array[i] == "viewFlashEffect" || array[i] == "worldShellEjectEffect" || array[i] == "viewShellEjectEffect") && array[i + 1].StartsWith("weapon/"))
				{
					array[i + 1] = "";
					flag = true;
				}
			}
			if (flag)
			{
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				if (File.Exists(text + weapon.Substring(GetRootDirectory().Length + 14)))
				{
					File.Delete(text + weapon.Substring(GetRootDirectory().Length + 14));
				}
				File.Move(weapon, text + weapon.Substring(GetRootDirectory().Length + 14));
				WriteMessage("INFO: Removing FX from weapon \"" + weapon.Substring(GetRootDirectory().Length) + "\". Created backup.\n");
				SaveWeaponFile(weapon, array);
			}
		}

		private static void SaveWeaponFile(string weapon, string[] weaponFile)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("WEAPONFILE\\");
			for (int i = 1; i < weaponFile.Length; i += 2)
			{
				stringBuilder.Append(weaponFile[i] + "\\" + weaponFile[i + 1]);
				if (i + 1 != weaponFile.Length - 1)
				{
					stringBuilder.Append("\\");
				}
			}
			File.WriteAllText(weapon, stringBuilder.ToString());
		}

		public static void MakeDirectory(string directoryName)
		{
			while (!Directory.Exists(directoryName))
			{
				string directoryName2 = Path.GetDirectoryName(directoryName);
				if (directoryName2 != directoryName)
				{
					MakeDirectory(directoryName2);
				}
				Directory.CreateDirectory(directoryName);
			}
		}

		public static string MakeMP(string name)
		{
			if (!IsMP(name))
			{
				return "mp_" + name;
			}
			return name;
		}

		public static bool MoveFile(string sourceFileName, string destinationFileName)
		{
			if (!File.Exists(sourceFileName))
			{
				return false;
			}
			WriteMessage("Moving   " + sourceFileName + "\n    to   " + destinationFileName + "\n");
			if (!DeleteFile(destinationFileName, verbose: false))
			{
				return false;
			}
			MakeDirectory(Path.GetDirectoryName(destinationFileName));
			try
			{
				File.Move(sourceFileName, destinationFileName);
			}
			catch (Exception ex)
			{
				WriteError("ERROR: " + ex.Message + "\n");
				return false;
			}
			return true;
		}

		public static void Publish()
		{
			PublishUsermaps();
			PublishMods();
		}
		
		public static void PublishModOnly()
		{
			PublishUsermaps();
			PublishMods();
		}

		public static void PublishMod(string modName)
		{
			string modDirectory = GetModDirectory(modName);
			string[] modFiles = GetModFiles(modName);
			foreach (string text in modFiles)
			{
				CopyFileSmart(text, Path.Combine(GetLocalApplicationModDirectory(modName), text.Substring(modDirectory.Length)));
			}
		}

		public static void PublishMods()
		{
			string[] modList = GetModList();
			foreach (string modName in modList)
			{
				PublishMod(modName);
			}
		}

		public static void PublishUsermaps()
		{
			string usermapsDirectory = GetUsermapsDirectory();
			string[] filesRecursively = GetFilesRecursively(usermapsDirectory, "*.ff");
			foreach (string text in filesRecursively)
			{
				CopyFileSmart(text, Path.Combine(GetLocalApplicationUsermapsDirectory(), text.Substring(usermapsDirectory.Length)));
			}
		}

		public static void SaveMapSettings(string mapName, Hashtable mapSettings)
		{
			SaveTextFile(GetMapSettingsFilename(mapName), HashTableToStringArray(mapSettings));
		}

		public static void SaveTextFile(string textFile, string[] text)
		{
			using StreamWriter streamWriter = new StreamWriter(textFile);
			foreach (string value in text)
			{
				streamWriter.WriteLine(value);
			}
		}

		public static decimal SetNumericUpDownValue(NumericUpDown ctrl, decimal Value)
		{
			decimal value = ctrl.Value;
			if (Value < ctrl.Minimum)
			{
				ctrl.Value = ctrl.Minimum;
				return value;
			}
			if (Value > ctrl.Maximum)
			{
				ctrl.Value = ctrl.Maximum;
				return value;
			}
			ctrl.Value = Value;
			return value;
		}

		public static void StringArrayAdd(ref string[] stringArray, string stringItem)
		{
			Array.Resize(ref stringArray, stringArray.Length + 1);
			stringArray[stringArray.Length - 1] = stringItem;
		}

		public static Hashtable StringArrayToHashTable(string[] stringArray)
		{
			Hashtable hashtable = new Hashtable(stringArray.Length);
			foreach (string text in stringArray)
			{
				string[] array = text.Split(',');
				if (array.Length > 0)
				{
					hashtable.Add(array[0], (array.Length > 1) ? array[1] : null);
				}
			}
			return hashtable;
		}

		public static string StringArrayToString(string[] stringArray)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in stringArray)
			{
				stringBuilder.Append(value).AppendLine();
			}
			return stringBuilder.ToString();
		}

		public static void WriteError(string s)
		{
			WriteMessage(s, Color.Red);
		}

		public static void WriteMessage(string s)
		{
			WriteMessage(s, Color.SlateBlue);
		}

		public static void WriteMessage(string s, Color messageColor)
		{
			Color selectionColor = TheLauncherForm.LauncherConsole.SelectionColor;
			TheLauncherForm.LauncherConsole.SelectionColor = messageColor;
			TheLauncherForm.LauncherConsole.AppendText(s);
			TheLauncherForm.LauncherConsole.SelectionColor = selectionColor;
			TheLauncherForm.LauncherConsole.Focus();
		}
	}
}
