namespace PowTrees.Algorithms;

public static class Algo_FoldR
{
	/// <summary>
	/// Map a tree recursively.
	/// For each node, we use the node and the mapped children as input
	/// </summary>
	public static TNod<U> FoldR<T, U>(
		this TNod<T> root,
		Func<T, IReadOnlyList<U>, U> fun
	)
	{
		TNod<U> Recurse(TNod<T> node)
		{
			var foldedChildren = node.Children
				.Select(Recurse).ToArray();
			var foldedNode = new TNod<U>(
				fun(node.V, foldedChildren.Select(e => e.V).ToArray()),
				foldedChildren
			);
			return foldedNode;
		}

		return Recurse(root);
	}

	public static Dictionary<T, U> FoldRDict<T, U>(
		this TNod<T> root,
		Func<T, IReadOnlyList<U>, U> fun
	)
		where T : notnull
		=>
			root
				.FoldR(fun)
				.Zip(root)
				.ToDictionary(
					e => e.Second.V,
					e => e.First.V
				);

	public static Dictionary<TNod<T>, U> FoldRDictN<T, U>(
		this TNod<T> root,
		Func<T, IReadOnlyList<U>, U> fun
	)
		where T : notnull
		=>
			root
				.FoldR(fun)
				.Zip(root)
				.ToDictionary(
					e => e.Second,
					e => e.First.V
				);
}