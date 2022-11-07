using PowBasics.Geom;

namespace PowTrees.Tests.Algorithms.Layout.TestSupport;

public record TestCase(
	TNod<Rec> Tree,
	R[] Rs
);