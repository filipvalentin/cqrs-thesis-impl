namespace Lunatic.Application.Utils {
	public static class ListExtensions {
		public static void AddIfNotExists<T>(this List<T> list, T item) {
			if (!list.Contains(item)) {
				list.Add(item);
			}
		}
	}
}
