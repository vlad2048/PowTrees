using PowTrees.Algorithms;
using Shouldly;

namespace PowTrees.Tests.TestSupport;

static class ShouldlyExt
{
	public static void CheckTrees<T>(this TNod<T>[] actTrees, TNod<T>[] expTrees)
	{
		actTrees.Log("Actual");
		expTrees.Log("Expected");
		actTrees.Length.ShouldBe(expTrees.Length);

		for (var i = 0; i < actTrees.Length; i++)
		{
			var actTree = actTrees[i];
			var expTree = expTrees[i];
			var isEqual = actTree.IsEqual(expTree);
			isEqual.ShouldBeTrue();
		}
	}

	public static void CheckTree<T>(this TNod<T> actRoot, TNod<T> expRoot)
	{
		actRoot.Log("Actual");
		expRoot.Log("Expected");

		var isEqual = actRoot.IsEqual(expRoot);
		isEqual.ShouldBeTrue();
	}

	public static void CheckLines(this string[] actLines, string expStr)
	{
		var expLines = expStr.SplitInLines();
		if (expLines.Length > 0)
		{
			if (expLines[0].Trim() == string.Empty)
				expLines = expLines.Skip(1).ToArray();
			if (expLines[^1].Trim() == string.Empty)
				expLines = expLines.Take(expLines.Length - 1).ToArray();
		}

		actLines.Log("Actual");
		expLines.Log("Expected");

		CollectionAssert.AreEqual(expLines, actLines);
	}

	private static string[] SplitInLines(this string? str) => str == null ? Array.Empty<string>() : str.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
}