using System.Text.Json.Serialization;
using PowBasics.Geom;

namespace PowTrees.Tests.Algorithms.Layout.TestSupport;

public record Rec(string Name, string Color, int Width, int Height) {
	public override string ToString() => Name;
	[JsonIgnore]
	public Sz Size => new(Width, Height);
}