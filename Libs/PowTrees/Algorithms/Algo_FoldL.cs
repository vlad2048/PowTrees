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
	)
	{
		TNod<U> Recurse(TNod<T> node, U? mayMappedParentVal)
		{
			var mappedNodeVal = fun(node.V, mayMappedParentVal);
			var mappedChildren = node.Children.Select(child => Recurse(child, mappedNodeVal));
			var mappedNode = Nod.Make(mappedNodeVal, mappedChildren);
			return mappedNode;
		}

		return Recurse(root, default);		
	}

	public static Dictionary<T, U> FoldLDict<T, U>(
		this TNod<T> root,
		Func<T, U?, U> fun
	)
		where T : notnull
		=>
			root.Zip(root.FoldL(fun))
				.ToDictionary(
					e => e.First.V,
					e => e.Second.V
				);

	public static Dictionary<TNod<T>, U> FoldLDictN<T, U>(
		this TNod<T> root,
		Func<T, U?, U> fun
	)
		=>
			root.Zip(root.FoldL(fun))
				.ToDictionary(
					e => e.First,
					e => e.Second.V
				);
}