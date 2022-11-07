<Query Kind="Program">
  <Reference>C:\Dev_Nuget\Libs\PowTrees\Libs\PowTrees\bin\Debug\net6.0\PowTrees.dll</Reference>
  <Reference>C:\Dev_Nuget\Libs\PowTrees\Tests\PowTrees.Tests\bin\Debug\net6.0\PowTrees.Tests.dll</Reference>
  <Namespace>PowTrees.Tests.Algorithms.Layout.TestSupport</Namespace>
  <Namespace>PowTrees.Algorithms</Namespace>
  <Namespace>PowBasics.Geom</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
</Query>

#load ".\libs\utils"

void Main()
{
	Utils.SetStyles();
	//Util.ReadLine();
	//var root = Utils.MakeRndTree(maxDepth: 3, maxChildCount: 3, seed: 7);
	
	var root = Utils.MakeRndTree(maxDepth: 4, maxChildCount: 5, seed: null);
	
	//root.Draw();
	root.Dump();
}

static class DrawUtils
{
	private const string BorderStyle = "1px dashed #FFFFFF30";
	
	public static void Draw(this TNod<Rec> root)
	{
		var layout = root.Layout(e => e.Size, e => e.AlignLevels = true);
		var sz = layout.Values.Union().Size;
		
		var spans = root
			.Select(
				nod => new Span(nod.V.Name)
					.SetR(layout[nod])
					.SetBackColor(nod.V.Color)
			)
			.ToList();
			
		for (var i = 0; i <= sz.Width; i++) spans.Add(
			new Span()
				.SetR(new R(i, 0, 1, sz.Height))
				.Set("border-left", BorderStyle)
		);
		for (var i = 0; i <= sz.Height; i++) spans.Add(
			new Span()
				.SetR(new R(0, i, sz.Width, 1))
				.Set("border-top", BorderStyle)
		);

		var div = new Div(spans)
			.Set("position", "relative")
			.Set("width", $"{sz.Width.h()}")
			.Set("height", $"{sz.Height.v()}");

		div.Dump();
	}

	private static TNod<R> GetRTree<T>(this Dictionary<TNod<T>, R> layout) =>
		layout
			.Keys.Single(e => e.Parent == null)
			.MapN(e => layout[e]);
}
