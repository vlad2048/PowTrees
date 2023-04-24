<Query Kind="Program">
  <NuGetReference>PowBasics.Geom</NuGetReference>
  <NuGetReference>PowTrees.LINQPad</NuGetReference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>PowBasics.Geom</Namespace>
  <Namespace>PowTrees.LINQPad</Namespace>
  <Namespace>PowTrees.LINQPad.DrawerLogic</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
</Query>

void Main()
{
	var tree = Utils.MakeRndTree(maxDepth: 3, maxChildCount: 3, seed: 7);
	
	var dc = new DumpContainer();
	
	var btnPrintText = new Button("Print text", _ =>
	{
		var prn = GenericTreePrinter.Print(tree);
		dc.UpdateContent(prn);
	});
	
	var btnPrintGraphical = new Button("Print graphical", _ =>
	{
		var prn = PanelGfx.Make(gfx =>
		{
			gfx.TreeCtrl(
				Pt.Empty,
				tree,
				n => n.Name,
				(n, s) => new Div(new Button(s)),
				opt =>
				{
					opt.AlignLevels = true;
					opt.GutterSz = new Sz(10, 5);
				}
			);
		});
		dc.UpdateContent(prn);
	});
	
	
	Util.VerticalRun(
		Util.HorizontalRun(true,
			btnPrintText,
			btnPrintGraphical
		),
		"",
		dc
	).Dump();
}



public record Rec(string Name, string Color, int Width, int Height) {
	public override string ToString() => Name;
	[JsonIgnore]
	public Sz Size => new(Width, Height);
}


static class Utils
{
	public static TNod<Rec> MakeRndTree(int maxDepth, int maxChildCount, int? seed = null)
	{
		var rnd = seed switch
		{
			not null => new Random(seed.Value),
			null => new Random((int)DateTime.Now.Ticks)
		};
		var nodIdx = 0;
		var colIdx = 0;
		string GetCol() { var col = Colors[colIdx]; colIdx = (colIdx + 1) % Colors.Length; return col; }
		string GetName() => $"n_{nodIdx++}";
		int GetWidth() => rnd!.Next() % 8 + 3;
		int GetHeight() => rnd!.Next() % 8 + 3;
		int GetChildCount() => rnd.Next() % maxChildCount;

		TNod<Rec> MkNod() => Nod.Make(new Rec(GetName(), GetCol(), GetWidth(), GetHeight()));

		TNod<Rec> Recurse(int level)
		{
			var nod = MkNod();
			if (level < maxDepth)
			{
				var childCount = GetChildCount();
				for (var i = 0; i < childCount; i++)
				{
					var childNod = Recurse(level + 1);
					nod.AddChild(childNod);
				}
			}
			return nod;
		}

		return Recurse(0);
	}
	
	
	
	
	private const int NumCols = 10;
	private const double Sat = 0.72;
	private const double Val = 0.58;

	private static readonly Lazy<string[]> colors = new(() =>
	{
		var list = new List<string>();
		var hue = 0.0;
		var delta = 360.0 / NumCols;
		while (hue < 360)
		{
			list.Add(ColorFromHSV(hue, Sat, Val));
			hue += delta;
		}
		return list.ToArray();
	});
	private static string[] Colors => colors.Value;

	private static string ColorFromHSV(double hue, double saturation, double value)
	{
		int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
		double f = hue / 60 - Math.Floor(hue / 60);

		value = value * 255;
		int v = Convert.ToInt32(value);
		int p = Convert.ToInt32(value * (1 - saturation));
		int q = Convert.ToInt32(value * (1 - f * saturation));
		int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

		static string Mk(int r, int g, int b) => $"#{r:X2}{g:X2}{b:X2}";

		if (hi == 0)
			return Mk(v, t, p);
		else if (hi == 1)
			return Mk(q, v, p);
		else if (hi == 2)
			return Mk(p, v, t);
		else if (hi == 3)
			return Mk(p, q, v);
		else if (hi == 4)
			return Mk(t, p, v);
		else
			return Mk(v, p, q);
	}
}