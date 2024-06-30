namespace PowTrees.Tests.TestSupport;

static class TreeMaker
{
	public static TNod<T> N<T>(T v, params TNod<T>[] kidren) => Nod.Make(v, kidren);

	public static T[] A<T>(params T[] arr) => arr;
}