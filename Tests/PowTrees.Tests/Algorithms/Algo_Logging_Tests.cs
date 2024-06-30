using PowBasics.Geom;
using PowTrees.Algorithms;

namespace PowTrees.Tests.Algorithms;

class Algo_Logging_Tests
{
	[Test]
	public void _00_Basic()
	{
		var root =
			N(n0,
				N(n1,
					N(n2),
					N(n3)
				)
			);
		var lines = root.Log(opt =>
		{
			opt.GutterSz = new Sz(6, 1);
			opt.AlignLevels = true;
		});
		foreach (var line in lines)
			Console.WriteLine($"'{line}'");
	}



	private const string n0 =
		"""
		First
		cnt=3
		""";

	private const string n1 =
		"""
		Second
		cnt=3
		R=124352222
		""";

	private const string n2 =
		"""
		Third one
		cnt=5
		""";

	private const string n3 =
		"""
		Fourth
		Blabla  bla
		""";
}
