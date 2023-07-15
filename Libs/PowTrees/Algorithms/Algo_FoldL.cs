namespace PowTrees.Algorithms;

public static class Algo_FoldL
{
	/// <summary>
	/// Map a tree recursively.
	/// For each node, we use the node and the mapped parent as input
	/// </summary>
	public static TNod<U> FoldL<T, U>(
		this TNod<T> root,
		Func<TNod<T>, U, U> fun,
		U seed
	)
	{
		TNod<U> Recurse(TNod<T> node, U mayMappedParentVal)
		{
			var mappedNodeVal = fun(node, mayMappedParentVal);
			var mappedChildren = node.Children.Select(child => Recurse(child, mappedNodeVal));
			var mappedNode = Nod.Make(mappedNodeVal, mappedChildren);
			return mappedNode;
		}

		return Recurse(root, seed);
	}
	
	public static U ParentOr<T, U>(this TNod<T> nod, Func<T, U> fun, U seed) => nod.Parent switch
	{
		not null => fun(nod.Parent.V),
		null => seed
	};
}