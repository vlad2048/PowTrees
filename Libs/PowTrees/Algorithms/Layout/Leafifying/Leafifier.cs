using PowTrees.Algorithms.Layout.Exts;
using PowTrees.Algorithms.Layout.Leafifying.Structs;

namespace PowTrees.Algorithms.Layout.Leafifying;

static class Leafifier
{
	public static TNod<IMix<T>> Leafify<T>(this TNod<T> root)
		=> root.MapIsLastN((nod, isLast) => isLast switch
		{
			false => (IMix<T>)new NodeMix<T>(nod),
			true => new LeafMix<T>(nod, Array.Empty<LeafMix<T>>())
		});


	public static TNod<IMix<T>> LeafifyFurther<T>(this TNod<IMix<T>> root)
	{
		var depth = root.GetDepth();
		return root.MapCut((nod, level) => (depth - level) switch
		{
			> 1 => nod.V,
			1 => new LeafMix<T>(
				nod.AssertNodeMixAndGetNod(),
				nod.Children.AssertLeafMixes()
			),
			0 => null,
			_ => throw new ArgumentException()
		});
	}


	private static TNod<U> MapIsLastN<T, U>(this TNod<T> root, Func<TNod<T>, bool, U> mapFun)
	{
		var depth = root.GetDepth();
		return root.MapN((nod, level) => mapFun(nod, level == depth));
	}

	public static TNod<T> MapCut<T>(
		this TNod<T> root,
		Func<TNod<T>, int, T?> fun
	)
	{
		TNod<T>? Recurse(TNod<T> nod, int level)
		{
			var mappedNode = fun(nod, level);
			if (mappedNode == null) return null;
			var mappedChildren = nod.Children
				.Select(child => Recurse(child, level + 1))
				.Where(e => e != null)
				.SelectToArray(e => e!);
			return Nod.Make(mappedNode, mappedChildren);
		}
		return Recurse(root, 0)!;
	}

	private static int GetDepth<T>(this TNod<T> root)
	{
		var depth = 0;
		root.ForEachWithLevel((_, level) =>
		{
			if (level > depth)
				depth = level;
		});
		return depth;
	}

	private static TNod<T> AssertNodeMixAndGetNod<T>(this TNod<IMix<T>> mixNod)
	{
		var node = mixNod.V;
		if (node is not NodeMix<T> nodeMix) throw new ArgumentException();
		return nodeMix.Node;
	}

	private static LeafMix<T>[] AssertLeafMixes<T>(this IReadOnlyList<TNod<IMix<T>>> mixNodes) =>
		mixNodes
			.SelectToArray(mixNode =>
			{
				if (mixNode.V is not LeafMix<T> leafMix) throw new ArgumentException();
				return leafMix;
			});
}