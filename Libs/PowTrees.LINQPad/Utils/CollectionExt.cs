namespace PowTrees.LINQPad.Utils;

static class CollectionExt
{
	public static U[] SelectToArray<T, U>(this IEnumerable<T> source, Func<T, U> fun) => source.Select(fun).ToArray();

	public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
	{
		foreach (var elt in source)
			action(elt);
	}
}