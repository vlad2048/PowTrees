namespace PowTrees.Tests.TestSupport;

static class TreeMaker
{
	public static TNod<T> N<T>(T v, params TNod<T>[] children) => Nod.Make(v, children);

	public static T[] A<T>(params T[] arr) => arr;
}