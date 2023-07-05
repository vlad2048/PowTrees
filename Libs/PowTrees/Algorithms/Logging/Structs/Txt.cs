using System.Drawing;
using PowBasics.Geom;

// ReSharper disable once CheckNamespace
namespace PowTrees.Algorithms;

public sealed record Txt(
	TxtChunk[][] Lines
)
{
	internal static Txt FromChunk(string text, Color color) => new(new[]
	{
		new[] { new TxtChunk(text, color) }
	});
}


static class TxtExt
{
	public static Sz GetSize(this Txt txt) => txt.Lines.All(e => e.GetLng() == 0) switch
	{
		true => Sz.Empty,
		false => new Sz(
			txt.Lines.Max(e => e.GetLng()),
			txt.Lines.Length
		)
	};

	private static int GetLng(this IEnumerable<TxtChunk> line) => line.Sum(e => e.Text.Length);
}