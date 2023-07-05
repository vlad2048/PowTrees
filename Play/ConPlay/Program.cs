using System.Drawing;
using PowTrees.Algorithms;

namespace ConPlay;

static class Program
{
	static void Main()
	{
		var root0 =
			N(0,
				N(1),
				N(2,
					N(3),
					N(4)
				)
			);

		var root1 =
			N(5,
				N(6,
					N(7),
					N(8)
				),
				N(9),
				N(10)
			);

		var root2 =
			N(11,
				N(12),
				N(13)
			);

		var treeRoot =
			N(root0,
				N(root1),
				N(root2)
			);

		var txt = treeRoot.LogColored(WriteTree);
		txt.PrintToConsole();
	}

	private static void WriteTree(TNod<int> root, ITxtWriter writer)
	{
		var txt = root.LogColored(WriteNode);
		writer.Write(txt);
		writer.SurroundWith(AsciiBoxes.Double, Color.Yellow, $"Tree {root.V}");
	}

	private static void WriteNode(int n, ITxtWriter writer)
	{
		writer.WriteLine($"Node {n}", Color.DarkOrchid);
		writer.WriteLine($"cnt: {n}", Color.DarkOrange);
		if (n % 3 == 0)
			writer.SurroundWith(AsciiBoxes.Single, Color.DodgerBlue);
	}

	private static TNod<T> N<T>(T v, params TNod<T>[] children) => Nod.Make(v, children);
}