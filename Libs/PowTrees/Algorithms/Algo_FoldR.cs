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
	{
		var mappedTree = root.FoldR(fun);
		var dict = root.Zip(mappedTree)
			.ToDictionary(
				e => e.First.V,
				e => e.Second.V
			);
		return dict;
	}
}