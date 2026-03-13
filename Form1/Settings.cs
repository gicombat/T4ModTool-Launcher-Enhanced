using System.Collections;

namespace Form1
{
	public struct Settings
	{
		public Hashtable settings;

		public Settings(Hashtable ht)
		{
			settings = ht;
		}

		public void Set(Hashtable newSettings)
		{
			settings = newSettings;
		}

		public Hashtable Get()
		{
			return settings;
		}

		public bool GetBoolean(string Key)
		{
			bool result = false;
			string value = (string)settings[Key];
			if (!bool.TryParse(value, out result))
			{
				return false;
			}
			return result;
		}

		public decimal GetDecimal(string Key)
		{
			decimal result = 0m;
			string s = (string)settings[Key];
			if (!decimal.TryParse(s, out result))
			{
				return 0m;
			}
			return result;
		}

		public string GetString(string Key)
		{
			string text = (string)settings[Key];
			if (text == null)
			{
				return "";
			}
			return text;
		}

		public void SetBoolean(string Key, bool Value)
		{
			settings[Key] = Value.ToString();
		}

		public void SetDecimal(string Key, decimal Value)
		{
			settings[Key] = Value.ToString();
		}

		public void SetString(string Key, string Value)
		{
			settings[Key] = ((Value != null) ? Value : "");
		}
	}
}
