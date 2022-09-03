using UnityEngine;

namespace I2.Loc
{
	public static class ScriptLocalization
	{

		public static string Mode1 		{ get{ return LocalizationManager.GetTranslation ("Mode1"); } }
		public static string Mode2 		{ get{ return LocalizationManager.GetTranslation ("Mode2"); } }
		public static string Mode3 		{ get{ return LocalizationManager.GetTranslation ("Mode3"); } }
		public static string Title 		{ get{ return LocalizationManager.GetTranslation ("Title"); } }
	}

    public static class ScriptTerms
	{

		public const string Mode1 = "Mode1";
		public const string Mode2 = "Mode2";
		public const string Mode3 = "Mode3";
		public const string Title = "Title";
	}
}