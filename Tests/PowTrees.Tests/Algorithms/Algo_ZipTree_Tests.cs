using PowTrees.Algorithms;
using PowTrees.Tests.TestSupport;
using Shouldly;

namespace PowTrees.Tests.Algorithms;

class Algo_ZipTree_Tests
{
	[Test]
	public static void _00_Basic()
	{
		var a =
			N(2,
				N(3),
				N(7,
					N(9),
					N(4)
				)
			);
		var b =
			N(12,
				N(13),
				N(17,
					N(19),
					N(14)
				)
			);

		a.ZipTree(b).CheckTree(
			N((2, 12),
				N((3, 13)),
				N((7, 17),
					N((9, 19)),
					N((4, 14))
				)
			)
		);
	}

	[Test]
	public static void _01_NodeMismatch()
	{
		var a =
			N(2,
				N(3),
				N(7,
					N(9),
					N(4)
				)
			);
		var b =
			N(12,
				N(17,
					N(19),
					N(14)
				)
			);

		Should.Throw<ArgumentException>(() => a.ZipTree(b));
	}
}