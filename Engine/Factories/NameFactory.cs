using Common.Enums;
using Common.Strings;
using Gunslinger.Models;
using Pluralize.NET.Core;
using System.Text.RegularExpressions;

namespace Gunslinger.Factories
{
	public class NameFactory
	{
		public static Regex rgx = new Regex("[^a-zA-Z0-9 -]");

		public static Name Create(string name, Template template, bool isClass)
		{
			// don't use pluralization on anything but class names
			if (!isClass)
			{
				return MakeName(name);
			}
			var pluralizer = new Pluralizer();
			switch (template.PluralizationSettings)
			{
				case Enum.PluralizationSettings.Plural:
					var pluralName = pluralizer.Pluralize(name);
					return MakeName(pluralName);

				case Enum.PluralizationSettings.Singular:
					var singularName = pluralizer.Singularize(name);
					return MakeName(singularName);

				case Enum.PluralizationSettings.None:
				default:
					return MakeName(name);
			}
		}

		private static Name MakeName(string name)
		{
			return new Name
			{
				Value = name,
				LowerCamelCase = rgx.Replace(StringConversion.Convert(name, StringCase.LowerCamelCase), ""),
				PascalCase = rgx.Replace(StringConversion.Convert(name, StringCase.PascalCase), ""),
				NameWithSpaces = name.UnCamelCase(),
			};
		}
	}
}