using PowBasics.Geom;
using PowTrees.Algorithms.Layout.Exts;

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
		var heightMap = rootSz.FoldRDictN<Sz, int>(
			(node, heights) => Math.Max(node.Height, heights.SumOrZero())
		);

		var ys = new Dictionary<TNod<Sz>, int>();

		int Recurse(TNod<Sz> nod, int y)
		{
			// "height of nod"
			var hn = nod.V.Height;

			// "height of children"
			var hc = nod.Children.SumOrZero(c => heightMap[c]);

			// "total height" of nod and its children
			var ht = heightMap[nod];

			// Layout nod and its children within [y .. y + ht]
			// ================================================
			// (1) ht >= hn     if ht > hn => shift nod      to be in the center of ht
			// (2) ht >= hc     if ht > hc => shift children to be in the center of ht

			// (1)
			ys[nod] = y + (ht - hn) / 2;	// => RESULT

			// (2)
			y += (ht - hc) / 2;
			foreach (var c in nod.Children)
				y += Recurse(c, y);

			return ht;
		}

		Recurse(rootSz, 0);

		return ys;
	}
}