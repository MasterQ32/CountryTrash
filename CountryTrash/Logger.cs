using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryTrash
{
	public static class Logger
	{
		public static T Log<T>(this T obj)
		{
			return Log<T>(obj, "{0}", "log");
		}

		public static T Log<T>(this T obj, string category)
		{
			return Log<T>(obj, "{0}", category);
		}

		public static T Log<T>(this T obj, string format, string category)
		{
			Console.WriteLine("[{0}] {1}", category, string.Format(format, obj));
			return obj;
		}
	}
}
