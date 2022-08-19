using PowTrees.Algorithms;

namespace PowTrees.Tests.TestSupport;

static class LogUtils
{
	public static void Log<T>(this IEnumerable<TNod<T>> roots, string title)
	{
		var arr = roots.ToArray();
		LogTitle($"{title}: {arr.Length}");
		arr.LogTrees();
		Log("");
	}

	public static void Log<T>(this TNod<T> root, string title)
	{
		LogTitle(title);
		root.LogTree();
		Log("");
	}

	public static void Log(this string[] arr, string title)
	{
		LogTitle(title);
		arr.LogArr();
		Log("");
	}

	
	private static void LogTrees<T>(this IEnumerable<TNod<T>> roots)
	{
		foreach (var root in roots)
			root.LogTree();
	}

	private static void LogTree<T>(this TNod<T> root)
	{
		root.LogToStrings().LogArr();
	}

	private static void LogArr(this string[] arr)
	{
		foreach (var line in arr)
			Log("  " + line);
	}

	private static void LogTitle(string str)
	{
		Log(str);
		Log(new string('=', str.Length));
	}
	private static void Log(string str) => Console.WriteLine(str);
}