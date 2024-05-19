using Markdig;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace Lunatic.UI.Pages.SharedFunctions {
	public class Functions {
		public static MarkupString StringToHtml(string markdown) {
			if (markdown == null) {
				return new MarkupString("");
			}
			return new MarkupString(Markdown.ToHtml(markdown));
		}

		public static string DaysToNaturalLanguage(int days) {
			StringBuilder sb = new StringBuilder();

			if (days > 7) {
				sb.Append((days / 7).ToString()).Append(" weeks");
				int remainingDays = days % 7;
				if (remainingDays != 0) {
					sb.Append(" and ").Append((days % 7).ToString());
					if (remainingDays == 1) {
						sb.Append(" day");
					}
					else {
						sb.Append(" days");
					}
				}
			}
			else {
				sb.Append(days.ToString());
				if (days == 1) {
					sb.Append(" day");
				}
				else {
					sb.Append(" days");
				}
			}
			return sb.ToString();
		}
	}
}
