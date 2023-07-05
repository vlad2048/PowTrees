// ReSharper disable once CheckNamespace
namespace PowTrees.Algorithms;

public interface IAsciiBox
{
	char Horz { get; }
	char Vert { get; }
	char TL { get; }
	char TR { get; }
	char BL { get; }
	char BR { get; }
}

public static class AsciiBoxes
{
	public static readonly IAsciiBox Single = new Box('─', '│', '┌', '┐', '└', '┘');
	public static readonly IAsciiBox Double = new Box('═', '║', '╔', '╗', '╚', '╝');
	public static readonly IAsciiBox Curved = new Box('─', '│', '╭', '╮', '╰', '╯');

	private class Box : IAsciiBox
	{
		public char Horz { get; }
		public char Vert { get; }
		public char TL { get; }
		public char TR { get; }
		public char BL { get; }
		public char BR { get; }
		public Box(char horz, char vert, char tl, char tr, char bl, char br)
		{
			Horz = horz;
			Vert = vert;
			TL = tl;
			TR = tr;
			BL = bl;
			BR = br;
		}
	}
}