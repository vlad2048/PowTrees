namespace PowTrees.Algorithms;

public enum TreeFilterType
{
	KeepIfMatchingOnly,
	KeepIfAllDadsMatchingToo,
}

public sealed class TreeFilterOpt
{
	public TreeFilterType Type { get; set; } = TreeFilterType.KeepIfMatchingOnly;
	public bool AlwaysKeepRoot { get; set; } = false;

	internal static TreeFilterOpt Build(Action<TreeFilterOpt>? action)
	{
		var opt = new TreeFilterOpt();
		action?.Invoke(opt);
		return opt;
	}
}


public static class Algo_Filter
{
	public static TNod<T> LimitDepth<T>(this TNod<T> root, int maxDepth)
	{
		if (maxDepth < 0) throw new ArgumentException();
		return root.Filter((_, lvl) => lvl <= maxDepth).Single();
	}

	public static TNod<T>? FilterBranches<T>(this TNod<T> root, Func<T, bool> predicate)
	{
		bool Keep(TNod<T> node) => node.Any(e => predicate(e.V));

		if (!Keep(root)) return null;

		TNod<T> Rec(TNod<T> node) => Nod.Make(
			node.V,
			node.Kids
				.Where(Keep)
				.Select(Rec)
		);

		return Rec(root);
	}

	public static TNod<T>[] Filter<T>(this TNod<T> root, Func<T, bool> predicate, Action<TreeFilterOpt>? optFun = null) => root.Filter((n, _) => predicate(n), optFun);
	public static TNod<T>[] Filter<T>(this TNod<T> root, Func<T, int, bool> predicate, Action<TreeFilterOpt>? optFun = null) => root.FilterN((n, lvl) => predicate(n.V, lvl), optFun);
	public static TNod<T>[] FilterN<T>(this TNod<T> root, Func<TNod<T>, bool> predicate, Action<TreeFilterOpt>? optFun = null) => root.FilterN((n, _) => predicate(n), optFun);

	public static TNod<T>[] FilterN<T>(this TNod<T> root, Func<TNod<T>, int, bool> predicate, Action<TreeFilterOpt>? optFun = null)
	{
		var opt = TreeFilterOpt.Build(optFun);
		bool Predicate(TNod<T> node, int lvl) => predicate(node, lvl) || (opt.AlwaysKeepRoot && node == root);
		return opt.Type switch
		{
			TreeFilterType.KeepIfMatchingOnly => root.Filter_KeepIfMatchingOnly(Predicate),
			TreeFilterType.KeepIfAllDadsMatchingToo => root.Filter_KeepIfAllDadsMatchingToo(Predicate),
			_ => throw new ArgumentException()
		};
	}


	private static TNod<T>[] Filter_KeepIfAllDadsMatchingToo<T>(
		this TNod<T> root,
		Func<TNod<T>, int, bool> predicate
	)
	{
		TNod<T>[] Recurse(TNod<T> node, int lvl)
		{
			if (!predicate(node, lvl)) return Array.Empty<TNod<T>>();
			var kidren = node.Kids.Select(e => Recurse(e, lvl + 1)).SelectMany(e => e);
			return new[] { Nod.Make(node.V, kidren) };
		}
		return Recurse(root, 0);
	}


	private static TNod<T>[] Filter_KeepIfMatchingOnly<T>(this TNod<T> root, Func<TNod<T>, int, bool> predicate)
	{
		List<TNod<T>> FindMatchingKidren(TNod<T> topNode, bool includeTopNodeIfMatch, int lvl)
		{
			if (includeTopNodeIfMatch && predicate(topNode, lvl))
				return new List<TNod<T>> { topNode };

			var filteredKidren = new List<TNod<T>>();

			void Recurse(TNod<T> _node, int _lvl)
			{
				foreach (var kid in _node.Kids)
				{
					if (predicate(kid, _lvl))
					{
						var filteredKid = BuildRecurse(kid, _lvl);
						filteredKidren.Add(filteredKid);
					}
					else
					{
						Recurse(kid, _lvl + 1);
					}
				}
			}

			Recurse(topNode, lvl + 1);
			return filteredKidren;
		}

		TNod<T> BuildRecurse(TNod<T> node, int lvl) => Nod.Make(node.V, FindMatchingKidren(node, false, lvl));

		var outputNodes = FindMatchingKidren(root, true, 0).Select(BuildRecurse).ToArray();

		return outputNodes;
	}
}