using PowBasics.Geom;
using PowTrees.Algorithms.Layout.Exts;
using PowTrees.Algorithms.Layout.Leafifying;
using PowTrees.Algorithms.Layout.Leafifying.Structs;

// ReSharper disable once CheckNamespace
namespace PowTrees.Algorithms;

public static class Algo_Layout
{
	public class AlgoLayoutOpt
	{
		public Sz GutterSz { get; set; } = new(3, 1);

		internal static AlgoLayoutOpt Make(Action<AlgoLayoutOpt>? optFun)
		{
			var opt = new AlgoLayoutOpt();
			optFun?.Invoke(opt);
			return opt;
		}
	}

	public static Dictionary<TNod<T>, R> Layout<T>(this TNod<T> root, Func<T, Sz> szFun, Action<AlgoLayoutOpt>? optFun = null)
	{
		var opt = AlgoLayoutOpt.Make(optFun);

		var rootSz = root.Map(e => szFun(e).MakeBigger(opt.GutterSz));
		var mapBack = rootSz.Zip(root).ToDictionary(e => e.First, e => e.Second);

		var xs = rootSz.SolveXs();
		var ys = rootSz.SolveYs();

		return rootSz.ToDictionaryWithLevel(
			nodSz => mapBack[nodSz],
			(nodSz, level) => new R(
				new Pt(
					xs[level],
					ys[nodSz]
				),
				nodSz.V.MakeSmaller(opt.GutterSz)
			)
		);
	}

	private static int[] SolveXs(this TNod<Sz> rootSz)
	{
		var levelGrps = rootSz.GetNodesByLevels();
		var arr = levelGrps.SelectToArray(nods => nods.Max(e => e.V.Width));
		return arr.FoldL(0, (elt, acc) => acc + elt);
	}

	private static Dictionary<TNod<Sz>, int> SolveYs(this TNod<Sz> rootSz)
	{
		var ltree = rootSz.Leafify();
		while (ltree.V is NodeMix<Sz>)
			ltree = ltree.LeafifyFurther();
		var topLeaf = (LeafMix<Sz>)ltree.V;

		var heightMap = new Dictionary<LeafMix<Sz>, int>();

		int SolveHeight(LeafMix<Sz> leaf)
		{
			if (heightMap.TryGetValue(leaf, out var height)) return height;
			var childrenHeights = leaf.SubLeaves.SelectToArray(SolveHeight);
			var parentHeight = leaf.Node.V.Height;
			var slnHeight = Math.Max(parentHeight, childrenHeights.SumOrZero());
			heightMap[leaf] = slnHeight;
			return slnHeight;
		}
		
		SolveHeight(topLeaf);


		var ys = new Dictionary<TNod<Sz>, int>();
		
		void SolveY(LeafMix<Sz> leaf, int ofs)
		{
			var parentHeight = leaf.Node.V.Height;
			var leafHeight = heightMap[leaf];
			var parentY = (leafHeight - parentHeight) / 2;
			ys[leaf.Node] = parentY + ofs;
			foreach (var subLeaf in leaf.SubLeaves)
			{
				SolveY(subLeaf, ofs);
				ofs += heightMap[subLeaf];
			}
		}
		
		SolveY(topLeaf, 0);

		return ys;
	}
}