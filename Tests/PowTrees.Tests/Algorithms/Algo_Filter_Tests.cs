using PowTrees.Algorithms;
using PowTrees.Tests.TestSupport;
using Shouldly;

namespace PowTrees.Tests.Algorithms;

class Algo_Filter_Tests
{
	[Test]
	public void _00_KeepIfMatchingOnly_Empty()
	{
		var t =
			N(1,
				N(2),
				N(3,
					N(4),
					N(5)
				),
				N(6),
				N(7,
					N(8,
						N(9),
						N(10),
						N(11)
					),
					N(12)
				)
			).ShowInput();

		t.FilterN(e => e.V >= 100).CheckTrees(A<TNod<int>>(
		));
	}

	
	[Test]
	public void _01_KeepIfMatchingOnly_RootOnly()
	{
		var t =
			N(100,
				N(2),
				N(3,
					N(4),
					N(5)
				),
				N(6),
				N(7,
					N(8,
						N(9),
						N(10),
						N(11)
					),
					N(12)
				)
			).ShowInput();

		t.FilterN(e => e.V >= 100).CheckTrees(A(
			N(100)
		));
	}

	[Test]
	public void _02_KeepIfMatchingOnly_RootAndOthers()
	{
		var t =
			N(100,
				N(2),
				N(3,
					N(104),
					N(5)
				),
				N(6),
				N(107,
					N(8,
						N(9),
						N(10),
						N(111)
					),
					N(12)
				)
			).ShowInput();

		t.FilterN(e => e.V >= 100).CheckTrees(A(
			N(100,
				N(104),
				N(107,
					N(111)
				)
			)
		));
	}

	
	[Test]
	public void _03_KeepIfMatchingOnly_NotRootSplitResults()
	{
		var t =
			N(1,
				N(2),
				N(3,
					N(104),
					N(5)
				),
				N(6),
				N(107,
					N(8,
						N(9),
						N(110),
						N(11)
					),
					N(112)
				)
			).ShowInput();

		t.FilterN(e => e.V >= 100).CheckTrees(A(
			N(104),

			N(107,
				N(110),
				N(112)
			)
		));
	}

	[Test]
	public void _04_KeepIfMatchingOnly_WithLevel()
	{
		var t =
			N(1,
				N(2),
				N(3,
					N(4),
					N(5)
				),
				N(6),
				N(7,
					N(8,
						N(9),
						N(10),
						N(11)
					),
					N(12)
				)
			).ShowInput();

		t.FilterN((_, lvl) => lvl < 2).CheckTrees(A(
			N(1,
				N(2),
				N(3),
				N(6),
				N(7)
			)
		));

		t.FilterN((e, lvl) => lvl > 1 && e.V >= 0).CheckTrees(A(
			N(4),
			N(5),
			N(8,
				N(9),
				N(10),
				N(11)
			),
			N(12)
		));
	}



	[Test]
	public void _10_KeepIfAllParentsMatchingToo_Empty()
	{
		var t =
			N(3,
				N(5),
				N(7)
			).ShowInput();
		t.Filter(e => e < 0, opt => opt.Type = TreeFilterType.KeepIfAllParentsMatchingToo).CheckTrees(A<TNod<int>>(
		));
	}
	
	[Test]
	public void _11_KeepIfAllParentsMatchingToo_OnlyChildMatching()
	{
		var t =
			N(3,
				N(5),
				N(7)
			).ShowInput();
		t.Filter(e => e == 5, opt => opt.Type = TreeFilterType.KeepIfAllParentsMatchingToo).CheckTrees(A<TNod<int>>(
		));
	}
	
	[Test]
	public void _11_KeepIfAllParentsMatchingToo_AllParentsMatching()
	{
		var t =
			N(3,
				N(5,
					N(10,
						N(1)
					)
				),
				N(7)
			).ShowInput();
		t.Filter(e => e <= 5, opt => opt.Type = TreeFilterType.KeepIfAllParentsMatchingToo).CheckTrees(A(
			N(3,
				N(5)
			)
		));
	}
	
	[Test]
	public void _12_KeepIfAllParentsMatchingToo_WithLevel()
	{
		tree.ShowInput().Filter((_, lvl) => lvl <= 0, opt => opt.Type = TreeFilterType.KeepIfAllParentsMatchingToo).CheckTrees(A(
			N(3)
		));

		tree.ShowInput().Filter((_, lvl) => lvl <= 1, opt => opt.Type = TreeFilterType.KeepIfAllParentsMatchingToo).CheckTrees(A(
			N(3,
				N(5),
				N(7)
			)
		));

		tree.ShowInput().Filter((_, lvl) => lvl <= 2, opt => opt.Type = TreeFilterType.KeepIfAllParentsMatchingToo).CheckTrees(A(
			N(3,
				N(5),
				N(7,
					N(11),
					N(13)
				)
			)
		));

		tree.ShowInput().Filter((_, lvl) => lvl <= 3, opt => opt.Type = TreeFilterType.KeepIfAllParentsMatchingToo).CheckTrees(A(
			N(3,
				N(5),
				N(7,
					N(11),
					N(13,
						N(17),
						N(21)
					)
				)
			)
		));

		tree.ShowInput().Filter((_, lvl) => lvl <= 4, opt => opt.Type = TreeFilterType.KeepIfAllParentsMatchingToo).CheckTrees(A(
			N(3,
				N(5),
				N(7,
					N(11),
					N(13,
						N(17),
						N(21)
					)
				)
			)
		));
	}



	private static readonly TNod<int> tree =
		N(3,
			N(5),
			N(7,
				N(11),
				N(13,
					N(17),
					N(21)
				)
			)
		);

	[Test]
	public void _20_LimitDepth_Negative() =>
		Should.Throw<ArgumentException>(() =>
			tree.ShowInput()
				.LimitDepth(-1)
		);

	[Test]
	public void _21_LimitDepth_Zero() =>
		tree.ShowInput()
			.LimitDepth(0)
			.CheckTree(
				N(3)
			);

	[Test]
	public void _22_LimitDepth_One() =>
		tree.ShowInput()
			.LimitDepth(1)
			.CheckTree(
				N(3,
					N(5),
					N(7)
				)
			);

	[Test]
	public void _23_LimitDepth_Two() =>
		tree.ShowInput()
			.LimitDepth(2)
			.CheckTree(
				N(3,
					N(5),
					N(7,
						N(11),
						N(13)
					)
				)
			);

	[Test]
	public void _24_LimitDepth_Three() =>
		tree.ShowInput()
			.LimitDepth(3)
			.CheckTree(
				N(3,
					N(5),
					N(7,
						N(11),
						N(13,
							N(17),
							N(21)
						)
					)
				)
			);

	[Test]
	public void _25_LimitDepth_Four() =>
		tree.ShowInput()
			.LimitDepth(4)
			.CheckTree(
				N(3,
					N(5),
					N(7,
						N(11),
						N(13,
							N(17),
							N(21)
						)
					)
				)
			);
}