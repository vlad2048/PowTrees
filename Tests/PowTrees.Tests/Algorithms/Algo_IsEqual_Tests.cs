using PowTrees.Algorithms;
using Shouldly;

namespace PowTrees.Tests.Algorithms;

class Algo_IsEqual_Tests
{
	[Test]
	public static void _00_Equal()
	{
		var a =
			N(2,
				N(3),
				N(7,
					N(9),
					N(11)
				)
			);
		var b =
			N(2,
				N(3),
				N(7,
					N(9),
					N(11)
				)
			);
		a.IsEqual(b).ShouldBeTrue();
	}

	[Test]
	public static void _01_DifferentValue()
	{
		var a =
			N(2,
				N(3),
				N(7,
					N(9),
					N(9999)
				)
			);
		var b =
			N(2,
				N(3),
				N(7,
					N(9),
					N(11)
				)
			);
		a.IsEqual(b).ShouldBeFalse();
	}

	[Test]
	public static void _02_DifferentNodes()
	{
		var a =
			N(2,
				N(3),
				N(7,
					N(9),
					N(11)
				)
			);
		var b =
			N(2,
				N(3),
				N(7,
					N(9)
				)
			);
		a.IsEqual(b).ShouldBeFalse();
	}
}