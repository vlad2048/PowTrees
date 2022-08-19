namespace PowTrees.Algorithms;

public static class Algo_Map
{
	public static TNod<U> Map<T, U>(this TNod<T> root, Func<T, U> mapFun)
	{
		TNod<U> Recurse(TNod<T> node) => new(mapFun(node.V), node.Children.Select(Recurse));
		return Recurse(root);
	}

	public static TNod<U> Map<T, U>(this TNod<T> root, Func<TNod<T>, U> mapFun)
	{
		TNod<U> Recurse(TNod<T> node) => new(mapFun(node), node.Children.Select(Recurse));
		return Recurse(root);
	}
}