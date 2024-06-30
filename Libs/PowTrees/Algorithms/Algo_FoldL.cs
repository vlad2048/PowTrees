namespace PowTrees.Algorithms;

public static class Algo_FoldL
{
	/// <summary>
	/// Map a tree recursively.
	/// For each node, we use the node and the mapped dad as input
	/// </summary>
	public static TNod<U> FoldL<T, U>(
		this TNod<T> root,
		Func<TNod<T>, U, U> fun,
		U seed
	)
	{
		TNod<U> Recurse(TNod<T> node, U mayMappedDadVal)
		{
			var mappedNodeVal = fun(node, mayMappedDadVal);
			var mappedKidren = node.Kids.Select(kid => Recurse(kid, mappedNodeVal));
			var mappedNode = Nod.Make(mappedNodeVal, mappedKidren);
			return mappedNode;
		}

		return Recurse(root, seed);
	}


	public static TNod<U> FoldL_Dad<T, U>(
		this TNod<T> root,
		Func<T, U> get,
		Func<U, U, U> fun,
		U seed
	) =>
		root.FoldL(
			(nod, acc) =>
				fun(
					nod.DadOr(get, seed),
					acc
				),
			seed
		);


	public static IReadOnlyDictionary<T, U> FoldL_Dict<T, U>(
		this TNod<T> root,
		Func<T, U, U> fun,
		U seed
	) where T : notnull
		=>
		root.Zip(root.FoldL((nod, acc) => fun(nod.V, acc), seed)).ToDictionary(e => e.First.V, e => e.Second.V);


	public static IReadOnlyDictionary<T, U> FoldL_Dad_Dict<T, U>(
		this TNod<T> root,
		Func<T, U> get,
		Func<U, U, U> fun,
		U seed
	) where T : notnull
		=>
		root.Zip(
			root.FoldL(
				(nod, acc) =>
					fun(
						nod.DadOr(get, seed),
						acc
					),
				seed
			)
		).ToDictionary(e => e.First.V, e => e.Second.V);

	
	public static U DadOr<T, U>(this TNod<T> nod, Func<T, U> fun, U seed) => nod.Dad switch
	{
		not null => fun(nod.Dad.V),
		null => seed
	};
}