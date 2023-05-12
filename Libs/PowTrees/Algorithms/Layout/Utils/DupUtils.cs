﻿// ReSharper disable once CheckNamespace
namespace PowTrees.Algorithms;

record Dup<T>(T Orig);

static class DupUtils
{
	public static TNod<Dup<T>> Dup<T>(this TNod<T> root) => root.Map(e => new Dup<T>(e));
	//public static TNod<T> Dedup<T>(this TNod<Dup<T>> root) => root.Map(e => e.Orig);

	public static TNod<R> ZipMapWithTree<T, V, R>(this Dictionary<TNod<T>, V> map, TNod<T> root, Func<T, V, R> combineFun) =>
		root
			.MapN(nod => combineFun(nod.V, map[nod]));
}