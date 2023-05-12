
using PowBasics.Geom;
using PowTrees.Algorithms.Layout.Exts;

// ReSharper disable once CheckNamespace
namespace PowTrees.Algorithms;

public class GraphLayout<T>
{
	public TNod<LayoutNode<T>> Root { get; }
	public R BBox { get; }

	internal GraphLayout(TNod<LayoutNode<T>> root)
	{
		Root = root;
		BBox = Root
			.Map(e => e.R)
			.Select(e => e.V)
			.Union();
	}
}

public static class GraphLayoutExt
{
	public static GraphLayout<T>[] LayVertically<T>(this GraphLayout<T>[] layouts, int gap)
	{
		var offsets = new int[layouts.Length];
		var ofs = 0;
		for (var i = 0; i < layouts.Length - 1; i++)
		{
			ofs += layouts[i].BBox.Height + gap;
			offsets[i + 1] = ofs;
		}

		return layouts
			.SelectToArray((e, i) => e.Offset(new Pt(0, offsets[i])));
	}

	public static GraphLayout<T> Offset<T>(this GraphLayout<T> layout, Pt ofs) => new(
		layout.Root.Map(layoutNode => layoutNode with { R = layoutNode.R + ofs })
	);
}