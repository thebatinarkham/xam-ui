using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ABPRenamer
{
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.2.0.0")]
	public sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

		public static Settings Default => defaultInstance;

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string setFilter
		{
			get
			{
				return (string)this["setFilter"];
			}
			set
			{
				this["setFilter"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string setOldCompanyName
		{
			get
			{
				return (string)this["setOldCompanyName"];
			}
			set
			{
				this["setOldCompanyName"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string setOldProjectName
		{
			get
			{
				return (string)this["setOldProjectName"];
			}
			set
			{
				this["setOldProjectName"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string setRootDir
		{
			get
			{
				return (string)this["setRootDir"];
			}
			set
			{
				this["setRootDir"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string setNewCompanyName
		{
			get
			{
				return (string)this["setNewCompanyName"];
			}
			set
			{
				this["setNewCompanyName"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string setNewProjectName
		{
			get
			{
				return (string)this["setNewProjectName"];
			}
			set
			{
				this["setNewProjectName"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string setOldAreaName
		{
			get
			{
				return (string)this["setOldAreaName"];
			}
			set
			{
				this["setOldAreaName"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string setNewAreaName
		{
			get
			{
				return (string)this["setNewAreaName"];
			}
			set
			{
				this["setNewAreaName"] = value;
			}
		}
	}
}
