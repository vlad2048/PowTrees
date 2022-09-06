namespace PowTrees.Algorithms;

public static class Algo_FoldL
{
	/// <summary>
	/// Map a tree recursively.
	/// For each node, we use the node and the mapped parent as input
	/// </summary>
	public static TNod<U> FoldL<T, U>(
		this TNod<T> root,
		Func<T, U?, U> fun
	) where U : class
	{
		TNod<U> Recurse(TNod<T> node, U? mayMappedParentVal)
		{
			var mappedNodeVal = fun(node.V, mayMappedParentVal);
			var mappedChildren = node.Children.Select(child => Recurse(child, mappedNodeVal));
			var mappedNode = new TNod<U>(mappedNodeVal, mappedChildren);
			return mappedNode;
		}

		return Recurse(root, null);		
	}

	public static Dictionary<T, U> FoldLDict<T, U>(
		this TNod<T> root,
		Func<T, U?, U> fun
	)
		where T : notnull
		where U : class
		=>
			root
				.FoldL(fun)
				.Zip(root)
				.ToDictionary(
					e => e.Second.V,
					e => e.First.V
				);

	public static Dictionary<TNod<T>, U> FoldLDictN<T, U>(
		this TNod<T> root,
		Func<T, U?, U> fun
	)
		where T : notnull
		where U : class
		=>
			root
				.FoldL(fun)
				.Zip(root)
				.ToDictionary(
					e => e.Second,
					e => e.First.V
				);
}