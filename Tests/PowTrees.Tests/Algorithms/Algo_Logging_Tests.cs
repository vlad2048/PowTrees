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


	/*
	[Test]
	public void _00_Traversal()
	{
		var t =
			N(3,
				N(5),
				N(7,
					N(9),
					N(11)
				)
			);

		t.LogToStrings(opt => opt.Type = TreeLogType.Traversal).CheckLines(@"
3
  5
  7
    9
    11
");
	}

	[Test]
	public void _01_Inline()
	{
		var t =
			N(3,
				N(5),
				N(7,
					N(9),
					N(11)
				)
			);

		t.LogToStrings(opt => opt.Type = TreeLogType.Inline).CheckLines(@"
  ┌►5     
3─┴►7─┬►9 
      └►11
");
	}
	*/
}
