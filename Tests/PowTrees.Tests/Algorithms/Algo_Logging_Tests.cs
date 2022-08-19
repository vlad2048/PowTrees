using PowTrees.Algorithms;
using PowTrees.Tests.TestSupport;

namespace PowTrees.Tests.Algorithms;

class Algo_Logging_Tests
{
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
3─┬►5     
  └►7─┬►9 
      └►11
");
	}
}