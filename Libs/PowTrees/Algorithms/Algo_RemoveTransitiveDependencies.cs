namespace PowTrees.Algorithms;

public static class Algo_RemoveTransitiveDependencies
{
	public static TNod<T> RemoveTransitiveDependencies<T>(this TNod<T> root, IEqualityComparer<T>? cmpFun = null)
	{
		var cmp = cmpFun ?? EqualityComparer<T>.Default;
		var descendentMap = root.ToDictionary(e => e, e => GetAllDescendents(e, cmp));
		bool IsInDescendents(IEnumerable<TNod<T>> parents, TNod<T> child) => parents.Any(parent => descendentMap[parent].Contains(child.V));

		TNod<T> Rec(TNod<T> n)
		{
			var listTodo = new Queue<TNod<T>>(n.Kids);
			var listDone = new List<TNod<T>>();
			var parents = n.GetParents(cmp);

			while (listTodo.TryDequeue(out var child))
			{
				var isSomewhereElse =
					parents.Contains(child.V) ||
					IsInDescendents(listTodo, child) ||
					IsInDescendents(listDone, child);
				if (!isSomewhereElse)
					listDone.Add(child);
			}

			return Nod.Make(n.V, listDone.Select(Rec));
		}

		return Rec(root);
	}

	private static HashSet<T> GetAllDescendents<T>(this TNod<T> root, IEqualityComparer<T> cmp) => new(root.Skip(1).Select(e => e.V), cmp);

	private static HashSet<T> GetParents<T>(this TNod<T> root, IEqualityComparer<T> cmp)
	{
		var set = new HashSet<T>(cmp);
		var node = root.Dad;
		while (node != null)
		{
			set.Add(node.V);
			node = node.Dad;
		}
		return set;
	}
}