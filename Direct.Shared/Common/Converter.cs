using System.Text;

namespace Direct.Shared.Common
{
	public static class Converter
	{
		public static string ConvertJsonToStringForPrint(string jsonString, string logPrefix) 
		{
			var sb = new StringBuilder();
			var isArray = false;
			string curPrefix = logPrefix+'\t';
			sb.Append('\n'+curPrefix);
			foreach (var symbol in jsonString)
			{
				if (symbol == '}')
				{
					sb.Append("\n");
					curPrefix = curPrefix.Remove(curPrefix.Length - 1);
					sb.Append(curPrefix);
				}
				sb.Append(symbol);
				if (symbol == '{') {
					sb.Append("\n");
					curPrefix = curPrefix + "\t";
					sb.Append(curPrefix);
				}  
				if (symbol == '[') isArray = true;
				if (symbol == ']') isArray = false;
				if (symbol == ',' && isArray == false)
				{
					sb.Append("\n");
					sb.Append(curPrefix);
				}
			}
			return sb.ToString();
		}
	}
}