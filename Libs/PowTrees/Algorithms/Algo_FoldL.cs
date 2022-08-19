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
	{
		var mappedTree = root.FoldL(fun);
		var dict = root.Zip(mappedTree)
			.ToDictionary(
				e => e.First.V,
				e => e.Second.V
			);
		return dict;
	}
}