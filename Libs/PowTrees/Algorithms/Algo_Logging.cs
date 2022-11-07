using Microsoft.VisualBasic;
using PowBasics.Geom;
using PowTrees.Algorithms.Layout.Exts;

namespace PowTrees.Algorithms;

public enum TreeLogType
{
	Inline,
	Traversal
}

public class TreeLogOpt<T>
{
	public TreeLogType Type { get; set; } = TreeLogType.Inline;
	public Func<T, string>? FormatFun { get; set; }
	public int TraversalIndentPerLevel { get; set; } = 2;
	public int LeadingSpaceCount { get; set; } = 0;

	internal static TreeLogOpt<T> Build(Action<TreeLogOpt<T>>? action)
	{
		var opt = new TreeLogOpt<T>();
		action?.Invoke(opt);
		return opt;
	}
}


public static class Algo_Logging
{
	public static string LogToString<T>(this TNod<T> root, Action<TreeLogOpt<T>>? optFun = null) => string.Join(Environment.NewLine, root.LogToStrings(optFun));
	public static string[] LogToStrings<T>(this TNod<T> root, Action<TreeLogOpt<T>>? optFun = null)
	{
		var opt = TreeLogOpt<T>.Build(optFun);
		static string FmtDefault(T v) => $"{v}";
		var strTree = root.Map(opt.FormatFun ?? FmtDefault);
		var lines = opt.Type switch
		{
			TreeLogType.Inline => strTree.LogInline(),
			TreeLogType.Traversal => strTree.LogTraversal(opt),
			_ => throw new ArgumentException()
		};
		var leadingStr = new string(' ', opt.LeadingSpaceCount);
		return lines.SelectToArray(e => $"{leadingStr}{e}");
	}



	private static string[] LogTraversal<T>(this TNod<string> root, TreeLogOpt<T> opt)
	{
		var lines = new List<string>();
		void Recurse(TNod<string> node, int level)
		{
			var padStr = new string(' ', level * opt.TraversalIndentPerLevel);
			lines.Add($"{padStr}{node.V}");
			foreach (var child in node.Children)
				Recurse(child, level + 1);
		}
		Recurse(root, 0);
		return lines.ToArray();
	}



	private static string[] LogInline(this TNod<string> root)
	{
		var layout = root.Layout(e => new Sz(e.Length, 1), opt => opt.GutterSz = new Sz(3, 0));
		var treeSz = layout.Values.Union().Size;
		var buffer = Enumerable.Range(0, treeSz.Height)
			.SelectToArray(_ => new string(' ', treeSz.Width));

		void Print(Pt pos, string s)
		{
			var line = buffer[pos.Y];
			var n = s.Length;
			buffer[pos.Y] = line[..pos.X] + s + line[(pos.X + n)..];
		}

		foreach (var (n, r) in layout)
			Print(r.Pos, n.V);

		DrawArrows(layout.GetRTree(), Print);

		return buffer;
	}


	private static void DrawArrows(TNod<R> root, Action<Pt, string> print)
	{
		//┌─┬─┐    ╭───╮
		//│ │ │    │   │
		//├─┼─┤    ╰───╯
		//└─┴─┘    

		var chHoriz = "─";
		var chVert = "│";
		var chCornerTop = "┌";
		var chCornerBottom = "└";
		var chCross = "┼";
		var chTUp = "┴";
		var chTDown = "┬";
		var chTLeft = "┤";
		var chTRight = "├";
		var chArrow = "►";

		root
			.Where(e => e.Children.Count == 1).ForEach(n =>
			{
				var d = n.Children[0].V.X - (n.V.X + n.V.Width);
				print(n.V.OnTheRight(), $"{new string(chHoriz[0], d - 1)}{chArrow}");
			});

		root
			.Where(e => e.Children.Count > 1)
			.ForEach(n =>
			{
				var rp = n.V;
				var rcs = n.Children.SelectToArray(e => e.V);
				var xMid = (rp.Right + rcs[0].X) / 2 + 1;
				var yMid = rp.YMid();
				var yMin = rcs.First().YMid();
				var yMax = rcs.Last().YMid();

				print(n.V.OnTheRight(), new string(chHoriz[0], xMid - (rp.X + rp.Width)));
				
				for (var i = yMin; i <= yMax; i++)
					print(new Pt(xMid, i), chVert);

				print(new Pt(xMid, rp.YMid()), chTLeft);

				for (var i = 0; i < rcs.Length; i++)
				{
					var r = rcs[i];
					var y = r.YMid();

					var ch = (i == 0, y == yMid, i == rcs.Length - 1) switch
					{
						(true, true, true) => chHoriz,
						(true, true, false) => chTDown,
						(true, false, false) => chCornerTop,
						(false, true, false) => chCross,
						(false, false, true) => chCornerBottom,
						(false, true, true) => chTUp,
						(false, false, false) => chTRight,
						//_ => throw new ArgumentException()
						_ => "V"
					};
					print(new Pt(xMid, y), $"{ch}{new string(chHoriz[0], r.X - xMid)}{chArrow}");
				}
			});
	}

