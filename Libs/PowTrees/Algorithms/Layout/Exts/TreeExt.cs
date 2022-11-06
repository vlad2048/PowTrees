﻿namespace PowTrees.Algorithms.Layout.Exts;

static class TreeExt
{
	public static Dictionary<K, V> ToDictionaryWithLevel<T, K, V>(this TNod<T> root, Func<TNod<T>, K> keyFun, Func<TNod<T>, int, V> valFun) where K : notnull
	{
		var map = new Dictionary<K, V>();
		root.ForEachWithLevel((nod, level) => map[keyFun(nod)] = valFun(nod, level));
		return map;
	}

	public static TNod<T>[][] GetNodesByLevels<T>(this TNod<T> root)
	{
		var lists = new List<List<TNod<T>>>();
		void AddToLevel(TNod<T> node, int level)
		{
			List<TNod<T>> list;
			if (level < lists.Count)
				list = lists[level];
			else if (level == lists.Count)
				lists.Add(list = new List<TNod<T>>());
			else
				throw new ArgumentException();
			list.Add(node);
		}
		root.ForEachWithLevel(AddToLevel);
		return lists.SelectToArray(e => e.ToArray());
	}

	public static void ForEachWithLevel<T>(this TNod<T> root, Action<TNod<T>, int> action)
	{
		void Recurse(TNod<T> node, int level)
		{
			action(node, level);
			foreach (var child in node.Children)
				Recurse(child, level + 1);
		}
		Recurse(root, 0);
	}
}