﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Hors;

namespace Direct.Parser
{
	public static class DateParser
	{
		public static DateTime? GetDateTimeFromText(string text)
		{
			var res = GetDatesRegex(text);
			if (res.Count == 0)
			{
				var horsTextParser = new HorsTextParser();
				var result = horsTextParser.Parse(text, new DateTime(2022, 2, 1));
				return result.Dates.LastOrDefault()?.DateTo;
			}

			return res.OrderByDescending(dt => dt.Date).FirstOrDefault();

		}

		private const string DATE_PATTERN = @"\d{2}.\d{2}";
		private static readonly char[] SEPARATORS = {'/', ',', '.'};

		private static List<DateTime> GetDatesRegex(string text)
		{
			var result = new List<DateTime>();
			var matches = Regex.Matches(text, DATE_PATTERN);
			foreach (Match match in matches)
			{
				var stringDate = text.Substring(match.Index, match.Length);
				var date = stringDate.Split(SEPARATORS);
				var day = StringToInt(date[0]);
				var month = StringToInt(date[1]);
				result.Add(new DateTime(DateTime.Now.Year, month, day));
			}

			return result;
		}

		private static int StringToInt(string s)
		{
			if (int.Parse(s.Substring(0, 1)) == 0)
				s = s.Substring(1, 1);
			return int.Parse(s);
		}
	}
}