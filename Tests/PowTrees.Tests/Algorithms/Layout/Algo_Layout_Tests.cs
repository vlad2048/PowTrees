using PowTrees.Tests.Algorithms.Layout.TestSupport;

namespace PowTrees.Tests.Algorithms.Layout;

class Algo_Layout_Tests
{
	private const string TestCaseFile = @"Algorithms\Layout\test-cases.json";

	[Test]
	public void CheckTestCases()
	{
		var str = File.ReadAllText(TestCaseFile);
		var tcs = str.Deser<TestCase[]>();

		for (var index = 0; index < tcs.Length; index++)
		{
			var tc = tcs[index];
			TestCaseUtils.Verify(tc);
		}
	}
}