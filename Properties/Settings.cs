using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Properties
{
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

		public static Settings Default => defaultInstance;

		[DebuggerNonUserCode]
		[UserScopedSetting]
		[DefaultSettingValue("White")]
		public string LauncherColor
		{
			get
			{
				return (string)this["LauncherColor"];
			}
			set
			{
				this["LauncherColor"] = value;
			}
		}
	}
}
