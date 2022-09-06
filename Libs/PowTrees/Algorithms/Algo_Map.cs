namespace PowTrees.Algorithms;

public static class Algo_Map
{
	public static TNod<U> Map<T, U>(this TNod<T> root, Func<T, U> mapFun) => root.MapN((nod, _) => mapFun(nod.V));
	public static TNod<U> Map<T, U>(this TNod<T> root, Func<T, int, U> mapFun) => root.MapN((nod, lvl) => mapFun(nod.V, lvl));
	public static TNod<U> MapN<T, U>(this TNod<T> root, Func<TNod<T>, U> mapFun) => root.MapN((nod, _) => mapFun(nod));

	public static TNod<U> MapN<T, U>(this TNod<T> root, Func<TNod<T>, int, U> mapFun)
	{
		TNod<U> Recurse(TNod<T> node, int lvl) => new(mapFun(node, lvl), node.Children.Select(e => Recurse(e, lvl + 1)));
		return Recurse(root, 0);
	}
}