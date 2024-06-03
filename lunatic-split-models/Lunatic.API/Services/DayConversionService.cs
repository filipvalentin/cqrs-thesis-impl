using Lunatic.API.Interfaces;
using System.Text;

namespace Lunatic.API.Services {
	public class DayConversionService : IDayConversionService {
		public string ConvertToString(int days) {
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
