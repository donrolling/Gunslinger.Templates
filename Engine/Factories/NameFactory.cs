using System.Text.RegularExpressions;
using Common.Enums;
using Common.Strings;
using Gunslinger.Models;

namespace Gunslinger.Factories
{
    public class NameFactory
    {
        public static Regex rgx = new Regex("[^a-zA-Z0-9 -]");

        public static Name Create(string name)
        {
            return new Name
            {
                Value = name,
                LowerCamelCase = rgx.Replace(StringConversion.Convert(name, StringCase.LowerCamelCase), ""),
                NameWithSpaces = name.UnCamelCase(),
            };
        }
    }
}