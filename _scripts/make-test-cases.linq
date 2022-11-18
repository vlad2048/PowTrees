<Query Kind="Program">
  <Reference>C:\Dev_Nuget\Libs\PowTrees\Libs\PowTrees\bin\Debug\net7.0\PowTrees.dll</Reference>
  <Reference>C:\Dev_Nuget\Libs\PowTrees\Tests\PowTrees.Tests\bin\Debug\net7.0\PowTrees.Tests.dll</Reference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>PowBasics.Geom</Namespace>
  <Namespace>PowBasics.Geom.Serializers</Namespace>
  <Namespace>PowTrees.Algorithms</Namespace>
  <Namespace>PowTrees.Serializer</Namespace>
  <Namespace>PowTrees.Tests.Algorithms.Layout.TestSupport</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
</Query>

#load ".\libs\utils"


void Main()
{
	Utils.SetStyles();
	
	var fileOut = @"C:\Dev_Nuget\Libs\PowTrees\Tests\PowTrees.Tests\Algorithms\Layout\test-cases.json";
	var testCount = 100;

	var testCases = Enumerable.Range(0, testCount)
		.SelectToArray(i =>
			TestCaseUtils.Make(
				Utils.MakeRndTree(maxDepth: 4, maxChildCount: 3, seed: i == 0 ? 7 : null)
			)
		);
	var str = testCases.Ser();
	File.WriteAllText(fileOut, str);
}

void MakeTestCases()
{
	var fileOut = @"C:\Dev_Nuget\Libs\PowTrees\Tests\PowTrees.Tests\Algorithms\Layout\test-cases.json";
	var testCount = 100;
	
	var testCases = Enumerable.Range(0, testCount)
		.SelectToArray(i =>
			TestCaseUtils.Make(
				Utils.MakeRndTree(maxDepth: 4, maxChildCount: 3, seed: i == 0 ? 7 : null)
			)
		);
	var str = testCases.Ser();
	File.WriteAllText(fileOut, str);
}


