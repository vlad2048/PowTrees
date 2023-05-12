using PowTrees.Algorithms;
using PowTrees.Tests.TestSupport;
using Shouldly;

namespace PowTrees.Tests.Algorithms;

class Algo_RemoveTransitiveDependencies_Tests
{
	[Test]
	public static void _00_DepthOne() =>
		N(1,
				N(100),
				N(2,
					N(100)
				)
			)
			.ShowInput()
			.RemoveTransitiveDependencies()
			.CheckTree(
				N(1,
					N(2,
						N(100)
					)
				)
			);

	[Test]
	public static void _01_DepthOne_OtherOrder() =>
		N(1,
				N(2,
					N(100)
				),
				N(100)
			)
			.ShowInput()
			.RemoveTransitiveDependencies()
			.CheckTree(
				N(1,
					N(2,
						N(100)
					)
				)
			);


	[Test]
	public static void _02_DepthTwo() =>
		N(1,
				N(2),
				N(100,
					N(4),
					N(5,
						N(6,
							N(7)
						),
						N(100)
					)
				)
			)
			.ShowInput()
			.RemoveTransitiveDependencies()
			.CheckTree(
				N(1,
					N(2),
					N(100,
						N(4),
						N(5,
							N(6,
								N(7)
							)
						)
					)
				)
			);


	/*[Test]
	public static void _03_OtherBranch() =>
		N(1,
				N(2),
				N(100,
					N(4),
					N(5)
				),
				N(6,
					N(7),
					N(100)
				)
			)
			.ShowInput()
			.RemoveTransitiveDependencies()
			.CheckTree(
				N(1,
					N(2),
					N(100,
						N(4),
						N(5)
					),
					N(6,
						N(7)
					)
				)
			);*/
}