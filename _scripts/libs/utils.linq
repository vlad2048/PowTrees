<Query Kind="Program">
  <Reference>C:\Dev_Nuget\Libs\PowTrees\Libs\PowTrees\bin\Debug\net7.0\PowTrees.dll</Reference>
  <Reference>C:\Dev_Nuget\Libs\PowTrees\Tests\PowTrees.Tests\bin\Debug\net7.0\PowTrees.Tests.dll</Reference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>PowBasics.Geom</Namespace>
  <Namespace>PowTrees.Algorithms</Namespace>
  <Namespace>PowTrees.Tests.Algorithms.Layout.TestSupport</Namespace>
</Query>

void Main()
{
	
}


public static class Utils
{
	private const int NumCols = 10;
	private const double Sat = 0.72;
	private const double Val = 0.58;

	public static U[] SelectToArray<T, U>(this IEnumerable<T> source, Func<T, U> mapFun) => source.Select(mapFun).ToArray();

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
		//string GetName() => new string('$', rnd.Next() % 6 + 1) + $"_{nodIdx++}";
		int GetWidth() => rnd.Next() % 8 + 3;
		int GetHeight() => rnd.Next() % 8 + 3;
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

	public static void SetStyles() => Util.HtmlHead.AddStyles(@"
		html {
			font-size: 16px;
		}
		body {
			margin: 5px;
			width: 100vw;
			height: 100vh;
			background-color: #030526;
			font-family: consolas;
		}
		div {
			color: #32FEC4;
		}
		a {
			display: block;
		}
	");


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







static class TreeDumpExt
{
	private static Lazy<MethodInfo> genLogMethodDef = new(() => typeof(Algo_Logging).GetMethod("LogToString")!);
	private static MethodInfo GenLogMethodDef => genLogMethodDef.Value;

	public static bool IsGenNod(this object o)
	{
		var t = o.GetType();
		return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(TNod<>);
	}

	public static string LogGenNod(this object o)
	{
		var method = GenLogMethodDef.MakeGenericMethod(o.GetGenNodType());
		var strObj = method.Invoke(null, new object[] { o, null! });
		var str = strObj as string;
		return str!;
	}

	private static Type GetGenNodType(this object o)
	{
		if (!o.IsGenNod()) throw new ArgumentException();
		return o.GetType().GenericTypeArguments.Single();
	}
}


public static object ToDump(object o)
{
	if (o.IsGenNod()) return new Div(new Span(o.LogGenNod()));
	return o switch
	{
		R e => $"{e}",
		_ => o
	};
}





public static class CtrlPropExt
{
	public static string h(this int v) => $"{v}ch";
	public static string v(this int v) => $"{v}em";
	public static string h(this double v) => $"{v}ch";
	public static string v(this double v) => $"{v}em";


	public static Control SetR(this Control ctrl, R r) => ctrl
		.Set("position", "absolute")
		.Set("display", "block")
		.Set("left", $"{r.X.h()}")
		.Set("top", $"{r.Y.v()}")
		.Set("width", $"{r.Width.h()}")
		.Set("height", $"{r.Height.v()}")
		.Set("line-height", $"{r.Height.v()}")
		.Set("text-align", "center");

	public static Control Set(this Control ctrl, string propName, string propVal)
	{
		ctrl.Styles[propName] = propVal;
		return ctrl;
	}

	public static Control SetIf(this Control ctrl, bool condition, string propName, string propVal) => condition switch
	{
		true => ctrl.Set(propName, propVal),
		false => ctrl
	};

	public static Control SetWithAction(this Control ctrl, Action<Control>? optAction)
	{
		optAction?.Invoke(ctrl);
		return ctrl;
	}

	public static Control SetForeColor(this Control ctrl, string? foreColor) => ctrl
		.SetIf(foreColor != null, "color", foreColor!);

	public static Control SetBackColor(this Control ctrl, string? backColor) => ctrl
		.SetIf(backColor != null, "background-color", backColor!);

	public static Control SetColors(this Control ctrl, string? foreColor, string? backColor) => ctrl
		.SetForeColor(foreColor)
		.SetBackColor(backColor);
}