	private static Pt OnTheRight(this R r) => new(r.X + r.Width, r.YMid());
	private static Pt OnTheLeft(this R r) => new(r.X - 1, r.YMid());
	//private static int YMid(this R r) => r.Y + r.Height / 2;
	private static int YMid(this R r) => r.Y;

	private static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
	{
		foreach (var elt in source)
			action(elt);
	}


	private static TNod<R> GetRTree<T>(this Dictionary<TNod<T>, R> layout) =>
		layout
			.Keys.Single(e => e.Parent == null)
			.MapN(e => layout[e]);



	/*private static string[] LogInline<T>(this TNod<string> root, TreeLogOpt<T> opt)
	{
		static List<TNod<string>>[] SortIntoLvlArr(TNod<string> root)
		{
			var dict = new SortedDictionary<int, List<TNod<string>>>();
			var maxLvl = 0;
			void Recurse(TNod<string> node, int lvl)
			{
				if (lvl > maxLvl) maxLvl = lvl;
				if (!dict.TryGetValue(lvl, out var list))
					list = dict[lvl] = new List<TNod<string>>();
				list.Add(node);
				foreach (var child in node.Children)
					Recurse(child, lvl + 1);
			}
			Recurse(root, 0);

			var res = new List<TNod<string>>[maxLvl + 1];
			foreach (var (k, v) in dict)
				res[k] = v;
			return res;
		}


			var lvlLists = SortIntoLvlArr(root);
			var lvlLengths = lvlLists.SelectToArray(e => e.Max(f => f.V.Length));

			const int gutterSize = 3;
			var gutterCount = lvlLists.Length - 1;

			var bufWidth = lvlLengths.Sum() + gutterSize * gutterCount;
			var bufHeight = 1 + lvlLists.Sum(e => e.Count - 1);
			var buffer = Enumerable.Range(0, bufHeight).SelectToArray(_ => new string(' ', bufWidth));

			var locMap = new Dictionary<TNod<string>, (int, int)>();

			void Print(int x, int y, TNod<string> node, int lvl)
			{
				var str = node.Children.Any() switch
				{
					true => node.V.PadRight(lvlLengths[lvl], '─'),
					false => node.V,
				};
				locMap[node] = (x, y);
				var isValid = x >= 0 && y >= 0 && y < bufHeight && x + str.Length <= bufWidth;
				if (isValid)
				{
					var l = buffer[y];
					var lBefore = l.Substring(0, x);
					var lAfter = l.Substring(x + str.Length);
					var newL = lBefore + str + lAfter;
					buffer[y] = newL;
				}
			}

			void PrintChar(int x, int y, char ch)
			{
				var str = $"{ch}";
				var isValid = x >= 0 && y >= 0 && y < bufHeight && x + str.Length <= bufWidth;
				if (isValid)
				{
					var l = buffer[y];
					var lBefore = l.Substring(0, x);
					var lAfter = l.Substring(x + str.Length);
					var newL = lBefore + str + lAfter;
					buffer[y] = newL;
				}
			}

			var absY = 0;
			int GetLvlX(int lvl) => lvlLengths.Take(lvl).Sum() + lvl * gutterSize;

			void Recurse(TNod<string> node, int lvl)
			{
				var x = GetLvlX(lvl);
				var y = absY;
				Print(x, y, node, lvl);

				for (var childIndex = 0; childIndex < node.Children.Count; childIndex++)
				{
					var child = node.Children[childIndex];
					Recurse(child, lvl + 1);
					if (childIndex < node.Children.Count - 1)
						absY++;
				}
			}

			Recurse(root, 0);

//┌─┬─┐    ╭───╮
//│ │ │    │   │
//├─┼─┤    ╰───╯
//└─┴─┘    

			for (var lvl = 0; lvl < lvlLengths.Length - 1; lvl++)
			{
				var startX = lvlLengths.Take(lvl + 1).Sum() + lvl * gutterSize;
				var parents = lvlLists[lvl];
				foreach (var parent in parents)
				{
					var parentLoc = locMap[parent];
					var endYs = parent.Children.SelectToArray(e => locMap[e].Item2);
					if (endYs.Length == 0) continue;
					var startY = parentLoc.Item2;
					PrintChar(startX, startY, '─');

					void FinishArrow(int _childIndex)
					{
						var childY = endYs[_childIndex];
						if (endYs.Length == 1)
							PrintChar(startX + 1, childY, '─');
						else if (_childIndex == 0)
							PrintChar(startX + 1, childY, '┬');
						else if (_childIndex == endYs.Length - 1)
							PrintChar(startX + 1, childY, '└');
						else
							PrintChar(startX + 1, childY, '├');
						PrintChar(startX + 2, childY, '►');
					}

					var curY = startY;
					for (var childIndex = 0; childIndex < endYs.Length; childIndex++)
					{
						var childY = endYs[childIndex];
						for (var y = curY + 1; y < childY; y++)
							PrintChar(startX + 1, y, '│');
						FinishArrow(childIndex);
						curY = childY;
					}
				}
			}

			return buffer
				.Where(e => !string.IsNullOrEmpty(e.Trim()))
				.ToArray();
	}*/
}