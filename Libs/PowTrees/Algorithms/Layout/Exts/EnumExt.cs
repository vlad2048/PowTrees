namespace PowTrees.Algorithms.Layout.Exts;

static class EnumExt
{
	public static U[] SelectToArray<T, U>(this IEnumerable<T> source, Func<T, U> mapFun) => source.Select(mapFun).ToArray();

	public static U[] FoldL<T, U>(this IEnumerable<T> source, U seed, Func<T, U, U> fun)
	{
		var list = new List<U>();
		foreach (var elt in source)
		{
			list.Add(seed);
			seed = fun(elt, seed);
		}
		return list.ToArray();
	}

	public static int SumOrZero<T>(this IEnumerable<T> source, Func<T, int> fun)
	{
		var sum = 0;
		foreach (var elt in source)
			sum += fun(elt);
		return sum;
	}

	public static int SumOrZero(this IEnumerable<int> source)
	{
		var sum = 0;
		foreach (var elt in source)
			sum += elt;
		return sum;
	}
}