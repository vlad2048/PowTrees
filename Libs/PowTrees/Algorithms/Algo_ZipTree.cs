namespace PowTrees.Algorithms;

public static class Algo_ZipTree
{
	public static TNod<(T, U)> ZipTree<T, U>(this TNod<T> rootA, TNod<U> rootB)
	{
		TNod<(T, U)> Recurse(TNod<T> nodeA, TNod<U> nodeB)
		{
			if (nodeA.Children.Count != nodeB.Children.Count)
				throw new ArgumentException("Cannot Zip trees with mismatched nodes");
			return Nod.Make(
				(nodeA.V, nodeB.V),
				nodeA.Children.Zip(nodeB.Children)
					.Select(t => Recurse(t.First, t.Second))
			);
		}

		return Recurse(rootA, rootB);
	}
}