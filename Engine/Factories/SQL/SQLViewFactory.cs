using Gunslinger.Models;
using Gunslinger.Models.SQL;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;

namespace Gunslinger.Factories.SQL
{
	public class SQLViewFactory
	{
		public static List<SQLView> Create(string @namespace, Enum.Language language, IEnumerable<View> smoViews, Template template)
		{
			return new List<SQLView>();
		}
	}
}