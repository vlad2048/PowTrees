using NUnit.Framework.Legacy;
using PowTrees.Algorithms;

namespace PowTrees.Tests.Algorithms.Layout.TestSupport;

public static class TestCaseUtils
{
	public static TestCase Make(TNod<Rec> tree)
	{
		var layout = tree.Layout(e => e.Size);
		var rs = tree.MapN(e => layout[e]).Select(e => e.V).ToArray();
		return new TestCase(tree, rs);
	}
	
	public static void Verify(TestCase tc)
	{
		var (tree, expRs) = tc;
		var layout = tree.Layout(e => e.Size);
		var actRs = tree.MapN(e => layout[e]).Select(e => e.V).ToArray();
		CollectionAssert.AreEqual(expRs, actRs);
	}
}